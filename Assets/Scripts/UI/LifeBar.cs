﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    // Start is called before the first frame update
    
    private Image imageTransition;
    private Image imageFilled;

    private uint level = 100;

    public uint GetLevel()
    {
        return level;
    }

    public void SetLevel(uint level)
    {
        if(level < this.level)
        {
            SubLevel(this.level - level);
        }
        else{
            AddLevel(level - this.level);
        }
    }


    public void AddLevel(uint level)
    {
        imageFilled.fillAmount = Mathf.Clamp(imageFilled.fillAmount + (level / 100),0,1);
        imageTransition.fillAmount = imageFilled.fillAmount;
    }

    public void SubLevel(uint level)
    {
        float diff = (float)level / 100;

        imageFilled.fillAmount = Mathf.Clamp(imageFilled.fillAmount - diff, 0, 1);
        StartCoroutine(SubTransition(.05f, imageFilled.fillAmount)); 
    }


    private IEnumerator SubTransition(float time, float level)
    {
        yield return new WaitForSeconds(.5f);

        for (float f = imageTransition.fillAmount; f>=level; f -= 0.05f)
        {
            imageTransition.fillAmount = f;
            yield return new WaitForSeconds(time);
        }

        imageTransition.fillAmount = level;
    }

    void Start()
    {
        imageFilled = this.transform.Find("Filled").GetComponent<Image>();
        imageTransition = this.transform.Find("Transition").GetComponent<Image>();
    }
}
