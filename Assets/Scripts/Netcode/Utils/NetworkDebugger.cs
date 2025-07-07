using Unity.Netcode;
using UnityEngine;
using TMPro;

namespace NetcodeTutorial.Utils
{
    public class NetworkDebugger : MonoBehaviour
    {
        [Header("Debug UI")]
        [SerializeField] private TextMeshProUGUI debugText;
        [SerializeField] private bool showDebugInfo = true;
        
        [Header("Network Info")]
        [SerializeField] private bool showConnectionInfo = true;
        [SerializeField] private bool showPlayerInfo = true;
        [SerializeField] private bool showNetworkStats = true;
        
        private NetworkManager networkManager;
        private float updateInterval = 0.5f;
        private float lastUpdateTime;
        
        private void Start()
        {
            networkManager = NetworkManager.Singleton;
        }
        
        private void Update()
        {
            if (!showDebugInfo || networkManager == null) return;
            
            if (Time.time - lastUpdateTime >= updateInterval)
            {
                UpdateDebugInfo();
                lastUpdateTime = Time.time;
            }
        }
        
        private void UpdateDebugInfo()
        {
            if (debugText == null) return;
            
            string debugInfo = "";
            
            if (showConnectionInfo)
            {
                debugInfo += GetConnectionInfo();
            }
            
            if (showPlayerInfo)
            {
                debugInfo += GetPlayerInfo();
            }
            
            if (showNetworkStats)
            {
                debugInfo += GetNetworkStats();
            }
            
            debugText.text = debugInfo;
        }
        
        private string GetConnectionInfo()
        {
            string info = "=== CONNECTION INFO ===\n";
            info += $"IsListening: {networkManager.IsListening}\n";
            info += $"IsHost: {networkManager.IsHost}\n";
            info += $"IsServer: {networkManager.IsServer}\n";
            info += $"IsClient: {networkManager.IsClient}\n";
            info += $"IsConnectedClient: {networkManager.IsConnectedClient}\n";
            info += $"LocalClientId: {networkManager.LocalClientId}\n";

            if (networkManager.IsServer)
            {
                info += $"ConnectedClientsCount: {networkManager.ConnectedClientsIds.Count}\n";
            }
            else
            {
                info += $"ConnectedClientsCount: (Chỉ xem được trên server)\n";
            }
            
            info += "\n";
            return info;
        }
        
        private string GetPlayerInfo()
        {
            string info = "=== PLAYER INFO ===\n";
            if (networkManager.IsServer)
            {
                foreach (ulong clientId in networkManager.ConnectedClientsIds)
                {
                    var client = networkManager.ConnectedClients[clientId];
                    info += $"Client {clientId}:\n";
                    info += $"  - PlayerObject: {(client.PlayerObject != null ? client.PlayerObject.name : "None")}\n";
                    info += $"  - IsLocalPlayer: {clientId == networkManager.LocalClientId}\n";
                    info += $"  - IsHost: {clientId == 0}\n";
                }
            }
            else
            {
                info += "Chỉ server mới xem được danh sách player.\n";
            }
            info += "\n";
            return info;
        }
        
        private string GetNetworkStats()
        {
            string info = "=== NETWORK STATS ===\n";
            
            if (networkManager.NetworkConfig != null)
            {
                info += $"TickRate: {networkManager.NetworkConfig.TickRate}\n";
                info += $"ClientConnectionBufferTimeout: {networkManager.NetworkConfig.ClientConnectionBufferTimeout}\n";
                info += $"ConnectionApproval: {networkManager.NetworkConfig.ConnectionApproval}\n";
            }
            
            // NetworkTransport access removed - not available in current Netcode version
            info += "Transport: Unity Transport (default)\n";
            
            info += "\n";
            return info;
        }
        
        // Public methods for external access
        public void ToggleDebugInfo()
        {
            showDebugInfo = !showDebugInfo;
            if (!showDebugInfo && debugText != null)
            {
                debugText.text = "";
            }
        }
        
        public void LogNetworkEvent(string eventName, string details = "")
        {
            if (showDebugInfo)
            {
                Debug.Log($"[NetworkDebugger] {eventName}: {details}");
            }
        }
        
        public void LogPlayerAction(ulong clientId, string action)
        {
            LogNetworkEvent($"Player {clientId} Action", action);
        }
        
        public void LogConnectionEvent(ulong clientId, bool isConnecting)
        {
            string eventType = isConnecting ? "Connected" : "Disconnected";
            LogNetworkEvent($"Client {clientId} {eventType}", "");
        }
        
        // Console commands for testing
        [System.Serializable]
        public class DebugCommands
        {
            [ContextMenu("Log Current State")]
            public void LogCurrentState()
            {
                if (NetworkManager.Singleton != null)
                {
                    Debug.Log($"Network State: IsHost={NetworkManager.Singleton.IsHost}, " +
                             $"IsClient={NetworkManager.Singleton.IsClient}, " +
                             $"IsServer={NetworkManager.Singleton.IsServer}");
                }
            }
            
            [ContextMenu("List All Players")]
            public void ListAllPlayers()
            {
                if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer)
                {
                    Debug.Log($"Total Players: {NetworkManager.Singleton.ConnectedClientsIds.Count}");
                    foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
                    {
                        Debug.Log($"Player {clientId}: {NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject?.name ?? "No Player Object"}");
                    }
                }
                else
                {
                    Debug.Log("Chỉ server mới xem được danh sách player.");
                }
            }
        }
        
        [SerializeField] private DebugCommands debugCommands;
    }
} 