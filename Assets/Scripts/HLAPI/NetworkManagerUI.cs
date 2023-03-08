using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField] private GameObject _labelUI;
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _clientButton;
    [SerializeField] private Button _serverButton;
    [SerializeField] private TextMeshProUGUI _playersOnServer;

    private bool _isActive = true;
     
     
    private void Start()
    {
        _hostButton.onClick.AddListener(() => StartHost());
        _clientButton.onClick.AddListener(() => ClientStart());
        _serverButton.onClick.AddListener(() => ServerStart());
        ActiveLabel(_isActive);
    }

    private void Update()
    {
        int playersOnServer = NetworkManager.singleton.numPlayers;
        _playersOnServer.text = $"Players: {playersOnServer}";
        ActiveLabel(_isActive);
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
        var go = _labelUI.GetComponentsInChildren<GameObject>();
        for (int i = 0; i < go.Length; i++)
        {
            go[i].SetActive(_isActive);
        }

        _playersOnServer.gameObject.SetActive(!_isActive);

    }
    
}
