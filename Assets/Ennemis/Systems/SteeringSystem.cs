using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;



public class SteeringSystem : NetworkBehaviour
{
    //Variables de débogage
    [ReadOnly] public Vector2 fleeVec = Vector2.zero;
    [ReadOnly] public Vector2 seekVec = Vector2.zero;
    [ReadOnly] public Vector2 wanderVec = Vector2.zero;
    [ReadOnly] public Vector2 pursuitVec = Vector2.zero;
    [ReadOnly] public Vector2 evadeVec = Vector2.zero;
    [ReadOnly] public Vector2 cohesionVec = Vector2.zero;
    [ReadOnly] public Vector2 separationVec = Vector2.zero;
    [ReadOnly] public Vector2 alignmentVec = Vector2.zero;
    [ReadOnly] public Vector2 interposeVec = Vector2.zero;
    [ReadOnly] public Vector2 hideVec = Vector2.zero;
    [ReadOnly] public Vector2 offsetPursuitVec = Vector2.zero;
    [ReadOnly] public Vector2 obstacleAvoidanceVec = Vector2.zero;
    [ReadOnly] public Vector2 wallAvoidanceVec = Vector2.zero;

    /* --- ------------------------- --- */
    /* --- Déclaration des variables --- */
    /* --- ------------------------- --- */
        /* --- Régulation du taux de rafraichissement --- */
        private float m_SteeringUpdateCooldown;
        private float m_SteeringUpdateCurrentCooldown;

        /* --- IDs comportements --- */
        enum behavior_type
        {
            none = 0x00000,
            seek = 0x00002,
            flee = 0x00004,
            offset_pursuit = 0x00008,
            wander = 0x00010,
            cohesion = 0x00020,
            separation = 0x00040,
            allignment = 0x00080,
            pursuit = 0x00100,
            evade = 0x00200,
            interpose = 0x00400,
            hide = 0x00800,
            flock = 0x01000,
            obstacle_avoidance = 0x02000,
            wall_avoidance = 0x04000,
        };

        private int m_iFlags;

        /* --- Somme des forces --- */
        [ReadOnly] public Vector2 m_vSteeringPos;

        /* --- Poids des comportements --- */
        private float m_dWeightSeparation;
        private float m_dWeightCohesion;
        private float m_dWeightAlignment;
        private float m_dWeightWander;
        private float m_dWeightSeek;
        private float m_dWeightFlee;
        private float m_dWeightPursuit;
        private float m_dWeightOffsetPursuit;
        private float m_dWeightInterpose;
        private float m_dWeightHide;
        private float m_dWeightEvade;
        private float m_dWeightObstacleAvoidance;
        private float m_dWeightWallAvoidance;

        /* --- Variables Wander --- */
        private Vector2 m_vWanderTarget;

        private float m_dWanderJitterElapsedTime;
        private float m_dWanderRadius;
        private float m_dWanderJitter;

        /* --- Variables pour comportements ciblés --- */
        private Vector2 seekPos;
        private Vector2 fleePos;

        private GameObject m_objTarget;
        private GameObject m_objTarget1;
        private GameObject m_objTarget2;

        /* --- Variable pour comportements avec offset --- */
        private Vector2 m_vOffset;

    /* --- ------------------------------------------- --- */
    /* --- Fonctions pour définition des comportements --- */
    /* --- ------------------------------------------- --- */
        //état des comportements
        private bool On(behavior_type bt) { return (m_iFlags & ((int) bt) ) == ((int)bt); }

        public bool IsFleeOn() { return On(behavior_type.flee); }
        public bool IsSeekOn() { return On(behavior_type.seek); }
        public bool IsWanderOn() { return On(behavior_type.wander); }
        public bool IsPursuitOn() { return On(behavior_type.pursuit); }
        public bool IsEvadeOn() { return On(behavior_type.evade); }
        public bool IsCohesionOn() { return On(behavior_type.cohesion); }
        public bool IsSeparationOn() { return On(behavior_type.separation); }
        public bool IsAlignmentOn() { return On(behavior_type.allignment); }
        public bool IsInterposeOn() { return On(behavior_type.interpose); }
        public bool IsHideOn() { return On(behavior_type.hide); }
        public bool IsOffsetPursuitOn() { return On(behavior_type.offset_pursuit); }
        public bool IsObstacleAvoidanceOn() { return On(behavior_type.hide); }
        public bool IsWallAvoidanceOn() { return On(behavior_type.offset_pursuit); }

        public void SeekOn() { m_iFlags = (int)behavior_type.seek; }
        public void WanderOn() { m_dWanderJitterElapsedTime = 0; SetWanderTargetPosition(m_dWanderRadius); m_iFlags = (int)behavior_type.wander; }
        public void PursuitOn(GameObject o1) { m_iFlags = (int)behavior_type.pursuit; m_objTarget = o1; }
        public void EvadeOn(GameObject o1) { m_iFlags = (int)behavior_type.evade; m_objTarget2 = o1; }
        //public void CohesionOn() { m_iFlags = (int)behavior_type.cohesion; }
        //public void SeparationOn() { m_iFlags = (int)behavior_type.separation; }
        //public void AlignmentOn() { m_iFlags = (int)behavior_type.allignment; }
        public void InterposeOn(GameObject o1, GameObject o2) { m_iFlags = (int)behavior_type.interpose; m_objTarget1 = o1; m_objTarget2 = o2; }
        public void HideOn(GameObject o1) { m_iFlags = (int)behavior_type.hide; m_objTarget1 = o1; }
        public void OffsetPursuitOn(GameObject o1, Vector2 offset) { m_iFlags = (int)behavior_type.offset_pursuit; m_vOffset = offset; m_objTarget1 = o1; }
        //public void FlockingOn() { CohesionOn(); AlignmentOn(); SeparationOn(); }
        //public void ObstacleAvoidanceOn() { m_iFlags = (int)behavior_type.obstacle_avoidance; }
        //public void WallAvoidanceOn() { m_iFlags = (int)behavior_type.wall_avoidance; }

        //Activation comportement OLD VERSION
        /*public void SeekOn() { m_iFlags |= (int)behavior_type.seek; }
        public void WanderOn() { m_dWanderJitterElapsedTime = 0; SetWanderTargetPosition(m_dWanderRadius); m_iFlags |= (int)behavior_type.wander; }
        public void PursuitOn(GameObject o1) { m_iFlags |= (int)behavior_type.pursuit; m_objTarget = o1; }
        public void EvadeOn(GameObject o1) { m_iFlags |= (int)behavior_type.evade; m_objTarget2 = o1; }
        public void CohesionOn() { m_iFlags |= (int)behavior_type.cohesion; }
        public void SeparationOn() { m_iFlags |= (int)behavior_type.separation; }
        public void AlignmentOn() { m_iFlags |= (int)behavior_type.allignment; }
        public void InterposeOn(GameObject o1, GameObject o2) { m_iFlags |= (int)behavior_type.interpose; m_objTarget1 = o1; m_objTarget2 = o2; }
        public void HideOn(GameObject o1) { m_iFlags |= (int)behavior_type.hide; m_objTarget1 = o1; }
        public void OffsetPursuitOn(GameObject o1, Vector2 offset){m_iFlags |= (int) behavior_type.offset_pursuit; m_vOffset = offset; m_objTarget1 = o1;}
        public void FlockingOn() { CohesionOn(); AlignmentOn(); SeparationOn(); }
        //public void ObstacleAvoidanceOn() { m_iFlags |= (int)behavior_type.obstacle_avoidance; }
        //public void WallAvoidanceOn() { m_iFlags |= (int)behavior_type.wall_avoidance; }

        //Désactivation comportement
        public void FleeOff() { if (On(behavior_type.flee)) m_iFlags ^= (int)behavior_type.flee; }
        public void SeekOff() { if (On(behavior_type.seek)) m_iFlags ^= (int)behavior_type.seek; }
        public void WanderOff() { if (On(behavior_type.wander)) m_iFlags ^= (int)behavior_type.wander; }
        public void PursuitOff() { if (On(behavior_type.pursuit)) m_iFlags ^= (int)behavior_type.pursuit; }
        public void EvadeOff() { if (On(behavior_type.evade)) m_iFlags ^= (int)behavior_type.evade; }
        public void CohesionOff() { if (On(behavior_type.cohesion)) m_iFlags ^= (int)behavior_type.cohesion; }
        public void SeparationOff() { if (On(behavior_type.separation)) m_iFlags ^= (int)behavior_type.separation; }
        public void AlignmentOff() { if (On(behavior_type.allignment)) m_iFlags ^= (int)behavior_type.allignment; }
        public void InterposeOff() { if (On(behavior_type.interpose)) m_iFlags ^= (int)behavior_type.interpose; }
        public void HideOff() { if (On(behavior_type.hide)) m_iFlags ^= (int)behavior_type.hide; }
        public void OffsetPursuitOff() { if (On(behavior_type.offset_pursuit)) m_iFlags ^= (int)behavior_type.offset_pursuit; }
        public void FlockingOff() { CohesionOff(); AlignmentOff(); SeparationOff(); WanderOff(); }
        public void ObstacleAvoidanceOff() { if (On(behavior_type.obstacle_avoidance)) m_iFlags ^= (int)behavior_type.obstacle_avoidance; }
        public void WallAvoidanceOff() { if (On(behavior_type.wall_avoidance)) m_iFlags ^= (int)behavior_type.wall_avoidance; }*/

        public void AllOff() { m_iFlags = 0; }

        /* --- ------------------ --- */
        /* --- Fonctions Usuelles --- */
        /* --- ------------------ --- */
        private Vector2 GetHidingPosition(Vector2 posOb,  double radiusOb, Vector2 posHunter)
        {
            /* A COMPLETER*/
            return gameObject.transform.position;
        }

    [Server]
    private void SetWanderTargetPosition(float radius)
        {
            m_vWanderTarget = new Vector2(Random.Range(-radius, radius), Random.Range(-radius, radius));
        }

    [Server]
    private void AddJitterToWanderTargetPosition(float jitter, float elpasedTime)
        {
            float effectiveJitter = elpasedTime * jitter;

            Vector3 vectorToRotate = new Vector3(m_vWanderTarget.x, 0, m_vWanderTarget.y);
            Vector3 rotatedVector = Quaternion.Euler(0, Random.Range(-effectiveJitter, effectiveJitter), 0) * vectorToRotate;
            m_vWanderTarget = new Vector2(rotatedVector.x, rotatedVector.z);
        }

    [Server]
    private void AccumulateForce(ref Vector2 sf, Vector2 ForceToAdd)
        {
            sf += ForceToAdd.normalized;
        }

    [Server] public void SetSeekPos(Vector3 pos) { seekPos = new Vector2(pos.x,pos.z); }
       [Server]
    public void SetTarget(GameObject agent) {m_objTarget = agent; }
       [Server]
    public void SetTargetAgent1(GameObject agent) { m_objTarget1 = agent; }
       [Server]
    public void SetTargetAgent2(GameObject agent) { m_objTarget2 = agent; }

       [Server]
    public void SetOffset(Vector3 offset) { m_vOffset = new Vector2(offset.x, offset.z); }
       [Server]
    public Vector3 GetOffset(){return new Vector3(m_vOffset.x, 0,m_vOffset.y); }



    public Vector3 SteeringPos(){return new Vector3(m_vSteeringPos.x, 0, m_vSteeringPos.y);}

    /* --- -------------------------------------------- --- */
    /* --- Fonctions calcul des forces comportementales --- */
    /* --- -------------------------------------------- --- */
        //Déplacement vers une position donnée
       [Server]
    private Vector2 Seek(Vector2 TargetPos)
        {
            return TargetPos;
        }

        //Déplacement visant à s'éloigner de la position donnée
       [Server]
    private Vector2 Flee(Vector2 TargetPos)
        {
            return -TargetPos;
        }

    //Déplacement qui consiste à "intercepter" l'objet ciblé
    [Server]
    private Vector2 Pursuit(GameObject agent)
        {
            Vector3 predictedEvaderPos = Vector3.zero;
            if (agent.GetComponent<CharacterController>() != null)
            {
                predictedEvaderPos = agent.transform.position + (agent.GetComponent<CharacterController>().velocity * Time.deltaTime);
            }
            else if (agent.GetComponent<NavMeshAgent>() != null)
            {
                predictedEvaderPos = agent.transform.position + (agent.GetComponent<NavMeshAgent>().velocity * Time.deltaTime);
            }
            else
            {
                predictedEvaderPos = agent.transform.position;
            }

            return new Vector2(predictedEvaderPos.x, predictedEvaderPos.z);
        }

    //Déplacement qui consiste à "intercepter" l'objet ciblé tout en maintenant un décalage par rapport à sa position
    [Server]
    private Vector2 OffsetPursuit(GameObject agent, Vector2 offset)
        {
            // A COMPLETER
            return Vector2.zero;
        }

    //Déplacement visant à s'éloigner d'un objet en prévoyant sa vélocité
    [Server]
    private Vector2 Evade(GameObject agent)
        {
            // A COMPLETER
            return Vector2.zero;
        }

        //Déplacement aléatoire
       [Server]
    private Vector2 Wander()
        {
            AddJitterToWanderTargetPosition(m_dWanderJitter, m_dWanderJitterElapsedTime);
            m_dWanderJitterElapsedTime = 0;

            Vector2 localPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
            return m_vWanderTarget + localPos;
        }

        //Déplacement visant à se positionner entre 2 objets donnés
       [Server]
    private Vector2 Interpose(GameObject agentA, GameObject agentB)
        {
            // A COMPLETER
            return Vector2.zero;
        }

    //Déplacement visant à se cacher d'un agent donné, à l'aide d'une liste des obstacles possibles
    [Server]
    private Vector2 Hide(GameObject hunter, List<GameObject> obstacles)
        {
            // A COMPLETER
            return Vector2.zero;
        }

    //Déplacement visant à se cacher d'un agent donné, à l'aide d'une liste des obstacles possibles
    [Server]
    private Vector2 ObstacleAvoidance(List<GameObject> obstacles)
        {
            // A COMPLETER
            return Vector2.zero;
        }

        //Déplacement visant à se cacher d'un agent donné, à l'aide d'une liste des obstacles possibles
       [Server]
    private Vector2 WallAvoidance(List<GameObject> walls)
        {
            // A COMPLETER
            return Vector2.zero;
        }

        //Comportements de flocking
        /*
         A ADAPTER
         */
        private Vector2 Cohesion(Horde horde)
        {
            Vector3 globalCenter = new Vector2(horde.getGlobalCenter().x, horde.getGlobalCenter().z);
            return Seek(globalCenter);
        }

    [Server]
    private Vector2 Separation(List<GameObject> agents)
        {
            Vector3 SteeringForce = Vector3.zero;

            foreach (GameObject agent in agents)
            {
                if(agent != null)
                {
                    Vector3 ToAgent = gameObject.transform.position - agent.transform.position;

                    SteeringForce += ToAgent.normalized / (ToAgent.magnitude > 1 ? ToAgent.magnitude : 1);
                }

            }

            return (new Vector2(SteeringForce.x, SteeringForce.z)).normalized ;
        }

    [Server]
    private Vector2 Alignment(Horde horde)
        {
            Vector2 hordeVelocity = new Vector2(horde.getGlobalVelocity().x, horde.getGlobalVelocity().z);
            return hordeVelocity.normalized;
        }

    /* --- -------------------------------------- --- */
    /* --- Fonction calcul de la somme des forces --- */
    /* --- -------------------------------------- --- */
    private void Calculate()
    {
        //reset the steering force
        m_vSteeringPos = Vector2.zero;

        //Si l'un de ces comportements est activés, on récupère la horde
        Horde horde = null;
        if (On(behavior_type.allignment) || On(behavior_type.cohesion))
        {
            horde = gameObject.GetComponent<HordeMemberComponent>().getHorde();
        }

        if (On(behavior_type.evade))
        {
            //vérif si target1 set
            if (m_objTarget1 == null)
                throw new System.Exception(System.Reflection.MethodBase.GetCurrentMethod().Name + " (in object " + gameObject.name + ") : Cible d'évasion non définie");
            fleeVec = Evade(m_objTarget1) * m_dWeightEvade;
            m_vSteeringPos = fleeVec ;
        }
        else if (On(behavior_type.wander))
        {
            wanderVec = Wander() * m_dWeightWander;
            m_vSteeringPos = wanderVec;
        }
        else if (On(behavior_type.seek))
        {
            seekVec = Seek(seekPos) * m_dWeightSeek;
            m_vSteeringPos = seekVec;
        }
        else if (On(behavior_type.flee))
        {
            fleeVec = Flee(fleePos) * m_dWeightFlee;
            m_vSteeringPos = fleeVec;
        }
        else if (On(behavior_type.pursuit))
        {
            //vérif si target1 set
            if (m_objTarget == null)
                throw new System.Exception(System.Reflection.MethodBase.GetCurrentMethod().Name + " (in object " + gameObject.name + ") : Cible de poursuite non définie");

            pursuitVec = Pursuit(m_objTarget) * m_dWeightPursuit;
            m_vSteeringPos = pursuitVec;
        }
        else if (On(behavior_type.offset_pursuit))
        {
            //vérif si target1 set
            if (m_objTarget1 == null)
                throw new System.Exception(System.Reflection.MethodBase.GetCurrentMethod().Name + " (in object " + gameObject.name + ") : Cible de poursuite non définie");
            //vérif si target2 set
            if (m_vOffset == Vector2.zero)
                throw new System.Exception(System.Reflection.MethodBase.GetCurrentMethod().Name + " (in object " + gameObject.name + ") : Offset nul");

            m_vSteeringPos = OffsetPursuit(m_objTarget1, m_vOffset) * m_dWeightOffsetPursuit;
        }
        else if (On(behavior_type.interpose))
        {
            //vérif si target1 set
            if (m_objTarget1 == null)
                throw new System.Exception(System.Reflection.MethodBase.GetCurrentMethod().Name + " (in object " + gameObject.name + ") : Cible d'interposition 1 non définie");
            //vérif si target2 set
            if (m_objTarget2 == null)
                throw new System.Exception(System.Reflection.MethodBase.GetCurrentMethod().Name + " (in object " + gameObject.name + ") : Cible d'interposition 2 non définie");

            m_vSteeringPos = Interpose(m_objTarget1, m_objTarget2) * m_dWeightInterpose;
        }
        else if (On(behavior_type.hide))
        {
            //vérif si target1 set
            if (m_objTarget1 == null)
                throw new System.Exception(System.Reflection.MethodBase.GetCurrentMethod().Name + " (in object " + gameObject.name + ") : Cible de dissmulation non définie");
            //On récupère les obstacles de la carte
            List<GameObject> obstacles = new List<GameObject>();
            m_vSteeringPos = Hide(m_objTarget1, obstacles) * m_dWeightHide;
        }

        //AJUSTEMENT FLOCKING
        /*
         A ADAPTER
         */

        /*if (On(behavior_type.separation))
        {
            //On récupère les voisins
            List<GameObject> neighbours = gameObject.transform.Find("NeighboursTrigger").gameObject.GetComponent<NeighboursSystem>().getNeighboursList();
            separationVec = Separation(neighbours) * m_dWeightSeparation;
            m_vSteeringPos = separationVec;
        }
        if (On(behavior_type.allignment))
        {
            if (horde != null)
                alignmentVec = Alignment(horde) * m_dWeightAlignment;
            m_vSteeringPos = alignmentVec;
        }
        if (On(behavior_type.cohesion))
        {
            if (horde != null)
                cohesionVec = Cohesion(horde) * m_dWeightCohesion;
            m_vSteeringPos = cohesionVec;
        }*/
    }

    // Start is called before the first frame update
    [ServerCallback]
    void Start()
    {

            m_iFlags = 0;

            EnnemiParams paramsScript = EnnemiParams.Instance;

            m_SteeringUpdateCooldown = paramsScript.SteeringUpdateCooldown;
            m_SteeringUpdateCurrentCooldown = 0;

            m_dWanderJitterElapsedTime = 0;
            m_dWanderRadius = paramsScript.WanderRadius;
            m_dWanderJitter = paramsScript.WanderJitter;

            m_dWeightSeparation = paramsScript.WeightSeparation;
            m_dWeightCohesion = paramsScript.WeightCohesion;
            m_dWeightAlignment = paramsScript.WeightAlignment;
            m_dWeightWander = paramsScript.WeightWander;
            m_dWeightSeek = paramsScript.WeightSeek;
            m_dWeightFlee = paramsScript.WeightFlee;
            m_dWeightPursuit = paramsScript.WeightPursuit;
            m_dWeightOffsetPursuit = paramsScript.WeightOffsetPursuit;
            m_dWeightInterpose = paramsScript.WeightInterpose;
            m_dWeightHide = paramsScript.WeightHide;
            m_dWeightEvade = paramsScript.WeightEvade;


    }

    // Update is called once per frame
    [ServerCallback]
    void Update()
    {

            m_dWanderJitterElapsedTime += Time.deltaTime;

        m_SteeringUpdateCurrentCooldown += Time.deltaTime;
        if (m_SteeringUpdateCurrentCooldown >= m_SteeringUpdateCooldown)
        {
            m_SteeringUpdateCurrentCooldown = 0;
            Calculate();

            if (gameObject.GetComponent<NavMeshAgent>().enabled)
            {
                Debug.Log("Velocity of " + this.gameObject.name + " : " + gameObject.GetComponent<NavMeshAgent>().velocity.magnitude);
                gameObject.GetComponent<NavMeshAgent>().SetDestination(SteeringPos());
            }
                
        }
    }
}
