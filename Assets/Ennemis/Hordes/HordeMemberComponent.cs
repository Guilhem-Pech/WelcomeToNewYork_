using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HordeMemberComponent : NetworkBehaviour
{
    private HordesManager hordesManager;
    private Horde m_Horde;

    // Start is called before the first frame update
    [ServerCallback]
    void Awake()
    {
       hordesManager = HordesSingleton.Instance.manager;
        m_Horde = null;
    }

    
 
    
    [ServerCallback]
    void Start()
    {
        // When code is undocumented it's usually because
        // I eventually got it to work through trial and error
        // and have no idea how it actually works. Bye.
       // if(hordesManager == null)
            hordesManager = HordesSingleton.Instance.manager;
    }

    /* Getters */
    public Horde getHorde()
    {
        return m_Horde;
    }

    //Horde
    [Server]
    public void joinHorde(Horde toJoin)
    {
        toJoin.addMember(gameObject);
        m_Horde = toJoin;
    }

    [Server]
    public void leaveCurrentHorde()
    {
        if(m_Horde != null)
        {
            m_Horde.removeMember(gameObject);
            m_Horde = null;
        }
    }

    //Neighbour events
    [Server]                                     
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
                    Horde createdHorde = hordesManager.CreateNewHorde();

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

    [Server]
    public void NeighbourExit(int neighboursLeft)
    {
        if (isServer)
        {
            if (neighboursLeft == 0)
            {
                leaveCurrentHorde();
            }
        }
       
        
    }

    [ServerCallback]
    private void OnDestroy()
    {
        if(isServer)
            leaveCurrentHorde();
    }

    [ServerCallback]
    private void OnDisable()
    {
        if(isServer)
            leaveCurrentHorde();
    }
}
