# 🎮 Unity Netcode for GameObjects - Hướng dẫn học tập

## 📋 **Tổng quan**
Đây là hướng dẫn học Unity Netcode for GameObjects từ cơ bản đến nâng cao. Project này bao gồm các thành phần cơ bản để tạo game multiplayer.

## 🏗️ **Cấu trúc Project**

### **Core Scripts:**
- `GameNetworkManager.cs` - Quản lý kết nối mạng chính
- `NetworkUI.cs` - UI để kết nối và hiển thị trạng thái
- `GameStateManager.cs` - Quản lý trạng thái game

### **Player Scripts:**
- `NetworkPlayer.cs` - Player với movement và ownership
- `PlayerSpawner.cs` - Tự động spawn player khi client kết nối

## 🚀 **Cách thiết lập và test**

### **Bước 1: Tạo Player Prefab**
1. Tạo GameObject mới trong scene
2. Thêm `SpriteRenderer` component
3. Thêm `NetworkObject` component
4. Thêm `NetworkPlayer` script
5. Gán sprite cho player (có thể dùng `green_ball_guy.png`)
6. Tạo prefab từ GameObject này

### **Bước 2: Thiết lập NetworkManager**
1. Tạo GameObject mới tên "NetworkManager"
2. Thêm `NetworkManager` component (Unity Netcode)
3. Thêm `GameNetworkManager` script
4. Gán player prefab vào NetworkManager
5. Thiết lập NetworkManager settings:
   - Transport: Unity Transport
   - Network Prefabs: Thêm player prefab

### **Bước 3: Tạo UI**
1. Tạo Canvas
2. Thêm các button: Host, Client, Server, Disconnect
3. Thêm Text để hiển thị status và player count
4. Thêm `NetworkUI` script vào Canvas
5. Gán các UI elements vào script

### **Bước 4: Test với ParrelSync**
1. Mở ParrelSync window (Tools > ParrelSync > Clones Manager)
2. Tạo clone project để test multiplayer
3. Mở clone project trong Unity Editor khác
4. Chạy game trên cả hai project

## 🎯 **Các khái niệm quan trọng**

### **1. NetworkBehaviour**
- Base class cho tất cả networked objects
- Cung cấp `IsOwner`, `IsServer`, `IsClient` properties
- Có lifecycle methods: `OnNetworkSpawn()`, `OnNetworkDespawn()`

### **2. NetworkVariable**
- Đồng bộ dữ liệu giữa server và clients
- Tự động cập nhật khi giá trị thay đổi
- Có thể subscribe để lắng nghe thay đổi

### **3. ServerRpc và ClientRpc**
- `ServerRpc`: Client gọi đến Server
- `ClientRpc`: Server gọi đến tất cả Clients
- Dùng để thực hiện actions trên network

### **4. Ownership**
- Mỗi NetworkObject có owner (client)
- Owner có quyền điều khiển object
- Server luôn có quyền với tất cả objects

## 🔧 **Các tính năng đã implement**

### **✅ NetworkPlayer**
- Movement với WASD/Arrow keys
- Random color cho mỗi player
- NetworkVariable cho position và color
- ServerRpc cho movement

### **✅ PlayerSpawner**
- Tự động spawn player khi client kết nối
- Random spawn position
- Xử lý client disconnect

### **✅ GameStateManager**
- Quản lý trạng thái game (Waiting, Playing, GameOver)
- Timer cho game duration
- Minimum players để start game
- NetworkVariable cho game state

### **✅ NetworkUI**
- Buttons để Host/Client/Server
- Hiển thị connection status
- Player count display
- Disconnect functionality

## 🎮 **Cách test**

### **Test 1: Basic Connection**
1. Chạy game trên project chính
2. Click "Host" button
3. Mở clone project
4. Click "Client" button
5. Kiểm tra player spawn và movement

### **Test 2: Multiple Players**
1. Tạo nhiều clone projects
2. Host game trên project chính
3. Join từ các clone projects
4. Test movement và interaction

### **Test 3: Game State**
1. Kết nối 2+ players
2. Game sẽ tự động start khi đủ players
3. Test game timer và state changes

## 📚 **Bài tập thực hành**

### **Bài 1: Thêm Chat System**
- Tạo UI input field cho chat
- Implement ClientRpc để gửi message
- Hiển thị chat history

### **Bài 2: Thêm Score System**
- NetworkVariable cho player scores
- UI hiển thị scoreboard
- Update score khi player thực hiện actions

### **Bài 3: Thêm Game Mechanics**
- Collectible items (coins)
- Player collision detection
- Win/lose conditions

### **Bài 4: Advanced Networking**
- Custom NetworkVariable types
- NetworkTransform cho smooth movement
- Lag compensation

## 🐛 **Troubleshooting**

### **Lỗi thường gặp:**
1. **Player không spawn**: Kiểm tra NetworkManager prefab settings
2. **Movement không sync**: Đảm bảo ServerRpc được gọi
3. **UI không update**: Kiểm tra NetworkVariable subscriptions
4. **Connection failed**: Kiểm tra firewall và port settings

### **Debug Tips:**
- Sử dụng Debug.Log để track network events
- Kiểm tra NetworkManager status trong Inspector
- Monitor NetworkVariable values
- Test với ParrelSync để simulate real network

## 🎯 **Bước tiếp theo**
Sau khi nắm vững cơ bản, bạn có thể:
1. Học về NetworkTransform
2. Implement custom networking protocols
3. Tối ưu hóa network performance
4. Thêm security và anti-cheat
5. Deploy game lên cloud servers

---

**Happy Networking! 🚀** 