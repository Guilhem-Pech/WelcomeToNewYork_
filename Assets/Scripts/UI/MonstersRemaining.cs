using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonstersRemaining : MonoBehaviour
{
    public Text text;
    public Color animColor = Color.red;
    public Color normalColor = Color.white;


    // Start is called before the first frame update
    void Start()
    {
        if (text == null)
            text = this.transform.Find("Text").GetComponent<Text>();
        text.text = "";
    }

   
    public void SetNumber(uint num)
    {
        if (!num.ToString().Equals(text.text))
            StartCoroutine(AnimSetNumver(num));
    }

    private IEnumerator AnimSetNumver(uint num)
    {
        text.text = num.ToString();

        float ElapsedTime = 0.0f;
        float TotalTime = 0.8f;
        while (ElapsedTime < TotalTime)
        {
            ElapsedTime += Time.deltaTime;
            text.color = Color.Lerp(animColor, normalColor, (ElapsedTime / TotalTime));
            yield return null;
        }
    }




    /*
     * text.color = animColor;
        text.text = num.ToString();
        yield return new WaitForSeconds(0.1f);
        text.color = normalColor; */

}
