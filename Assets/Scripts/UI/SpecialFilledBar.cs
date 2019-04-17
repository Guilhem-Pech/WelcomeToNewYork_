using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialFilledBar : MonoBehaviour
{
    private bool effectActive = false;
    private Coffee.UIExtensions.UIEffect colorAdd;
    private Image level;

    private void Start()
    {
        colorAdd = this.GetComponent<Coffee.UIExtensions.UIEffect>();
        level = this.GetComponent<Image>();
    }


   

    public void SetLevel(float count, float timeCountdown)
    {
        float f = Mathf.Clamp(1 - count / timeCountdown, 0, 1);
        this.level.fillAmount = f;
    }


    public void TurnOnColorEffect()
    {
        effectActive = true;
        StartCoroutine(EffectTransition(0.05f));
    }

    public void TurnOffColorEffect()
    {
        colorAdd.colorFactor = 0;
        effectActive = false;
    }

    private IEnumerator EffectTransition(float time)
    {
        for (float b = 0; effectActive; b += 0.25f)
        {
            colorAdd.colorFactor = Mathf.Cos(b);
            yield return new WaitForSeconds(time);
        }
    }
}
