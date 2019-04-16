﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveText : MonoBehaviour
{
    private Text text;
    public Color startedColor = new Color(255, 0, 68);
    public Color clearedColor = new Color(99, 199, 77);


    private void Start()
    {
        text = this.GetComponent<Text>();
        text.text = "";
    }

    public void SetText(string str)
    {
        text.text = "";
        StartCoroutine(AnimSetText(str));
    }

    private IEnumerator AnimSetText(string str)
    {
        for(int i=0; i < str.Length; ++i)
        {
            text.text += str[i];
            yield return new WaitForSeconds(0.08f);
        }

        yield return new WaitForSeconds(3f);

        for (Color k = text.color; k.a > 0 ; k.a -=0.05f )
        {
            text.color = k; 
            yield return new WaitForSeconds(0.03f);
        }
        text.text = "";
    }

    public void SetWaveStartedText(uint number)
    {
        text.color = startedColor;
        SetText("Wave " + number + " has Started !");
    }

    public void SetWaveEndedText(uint number)
    {
        text.color = clearedColor;
        SetText("Wave " + number + " cleared !");
    }

}
