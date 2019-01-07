using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts {
    public class MyNetworkManager : NetworkManager
    {
        public int LocalChosenCharacter;
        public GameObject[] Characters;

        //subclass for sending network messages
        public class NetworkMessage : MessageBase
        {
            public int ChosenCharacter;
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
        {
            var message = extraMessageReader.ReadMessage<NetworkMessage>();
            int clientChosenCharacter = message.ChosenCharacter;
            Transform startPos = GetStartPosition();

            GameObject player = startPos != null ? 
                Instantiate(Characters[clientChosenCharacter], startPos.position, startPos.rotation) 
                : Instantiate(Characters[clientChosenCharacter], Vector3.zero, Quaternion.identity);
 
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
 
        public override void OnClientConnect(NetworkConnection conn)
        {
            var msg = new NetworkMessage { ChosenCharacter = LocalChosenCharacter };
            ClientScene.AddPlayer(conn, 0, msg);
        }
    }
}
