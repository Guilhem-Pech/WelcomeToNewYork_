using Mirror;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class LobbyJoin : MonoBehaviour
{


    public Text ipTextBox;
    private NetworkManager manager;
    private NetworkClient connection;
    private GameObject lobbyMenu;
    public Text connecting;
    bool isFailed = false;

    public void JoinServer(GameObject LobbyMenu)
    {
        this.lobbyMenu = LobbyMenu;
        if (!IPAddress.TryParse(ipTextBox.text, out IPAddress ip) && ipTextBox.text.ToLower() != "localhost")
        {
            // TODO: Feedback IP NOT CORRECT
            return;
        }

        manager.networkAddress = ipTextBox.text;
        connection = manager.StartClient();
               
    }

    private void Start()
    {
        manager = GameObject.FindObjectOfType<NetworkManager>();

    }

    public void Update()
    {
        bool noConnection = (manager.client == null || manager.client.connection == null ||
                                 manager.client.connection.connectionId == -1);

        if (!manager.IsClientConnected() && !NetworkServer.active)
        {
            if (noConnection)
            {
                // Pas de connection en cours
                if (isFailed){
                    connecting.color = Color.red;
                    connecting.text = "Connection failed";
                    Button button = GameObject.Find("ButtonJoin").GetComponent<Button>();
                    button.interactable = true;
                    isFailed = false;
                }
            }
            else
            {
                // Connecting
                connecting.color = Color.green;
                connecting.text = "Connecting...";
                Button button = GameObject.Find("ButtonJoin").GetComponent<Button>();
                button.interactable = false;
                isFailed = true;
            }
        }
        else
        {
            // Connected 
            lobbyMenu.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
