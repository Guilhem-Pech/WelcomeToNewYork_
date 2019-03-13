using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HordesManager))]
public class HordesSingleton : Singleton<HordesSingleton>
{
    public HordesManager manager;

    private void Start()
    {
        if (manager == null)
            manager = this.GetComponent<HordesManager>();
    }
}
