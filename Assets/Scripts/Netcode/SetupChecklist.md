# ✅ Unity Netcode Setup Checklist

## 🎯 **Bước 1: Kiểm tra Dependencies**
- [ ] Unity Netcode for GameObjects đã được cài đặt (version 1.7.1)
- [ ] ParrelSync đã được cài đặt
- [ ] TextMesh Pro đã được cài đặt

## 🎯 **Bước 2: Tạo Player Prefab**
- [ ] Tạo GameObject mới trong scene
- [ ] Thêm `SpriteRenderer` component
- [ ] Gán sprite (ví dụ: `green_ball_guy.png`)
- [ ] Thêm `NetworkObject` component
- [ ] Thêm `NetworkPlayer` script
- [ ] Thiết lập `moveSpeed` và `playerSprite` trong inspector
- [ ] Tạo prefab từ GameObject
- [ ] Đặt tên prefab: "NetworkPlayer"

## 🎯 **Bước 3: Thiết lập NetworkManager**
- [ ] Tạo GameObject mới tên "NetworkManager"
- [ ] Thêm `NetworkManager` component (Unity Netcode)
- [ ] Thêm `GameNetworkManager` script
- [ ] Gán player prefab vào `Player Prefab` field
- [ ] Thiết lập NetworkManager settings:
  - [ ] Transport: Unity Transport
  - [ ] Network Prefabs: Thêm player prefab
  - [ ] Player Prefab: Gán player prefab
  - [ ] Network Address: "127.0.0.1"
  - [ ] Network Port: 7777

## 🎯 **Bước 4: Tạo PlayerSpawner**
- [ ] Tạo GameObject mới tên "PlayerSpawner"
- [ ] Thêm `NetworkObject` component
- [ ] Thêm `PlayerSpawner` script
- [ ] Gán player prefab vào `Player Prefab` field
- [ ] Tạo spawn points (optional):
  - [ ] Tạo empty GameObjects làm spawn points
  - [ ] Gán vào `Spawn Points` array

## 🎯 **Bước 5: Tạo GameStateManager**
- [ ] Tạo GameObject mới tên "GameStateManager"
- [ ] Thêm `NetworkObject` component
- [ ] Thêm `GameStateManager` script
- [ ] Thiết lập settings:
  - [ ] Min Players To Start: 2
  - [ ] Game Duration: 60

## 🎯 **Bước 6: Tạo UI**
- [ ] Tạo Canvas
- [ ] Thêm UI elements:
  - [ ] Button "Host"
  - [ ] Button "Client"
  - [ ] Button "Server"
  - [ ] Button "Disconnect"
  - [ ] Text "Status: Disconnected"
  - [ ] Text "Players: 0"
  - [ ] InputField "IP Address" (default: 127.0.0.1)
  - [ ] InputField "Port" (default: 7777)
- [ ] Thêm `NetworkUI` script vào Canvas
- [ ] Gán tất cả UI elements vào script

## 🎯 **Bước 7: Tạo Debug Tools (Optional)**
- [ ] Tạo GameObject mới tên "NetworkDebugger"
- [ ] Thêm `NetworkDebugger` script
- [ ] Tạo Text UI element cho debug info
- [ ] Gán Text vào `Debug Text` field
- [ ] Tạo GameObject mới tên "QuickTest"
- [ ] Thêm `QuickTest` script

## 🎯 **Bước 8: Thiết lập Scene**
- [ ] Đảm bảo tất cả GameObjects có `NetworkObject` component
- [ ] Kiểm tra hierarchy:
  ```
  Scene
  ├── NetworkManager
  ├── PlayerSpawner
  ├── GameStateManager
  ├── Canvas
  │   └── NetworkUI
  ├── NetworkDebugger (optional)
  └── QuickTest (optional)
  ```

## 🎯 **Bước 9: Test Setup**
- [ ] Chạy scene
- [ ] Kiểm tra console không có errors
- [ ] Test Host button
- [ ] Test Client button
- [ ] Test Disconnect button
- [ ] Kiểm tra player spawn

## 🎯 **Bước 10: Test với ParrelSync**
- [ ] Mở ParrelSync window (Tools > ParrelSync > Clones Manager)
- [ ] Tạo clone project
- [ ] Mở clone project trong Unity Editor khác
- [ ] Test multiplayer:
  - [ ] Host trên project chính
  - [ ] Join từ clone project
  - [ ] Test player movement
  - [ ] Test multiple players

## 🎯 **Bước 11: Kiểm tra Network Variables**
- [ ] Player position sync
- [ ] Player color sync
- [ ] Game state sync
- [ ] Player count sync

## 🎯 **Bước 12: Test Game Flow**
- [ ] Kết nối 2+ players
- [ ] Game tự động start khi đủ players
- [ ] Game timer hoạt động
- [ ] Game end khi hết thời gian
- [ ] Game reset hoạt động

## 🐛 **Troubleshooting Checklist**

### **Nếu Player không spawn:**
- [ ] Kiểm tra NetworkManager Player Prefab
- [ ] Kiểm tra PlayerSpawner Player Prefab
- [ ] Kiểm tra NetworkObject component trên player prefab
- [ ] Kiểm tra console errors

### **Nếu Movement không sync:**
- [ ] Kiểm tra ServerRpc trong NetworkPlayer
- [ ] Kiểm tra NetworkVariable subscriptions
- [ ] Kiểm tra IsOwner condition

### **Nếu UI không update:**
- [ ] Kiểm tra NetworkUI script assignments
- [ ] Kiểm tra NetworkVariable subscriptions
- [ ] Kiểm tra Update() method

### **Nếu Connection failed:**
- [ ] Kiểm tra firewall settings
- [ ] Kiểm tra port availability
- [ ] Kiểm tra IP address
- [ ] Test với localhost (127.0.0.1)

## 🎮 **Test Scenarios**

### **Scenario 1: Basic Connection**
1. Click "Host" button
2. Verify player spawns
3. Click "Disconnect" button
4. Verify player despawns

### **Scenario 2: Client Connection**
1. Click "Host" button on main project
2. Click "Client" button on clone project
3. Verify both players spawn
4. Test movement on both players

### **Scenario 3: Multiple Players**
1. Host game on main project
2. Join from multiple clone projects
3. Verify all players spawn
4. Test movement and interaction

### **Scenario 4: Game State**
1. Connect 2+ players
2. Verify game starts automatically
3. Monitor game timer
4. Verify game ends after duration

## ✅ **Success Criteria**
- [ ] Players spawn automatically when connecting
- [ ] Movement syncs across network
- [ ] UI updates correctly
- [ ] Game state manages properly
- [ ] Multiple players can connect
- [ ] No console errors
- [ ] Smooth gameplay experience

---

**🎯 Khi hoàn thành checklist này, bạn đã có một game multiplayer cơ bản hoạt động!** 