# QuanLyKhachHang_HRM

Dự án **Quản lý khách hàng HRM** sử dụng **ASP.NET Core** cho Backend và Frontend, container hóa bằng **Docker**.

# Hướng dẫn chạy dự án ASP.NET với Docker

# 1️⃣ Kiểm tra & cài đặt WSL + Ubuntu
#### wsl --list --verbose            # Xem các distro Linux đã cài
#### wsl --update                    # Cập nhật WSL lên phiên bản mới nhất
#### wsl --install -d Ubuntu-22.04   # Cài Ubuntu 22.04 nếu chưa có
#### wsl --shutdown                  # Tắt tất cả WSL để áp dụng cập nhật
#### wsl --list --verbose            # Kiểm tra lại distro đã cài

# 2️⃣ Cài đặt Docker & Docker Compose
#### docker --version
#### docker-compose --version

# 3️⃣ Build Docker Image
#### docker build -t hrm-be ./BE     # Build image cho Backend
#### docker build -t hrm-fe ./FE     # Build image cho Frontend

#4️⃣ Docker Compose (chạy toàn bộ dự án)
#### docker-compose down -v          # Dừng container cũ + xóa volume
#### docker-compose up --build -d    # Build và chạy toàn bộ container BE + FE
#### docker-compose stop             # Dừng container
#### docker-compose start            # Khởi động lại container

# 5️⃣ Chạy SQL Server Container (tuỳ chọn)
#### docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Abc12345!" `
-p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest

# 6️⃣ Tạo chứng chỉ HTTPS cho ASP.NET
#### dotnet dev-certs https -ep ./https/aspnetapp.pfx -p 123456
#### dotnet dev-certs https --trust

# 7️⃣ Cấu hình file .env
ASPNETCORE_ENVIRONMENT=Docker
ASPNETCORE_URLS=https://+:443;http://+:8080
ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ASPNETCORE_Kestrel__Certificates__Default__Password=123456

API_URL=https://be:443

DB_HOST=sqlserver
DB_PORT=1433
DB_NAME=HeThongQLKH_DB
DB_USER=sa
SA_PASSWORD=Abc12345!


# 8️⃣ Cấu hình chuỗi kết nối trong appsettings.json (BE)
"ConnectionStrings": {
  "DefaultConnection": "Server=sqlserver,1433;Database=HeThongQLKH_DB;User Id=sa;Password=Abc12345!;TrustServerCertificate=True;"
},
"Frontend": {
  "BaseUrl": "http://localhost:5001"
}

# 9️⃣ Push Image lên Docker Hub (tùy chọn)
### Đăng nhập Docker Hub
#### docker login

### Build & gắn tag
#### docker build -t nguyentranthanhliem221002/be:1.0 ./BE -f ./BE/Dockerfile
#### docker build -t nguyentranthanhliem221002/fe:1.0 ./FE -f ./FE/Dockerfile

### Push lên Docker Hub
#### docker push nguyentranthanhliem221002/be:1.0
#### docker push nguyentranthanhliem221002/fe:1.0

### 👉 Sau khi đẩy, có thể pull image từ bất kỳ máy nào khác:
#### docker pull nguyentranthanhliem221002/be:1.0
#### docker pull nguyentranthanhliem221002/fe:1.0

# 🔟 Triển khai Lên AWS (ECS + ECR)
## 🧱 Bước 1: Cấu hình AWS CLI

### Cài đặt AWS CLI:
#### https://docs.aws.amazon.com/cli/latest/userguide/getting-started-install.html
### 🔗 Hướng dẫn chính thức
#### aws configure
### Nhập thông tin:
####  AWS Access Key ID: <access-key>
####  AWS Secret Access Key: <secret-key>
####  Default region name: ap-southeast-1
####  Default output format: json

## 📦 Bước 2: Tạo Repository ECR
#### aws ecr create-repository --repository-name hrm-be --region ap-southeast-1
#### aws ecr create-repository --repository-name hrm-fe --region ap-southeast-1


### AWS sẽ trả về URL dạng:

#### 123456789012.dkr.ecr.ap-southeast-1.amazonaws.com/hrm-be

## 🚚 Bước 3: Push Image lên AWS ECR
### Đăng nhập vào ECR
aws ecr get-login-password --region ap-southeast-1 | docker login --username AWS --password-stdin 123456789012.dkr.ecr.ap-southeast-1.amazonaws.com

### Tag lại image
#### docker tag hrm-be 123456789012.dkr.ecr.ap-southeast-1.amazonaws.com/hrm-be:1.0
#### docker tag hrm-fe 123456789012.dkr.ecr.ap-southeast-1.amazonaws.com/hrm-fe:1.0

### Push image
#### docker push 123456789012.dkr.ecr.ap-southeast-1.amazonaws.com/hrm-be:1.0
#### docker push 123456789012.dkr.ecr.ap-southeast-1.amazonaws.com/hrm-fe:1.0

## ☁️ Bước 4: Triển khai Lên AWS ECS (Fargate)

Truy cập AWS Console → ECS → Create Cluster

Chọn Networking Only (Fargate)

Tạo Task Definition:

Container 1: hrm-be → Image từ ECR → Port 8080

Container 2: hrm-fe → Image từ ECR → Port 80

Add environment variables (API_URL, DB connection string, …)

Tạo Service → Fargate → Cluster → Deploy Task

ECS sẽ tự động tạo Load Balancer nếu chọn, FE sẽ có URL public.

## 🌐 Bước 5: Truy cập Ứng Dụng

#### FE: Truy cập Load Balancer URL hiển thị trong ECS console

#### BE: API nội bộ gọi từ FE qua http://be:8080 hoặc qua domain ECS nội bộ
