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

    [SyncVar (hook= nameof(OnChangePseudo))]
    public string pseudo = "Player";

    private Text ReadyText;
    public String ReadyTextStr = "Prêt";
    public String NotReadyStr = "En attente";
    public String NotReadyLocalStr = "Prêt ?";
    private bool IsReady;
    public InputField field;
    public Text textPseudo;

    [SyncVar(hook = nameof(OnChangechar))]
    public int ChoosenGameplayPlayer = 0;

    private Transform parent;

    
    public NetworkIdentity choosenChar;

    private void Awake()
    {
        field = this.transform.Find("InputField").gameObject.GetComponent<InputField>();
        textPseudo = this.transform.Find("Pseudo").gameObject.GetComponent<Text>();
        GameObject canvas = GameObject.Find("Canvas");
        parent = canvas.transform.Find("LobbyMenuJoin");
        ReadyButton = this.transform.Find("ButtonReady").gameObject.GetComponent<Button>();
        ReadyText = ReadyButton.GetComponentInChildren<Text>();
    }

    public void OnChangePseudo(string newPseudo)
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

    public void OnChangechar(int newChar){ 
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
    public void CmdChangeChar(bool previous)
    {
        
        List<NetworkIdentity> Choosable = parent.gameObject.GetComponent<LobbyHUD>().GameplayPlayers;

     

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

        choosenChar = Choosable[ChoosenGameplayPlayer];

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

        networkLobbyPlayer = gameObject.GetComponent<NetworkLobbyPlayer>();
        base.OnStartClient();

        GameObject canvas = GameObject.Find("Canvas");
       
        parent = canvas.transform.Find("LobbyMenuJoin");

        this.gameObject.transform.SetParent(parent);

        RectTransform rectTransform = this.GetComponent<RectTransform>();
        int i = networkLobbyPlayer.Index;
        int round = (i / 4);
        float posX = -270 + (182 * (i % 4));
        float posY = 66 - (122 * round);


        rectTransform.localPosition = new Vector3(posX, posY, 0);

        //textPseudo.text = this.pseudo;
        
        if (!isLocalPlayer)
            ReadyButton.interactable = false;
         else 
            NotReadyStr = NotReadyLocalStr;
        
        ReadyText.text = NotReadyStr;

        CmdChangePseudo("Joueur " + networkLobbyPlayer.Index);
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
    public void CmdChangePseudo(string inputField)
    {
        SetPseudo(inputField);
    }

    private void SetPseudo(string pseudo)
    {
        this.pseudo = pseudo;
    }


    public void ClicReady()
    {
        if (!isLocalPlayer)
            return;

        CmdChangeReadyState(!networkLobbyPlayer.ReadyToBegin);
        CmdChangePseudo(field.text);
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
        
        
    }

}
