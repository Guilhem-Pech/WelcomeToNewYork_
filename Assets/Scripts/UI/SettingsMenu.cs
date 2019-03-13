using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Dropdown ResolutionDropdown;

    Resolution[] resolutions;

    //on creer une liste de largeur et hauteur
    List<int> Width = new List<int>();
    List<int> Height = new List<int>();

    //on creer une liste d options vides
    List<string> options = new List<string>();

    void Start()
    {
        //on recupere toutes les resolutions que peut supporter le PC
        resolutions = Screen.resolutions;
     

        //On clear le drop down resolution
        ResolutionDropdown.ClearOptions();


        //int currentResolutionIndex = 0;
        //on fait le tour de toutes les resolutions possible
        for (int i = 0; i < resolutions.Length; i++)
        {
         
            //on enleve toutes les resolutions en dessous de 800 * 600
            if (resolutions[i].width >= 800 && resolutions[i].height >= 600)
            {
              
                // on evite les doublons dans la liste des resolutions
                var result = Width.Exists(x => x == resolutions[i].width);
                var result1 = Height.Exists(x => x == resolutions[i].height);

                // si pas de doublons alors les inserer
                if(result == false || result1 == false)
                {
                    string option = resolutions[i].width + " x " + resolutions[i].height;
                    options.Add(option);

                    Width.Add(resolutions[i].width);
                    Height.Add(resolutions[i].height);
                }
            }
        }

        //On insere toutes les options de resolutions dans le dropdown
        ResolutionDropdown.AddOptions(options);

        //on met la resolution courante dans la valeur du drop down
        //ResolutionDropdown.value = currentResolutionIndex;
        // et on refresh pour faire apparaitre la mise a jour
        ResolutionDropdown.RefreshShownValue();
 

    } 
    
     public void SetResolutions (int resolutionIndex) {
       
        // Resolution resolution = options[resolutionIndex];
       
        Screen.SetResolution(Width[resolutionIndex], Height[resolutionIndex], Screen.fullScreen);
    }
         
    
    public void SetVolume(float volume)
    {
        Debug.Log(volume);
        audioMixer.SetFloat("volume", volume);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
