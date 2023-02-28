using System.Collections.Generic;
using Characters;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Main
{
    public class SolarSystemNetworkManager : NetworkManager
    {
        [SerializeField] private InputField _inputField;
        [SerializeField] private GameObject _labelUI;
        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _clientButton;
        [SerializeField] private Button _serverButton;
        [SerializeField] private TextMeshProUGUI _playersOnServer;
        Dictionary<int, ShipController> _players = new Dictionary<int, ShipController>();

        private bool _isActive = true;


        private void Start()
        {
            _hostButton.onClick.AddListener(() => StartHost());
            _clientButton.onClick.AddListener(() => ClientStart());
            _serverButton.onClick.AddListener(() => ServerStart());
        }

        private void Update()
        {
            int playersOnServer = NetworkManager.singleton.numPlayers;
            _playersOnServer.text = $"Players: {playersOnServer}";
            ActiveLabel(_isActive);
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            var spawnTransform = GetStartPosition();
            var player = Instantiate(playerPrefab, spawnTransform.position, spawnTransform.rotation);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }

        private void StartHost()
        {
            NetworkManager.singleton.StartHost();
            _isActive = false;

        }

        private void ClientStart()
        {
            NetworkManager.singleton.StartClient();
            _isActive = false;
        }

        private void ServerStart()
        {
            NetworkManager.singleton.StartServer();
            _isActive = false;
        }

        private void ActiveLabel(bool _isActive)
        {
            _labelUI.SetActive(_isActive);
            _playersOnServer.gameObject.SetActive(!_isActive);
            _inputField.gameObject.SetActive(_isActive);

        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            NetworkServer.RegisterHandler(100, ReceiveName);
        }

        public class MessageLogin : MessageBase
        {
            public string login;
            public override void Deserialize(NetworkReader reader)
            {
                login = reader.ReadString();
            }

            public override void Serialize(NetworkWriter writer)
            {
                writer.Write(login);
            }
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
            MessageLogin _login = new MessageLogin();
            _login.login = _inputField.text;
            conn.Send(100, _login);
        }

        public void ReceiveName(NetworkMessage networkMessage)
        {
            _players[networkMessage.conn.connectionId].PlayerName = networkMessage.reader.ReadString();
            _players[networkMessage.conn.connectionId].gameObject.name = _players[networkMessage.conn.connectionId].PlayerName;
            Debug.Log(_players[networkMessage.conn.connectionId]);
        }
    }
}
