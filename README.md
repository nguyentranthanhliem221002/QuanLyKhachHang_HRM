# QuanLyKhachHang_HRM

Dự án **Quản lý khách hàng HRM** sử dụng **ASP.NET Core** cho Backend và Frontend, container hóa bằng **Docker**.

# Hướng dẫn chạy dự án ASP.NET với Docker

```powershell
# =========================
# 1️⃣ Kiểm tra & cài WSL + Ubuntu
# =========================
wsl --list --verbose            # Xem các distro Linux đã cài
wsl --update                    # Cập nhật WSL lên phiên bản mới nhất
wsl --install -d Ubuntu-22.04   # Cài Ubuntu 22.04 nếu chưa có
wsl --shutdown                  # Tắt tất cả WSL để áp dụng cập nhật
wsl --list --verbose            # Kiểm tra lại distro đã cài
\```

# =========================
# 2️⃣ Build Docker image
# =========================
docker build -t hrm-be ./BE     # Build image cho Backend
docker build -t hrm-fe ./FE     # Build image cho Frontend

# =========================
# 3️⃣ Docker Compose (chạy toàn bộ dự án)
# =========================
docker-compose down -v          # Dừng container cũ + xóa volume
docker-compose up --build -d    # Build và chạy toàn bộ container BE + FE
docker-compose stop             # Dừng container
docker-compose start            # Khởi động lại container

# =========================
# 4️⃣ SQL Server container (tuỳ chọn)
# =========================
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Abc12345!" `
-p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
# Chạy SQL Server container, mở port 1433, mật khẩu SA = Abc12345!

# =========================
# 5️⃣ HTTPS cho ASP.NET
# =========================
dotnet dev-certs https -ep ./https/aspnetapp.pfx -p 123456
# Tạo chứng chỉ HTTPS cho ASP.NET, lưu vào thư mục ./https

# =========================
# 6️⃣ Push Docker Hub (tuỳ chọn)
# =========================
docker build -t nguyentranthanhliem221002/be:1.0 ./BE -f ./BE/Dockerfile
docker push nguyentranthanhliem221002/be:1.0
docker build -t nguyentranthanhliem221002/fe:1.0 ./FE -f ./FE/Dockerfile
docker push nguyentranthanhliem221002/fe:1.0
# Build & push image lên Docker Hub

---

### 🧠 Giải thích nhanh:
- Mỗi mục (`##`) nằm **ngoài** code block → Markdown sẽ render thành **tiêu đề lớn, rõ ràng**.  
- Các lệnh nằm **trong khối code riêng (` ```powershell `)** → được tô màu và dễ đọc.  
- Bạn có thể mở file `README.md` trên GitHub để thấy hiệu ứng ngay — nó sẽ tự phân chia thành 6 phần rõ ràng, đẹp như hướng dẫn chuyên nghiệp.

