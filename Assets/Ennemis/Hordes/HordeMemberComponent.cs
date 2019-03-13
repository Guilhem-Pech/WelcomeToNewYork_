using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeMemberComponent : MonoBehaviour
{
    private HordesManager hordesManager;
    private Horde m_Horde;

    // Start is called before the first frame update
    void Awake()
    {
        hordesManager = HordesManager.Instance;
        m_Horde = null;
    }

    // Update is called once per frame
    void Update()
    {
    }

    /* Getters */
    public Horde getHorde()
    {
        return m_Horde;
    }

    //Horde
    public void joinHorde(Horde toJoin)
    {
        toJoin.addMember(gameObject);
        m_Horde = toJoin;
    }

    public void leaveCurrentHorde()
    {
        if(m_Horde != null)
        {
            m_Horde.removeMember(gameObject);
            m_Horde = null;
        }
    }

    //Neighbour events
    public void NeighbourEnter(GameObject neighbour)
    {
        Horde neighbourHorde = neighbour.GetComponent<HordeMemberComponent>().getHorde();

        //Si il n'a pas de horde
        if (m_Horde == null)
        {
            //Si son nouveau voisin est dans une horde
            if (neighbourHorde != null)
            { //Il la rejoint 
                joinHorde(neighbourHorde);
            }
            //Si son nouveau voisin n'a pas de horde non plus
            else
            { //Il crée une nouvelle horde
                Horde createdHorde = hordesManager.createNewHorde();

                joinHorde(createdHorde);
                neighbour.GetComponent<HordeMemberComponent>().joinHorde(createdHorde);
            }
        }
        //Si il est déjà dans une horde
        else
        {
            //Si son nouveau voisin est dans une horde différente et qu'elle est plus grande
            if (neighbourHorde != null 
                && neighbourHorde.getID() != m_Horde.getID()
                && neighbourHorde.size() >= m_Horde.size())
            {
                //Il informe le manager afin qu'il les merge
                leaveCurrentHorde();
                joinHorde(neighbourHorde);
            }
        }
    }

    public void NeighbourExit(int neighboursLeft)
    {
        if (neighboursLeft == 0)
        { 
            leaveCurrentHorde();
        }
        
    }

    private void OnDestroy()
    {
        leaveCurrentHorde();
    }

    private void OnDisable()
    {
        leaveCurrentHorde();
    }
}
