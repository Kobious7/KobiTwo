using Unity.Netcode;
using UnityEngine;

namespace NetcodeTutorial.Player
{
    public class NetworkPlayer : NetworkBehaviour
    {
        [Header("Player Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private SpriteRenderer playerSprite;
        
        [Header("Network Variables")]
        private NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>();
        private NetworkVariable<Color> networkColor = new NetworkVariable<Color>();
        
        private Vector3 moveInput;
        
        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                // Set random color for this player
                Color randomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                SetPlayerColorServerRpc(randomColor);
                
                Debug.Log($"Player spawned! IsOwner: {IsOwner}, IsServer: {IsServer}, IsClient: {IsClient}");
            }
            
            // Subscribe to network variable changes
            networkColor.OnValueChanged += OnColorChanged;
            networkPosition.OnValueChanged += OnPositionChanged;
        }
        
        public override void OnNetworkDespawn()
        {
            networkColor.OnValueChanged -= OnColorChanged;
            networkPosition.OnValueChanged -= OnPositionChanged;
        }
        
        private void Update()
        {
            if (IsOwner)
            {
                HandleInput();
                HandleMovement();
            }
        }
        
        private void HandleInput()
        {
            // Get input
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            moveInput = new Vector3(horizontal, vertical, 0).normalized;
        }
        
        private void HandleMovement()
        {
            if (moveInput.magnitude > 0.1f)
            {
                Vector3 newPosition = transform.position + moveInput * moveSpeed * Time.deltaTime;
                // Cho owner tự di chuyển local
                transform.position = newPosition;
                MoveServerRpc(newPosition);
            }
        }
        
        [ServerRpc]
        private void MoveServerRpc(Vector3 newPosition)
        {
            networkPosition.Value = newPosition;
        }
        
        [ServerRpc]
        private void SetPlayerColorServerRpc(Color color)
        {
            networkColor.Value = color;
        }
        
        private void OnColorChanged(Color previousValue, Color newValue)
        {
            if (playerSprite != null)
            {
                playerSprite.color = newValue;
            }
        }
        
        private void OnPositionChanged(Vector3 previousValue, Vector3 newValue)
        {
            // Nếu không phải owner thì luôn sync vị trí từ server
            if (!IsOwner)
                transform.position = newValue;
            // Nếu là owner, chỉ update nếu lệch quá xa (giảm giật)
            else if (Vector3.Distance(transform.position, newValue) > 0.5f)
                transform.position = newValue;
        }
        
        // Public method to get player info
        public string GetPlayerInfo()
        {
            return $"Player {OwnerClientId} - Owner: {IsOwner}, Server: {IsServer}, Client: {IsClient}";
        }
    }
} 