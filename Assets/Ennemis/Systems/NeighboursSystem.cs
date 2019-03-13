using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighboursSystem : MonoBehaviour
{
    private SphereCollider neighboursCollider;
    private Dictionary<int, GameObject> currentNeighbours;
    private List<GameObject> currentNeighboursList;
    private GameObject m_parentEntity;

    // Start is called before the first frame update
    void Awake()
    {
        currentNeighboursList = new List<GameObject>();
        currentNeighbours = new Dictionary<int, GameObject>();
        m_parentEntity = gameObject.transform.parent.gameObject;
        neighboursCollider = gameObject.GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentNeighboursList = new List<GameObject>();
        List<int>  toDeleteNeighboursList = new List<int>();

        foreach (KeyValuePair<int, GameObject> neighbour in currentNeighbours)
        {
            if (neighbour.Value == null)
            {
                toDeleteNeighboursList.Add(neighbour.Key);
            }
            else
            {
                currentNeighboursList.Add(neighbour.Value);
            }
        }

        foreach (int key in toDeleteNeighboursList)
        {
            currentNeighbours.Remove(key);
        }
    }

    /* Getter */
    public bool IsNeighbour(GameObject entity)
    {
        return currentNeighbours.ContainsKey(entity.GetInstanceID());
    }

    public List<int> getNeighboursIDs()
    {
        List<int> returnList = new List<int>();

        foreach (KeyValuePair<int, GameObject> neighbour in currentNeighbours)
        {
            returnList.Add(neighbour.Value.GetInstanceID());
        }

        return returnList;
    }

    public List<GameObject> getNeighboursList()
    {
        return currentNeighboursList;
    }

    /* Triggers */
    private void OnTriggerEnter(Collider other)
    {
        GameObject collidedEntity;
        
        if (other.gameObject.transform.parent != null
            && (collidedEntity = other.gameObject.transform.parent.gameObject) != null
            && collidedEntity.tag == "ennemy"
            && collidedEntity.GetInstanceID() != m_parentEntity.GetInstanceID()
            && !IsNeighbour(collidedEntity))
        {
            m_parentEntity.SendMessage("NeighbourEnter", collidedEntity);
            currentNeighbours.Add(collidedEntity.GetInstanceID(), collidedEntity);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject collidedEntity;

        if (other.gameObject.transform.parent != null
            && (collidedEntity = other.gameObject.transform.parent.gameObject) != null
            && collidedEntity.tag == "ennemy"
            && collidedEntity.GetInstanceID() != m_parentEntity.GetInstanceID()
            && IsNeighbour(collidedEntity))
        {
            currentNeighbours.Remove(collidedEntity.GetInstanceID());
            m_parentEntity.SendMessage("NeighbourExit", currentNeighbours.Count);
        }
    }
}
