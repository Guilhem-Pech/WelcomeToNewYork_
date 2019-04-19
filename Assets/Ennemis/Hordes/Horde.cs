using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Horde : ScriptableObject
{
    private Dictionary<int, GameObject> members;
    private List<GameObject> targets;

    private Vector3 globalVelocity;
    private Vector3 globalCenter;

    private static int nextID = 1;

    private int ID;

    //Steering Variables
    enum behavior_type
    {
        none = 0x00000,
        goTo = 0x00002,
        hunting = 0x00004,
    }
    public int m_iFlags;
    public int lastReceivedCallNumber = -1;

    private Vector2 seekPos = Vector2.zero;
    private Vector2 huntPos = Vector2.zero;

    private void SetSeekPos(Vector3 pos) { seekPos = new Vector2(pos.x, pos.z); }
    public Vector3 GetSeekPos() { return new Vector3(seekPos.x, 0, seekPos.y); }
    private void SetHuntPos(Vector3 pos) { huntPos = new Vector2(pos.x, pos.z); }
    public Vector3 GetHuntPos() { return new Vector3(huntPos.x, 0, huntPos.y); }

    //Steering methods
    //état des comportements
    private bool On(behavior_type bt) { return (m_iFlags == ((int)bt)); }
    private void ResetBehaviour() { m_iFlags = (int)behavior_type.none; }

    public bool isSeekOn() { return On(behavior_type.goTo); }
    public void SeekOn(Vector3 pos) { m_iFlags = (int)behavior_type.goTo; SetSeekPos(pos); }

    public bool isHuntingOn() { return On(behavior_type.hunting); }
    public void HuntingOn() { m_iFlags = (int)behavior_type.hunting; }

    //action des comportements
    //Déplacement de tout les membres jusqu'à proximité d'un point
    private void Seek()
    {
        foreach (KeyValuePair<int, GameObject> it in members)
            it.Value.GetComponent<Animator>().SetBool("IsPackTravelling", true);
    }

    //Déplacement de tout les membres vers le joueur le plus proche (modélisé par seekPos)
    private void Hunt()
    {
        foreach (KeyValuePair<int, GameObject> it in members)
            if (!it.Value.GetComponent<Animator>().GetBool("IsPackHunting"))
            {
                it.Value.GetComponent<Animator>().SetBool("IsPackHunting", true);
                it.Value.GetComponent<Animator>().SetBool("IsPackTravelling", false);
            }
    }

    //gestion des comportements
    private void applyBehaviour()
    {
        if (isSeekOn())
        {
            if (isNearEnoughOfPos(GetSeekPos()))
            {
                HuntingOn();
            }
            else
            {
                if (seekPos == null)
                    throw new System.Exception(System.Reflection.MethodBase.GetCurrentMethod().Name + " (in horde " + ID + ") : seekPos non défini");
                Seek();
            }
        }

        if (isHuntingOn())
        {
            //Déterminer huntPos
            SetHuntPos(getNearestPlayerPosition(GameObject.FindGameObjectsWithTag("Player")));
            Hunt();
        }
    }
    
    public bool isNearEnoughOfPos(Vector3 pos)
    {
        GameObject nearestMember;
        float nearestMemberDistance = float.MaxValue;

        foreach (KeyValuePair<int, GameObject> it in members)
            if (nearestMemberDistance > (pos - it.Value.transform.position).magnitude)
            {
                nearestMember = it.Value;
                nearestMemberDistance = (pos - it.Value.transform.position).magnitude;
            }

        return nearestMemberDistance <= EnnemiParams.Instance.ProximityDistance;
    }

    public Vector3 getNearestPlayerPosition(GameObject[] players)
    {
        Vector3 centre = getGlobalCenter();

        GameObject nearestPlayer = null;
        float nearestPlayerDistance = float.MaxValue;

        foreach (GameObject player in players)
            if (nearestPlayerDistance > (centre - player.transform.position).magnitude && player.GetComponent<BaseEntity>().enabled)
            {
                nearestPlayer = player;
                nearestPlayerDistance = (centre - player.transform.position).magnitude;
            }

        if (nearestPlayer == null)
            throw new System.Exception(System.Reflection.MethodBase.GetCurrentMethod().Name + " (in horde " + ID + ") : Aucun joueurs donnés");

        return nearestPlayer.transform.position;
    }

    // Start is called before the first frame update
    void Awake()
    {
        members = new Dictionary<int, GameObject>();
        targets = new List<GameObject>();
        globalVelocity = Vector3.zero;
        globalCenter = Vector3.zero;

        ID = nextID;
        nextID++;

        ResetBehaviour();
    }

    // Update is called once per frame
    public void Update()
    {
        //Séparation des éventuels groupes au sein de la horde
        List<List<int>> sousHordes = getSeparatedHordes();
        sousHordes.RemoveAt(0); //La premiére sous-horde reste dans la horde actuelle
        foreach (List<int> soushorde in sousHordes)
        {
            if (soushorde.Count > 1)
            {
                Horde newHorde = HordesSingleton.Instance.manager.CreateNewHorde();
                foreach (int memberID in soushorde)
                {
                    if (members.ContainsKey(memberID))
                    {
                        GameObject member = members[memberID];
                        member.GetComponent<HordeMemberComponent>().leaveCurrentHorde();
                        member.GetComponent<HordeMemberComponent>().joinHorde(newHorde);
                    }
                }
            }
            else if (soushorde.Count == 1)
            {
                foreach (int memberID in soushorde)
                {
                    if (members.ContainsKey(memberID))
                    {
                        GameObject member = members[memberID];
                        member.GetComponent<HordeMemberComponent>().leaveCurrentHorde();
                    }
                }
                
            }
        }
        

        //Mise à jour des Variables de la horde
        varUpdate();

        //Mise à jour des comportements
        //applyBehaviour();
    }

    private List<List<int>> getSeparatedHordes()
    { //On récupére la liste des "sous-hordes" représentées par la liste de ID de leurs membres
        List<List<int>> temp_hordes = new List<List<int>>();

        //On itère parmis tout les membres
        foreach (KeyValuePair<int, GameObject> it in members)
        {
            GameObject member = it.Value;

            //On cherche la sous-horde du membre actuel
            List<int> his_temp_horde = null;
            foreach (List<int> temp_horde in temp_hordes)
            {
                if (temp_horde.Contains(member.GetInstanceID()))
                {
                    his_temp_horde = temp_horde;
                    break;
                }
            }

            //Si il n'en a pas on en crée une nouvelle
            if (his_temp_horde == null)
            {
                his_temp_horde = new List<int>();
                his_temp_horde.Add(member.GetInstanceID());
                temp_hordes.Add(his_temp_horde);
            }

            //On assigne ses voisins à sa horde si ils n'y sont pas déjà
            foreach (int id in member.transform.Find("NeighboursTrigger").gameObject.GetComponent<NeighboursSystem>().getNeighboursIDs())
            {
                if (members.ContainsKey(id) && !his_temp_horde.Contains(id))
                {
                    his_temp_horde.Add(id);
                }
            }
        }

        return temp_hordes;
    }

    private void varUpdate()
    {
        targets.Clear();
        Vector3 bufferGlobalVelocity = Vector3.zero;
        Vector3 bufferGlobalCenter = Vector3.zero;

        foreach (KeyValuePair<int, GameObject> it in members)
        {
            bufferGlobalVelocity += it.Value.GetComponent<NavMeshAgent>().velocity.normalized;
            bufferGlobalCenter += it.Value.transform.localPosition;
            if (it.Value.GetComponent<TargetingSystem>().hasTarget()
                && !targets.Contains(it.Value.GetComponent<TargetingSystem>().getTarget()))
            {
                targets.Add(it.Value.GetComponent<TargetingSystem>().getTarget());
            }
        }

        bufferGlobalVelocity = bufferGlobalVelocity / members.Count;
        bufferGlobalCenter = bufferGlobalCenter / members.Count;

        globalVelocity = bufferGlobalVelocity;
        globalCenter = bufferGlobalCenter;
    }

    /* Getters */
    public List<GameObject> getTargets()
    {
        return targets;
    }

    public Vector3 getGlobalVelocity() {
        return globalVelocity;
    }

    public Vector3 getGlobalCenter()
    {
        return globalCenter;
    }

    public bool isEmpty()
    {
        return members.Count == 0;
    }

    public int size()
    {
        return members.Count;
    }

    public int getID()
    {
        return ID;
    }

    /* Members management */
    public bool addMember(GameObject entity)
    {
        if (entity.GetComponent<TargetingSystem>() == null
            || entity.GetComponent<NavMeshAgent>() == null
            || entity.GetComponent<HordeMemberComponent>() == null
            || members.ContainsKey(entity.GetInstanceID()))
        {
            return false;
        }

        members.Add(entity.GetInstanceID(),entity);
       // Debug.Log(ID + ".size = " + members.Count);
        return true;
    }

    public bool removeMember(GameObject entity)
    {
        if (!members.ContainsKey(entity.GetInstanceID()))
        {
            return false;
        }
        
        members.Remove(entity.GetInstanceID());
       // Debug.Log(ID + ".size = " + members.Count);
        return true;
    }

    /* Destroy-Management */
    public void clearHorde()
    {
        //Désinscrire les entitées de la horde
        foreach (KeyValuePair<int, GameObject> it in members)
        {
            it.Value.GetComponent<HordeMemberComponent>().leaveCurrentHorde();
        }
    }
}
