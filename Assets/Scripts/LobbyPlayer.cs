using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class LobbyPlayer : NetworkBehaviour
{
    private NetworkLobbyPlayer networkLobbyPlayer;
    public Button ReadyButton;

    [SyncVar (hook="OnChangePseudo")]
    public string pseudo;

    private Text ReadyText;
    public String ReadyTextStr = "Prêt";
    public String NotReadyStr = "En attente";
    public String NotReadyLocalStr = "Prêt ?";
    private bool IsReady;
    private InputField field;
    private Text textPseudo;

    [SyncVar(hook = "OnChangechar")]
    private int ChoosenGameplayPlayer = 0;

    private Transform parent;

    
    private NetworkIdentity ChoosenChar;

    private void OnChangePseudo(String newPseudo)
    {
        textPseudo.text = newPseudo;
        field.text = newPseudo;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        this.transform.Find("ButtonsChangeSkin").gameObject.SetActive(true);
        this.transform.Find("InputField").gameObject.SetActive(true);
        this.transform.Find("Pseudo").gameObject.SetActive(false);
        CmdChangeChar(true);
    }

    private void OnChangechar(int newChar){
       Image sprite = transform.Find("clothes").GetComponent<Image>();
       List<NetworkIdentity> Choosable = parent.gameObject.GetComponent<LobbyHUD>().GameplayPlayers;
       sprite.sprite = Choosable[newChar].gameObject.GetComponent<PlayerSprite>().PlayerLobbySprite;
    }

    public void ClicChange(bool isPrevious)
    {
        if (isLocalPlayer)
            CmdChangeChar(isPrevious);
    }

    [Command]
    private void CmdChangeChar(bool previous)
    {
        print("Do you want to RP");
        List<NetworkIdentity> Choosable = parent.gameObject.GetComponent<LobbyHUD>().GameplayPlayers;

        foreach (NetworkIdentity i in Choosable)
            print(i.name);

        if (previous)
        {
            if (ChoosenGameplayPlayer <= 0)
                ChoosenGameplayPlayer = Choosable.Count - 1;
            else --ChoosenGameplayPlayer;
        }
        else
        {
            if (ChoosenGameplayPlayer >= Choosable.Count - 1)
                ChoosenGameplayPlayer = 0;
            else ++ChoosenGameplayPlayer;
        }
        
    }


    private static void TurnRed(Button button)
    {
        
        ColorBlock colors = button.colors;
        colors.normalColor = Color.red;
        colors.highlightedColor = new Color32(255, 100, 100, 255);
        button.colors = colors;
    }

    private static void TurnGreen(Button button)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = Color.green;
        colors.highlightedColor = new Color32(10, 225, 10, 255);
        button.colors = colors;
    }

    private void Start()
    {
        field = this.transform.Find("InputField").gameObject.GetComponent<InputField>();
        textPseudo = this.transform.Find("Pseudo").gameObject.GetComponent<Text>();
        GameObject canvas = GameObject.Find("Canvas");
        parent = canvas.transform.Find("LobbyMenuHost");

        if (isServer)
            this.pseudo = "Joueur " + networkLobbyPlayer.Index;
        textPseudo.text = this.pseudo;
        networkLobbyPlayer = this.GetComponent<NetworkLobbyPlayer>();
        ReadyText = ReadyButton.GetComponentInChildren<Text>(); 
        if (!isLocalPlayer)
            ReadyButton.interactable = false;
         else 
            NotReadyStr = NotReadyLocalStr;
        
        ReadyText.text = NotReadyStr; 
    }


    [Command]
    public void CmdChangeReadyState(bool ReadyState)
    {
        networkLobbyPlayer.ReadyToBegin = ReadyState;
        var lobby = NetworkManager.singleton as NetworkLobbyManager;
        if (lobby)
            lobby.ReadyStatusChanged();
    }

    [Command]
    private void CmdChangePseudo(string inputField)
    {
        pseudo = inputField;
    }

   

    public void ClicReady()
    {
        if (!isLocalPlayer)
            return;

        CmdChangeReadyState(!networkLobbyPlayer.ReadyToBegin);
       // ReadyButton.colors = networkLobbyPlayer.ReadyToBegin ? ReadyColor : NotReadyColor;

    }


    private void Update()
    {
       if (isClient && IsReady != networkLobbyPlayer.ReadyToBegin)
        {
            IsReady = networkLobbyPlayer.ReadyToBegin;
            ReadyText.text = IsReady ? ReadyTextStr : NotReadyStr;
            if (IsReady)
            {
                
                TurnGreen(ReadyButton);
                CmdChangePseudo(field.text);

                if (isLocalPlayer)
                { 
                    field.gameObject.SetActive(false);                
                    textPseudo.gameObject.SetActive(true);
                    this.transform.Find("ButtonsChangeSkin").gameObject.SetActive(false);
                }
            }
            else
            {
                TurnRed(ReadyButton);
                if (isLocalPlayer)
                {
                    field.gameObject.SetActive(true);
                    textPseudo.gameObject.SetActive(false);
                    this.transform.Find("ButtonsChangeSkin").gameObject.SetActive(true);
                }
                
            }
        }
                               
        
    }

    
    public override void OnStartClient()
    {
        networkLobbyPlayer = gameObject.GetComponent<NetworkLobbyPlayer>();
        base.OnStartLocalPlayer();

        GameObject canvas = GameObject.Find("Canvas");
        if (isClientOnly)
            parent = canvas.transform.Find("LobbyMenuJoin");
        else
            parent = canvas.transform.Find("LobbyMenuHost");

        this.gameObject.transform.SetParent(parent);
 
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        int i = networkLobbyPlayer.Index;
        int round = (i / 4); 
        float posX = -270 + (182 * (i%4));
        float posY = 66 - (122 * round);  
        
      
        rectTransform.localPosition = new Vector3(posX, posY, 0);
        

    }
}
