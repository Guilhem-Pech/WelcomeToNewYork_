using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HordesManager : Singleton<HordesManager>
{
    private Dictionary<int, Horde> hordes;
    private bool goToActivated;
    private Vector3 goToPos;
    public int currentCallNumber = 0;

    //Gestion de mode Seek
    public void GoToOn(Vector3 posToGo) { currentCallNumber++; goToActivated = true; goToPos = posToGo; }
    public void GoToOff() { goToActivated = false; }
    public bool isGoToOn() { return goToActivated; }

    // Start is called before the first frame update
    void Awake()
    {
        hordes = new Dictionary<int, Horde>();

        //GoToOn(GameObject.FindGameObjectsWithTag("Player")[0].transform.position);
        GoToOff();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Horde horde in hordesToList())
        {
            if (horde.isEmpty())
            {
                deleteHorde(horde);
            }
            else
            {
                if (isGoToOn() && currentCallNumber != 0 && currentCallNumber != horde.lastReceivedCallNumber)
                {
                    horde.lastReceivedCallNumber = currentCallNumber;
                    horde.SeekOn(goToPos);
                }

                horde.Update();
            }
        }
    }

    List<Horde> hordesToList()
    {
        List<Horde> toList = new List<Horde>();

        foreach (KeyValuePair<int,Horde> it in hordes)
        {
            toList.Add(it.Value);
;       }

        return toList;
    }

    //Method to add an entity to the Horde
    public Horde createNewHorde()
    {
        Horde newHorde = Horde.CreateInstance<Horde>();

        Debug.Log("Creating Horde n°" + newHorde.getID());
        hordes.Add(newHorde.getID(), newHorde);
        
        if (isGoToOn() && currentCallNumber != 0)
        {
            newHorde.lastReceivedCallNumber = currentCallNumber;
            newHorde.SeekOn(goToPos);
        }
        else {
            newHorde.HuntingOn();
        }
            

        return newHorde;
    }

    private void deleteHorde(Horde toDelete)
    {
        Debug.Log("Deleting Horde n°" + toDelete.getID());
        toDelete.clearHorde();
        hordes.Remove(toDelete.getID());
    }

    public Horde getNearestHordeFromPos(Vector3 pos)
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
