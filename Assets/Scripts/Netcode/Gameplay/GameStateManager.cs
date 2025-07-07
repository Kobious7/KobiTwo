using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

namespace NetcodeTutorial.Gameplay
{
    public enum GameState
    {
        WaitingForPlayers,
        Playing,
        GameOver
    }
    
    public class GameStateManager : NetworkBehaviour
    {
        [Header("Game Settings")]
        [SerializeField] private int minPlayersToStart = 2;
        [SerializeField] private float gameDuration = 60f;
        
        [Header("Network Variables")]
        private NetworkVariable<GameState> networkGameState = new NetworkVariable<GameState>(GameState.WaitingForPlayers);
        private NetworkVariable<float> networkGameTime = new NetworkVariable<float>(0f);
        private NetworkVariable<int> networkPlayerCount = new NetworkVariable<int>(0);
        
        [Header("Events")]
        public System.Action<GameState> OnGameStateChangedEvent;
        public System.Action<float> OnGameTimeChangedEvent;
        public System.Action<int> OnPlayerCountChangedEvent;
        
        private float gameTimer;
        private bool gameStarted = false;
        
        public GameState CurrentGameState => networkGameState.Value;
        public float CurrentGameTime => networkGameTime.Value;
        public int CurrentPlayerCount => networkPlayerCount.Value;
        
        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                // Subscribe to client connection events
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
                
                // Update initial player count
                UpdatePlayerCount();
            }
            
            // Subscribe to network variable changes
            networkGameState.OnValueChanged += OnGameStateChanged;
            networkGameTime.OnValueChanged += OnGameTimeChanged;
            networkPlayerCount.OnValueChanged += OnPlayerCountChanged;
        }
        
        public override void OnNetworkDespawn()
        {
            if (IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
            }
            
            networkGameState.OnValueChanged -= OnGameStateChanged;
            networkGameTime.OnValueChanged -= OnGameTimeChanged;
            networkPlayerCount.OnValueChanged -= OnPlayerCountChanged;
        }
        
        private void Update()
        {
            if (!IsSpawned) return;
            if (IsServer && gameStarted)
            {
                UpdateGameTimer();
            }
        }
        
        private void OnClientConnected(ulong clientId)
        {
            UpdatePlayerCount();
            CheckGameStart();
        }
        
        private void OnClientDisconnected(ulong clientId)
        {
            UpdatePlayerCount();
            CheckGameEnd();
        }
        
        private void UpdatePlayerCount()
        {
            networkPlayerCount.Value = NetworkManager.Singleton.ConnectedClientsIds.Count;
        }
        
        private void CheckGameStart()
        {
            if (networkGameState.Value == GameState.WaitingForPlayers && 
                networkPlayerCount.Value >= minPlayersToStart)
            {
                StartGameServerRpc();
            }
        }
        
        private void CheckGameEnd()
        {
            if (networkGameState.Value == GameState.Playing && 
                networkPlayerCount.Value < minPlayersToStart)
            {
                EndGameServerRpc();
            }
        }
        
        private void UpdateGameTimer()
        {
            if (!IsSpawned) return;
            if (networkGameState.Value == GameState.Playing)
            {
                gameTimer += Time.deltaTime;
                networkGameTime.Value = gameTimer;
                
                if (gameTimer >= gameDuration)
                {
                    EndGameServerRpc();
                }
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void StartGameServerRpc()
        {
            if (networkGameState.Value == GameState.WaitingForPlayers)
            {
                networkGameState.Value = GameState.Playing;
                gameTimer = 0f;
                gameStarted = true;
                Debug.Log("Game started!");
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void EndGameServerRpc()
        {
            if (networkGameState.Value == GameState.Playing)
            {
                networkGameState.Value = GameState.GameOver;
                gameStarted = false;
                Debug.Log("Game ended!");
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void ResetGameServerRpc()
        {
            networkGameState.Value = GameState.WaitingForPlayers;
            networkGameTime.Value = 0f;
            gameTimer = 0f;
            gameStarted = false;
            Debug.Log("Game reset!");
        }
        
        private void OnGameStateChanged(GameState previousValue, GameState newValue)
        {
            Debug.Log($"Game state changed from {previousValue} to {newValue}");
            OnGameStateChangedEvent?.Invoke(newValue);
        }
        
        private void OnGameTimeChanged(float previousValue, float newValue)
        {
            OnGameTimeChangedEvent?.Invoke(newValue);
        }
        
        private void OnPlayerCountChanged(int previousValue, int newValue)
        {
            Debug.Log($"Player count changed from {previousValue} to {newValue}");
            OnPlayerCountChangedEvent?.Invoke(newValue);
        }
        
        // Public methods for UI
        public string GetGameStateText()
        {
            switch (networkGameState.Value)
            {
                case GameState.WaitingForPlayers:
                    return $"Waiting for players ({networkPlayerCount.Value}/{minPlayersToStart})";
                case GameState.Playing:
                    return $"Playing - Time: {networkGameTime.Value:F1}s";
                case GameState.GameOver:
                    return "Game Over!";
                default:
                    return "Unknown State";
            }
        }
        
        public bool CanStartGame()
        {
            return networkGameState.Value == GameState.WaitingForPlayers && 
                   networkPlayerCount.Value >= minPlayersToStart;
        }
    }
} 