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
    private float fullbarValueStamina = 50;
    private float staminaCurrent;
    private Image bar;
    private GameObject[] listBar;
    private Image[] listBarImage;
    private Image barManaY, barManaO;
    private GameObject[] listBarRed;
    private Image[] listBarImageRed;
    private long timeStart, timeUpdate, timeStartStamina, timeUpdateStamina;
    protected GameObject Player;


    public void Update()
    {
        if(Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            return;
        }

        fullbarValueStamina = Player.GetComponentInChildren<BaseChar>().getMaxStamina();
        staminaCurrent = Player.GetComponentInChildren<BaseChar>().getStamina();
 
        BarRed t = GetLastRed();
        float nbBarreR = t.x;
        float ratioR = t.y;
        Image image = t.c;

        float nbBarreG = GetLastGreen()[0];
        float ratioG = GetLastGreen()[1];
        timeUpdate = long.Parse(GetTimestamp(DateTime.Now));
        timeUpdateStamina = long.Parse(GetTimestamp(DateTime.Now));

        // Debug.Log("ratioG " + ratioG + " ratioR " + ratioR + " BarreR " + nbBarreR + " BarreG " + nbBarreG);
        float ratioO = barManaO.fillAmount;
        float ratioY = barManaY.fillAmount;
        //ratioO = (float)Math.Round((double)ratioO, 1);
        // ratioY = (float)Math.Round((double)ratioY, 1);

        //on verifie si la barre rouge est egal a la barre verte
        if ((ratioR != ratioG || nbBarreR < nbBarreG) && timeStart <= timeUpdate)
        {

            float ratio = image.fillAmount - (float)0.1;
            ratio = (float)Math.Round((double)ratio, 1);
            image.fillAmount = ratio;
            timeStart = timeUpdate + 400;
        }
        //On verifie pour la stamina
        // si le ratio du orange est superieur au ratio du jaune
        if (ratioO >= ratioY && timeStartStamina <= timeUpdateStamina)
        {
            float ratioB = barManaO.fillAmount - (float)0.01;
            //ratioB = (float)Math.Round((double)ratioB, 1);

            barManaO.fillAmount = ratioB;
            timeStartStamina = timeUpdateStamina + 500;
        }

        //Si le ratio orange et jaune est pas le meme que au max
        // alors on augmente petit a petit jusquau max
        
        //ratioB = (float)Math.Round((double)ratioB, 1);
        if((staminaCurrent / fullbarValueStamina) != barManaY.fillAmount)
        {
            barManaO.fillAmount = (staminaCurrent / fullbarValueStamina);
            barManaY.fillAmount = (staminaCurrent / fullbarValueStamina);
        }
        
        /*if (ratioY <= (fullbarValueStamina/100))
        {
            float ratioB = barManaO.fillAmount - (float)0.01;
            //ratioB = (float)Math.Round((double)ratioB, 1);

            barManaO.fillAmount = ratioB;
            timeStartStamina = timeUpdateStamina + 500;
        }*/



    }
    public void BaisserLaVie()
    {

        DisplayLife(50, 250);
    }
    private void Start()
    {
        
        
        
        
        // fullbarValueStamina = 100;
        InitialiseObjectLife();

        timeStart = long.Parse(GetTimestamp(DateTime.Now));
        timeStartStamina = long.Parse(GetTimestamp(DateTime.Now));
    }

    public void InitialiseObjectLife()
    {
        listBar = GameObject.FindGameObjectsWithTag("UIImageBar");
        listBarRed = GameObject.FindGameObjectsWithTag("UIImageBarRed");
        barManaY = GameObject.FindGameObjectWithTag("UIImageStaminaYellow").GetComponent<Image>();
        barManaO = GameObject.FindGameObjectWithTag("UIImageStaminaOrange").GetComponent<Image>();
    }

    public void DisplayLife(float value, float lifeMax)
    {


        fullbarValue = lifeMax / listBar.Length;
        fullbarValueRed = lifeMax / listBar.Length;
        DisplayLifeGreen(value);

    }

    public List<float> GetLastGreen()
    {
        var retList = new List<float>();

        int lengthBar = listBar.Length;
        listBarImage = new Image[lengthBar];

        for (int i = 0; i < lengthBar; i++)
        {
            listBarImage[i] = listBar[i].GetComponent<Image>();
        }
        //Pour toutes les barres vertes
        for (int i = 0; i < listBarImage.Length; i++)
        {
            //On recupere le ratio de la derniere barre
            float ratio = listBarImage[i].fillAmount;
            ratio = (float)Math.Round((double)ratio, 1);
            // Si elle est superieur a 0.1
            if (ratio >= 0.1)
            {
                retList.Add(i);
                retList.Add(ratio);
                return retList;
            }
        }
        retList.Add(listBarImage.Length);
        retList.Add(0);
        return retList;
    }


    public struct BarRed
    {
        public float x, y;
        public Image c;

        public BarRed(float x, float y, Image i)
        {
            this.x = x;
            this.y = y;
            c = i;
        }
    }

    public BarRed GetLastRed()
    {
        //ArrayList retList = new ArrayList();

        int lengthBar = listBarRed.Length;
        listBarImageRed = new Image[lengthBar];

        for (int i = 0; i < lengthBar; i++)
        {
            listBarImageRed[i] = listBarRed[i].GetComponent<Image>();
        }
        //Pour toutes les barres vertes
        for (int i = 0; i < listBarImageRed.Length; i++)
        {
            //On recupere le ratio de la derniere barre
            float ratio = listBarImageRed[i].fillAmount;
            ratio = (float)Math.Round((double)ratio, 1);
            // Si elle est superieur a 0.1
            if (ratio >= 0.1)
            {

                return new BarRed(i, ratio, listBarImageRed[i]);
            }
        }
        return new BarRed(0, 0, null);

    }

    // Update is called once per frame
    public void DisplayLifeGreen(float value)
    {
        //On recupere combien on possede barre vertes
        int lengthBar = listBar.Length;
        listBarImage = new Image[lengthBar];

        for (int i = 0; i < listBar.Length; i++)
        {
            listBarImage[i] = listBar[i].GetComponent<Image>();
        }
        //Pour toutes les barres vertes
        for (int i = 0; i < listBarImage.Length; i++)
        {
            //On recupere le ratio de la derniere barre
            float ratio = listBarImage[i].fillAmount;
            ratio = (float)Math.Round((double)ratio, 1);
            // Si elle est superieur a 0.1
            if (ratio >= 0.1)
            {
                //recupere le nombre de vie que possede la barre au debut
                float DebutRatio = (fullbarValue * ratio);
                DebutRatio = (float)Math.Round((double)DebutRatio, 1);
                float clampedValue = Mathf.Clamp((fullbarValue * ratio) - value, 0, fullbarValue);
                //On recupere le nouveau ratio
                ratio = clampedValue / fullbarValue;
                ratio = (float)Math.Round((double)ratio, 1);


                //recupere le nombre de vie que possede la barre apres avoir enlever le dommage
                float NouveauRatio = (fullbarValue * ratio);
                NouveauRatio = (float)Math.Round((double)NouveauRatio, 1);
                // Debug.Log(DebutRatio + " ok " + NouveauRatio);
                //Si le ratio de maintenant est egal a 0 alors on met la derniere barre a 0
                if (ratio > 0 && ratio < 0.1)
                {
                    listBarImage[i].fillAmount = 0;
                }
                else
                //Si le ratio de maintenant est superieur a 0 alors on met la derniere barre au ratio correspondant
                {
                    listBarImage[i].fillAmount = ratio;
                }
                //Si on a vider toute la value que l on voulait enlever aors on sort de la boucle
                if (Mathf.Abs((DebutRatio - NouveauRatio) - value) <= 0)
                {

                    break;
                }
                else
                //Sinon on continue jusqua enlever tout les dommage
                {
                    value = Mathf.Abs((DebutRatio - NouveauRatio) - value);
                }
            }
        }
    }


    // public async Task<string> DisplayLifeRedAsync(float valueRed)
    //     {
    // 
    //         //On recupere combien on possede barre rouge
    //         int lengthBarRed = listBarRed.Length;
    //         listBarImageRed = new Image[lengthBarRed];
    // 
    //         for (int i = 0; i < listBarRed.Length; i++)
    //         {
    //             listBarImageRed[i] = listBarRed[i].GetComponent<Image>();
    //         }
    //         //Pour toutes les barres rouges
    //         for (int x = 0; x < listBarImageRed.Length; x++)
    //         {
    //             //On recupere le ratio de la derniere barre
    //             float ratioRed = listBarImageRed[x].fillAmount;
    //             ratioRed = (float)Math.Round((double)ratioRed, 1);
    //             // Si elle est superieur a 0.1
    //             if (ratioRed >= 0.1)
    //             {
    //                 //recupere le nombre de vie que possede la barre au debut
    //                 float DebutRatioRed = (fullbarValueRed * ratioRed);
    //                 DebutRatioRed = (float)Math.Round((double)DebutRatioRed, 1);
    //                 float clampedValueRed = Mathf.Clamp((fullbarValueRed * ratioRed) - valueRed, 0, fullbarValueRed);
    // 
    //                 ratioRed = clampedValueRed / fullbarValue;
    //                 ratioRed = (float)Math.Round((double)ratioRed, 1);
    //                 float NouveauRatioRed = (fullbarValueRed * ratioRed);
    //                 NouveauRatioRed = (float)Math.Round((double)NouveauRatioRed, 1);
    //                 //Si le ratio courant passe à 0 alors on change de barre
    //                 if (ratioRed > 0 && ratioRed < 0.1)
    //                 {
    //                     listBarImageRed[x].fillAmount = 0;
    //                 }
    //                 else
    //                 {
    //                     listBarImageRed[x].fillAmount = ratioRed;
    // 
    // 
    //                 }
    // 
    //                 if (Mathf.Abs((DebutRatioRed - NouveauRatioRed) - valueRed) <= 0)
    //                 {
    //                     break;
    //                 }
    //                 else
    //                 {
    //                     valueRed = Mathf.Abs((DebutRatioRed - NouveauRatioRed) - valueRed);
    //                 }
    //             }
    // 
    //         }
    //         return ok;
    // 
    //     }

    public void AddLife(float value)
    {
        //okGo = false;
        int lengthBar = listBar.Length;
        listBarImage = new Image[lengthBar];
        for (int i = 0; i < listBar.Length; i++)
        {
            listBarImage[i] = listBar[i].GetComponent<Image>();
        }
        int lengthBarRed = listBarRed.Length;
        listBarImageRed = new Image[lengthBarRed];

        for (int i = 0; i < listBarRed.Length; i++)
        {
            listBarImageRed[i] = listBarRed[i].GetComponent<Image>();

        }
        for (int i = (listBarImage.Length - 1); i >= 0; i--)
        {
            //Debug.Log(i);
            float ratio = listBarImage[i].fillAmount;
            ratio = (float)Math.Round((double)ratio, 1);
            if (ratio <= 0.9)
            {
                float DebutRatio = (fullbarValue * ratio);
                DebutRatio = (float)Math.Round((double)DebutRatio, 1);
                float clampedValue = Mathf.Clamp((fullbarValue * ratio) + value, 0, fullbarValue);
                ratio = clampedValue / fullbarValue;
                ratio = (float)Math.Round((double)ratio, 1);
                float NouveauRatio = (fullbarValue * ratio);
                NouveauRatio = (float)Math.Round((double)NouveauRatio, 1);
                listBarImageRed[i].fillAmount = ratio;
                listBarImage[i].fillAmount = ratio;
                Debug.Log(i);
                for (int x = 0; x < i; x++)
                {
                    Debug.Log(x);
                    listBarImageRed[x].fillAmount = 0;
                }


                if (Mathf.Abs((DebutRatio - NouveauRatio) + value) <= 0)
                {
                    //Debug.Log("on sort");
                    break;
                }
                else
                {
                    // Debug.Log("on continue");
                    value = Mathf.Abs((DebutRatio - NouveauRatio) + value);
                }

            }

        }
    }

    //     public async Task<string> Slowly(Image objetBarre, float ratioCurrent, float ratioAfter)
    //     {
    // 
    //         ratioCurrent = (float)Math.Round((double)ratioCurrent, 2);
    //        
    //         int j = 0;
    // 
    //         for (float i = ratioCurrent; i >= ratioAfter; i = i - (float)0.1)
    //         {
    //          
    //             if (ratioCurrent >= ratioAfter)
    //             {
    //                 j++;
    //           
    //                 objetBarre.fillAmount -= (float)0.1;
    //                 await Task.Delay(100);
    //               
    //             }
    //             else
    //             {
    //                 j++;
    //              
    //                 objetBarre.fillAmount = ratioAfter;
    //              
    //                 return ok;
    //              
    //             }
    // 
    //         }
    // 
    //      
    //         objetBarre.fillAmount = ratioAfter;
    //       
    //     }

    public static String GetTimestamp(DateTime value)
    {
        return value.ToString("yyyyMMddHHmmssffff");
    }

    public void DisplayStamina(float value)
    {
        //Debug.Log(fullbarValueStamina);
        float ratioStaminaY = barManaY.fillAmount;
        //Debug.Log("Stamina");
        if (ratioStaminaY >= 0.1)
        {
            //Debug.Log(ratioStaminaY);
            //recupere le nombre de vie que possede la barre au debut
            float DebutRatioStamina = (fullbarValueStamina * ratioStaminaY);
            DebutRatioStamina = (float)Math.Round((double)DebutRatioStamina, 1);

            float clampedValueStamina = Mathf.Clamp((fullbarValueStamina * ratioStaminaY) - value, 0, fullbarValueStamina);

            ratioStaminaY = clampedValueStamina / fullbarValueStamina;
            // Debug.Log(ratioStaminaY);
            ratioStaminaY = (float)Math.Round((double)ratioStaminaY, 1);
            float NouveauRatioStaminaY = (fullbarValueRed * ratioStaminaY);
            NouveauRatioStaminaY = (float)Math.Round((double)NouveauRatioStaminaY, 1);
            //Si le ratio courant passe à 0 alors on change de barre
            if (ratioStaminaY > 0 && ratioStaminaY < 0.1)
            {
                barManaY.fillAmount = 0;
            }
            else
            {
                barManaY.fillAmount = ratioStaminaY;


            }

        }
    }

    public void AddStamina(float value)
    {
        // Debug.Log(fullbarValueStamina);
        float ratioStaminaY = barManaY.fillAmount;
        Debug.Log("STAMINNAAAAA");

        // Debug.Log(ratioStaminaY);
        //recupere le nombre de vie que possede la barre au debut
        float DebutRatioStamina = (fullbarValueStamina * ratioStaminaY);
        DebutRatioStamina = (float)Math.Round((double)DebutRatioStamina, 1);

        float clampedValueStamina = Mathf.Clamp((fullbarValueStamina * ratioStaminaY) + value, 0, fullbarValueStamina);

        ratioStaminaY = clampedValueStamina / fullbarValueStamina;
        Debug.Log("STAMINA RATIO "+ ratioStaminaY);
        ratioStaminaY = (float)Math.Round((double)ratioStaminaY, 1);
        float NouveauRatioStaminaY = (fullbarValueRed * ratioStaminaY);
        NouveauRatioStaminaY = (float)Math.Round((double)NouveauRatioStaminaY, 1);
        //Si le ratio courant passe à 0 alors on change de barre
        barManaY.fillAmount = ratioStaminaY;
        barManaO.fillAmount = ratioStaminaY;

    }
}


