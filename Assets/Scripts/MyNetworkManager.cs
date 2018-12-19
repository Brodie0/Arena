using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager
{
    public int ChosenCharacter;
    public GameObject[] Characters;

    //subclass for sending network messages
    public class NetworkMessage : MessageBase
    {
        public int ChosenClass;
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        var message = extraMessageReader.ReadMessage<NetworkMessage>();
        int selectedClass = message.ChosenClass;
        Debug.Log("server add with message " + selectedClass);

        Transform startPos = GetStartPosition();
        Debug.Log("pos: " + startPos);

        GameObject player = startPos != null ? 
            Instantiate(Characters[ChosenCharacter], startPos.position, startPos.rotation) 
            : Instantiate(Characters[ChosenCharacter], Vector3.zero, Quaternion.identity);
 
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
 
    public override void OnClientConnect(NetworkConnection conn)
    {
        var test = new NetworkMessage {ChosenClass = ChosenCharacter};
        ClientScene.AddPlayer(conn, 0, test);
    }
 
 
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        //base.OnClientSceneChanged(conn);
    }
 
    public void btn1()
    {
        ChosenCharacter = 0;
    }
 
    public void btn2()
    {
        ChosenCharacter = 1;
    }
}
