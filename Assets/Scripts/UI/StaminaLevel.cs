using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaLevel : MonoBehaviour
{
    private Image level;

    private void Start()
    {
        level = transform.Find("Level").GetComponent<Image>();
    }


    public void SetLevel(int level, int levelMax = 100)
    {
        float f = (float)level / (float)levelMax; 
        this.level.fillAmount =Mathf.Clamp(f, 0, 1);
    }


}
