using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialLevel : MonoBehaviour
{
    [SerializeField]
    private Image level;
    

    public void SetLevel(float count, float timeCountdown)
    {
        float f = Mathf.Clamp(1 - count / timeCountdown, 0, 1);
        this.level.fillAmount = f;
    }
    
}
