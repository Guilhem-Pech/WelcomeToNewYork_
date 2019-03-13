using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;


//[RequireComponent(typeof(Image))]
public class Life : MonoBehaviour
{

    //[SerializeField] float startValue;
    private float fullbarValue = 10;
    private float fullbarValueRed = 10;
    private Image bar;
    private GameObject[] listBar;
    private Image[] listBarImage;
    private GameObject[] listBarRed;
    private Image[] listBarImageRed;



    // Start is called before the first frame update
    /*void Reset()
    {
        bar = GetComponent<Image>();
        
    }*/
    public void BaisserLaVie()
    {
        DisplayLife(15);
    }
    private void Start()
    {
        ListBar();
    }
    public void ListBar()
    {
        listBar =  GameObject.FindGameObjectsWithTag("UIImageBar");
        listBarRed = GameObject.FindGameObjectsWithTag("UIImageBarRed");
    }

    public void DisplayLife(float value)
    {
        DisplayLifeGreen(value);
        
        DisplayLifeRedAsync(value);
    }
    // Update is called once per frame
    public void DisplayLifeGreen(float value)
    {
       
        int lengthBar = listBar.Length;
        listBarImage = new Image[lengthBar];

        for (int i = 0; i < listBar.Length; i++)
        {
            listBarImage[i] = listBar[i].GetComponent<Image>();
           
        }

       

        //Pour les barres vertes
        for (int i = 0; i < listBarImage.Length; i++)
        {
            
            float ratio = listBarImage[i].fillAmount;
           // float restRatio;
            //Debug.Log("Value ="+value);
            if (ratio >= 0.1)
            {
                float DebutRatio = (fullbarValue * ratio);
                // Debug.Log((startValue + fullbarValue) - value);
                float clampedValue = Mathf.Clamp((fullbarValue * ratio) - value, 0, fullbarValue);
                //Debug.Log(fullbarValue);

                ratio = clampedValue / fullbarValue;
                float NouveauRatio = (fullbarValue * ratio);

                // Debug.Log("Debut" + DebutRatio + "Nouveau" + NouveauRatio);
               // restRatio = ((DebutRatio - NouveauRatio) - value) / fullbarValue;
               // Debug.Log("ratio enelever =" + (DebutRatio - NouveauRatio) / fullbarValue);
                if (ratio > 0 && ratio < 0.1)
                   // for(int x = 0; )
                    listBarImage[i].fillAmount = 0;
                else
                    listBarImage[i].fillAmount = ratio;

                
               // Debug.Log("ratio =" + listBarImage[i].fillAmount);
               // Debug.Log("enlever =" + (DebutRatio - NouveauRatio));
                if (Mathf.Abs((DebutRatio - NouveauRatio) - value) <= 0)
                {
                    break;
                }
                else
                {
                    value = Mathf.Abs((DebutRatio - NouveauRatio) - value);
                //    Debug.Log(value);
                }

            }

        }

        //Debug.Log("Sortis");
        
       

    }

    public async void DisplayLifeRedAsync(float valueRed)
    {
        await Task.Delay(500);
        //Debug.Log("Red");
        int lengthBarRed = listBarRed.Length;
        listBarImageRed = new Image[lengthBarRed];

        for (int i = 0; i < listBarRed.Length; i++)
        {
            listBarImageRed[i] = listBarRed[i].GetComponent<Image>();

        }
        //Pour les barres rouges
        for (int x = 0; x < listBarImageRed.Length; x++)
        {
            //System.Threading.Thread.Sleep(500);
            float ratioRed = listBarImageRed[x].fillAmount;
            //Debug.Log("Value =" + valueRed);
            if (ratioRed >= 0.1)
            {
                float DebutRatioRed = (fullbarValueRed * ratioRed);
                float clampedValueRed = Mathf.Clamp((fullbarValueRed * ratioRed) - valueRed, 0, fullbarValueRed);

                ratioRed = clampedValueRed / fullbarValue;
                float NouveauRatioRed = (fullbarValueRed * ratioRed);


                //Debug.Log("Clamp =" + clampedValue);
                if (ratioRed > 0 && ratioRed < 0.1)
                    listBarImageRed[x].fillAmount = 0;
                else
                    listBarImageRed[x].fillAmount = ratioRed;

                if (Mathf.Abs((DebutRatioRed - NouveauRatioRed) - valueRed) <= 0)
                {
                    break;
                }
                else
                {
                    valueRed = Mathf.Abs((DebutRatioRed - NouveauRatioRed) - valueRed);
                }

            }



        }
    }

    /*decimal ratioToDisplay;
                    for (float x = (DebutRatio - NouveauRatio); x >= 0; x--)
                    {
                        ratioToDisplay = (decimal)listBarImage[i].fillAmount;
                        ratioToDisplay = ratioToDisplay - (decimal)0.1;
                        //Debug.Log(ratioToDisplay);
                        listBarImage[i].fillAmount = (float)ratioToDisplay;
                        await Task.Delay(100);
                    }*/

    public void AddLife(float value)
    {
        int lengthBar = listBar.Length;
        listBarImage = new Image[lengthBar];
        for (int i = 0; i < listBar.Length; i++)
        {
            listBarImage[i] = listBar[i].GetComponent<Image>();
            //Debug.Log(listBarImage[i]);
        }
        int lengthBarRed = listBarRed.Length;
        listBarImageRed = new Image[lengthBarRed];

        for (int i = 0; i < listBarRed.Length; i++)
        {
            listBarImageRed[i] = listBarRed[i].GetComponent<Image>();

        }
        for (int i = (listBarImage.Length-1); i >= 0; i--)
        {
            Debug.Log(i);
            float ratio = listBarImage[i].fillAmount;
            //Debug.Log("Value ="+value);
            if (ratio <= 0.9)
            {
                float DebutRatio = (fullbarValue * ratio);
                // Debug.Log((startValue + fullbarValue) - value);
                float clampedValue = Mathf.Clamp((fullbarValue * ratio) + value, 0, fullbarValue);
                //Debug.Log(fullbarValue);

                ratio = clampedValue / fullbarValue;
                float NouveauRatio = (fullbarValue * ratio);

                // Debug.Log("Debut" + DebutRatio + "Nouveau" + NouveauRatio);

                //Debug.Log("Clamp =" + clampedValue);
               // if (ratio > 0 && ratio < 0.1)
                //    listBarImage[i].fillAmount = 0;
                //else
                    listBarImage[i].fillAmount = ratio;
                    listBarImageRed[i].fillAmount = ratio;

                // Debug.Log("ratio =" + listBarImage[i].fillAmount);
                // Debug.Log("enlever =" + (DebutRatio - NouveauRatio));
                // Debug.Log("rest =" + Mathf.Abs((DebutRatio - NouveauRatio) - value));
                if (Mathf.Abs((DebutRatio - NouveauRatio) + value) <= 0)
                {
                    break;
                }
                else
                {
                    value = Mathf.Abs((DebutRatio - NouveauRatio) + value);
                    //    Debug.Log(value);
                }

            }
            //if (ratio < 0.1)
            //   i++;


        }
        //GameObject bartest = listBar.Last();
        //Debug.Log(bartest); 
        /*float ratio = bar.fillAmount;
        if(ratio > 0 && ratio <= 1)
        {
            Debug.Log(ratio);
            // Debug.Log((startValue + fullbarValue) - value);
            float clampedValue = Mathf.Clamp((fullbarValue*ratio)-value, 0, fullbarValue);
            Debug.Log(clampedValue);
    
            ratio = clampedValue / fullbarValue;
            Debug.Log(ratio);
            bar.fillAmount = ratio;
            Debug.Log(bar);
        }else if(ratio == 0)
        {
            
        }*/

}

}

