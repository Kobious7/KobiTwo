using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NetcodeTutorial.Core;

namespace NetcodeTutorial.UI
{
    public class NetworkUI : MonoBehaviour
    {
        [Header("Connection Buttons")]
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;
        [SerializeField] private Button serverButton;
        [SerializeField] private Button disconnectButton;
        
        [Header("Status Display")]
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private TextMeshProUGUI playerCountText;
        
        [Header("Connection Info")]
        [SerializeField] private TMP_InputField ipInputField;
        [SerializeField] private TMP_InputField portInputField;
        
        private GameNetworkManager networkManager;
        
        private void Start()
        {
            networkManager = GameNetworkManager.Instance;
            
            // Setup button listeners
            if (hostButton != null)
                hostButton.onClick.AddListener(OnHostButtonClicked);
            
            if (clientButton != null)
                clientButton.onClick.AddListener(OnClientButtonClicked);
            
            if (serverButton != null)
                serverButton.onClick.AddListener(OnServerButtonClicked);
            
            if (disconnectButton != null)
                disconnectButton.onClick.AddListener(OnDisconnectButtonClicked);
            
            // Setup default values
            if (ipInputField != null)
                ipInputField.text = "127.0.0.1";
            
            if (portInputField != null)
                portInputField.text = "7777";
            
            UpdateUI();
        }
        
        private void Update()
        {
            UpdateUI();
        }
        
        private void UpdateUI()
        {
            if (networkManager == null) return;
            
            // Update status text
            if (statusText != null)
            {
                if (networkManager.IsConnected)
                {
                    if (networkManager.IsHost)
                        statusText.text = "Status: Host";
                    else if (networkManager.IsServer)
                        statusText.text = "Status: Server";
                    else if (networkManager.IsClient)
                        statusText.text = "Status: Client";
                }
                else
                {
                    statusText.text = "Status: Disconnected";
                }
            }
            
            // Update player count
            if (playerCountText != null)
            {
                int playerCount = 0;
                var netMgr = Unity.Netcode.NetworkManager.Singleton;
                if (netMgr != null)
                {
                    if (netMgr.IsServer)
                    {
                        playerCount = netMgr.ConnectedClientsIds.Count;
                    }
                    else if (netMgr.IsClient)
                    {
                        // Client chỉ biết về chính nó, hoặc có thể lấy từ LocalClientId, hoặc để là 1
                        playerCount = 1;
                    }
                    // Nếu chưa kết nối, giữ playerCount = 0
                }
                playerCountText.text = $"Players: {playerCount}";
            }
            
            // Update button states
            bool isConnected = networkManager.IsConnected;
            
            if (hostButton != null)
                hostButton.interactable = !isConnected;
            
            if (clientButton != null)
                clientButton.interactable = !isConnected;
            
            if (serverButton != null)
                serverButton.interactable = !isConnected;
            
            if (disconnectButton != null)
                disconnectButton.interactable = isConnected;
        }
        
        private void OnHostButtonClicked()
        {
            Debug.Log("Host button clicked!");
            networkManager.StartHost();
        }
        
        private void OnClientButtonClicked()
        {
            Debug.Log("Client button clicked!");
            networkManager.StartClient();
        }
        
        private void OnServerButtonClicked()
        {
            Debug.Log("Server button clicked!");
            networkManager.StartServer();
        }
        
        private void OnDisconnectButtonClicked()
        {
            Debug.Log("Disconnect button clicked!");
            networkManager.Disconnect();
        }
        
        // Get IP address from input field
        public string GetIPAddress()
        {
            return ipInputField != null ? ipInputField.text : "127.0.0.1";
        }
        
        // Get port from input field
        public ushort GetPort()
        {
            if (portInputField != null && ushort.TryParse(portInputField.text, out ushort port))
            {
                return port;
            }
            return 7777;
        }
    }
} 