# ==========================
# QuanLyKhachHang_HRM - Docker & Git Setup (2 EC2: BE & FE)
# ==========================

# 1/ SSH vào Backend EC2 (BE)
#### ssh -i "your-key.pem" ec2-user@98.95.20.86

# 2/ Gỡ Docker & Git cũ
#### sudo dnf remove -y docker docker-client docker-client-latest docker-common docker-latest docker-latest-logrotate docker-logrotate docker-engine containerd.io
#### sudo rm -rf /var/lib/docker /var/lib/containerd /etc/docker /run/docker
#### sudo rm -f /usr/lib/systemd/system/docker.service
#### sudo rm -f /usr/lib/systemd/system/docker.socket
#### sudo systemctl daemon-reload
#### sudo rm -rf /var/lib/docker /var/lib/containerd /etc/docker /run/docker
#### sudo yum remove -y git

# 3/ Cài Docker & Git mới
#### sudo dnf install -y docker
#### sudo systemctl enable --now docker
#### sudo usermod -aG docker ec2-user
#### sudo dnf install -y git

# 4/ Kiểm tra Docker & Git
#### docker --version
#### sudo systemctl status docker
#### docker ps
#### docker run hello-world
#### git --version
#### echo "✅ Docker & Git đã cài xong! Logout & login lại SSH để áp dụng quyền Docker cho user."

# 5/ Các thao tác Docker cơ bản
#### docker ps           # Xem container đang chạy
#### docker ps -a        # Xem tất cả container
#### docker stop container_id
#### docker start container_id
#### docker restart container_id
#### docker rm container_id
#### docker rm -f container_id
#### docker logs container_id
#### docker logs -f container_id
#### docker build -t myapp-be:1.0 ./BE
#### docker images
#### docker rmi image_id
#### docker rmi image_name:tag

# 6/ Docker Compose cho Backend + SQL Server
cat <<EOF > docker-compose.yml
version: "3.9"
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: sqlserver
    environment:
      ACCEPT_EULA: "Y"
      SA_PASSWORD: "Abc12345!"
    ports:
      - "3001:1433"   # SQL Server port 1
      - "8081:1433"   # SQL Server port 2
    volumes:
      - sql_data:/var/opt/mssql

  backend:
    build: ./BE
    ports:
      - "5000:5000"   # BE API
    depends_on:
      - sqlserver
    environment:
      ConnectionStrings__DefaultConnection: "Server=sqlserver,1433;Database=YourDB;User Id=sa;Password=Abc12345!;"

volumes:
  sql_data:
EOF

# 7/ Chạy tất cả service trên BE
#### docker-compose up -d --build
#### docker-compose ps
#### docker-compose logs -f

# 8/ Truy cập Backend
####  Swagger: https://98.95.20.86:5000/swagger
####  SQL Server (nếu mở port 3001/8081): 98.95.20.86,3001

# 9/ SSH vào Frontend EC2 (FE)
#### ssh -i "your-key.pem" ec2-user@13.223.107.213

# 10/ Gỡ Docker & Git cũ trên FE
#### sudo dnf remove -y docker docker-client docker-client-latest docker-common docker-latest docker-latest-logrotate docker-logrotate docker-engine containerd.io
#### sudo rm -rf /var/lib/docker /var/lib/containerd /etc/docker /run/docker
#### sudo rm -f /usr/lib/systemd/system/docker.service
#### sudo rm -f /usr/lib/systemd/system/docker.socket
#### sudo systemctl daemon-reload
#### sudo yum remove -y git

# 11/ Cài Docker & Git mới trên FE
#### sudo dnf install -y docker
#### sudo systemctl enable --now docker
#### sudo usermod -aG docker ec2-user
#### sudo dnf install -y git

# 12/ Kiểm tra Docker & Git trên FE
#### docker --version
#### sudo systemctl status docker
#### docker ps
#### docker run hello-world
#### git --version
#### echo "✅ Docker & Git FE đã cài xong!"

# 13/ Chạy FE container (FE gọi BE API: https://98.95.20.86:5000)
#### docker build -t myapp-fe:1.0 ./FE
#### docker run -d -p 5001:5001 myapp-fe:1.0

# 14/ Truy cập Frontend
#### https://13.223.107.213:5001

# 15/ Truy cập Frontend & Backend với IP tĩnh (Elastic IP) và các port dịch vụ

## Mục tiêu: cả FE và BE dùng IP public cố định (Elastic IP) để truy cập dịch vụ ổn định, không thay đổi khi restart EC2.

### a) Tạo và gán Elastic IP trên AWS cho cả 2 EC2
#### - Vào AWS Console → VPC → Elastic IPs → Allocate Elastic IP
#### - Tạo 1 IP tĩnh cho BE, 1 IP tĩnh cho FE
#### - Gán Elastic IP cho từng EC2:
####     + Elastic IP BE → EC2 Backend
####     + Elastic IP FE → EC2 Frontend
####  - Chọn Elastic IP → Actions → Associate Elastic IP → chọn instance tương ứng

### b) Cấu hình các port trên Backend (BE)
####  - 5000 → Swagger / API (HTTPS)
#### - 3001 → SQLPad (HTTP, web quản lý SQL Server)
#### - 8081 → Adminer (HTTP, web quản lý database)

### Cấu hình port trên Frontend (FE)
#### - 5001 → Frontend web (HTTPS)

### c) Truy cập các dịch vụ bằng Elastic IP
#### - Backend Swagger/API: https://<BE-Elastic-IP>:5000/swagger
#### - SQLPad: http://<BE-Elastic-IP>:3001
#### - Adminer: http://<BE-Elastic-IP>:8081
#### - Frontend web: https://<FE-Elastic-IP>:5001

### d) FE gọi API BE
#### - Trong cấu hình FE (file config hoặc biến môi trường), dùng URL BE Elastic IP:
####    API_URL=https://<BE-Elastic-IP>:5000

### e) Kết quả:
#### - FE & BE luôn dùng IP tĩnh
#### - Các dịch vụ trên BE có port cố định: Swagger 5000 (HTTPS), SQLPad 3001 (HTTP), Adminer 8081 (HTTP)
#### - Frontend luôn truy cập được qua port 5001 (HTTPS)
#### - Không cần thay đổi khi restart EC2

