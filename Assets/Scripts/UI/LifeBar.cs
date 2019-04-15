
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    // Start is called before the first frame update
    
    private Image imageTransition;
    private Image imageFilled;

    [SerializeField]
    private uint level = 100;

    public uint GetLevel()
    {
        return level;
    }

    public void SetLevel(uint level)
    {
        this.name = level.ToString();
        if (level < this.level)
        {
            SubLevel(this.level - level);
           
        }
        else{
            SetExactLevel(level);
            //AddLevel(level - this.level); May enhance a little the performances
        }
    }

    public void SetExactLevel(uint lev)
    {
        this.level = lev;
        imageFilled.fillAmount = Mathf.Clamp( (lev / 100), 0, 1);
        imageTransition.fillAmount = Mathf.Clamp((lev / 100), 0, 1);

    }

    public void AddLevel(uint level)
    {
        this.level += level;
        imageFilled.fillAmount = Mathf.Clamp(imageFilled.fillAmount + ((float)level / 100f),0,1);
        imageTransition.fillAmount = imageFilled.fillAmount;
    }

    public void SubLevel(uint level)
    {
        this.level -= level;
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
