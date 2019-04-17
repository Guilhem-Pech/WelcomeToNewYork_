using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialLevel : MonoBehaviour
{
    [SerializeField]
    private SpecialFilledBar level;

    
  


    public void SetLevel(float count, float timeCountdown)
    {
        level.SetLevel(count, timeCountdown);
    }
    
    public void TurnOnEffect()
    {
        level.TurnOnColorEffect();
    }
    public void TurnOffEffect()
    {      
        level.TurnOffColorEffect();
    }


}
