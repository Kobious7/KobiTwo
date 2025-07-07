using Unity.Netcode;
using UnityEngine;
using NetcodeTutorial.Core;
using NetcodeTutorial.Player;

namespace NetcodeTutorial.Utils
{
    public class QuickTest : MonoBehaviour
    {
        [Header("Test Settings")]
        [SerializeField] private bool enableQuickTest = true;
        [SerializeField] private KeyCode hostKey = KeyCode.H;
        [SerializeField] private KeyCode clientKey = KeyCode.C;
        [SerializeField] private KeyCode disconnectKey = KeyCode.D;
        [SerializeField] private KeyCode spawnPlayerKey = KeyCode.S;
        
        private GameNetworkManager networkManager;
        
        private void Start()
        {
            if (!enableQuickTest) return;
            
            networkManager = GameNetworkManager.Instance;
            if (networkManager == null)
            {
                Debug.LogWarning("GameNetworkManager not found! Make sure it's in the scene.");
            }
        }
        
        private void Update()
        {
            if (!enableQuickTest || networkManager == null) return;
            
            // Quick test keys
            if (Input.GetKeyDown(hostKey))
            {
                Debug.Log("Quick Test: Starting Host");
                networkManager.StartHost();
            }
            
            if (Input.GetKeyDown(clientKey))
            {
                Debug.Log("Quick Test: Starting Client");
                networkManager.StartClient();
            }
            
            if (Input.GetKeyDown(disconnectKey))
            {
                Debug.Log("Quick Test: Disconnecting");
                networkManager.Disconnect();
            }
            
            if (Input.GetKeyDown(spawnPlayerKey))
            {
                Debug.Log("Quick Test: Manual Player Spawn");
                SpawnPlayerManually();
            }
            
            // Additional test keys
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                TestNetworkVariables();
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                TestRPCs();
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                TestPlayerInfo();
            }
        }
        
        private void SpawnPlayerManually()
        {
            if (!NetworkManager.Singleton.IsServer) return;
            
            // Find PlayerSpawner in scene
            PlayerSpawner spawner = FindObjectOfType<PlayerSpawner>();
            if (spawner != null)
            {
                // Spawn for local client
                spawner.SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
            }
            else
            {
                Debug.LogWarning("PlayerSpawner not found in scene!");
            }
        }
        
        private void TestNetworkVariables()
        {
            Debug.Log("=== Testing Network Variables ===");
            
            // Find all NetworkPlayers in scene
            NetcodeTutorial.Player.NetworkPlayer[] players = FindObjectsOfType<NetcodeTutorial.Player.NetworkPlayer>();
            Debug.Log($"Found {players.Length} NetworkPlayers");
            
            foreach (var player in players)
            {
                Debug.Log($"Player {player.OwnerClientId}: {player.GetPlayerInfo()}");
            }
        }
        
        private void TestRPCs()
        {
            Debug.Log("=== Testing RPCs ===");
            
            // Find local player
            NetcodeTutorial.Player.NetworkPlayer localPlayer = null;
            NetcodeTutorial.Player.NetworkPlayer[] players = FindObjectsOfType<NetcodeTutorial.Player.NetworkPlayer>();
            
            foreach (var player in players)
            {
                if (player.IsOwner)
                {
                    localPlayer = player;
                    break;
                }
            }
            
            if (localPlayer != null)
            {
                Debug.Log($"Local player found: {localPlayer.GetPlayerInfo()}");
                // You can add RPC test calls here
            }
            else
            {
                Debug.Log("No local player found!");
            }
        }
        
        private void TestPlayerInfo()
        {
            Debug.Log("=== Testing Player Info ===");
            
            if (NetworkManager.Singleton != null)
            {
                Debug.Log($"NetworkManager State:");
                Debug.Log($"  - IsListening: {NetworkManager.Singleton.IsListening}");
                Debug.Log($"  - IsHost: {NetworkManager.Singleton.IsHost}");
                Debug.Log($"  - IsServer: {NetworkManager.Singleton.IsServer}");
                Debug.Log($"  - IsClient: {NetworkManager.Singleton.IsClient}");
                Debug.Log($"  - LocalClientId: {NetworkManager.Singleton.LocalClientId}");
                Debug.Log($"  - Connected Clients: {NetworkManager.Singleton.ConnectedClientsIds.Count}");
                
                foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
                {
                    var client = NetworkManager.Singleton.ConnectedClients[clientId];
                    Debug.Log($"  - Client {clientId}: {(client.PlayerObject != null ? client.PlayerObject.name : "No Player Object")}");
                }
            }
        }
        
        // Public methods for UI buttons
        [ContextMenu("Quick Host")]
        public void QuickHost()
        {
            if (networkManager != null)
            {
                networkManager.StartHost();
            }
        }
        
        [ContextMenu("Quick Client")]
        public void QuickClient()
        {
            if (networkManager != null)
            {
                networkManager.StartClient();
            }
        }
        
        [ContextMenu("Quick Disconnect")]
        public void QuickDisconnect()
        {
            if (networkManager != null)
            {
                networkManager.Disconnect();
            }
        }
        
        [ContextMenu("Run All Tests")]
        public void RunAllTests()
        {
            TestNetworkVariables();
            TestRPCs();
            TestPlayerInfo();
        }
        
        // OnGUI for quick debug info
        private void OnGUI()
        {
            if (!enableQuickTest) return;
            
            GUILayout.BeginArea(new Rect(10, 10, 300, 200));
            GUILayout.Label("=== Quick Test Controls ===");
            GUILayout.Label($"H: Host | C: Client | D: Disconnect | S: Spawn Player");
            GUILayout.Label($"1: Test Network Variables | 2: Test RPCs | 3: Test Player Info");
            
            if (NetworkManager.Singleton != null)
            {
                GUILayout.Label($"Status: {(NetworkManager.Singleton.IsListening ? "Connected" : "Disconnected")}");
                if (NetworkManager.Singleton.IsServer)
                {
                    GUILayout.Label($"Players: {NetworkManager.Singleton.ConnectedClientsIds.Count}");
                }
            }
            
            GUILayout.EndArea();
        }
    }
} 