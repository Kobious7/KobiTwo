using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NetcodeTutorial.Core
{
    public class GameNetworkManager : NetworkBehaviour
    {
        [Header("Network Settings")]
        [SerializeField] private string gameSceneName = "SampleScene";
        [SerializeField] private string lobbySceneName = "LobbyScene";
        
        [Header("Player Settings")]
        [SerializeField] private GameObject playerPrefab;
        
        public static GameNetworkManager Instance { get; private set; }
        
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                NetworkManager.Singleton.SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
            }
        }
        
        // Host a game
        public void StartHost()
        {
            NetworkManager.Singleton.StartHost();
            Debug.Log("Starting as Host...");
        }
        
        // Join as client
        public void StartClient()
        {
            NetworkManager.Singleton.StartClient();
            Debug.Log("Joining as Client...");
        }
        
        // Start server only
        public void StartServer()
        {
            NetworkManager.Singleton.StartServer();
            Debug.Log("Starting as Server...");
        }
        
        // Disconnect
        public void Disconnect()
        {
            if (NetworkManager.Singleton.IsListening)
            {
                NetworkManager.Singleton.Shutdown();
                SceneManager.LoadScene(lobbySceneName);
            }
        }
        
        // Get connection status
        public bool IsConnected => NetworkManager.Singleton.IsListening;
        public bool IsHost => NetworkManager.Singleton.IsHost;
        public bool IsClient => NetworkManager.Singleton.IsClient;
        public bool IsServer => NetworkManager.Singleton.IsServer;
    }
} 