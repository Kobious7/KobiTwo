using Unity.Netcode;
using UnityEngine;

namespace NetcodeTutorial.Player
{
    public class PlayerSpawner : NetworkBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private float spawnRadius = 2f;
        
        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
            }
        }
        
        public override void OnNetworkDespawn()
        {
            if (IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
            }
        }
        
        private void OnClientConnected(ulong clientId)
        {
            Debug.Log($"Client {clientId} connected! Spawning player...");
            SpawnPlayer(clientId);
        }
        
        private void OnClientDisconnected(ulong clientId)
        {
            Debug.Log($"Client {clientId} disconnected!");
        }
        
        private void SpawnPlayer(ulong clientId)
        {
            if (playerPrefab == null)
            {
                Debug.LogError("Player prefab is not assigned!");
                return;
            }
            
            // Tránh spawn trùng: chỉ spawn nếu client chưa có player object
            if (NetworkManager.Singleton.ConnectedClients.ContainsKey(clientId) &&
                NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject != null)
            {
                Debug.LogWarning($"Client {clientId} đã có player object, không spawn lại.");
                return;
            }
            
            // Get spawn position
            Vector3 spawnPosition = GetSpawnPosition();
            
            // Spawn the player
            GameObject playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();
            
            if (networkObject != null)
            {
                networkObject.SpawnAsPlayerObject(clientId);
                Debug.Log($"Player spawned for client {clientId} at position {spawnPosition}");
            }
            else
            {
                Debug.LogError("Player prefab doesn't have NetworkObject component!");
                Destroy(playerInstance);
            }
        }
        
        private Vector3 GetSpawnPosition()
        {
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                // Use random spawn point
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
                randomOffset.z = 0; // Keep on 2D plane
                return spawnPoint.position + randomOffset;
            }
            else
            {
                // Default spawn position
                Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
                randomOffset.z = 0;
                return randomOffset;
            }
        }
        
        // Manual spawn for testing
        [ServerRpc(RequireOwnership = false)]
        public void SpawnPlayerServerRpc(ulong clientId)
        {
            SpawnPlayer(clientId);
        }
    }
} 