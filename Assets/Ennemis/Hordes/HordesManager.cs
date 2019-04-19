using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;


public class HordesManager : NetworkBehaviour
{
    private Dictionary<int, Horde> hordes;
    private bool goToActivated;
    private Vector3 goToPos;
    public int currentCallNumber = 0;

    //Gestion de mode Seek
    public void GoToOn(Vector3 posToGo) { currentCallNumber++; goToActivated = true; goToPos = posToGo; }
    public void GoToOff() { goToActivated = false; }
    public bool IsGoToOn() { return goToActivated; }

    // Start is called before the first frame update
    [ServerCallback]
    void Awake()
    {
        hordes = new Dictionary<int, Horde>();

        //GoToOn(GameObject.FindGameObjectsWithTag("Player")[0].transform.position);
        GoToOff();
    }

    // Update is called once per frame
    [ServerCallback]
    void Update()
    {
        foreach (Horde horde in HordesToList())
        {
            if (horde.isEmpty())
            {
                DeleteHorde(horde);
            }
            else
            {
                if (IsGoToOn() && currentCallNumber != 0 && currentCallNumber != horde.lastReceivedCallNumber)
                {
                    horde.lastReceivedCallNumber = currentCallNumber;
                    horde.SeekOn(goToPos);
                }

                horde.Update();
            }
        }
    }

    [Server]
    List<Horde> HordesToList()
    {
        List<Horde> toList = new List<Horde>();

        foreach (KeyValuePair<int,Horde> it in hordes)
        {
            toList.Add(it.Value);
;       }

        return toList;
    }

    //Method to add an entity to the Horde
    [Server]
    public Horde CreateNewHorde()
    {
        
        Horde newHorde = Horde.CreateInstance<Horde>();

        //Debug.Log("Creating Horde n°" + newHorde.getID());
        hordes.Add(newHorde.getID(), newHorde);

        if (IsGoToOn() && currentCallNumber != 0)
        {
            newHorde.lastReceivedCallNumber = currentCallNumber;
            newHorde.SeekOn(goToPos);
        }
        else
        {
            newHorde.HuntingOn();
        }


        return newHorde;
    }

    [Server]
    private void DeleteHorde(Horde toDelete)
    {
      // // Debug.Log("Deleting Horde n°" + toDelete.getID());
        toDelete.clearHorde();
        hordes.Remove(toDelete.getID());
    }

    [Server]
    public Horde GetNearestHordeFromPos(Vector3 pos)
    {
        Horde nearestHorde = null;
        float nearestHordeDistance = float.MaxValue;

        foreach (KeyValuePair<int, Horde> it in hordes)
        {
            if (nearestHordeDistance > (pos - it.Value.getGlobalCenter()).magnitude)
            {
                nearestHorde = it.Value;
                nearestHordeDistance = (pos - it.Value.getGlobalCenter()).magnitude;
            }
        }

        return nearestHorde;
    }
}
