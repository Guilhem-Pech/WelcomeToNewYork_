using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class Trailer : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            this.GetComponent<PlayerController>().gameObject.SetActive(false);
            foreach (Transform child in transform)
                child.gameObject.SetActive(false);
            GameObject go = (GameObject)Instantiate(Resources.Load("Camera"));

        }

    }
}
