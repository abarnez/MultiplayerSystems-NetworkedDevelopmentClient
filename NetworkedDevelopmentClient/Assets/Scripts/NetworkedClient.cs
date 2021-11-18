using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkedClient : MonoBehaviour
{
    public Text Message, numPlayers;
    int connectionID;
    int maxConnections = 1000;
    int reliableChannelID;
    int unreliableChannelID;
    int hostID;
    int socketPort = 5491;
    byte error;
    public bool isTurn, P1, GameOver;
    bool isConnected = false;
    int ourClientID;
    public GameObject gameCanvas, gameroomCanvas, observerCanvas, playButton, replayButton, observerText;
    public Toggle ObserverSwitch, Pos1, Pos2, Pos3, Pos4, Pos5, Pos6, Pos7, Pos8, Pos9;
    public Image pos1Image, pos2Image, pos3Image, pos4Image, pos5Image, pos6Image, pos7Image, pos8Image, pos9Image;
    public Sprite circle, X;

    // Start is called before the first frame update
    void Start()
    {
        Connect();
        Message.text = "Player1: ";
        P1 = false;
              
    }

    // Update is called once per frame
    void Update()
    {
 

        UpdateNetworkConnection();

        if (isTurn)
        {
            if(!GameOver)
            playButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(false);
        }
        if (GameOver)
        {
            replayButton.SetActive(true);
        } 
        else
        {
            replayButton.SetActive(false);
        }

    }
    private void UpdateNetworkConnection()
    {
        if (isConnected)
        {
            int recHostID;
            int recConnectionID;
            int recChannelID;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recHostID, out recConnectionID, out recChannelID, recBuffer, bufferSize, out dataSize, out error);

            switch (recNetworkEvent)
            {
                case NetworkEventType.ConnectEvent:
                    Debug.Log("connected.  " + recConnectionID);
                    ourClientID = recConnectionID;
                    break;
                case NetworkEventType.DataEvent:
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    ProcessRecievedMsg(msg, recConnectionID);
                    //Debug.Log("got msg = " + msg);
                    break;
                case NetworkEventType.DisconnectEvent:
                    isConnected = false;
                    Debug.Log("disconnected.  " + recConnectionID);
                    break;
            }
        }
    }
    private void Connect()
    {

        if (!isConnected)
        {
            Debug.Log("Attempting to create connection");

            NetworkTransport.Init();

            ConnectionConfig config = new ConnectionConfig();
            reliableChannelID = config.AddChannel(QosType.Reliable);
            unreliableChannelID = config.AddChannel(QosType.Unreliable);
            HostTopology topology = new HostTopology(config, maxConnections);
            hostID = NetworkTransport.AddHost(topology, 0);
            Debug.Log("Socket open.  Host ID = " + hostID);

            connectionID = NetworkTransport.Connect(hostID, "192.168.56.1", socketPort, 0, out error); // server is local on network

            if (error == 0)
            {
                isConnected = true;

                Debug.Log("Connected, id = " + connectionID);

            }
        }
    }

    public void Disconnect()
    {
        NetworkTransport.Disconnect(hostID, connectionID, out error);
    }
    public void SendMessageToHost(string msg)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(msg);
        NetworkTransport.Send(hostID, connectionID, reliableChannelID, buffer, msg.Length * sizeof(char), out error);
    }
    private void ProcessRecievedMsg(string msg, int id)
    {
        Debug.Log("msg recieved = " + msg + ".  connection id = " + id);
        string[] csv = msg.Split(',');
        int signifier = int.Parse(csv[0]);
        if (signifier == ServerToClientChatSignifiers.GG)
        {
            changeChatMsg();
        }
        if (signifier == ServerToClientChatSignifiers.Rematch)
        {
            changeChatMsg1();
        }
        if (signifier == ServerToClientChatSignifiers.EZCLap)
        {
            changeChatMsg2();
        }
        if (signifier == ServerToClientGameSignifiers.JoinGame)
        {
            JoinRoom();        
        }
        if (signifier == ServerToClientGameSignifiers.JoinAsObserver)
        {
            JoinRoomObserver();

        }
        if (signifier == ServerToClientMoveSignifiers.Pos1)
        {
            pos1update();
            Debug.Log("sig recieve");
        }
        if (signifier == ServerToClientMoveSignifiers.Pos2)
        {
            pos2update();
            Debug.Log("sig recieve");
        }
        if (signifier == ServerToClientMoveSignifiers.Pos3)
        {
            pos3update();
            Debug.Log("sig recieve");
        }
        if (signifier == ServerToClientMoveSignifiers.Pos4)
        {
            pos4update();
            Debug.Log("sig recieve");
        }
        if (signifier == ServerToClientMoveSignifiers.Pos5)
        {
            pos5update();
            Debug.Log("sig recieve");
        }
        if (signifier == ServerToClientMoveSignifiers.Pos6)
        {
            pos6update();
            Debug.Log("sig recieve");
        }
        if (signifier == ServerToClientMoveSignifiers.Pos7)
        {
            pos7update();
            Debug.Log("sig recieve");
        }
        if (signifier == ServerToClientMoveSignifiers.Pos8)
        {
            pos8update();
            Debug.Log("sig recieve");
        }
        if (signifier == ServerToClientMoveSignifiers.Pos9)
        {
            pos9update();
            Debug.Log("sig recieve");
        }
        if (signifier == ServerToClientMoveSignifiers2.Pos1)
        {
            pos1update2();
            Debug.Log("sig recieve");
        }
        if (signifier == ServerToClientMoveSignifiers2.Pos2)
        {
            pos2update2();
            Debug.Log("sig recieve");
        }
        if (signifier == ServerToClientMoveSignifiers2.Pos3)
        {
            pos3update2();
            Debug.Log("sig recieve");
        }
        if (signifier == ServerToClientMoveSignifiers2.Pos4)
        {
            pos4update2();
            Debug.Log("sig recieve");
        }
        if (signifier == ServerToClientMoveSignifiers2.Pos5)
        {
            pos5update2();
            Debug.Log("sig recieve");
        }
        if (signifier == ServerToClientMoveSignifiers2.Pos6)
        {
            pos6update2();
            Debug.Log("sig recieve");
        }
        if (signifier == ServerToClientMoveSignifiers2.Pos7)
        {
            pos7update2();
            Debug.Log("sig recieve");
        }
        if (signifier == ServerToClientMoveSignifiers2.Pos8)
        {
            pos8update2();
            Debug.Log("sig recieve");
        }
        if (signifier == ServerToClientMoveSignifiers2.Pos9)
        {
            pos9update2();
            Debug.Log("sig recieve");
        }
        if (signifier == ServerToClientTurnSignifiers.IsMyTurn)
        {
            isTurn = true;
        
        }
        if (signifier == ServerToClientTurnSignifiers.NotMyTurn)
        {
            isTurn = false; 
        }
        if (signifier == ServerToClientXSignifiers.X)
        {
            P1 = true;
            if (P1)
            {
                pos1Image.sprite = circle;
                pos2Image.sprite = circle;
                pos3Image.sprite = circle;
                pos4Image.sprite = circle;
                pos5Image.sprite = circle;
                pos6Image.sprite = circle;
                pos7Image.sprite = circle;
                pos8Image.sprite = circle;
                pos9Image.sprite = circle;
            }
            if (!P1)
            {
                pos1Image.sprite = X;
                pos2Image.sprite = X;
                pos3Image.sprite = X;
                pos4Image.sprite = X;
                pos5Image.sprite = X;
                pos6Image.sprite = X;
                pos7Image.sprite = X;
                pos8Image.sprite = X;
                pos9Image.sprite = X;
            }
        }
        if (signifier == ServerToClientXSignifiers.O)
        {
            P1 = false;
            if (P1)
            {
                pos1Image.sprite = circle;
                pos2Image.sprite = circle;
                pos3Image.sprite = circle;
                pos4Image.sprite = circle;
                pos5Image.sprite = circle;
                pos6Image.sprite = circle;
                pos7Image.sprite = circle;
                pos8Image.sprite = circle;
                pos9Image.sprite = circle;
            }
            if (!P1)
            {
                pos1Image.sprite = X;
                pos2Image.sprite = X;
                pos3Image.sprite = X;
                pos4Image.sprite = X;
                pos5Image.sprite = X;
                pos6Image.sprite = X;
                pos7Image.sprite = X;
                pos8Image.sprite = X;
                pos9Image.sprite = X;
            }
        }
        if (signifier == ClientToServerGOSignifiers.p1Won)
        {
            GameOver = true;
        }
        if (signifier == ClientToServerGOSignifiers.p2Won)
        {
            GameOver = true;
        }
        if (signifier == ServerToClientRPSignifiers.Replay)
        {
            clearBoard();
        }
    }
    public void ReplayRequest()
    {
        SendMessageToHost(ClientToServerRPSignifiers.Replay + "," + "");
    }
    public void chatMessage()
    {
        SendMessageToHost(ClientToServerChatSignifiers.GG + "," + "player is saying gg");
    }
    public void chatMessage1()
    {
        SendMessageToHost(ClientToServerChatSignifiers.Rematch + "," + "player is saying rematch");
    }
    public void chatMessage2()
    {
        SendMessageToHost(ClientToServerChatSignifiers.EZCLap + "," + "player is saying ezclap");
    }

    public void joinGameRequest()
    {
        if (ObserverSwitch.isOn)
        {
            SendMessageToHost(ClientToServerGameSignifiers.JoinAsObserver + "," + "player is trying to join game room");
        }
        else
        {
            SendMessageToHost(ClientToServerGameSignifiers.JoinGame + "," + "player is trying to join game room");
        }
    }

    public void sendPlay()
    {
        if (Pos1.isOn && Pos1.interactable)
        {
            SendMessageToHost(ClientToServerMoveSignifiers.Pos1 + "," + "player is playing on pos1");
            Debug.Log("Sig Sent");
        }
        if (Pos2.isOn && Pos2.interactable)
        {
            SendMessageToHost(ClientToServerMoveSignifiers.Pos2 + "," + "player is playing on pos1");
            Debug.Log("Sig Sent");
        }
        if (Pos3.isOn && Pos3.interactable)
        {
            SendMessageToHost(ClientToServerMoveSignifiers.Pos3 + "," + "player is playing on pos1");
            Debug.Log("Sig Sent");
        }
        if (Pos4.isOn && Pos4.interactable)
        {
            SendMessageToHost(ClientToServerMoveSignifiers.Pos4 + "," + "player is playing on pos1");
            Debug.Log("Sig Sent");
        }
        if (Pos5.isOn && Pos5.interactable)
        {
            SendMessageToHost(ClientToServerMoveSignifiers.Pos5 + "," + "player is playing on pos1");
            Debug.Log("Sig Sent");
        }
        if (Pos6.isOn && Pos6.interactable)
        {
            SendMessageToHost(ClientToServerMoveSignifiers.Pos6 + "," + "player is playing on pos1");
            Debug.Log("Sig Sent");
        }
        if (Pos7.isOn && Pos7.interactable)
        {
            SendMessageToHost(ClientToServerMoveSignifiers.Pos7 + "," + "player is playing on pos1");
            Debug.Log("Sig Sent");
        }
        if (Pos8.isOn && Pos8.interactable)
        {
            SendMessageToHost(ClientToServerMoveSignifiers.Pos8 + "," + "player is playing on pos1");
            Debug.Log("Sig Sent");
        }
        if (Pos9.isOn && Pos9.interactable)
        {
            SendMessageToHost(ClientToServerMoveSignifiers.Pos9 + "," + "player is playing on pos1");
            Debug.Log("Sig Sent");
        }
    }

    public void changeChatMsg()
    {
        Message.text = "Player1: " + "GG";
    }
    public void changeChatMsg1()
    {
        Message.text = "Player1: " + "Rematch?";
    }
    public void changeChatMsg2()
    {
        Message.text = "Player1: " + "EZClap";
    }
    public void JoinRoom()
    {
        gameroomCanvas.SetActive(false);
        gameCanvas.SetActive(true);                     
    }

    public void JoinRoomObserver()
    {
        gameroomCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        playButton.SetActive(false);
        observerText.SetActive(true);
    }

    public void pos1update()
    {
        Pos1.group = null;
        Pos1.isOn = true;
        Pos1.interactable = false;
        pos1Image.sprite = circle;
    }
    public void pos2update()
    {
        Pos2.group = null;
        Pos2.isOn = true;
        Pos2.interactable = false;
        pos2Image.sprite = circle;
    }

    public void pos3update()
    {
        Pos3.group = null;
        Pos3.isOn = true;
        Pos3.interactable = false;
        pos3Image.sprite = circle;
    }

    public void pos4update()
    {
        Pos4.group = null;
        Pos4.isOn = true;
        Pos4.interactable = false;
        pos4Image.sprite = circle;
    }
    public void pos5update()
    {
        Pos5.group = null;
        Pos5.isOn = true;
        Pos5.interactable = false;
        pos5Image.sprite = circle;
    }

    public void pos6update()
    {
        Pos6.group = null;
        Pos6.isOn = true;
        Pos6.interactable = false;
        pos6Image.sprite = circle;
    }

    public void pos7update()
    {
        Pos7.group = null;
        Pos7.isOn = true;
        Pos7.interactable = false;
        pos7Image.sprite = circle;
    }

    public void pos8update()
    {
        Pos8.group = null;
        Pos8.isOn = true;
        Pos8.interactable = false;
        pos8Image.sprite = circle;
    }

    public void pos9update()
    {
        Pos9.group = null;
        Pos9.isOn = true;
        Pos9.interactable = false;
        pos9Image.sprite = circle;
    }

    public void pos1update2()
    {
        Pos1.group = null;
        Pos1.isOn = true;
        Pos1.interactable = false;
        pos1Image.sprite = X;

    }
    public void pos2update2()
    {
        Pos2.group = null;
        Pos2.isOn = true;
        Pos2.interactable = false;
        pos2Image.sprite = X;
    }

    public void pos3update2()
    {
        Pos3.group = null;
        Pos3.isOn = true;
        Pos3.interactable = false;
        pos3Image.sprite = X;
    }

    public void pos4update2()
    {
        Pos4.group = null;
        Pos4.isOn = true;
        Pos4.interactable = false;
        pos4Image.sprite = X;
    }
    public void pos5update2()
    {
        Pos5.group = null;
        Pos5.isOn = true;
        Pos5.interactable = false;
        pos5Image.sprite = X;
    }

    public void pos6update2()
    {
        Pos6.group = null;
        Pos6.isOn = true;
        Pos6.interactable = false;
        pos6Image.sprite = X;
    }

    public void pos7update2()
    {
        Pos7.group = null;
        Pos7.isOn = true;
        Pos7.interactable = false;
        pos7Image.sprite = X;
    }

    public void pos8update2()
    {
        Pos8.group = null;
        Pos8.isOn = true;
        Pos8.interactable = false;
        pos8Image.sprite = X;
    }

    public void pos9update2()
    {
        Pos9.group = null;
        Pos9.isOn = true;
        Pos9.interactable = false;
        pos9Image.sprite = X;
    }

    public void clearBoard()
    {
        Pos1.isOn = false;
        Pos2.isOn = false;
        Pos3.isOn = false;
        Pos4.isOn = false;
        Pos5.isOn = false;
        Pos6.isOn = false;
        Pos7.isOn = false;
        Pos8.isOn = false;
        Pos9.isOn = false;
    }

    public static class ClientToServerChatSignifiers
    {
        public const int GG = 1;
        public const int Rematch = 2;
        public const int EZCLap = 3;
    }

    public static class ServerToClientChatSignifiers
    {
        public const int GG = 1;
        public const int Rematch = 2;
        public const int EZCLap = 3;
    }

    public static class ClientToServerGameSignifiers
    {
        public const int JoinGame = 4;
        public const int JoinAsObserver = 5;
    }

    
    public static class ServerToClientGameSignifiers
    {
        public const int JoinGame = 4;
        public const int JoinAsObserver = 5;
    }

    public static class ServerToClientMoveSignifiers
    {
        public const int Pos1 = 11;
        public const int Pos2 = 12;
        public const int Pos3 = 13;
        public const int Pos4 = 14;
        public const int Pos5 = 15;
        public const int Pos6 = 16;
        public const int Pos7 = 17;
        public const int Pos8 = 18;
        public const int Pos9 = 19;       
    }

    public static class ClientToServerMoveSignifiers
    {
        public const int Pos1 = 11;
        public const int Pos2 = 12;
        public const int Pos3 = 13;
        public const int Pos4 = 14;
        public const int Pos5 = 15;
        public const int Pos6 = 16;
        public const int Pos7 = 17;
        public const int Pos8 = 18;
        public const int Pos9 = 19;
    }
    public static class ServerToClientTurnSignifiers
    {
        public const int IsMyTurn = 20;
        public const int NotMyTurn = 21;
    }
    public static class ClientToServerTurnSignifiers
    {
        public const int IsMyTurn = 20;
        public const int NotMyTurn = 21;
    }

    public static class ServerToClientMoveSignifiers2
    {
        public const int Pos1 = 22;
        public const int Pos2 = 23;
        public const int Pos3 = 24;
        public const int Pos4 = 25;
        public const int Pos5 = 26;
        public const int Pos6 = 27;
        public const int Pos7 = 28;
        public const int Pos8 = 29;
        public const int Pos9 = 30;
    }

    public static class ClientToServerMoveSignifiers2
    {
        public const int Pos1 = 22;
        public const int Pos2 = 23;
        public const int Pos3 = 24;
        public const int Pos4 = 25;
        public const int Pos5 = 26;
        public const int Pos6 = 27;
        public const int Pos7 = 28;
        public const int Pos8 = 29;
        public const int Pos9 = 30;
    }

    public static class ClientToServerXSignifiers
    {
        public const int X = 31;
        public const int O = 32;
    }
    public static class ServerToClientXSignifiers
    {
        public const int X = 31;
        public const int O = 32;
    }
    public static class ClientToServerGOSignifiers
    {
        public const int p1Won = 33;
        public const int p2Won = 34;
    }
    public static class ServerToClientGOSignifiers
    {
        public const int p1Won = 33;
        public const int p2Won = 34;
    }

    public static class ClientToServerRPSignifiers
    {
        public const int Replay = 35;
    }
    public static class ServerToClientRPSignifiers
    {
        public const int Replay = 35;
    }
}
