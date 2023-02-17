using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button _hostBtn;
    [SerializeField] private Button _clientBtn;
    [SerializeField] private TMP_InputField _ipField;

    private void Awake()
    {
        _hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            Destroy(gameObject);
        });

        _clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(_ipField.text, 7777);
            NetworkManager.Singleton.StartClient();
            Destroy(gameObject);
        });
    }
}
