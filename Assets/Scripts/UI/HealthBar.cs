using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    public LifeBar[] bars = new LifeBar[10];


    public void SetCurLife(int life,int maxLife)
    {
       
        uint health = (uint) (life * 1000 / maxLife);
        uint filledBar = health / 100;
        uint remain = health % 100;
        for (uint i = 0; i < bars.Length; ++i)
        {
            if (i < filledBar)
            {
                bars[i].SetLevel(100);
            }
                
            else if (i == filledBar)
            {
                bars[i].SetLevel(remain);
            }
            else
            {
                bars[i].SetLevel(0);
            }
                
        }


    }

}
