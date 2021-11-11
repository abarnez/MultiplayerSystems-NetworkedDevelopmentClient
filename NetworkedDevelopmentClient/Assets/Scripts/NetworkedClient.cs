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
    bool isConnected = false;
    int ourClientID;
    public GameObject gameCanvas, gameroomCanvas;
    public Toggle ObserverSwitch;
    // Start is called before the first frame update
    void Start()
    {
        Connect();
        Message.text = "Player1: ";
    }

    // Update is called once per frame
    void Update()
    {
        UpdateNetworkConnection();

        if (Input.GetKeyDown(KeyCode.S))
        {
            chatMessage();
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
        if (signifier == ClientToServerChatSignifiers.GG)
        {
            changeChatMsg();
        }
        if (signifier == ClientToServerChatSignifiers.Rematch)
        {
            changeChatMsg1();
        }
        if (signifier == ClientToServerChatSignifiers.EZCLap)
        {
            changeChatMsg2();
        }
        if (signifier == ServerToClientGameSignifiers.JoinGame)
        {
            JoinRoom();
        }
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
        public const int JoinGame = 1;
        public const int JoinAsObserver = 2;
    }

    
    public static class ServerToClientGameSignifiers
    {
        public const int JoinGame = 1;
        public const int JoinAsObserver = 2;
    }
    
}
