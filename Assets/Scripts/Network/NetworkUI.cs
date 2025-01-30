using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUI : MonoBehaviour
{
    [SerializeField] Button btn_startHost;
    [SerializeField] Button btn_startClient;

    private void Awake()
    {
        btn_startClient.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });  
        btn_startHost.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
    }
}
