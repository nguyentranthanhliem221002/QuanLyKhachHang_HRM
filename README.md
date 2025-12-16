# ==========================
# QuanLyKhachHang_HRM - Docker & Git Setup
# ==========================

# 1️⃣ SSH vào EC2
### ssh -i "your-key.pem" ec2-user@EC2_PUBLIC_IP

# 2️⃣ Gỡ Docker & Git cũ
#### sudo dnf remove -y docker docker-client docker-client-latest docker-common docker-latest docker-latest-logrotate docker-logrotate docker-engine containerd.io
#### sudo rm -rf /var/lib/docker /var/lib/containerd /etc/docker /run/docker
#### sudo rm -f /usr/lib/systemd/system/docker.service
#### sudo rm -f /usr/lib/systemd/system/docker.socket
#### sudo systemctl daemon-reload
#### sudo rm -rf /var/lib/docker /var/lib/containerd /etc/docker /run/docker
#### sudo yum remove -y git

# 3️⃣ Cài Docker & Git mới
#### sudo dnf install -y docker
#### sudo systemctl enable --now docker
#### sudo usermod -aG docker ec2-user
#### sudo dnf install -y git

# 4️⃣ Kiểm tra Docker & Git
#### docker --version
#### sudo systemctl status docker
#### docker ps
#### docker run hello-world
#### git --version

#### echo "✅ Docker & Git đã cài xong! Logout & login lại SSH để áp dụng quyền Docker cho user."

# 5️⃣ Các thao tác Docker cơ bản

## Xem container
#### docker ps           # Xem container đang chạy
#### docker ps -a        # Xem tất cả container

## Chạy container
#### docker run -d -p 5000:5000 myapp-be:1.0
#### docker run -d -p 5001:5001 myapp-fe:1.0

## Dừng / Restart / Xóa container
#### docker stop container_id
#### docker start container_id
#### docker restart container_id
#### docker rm container_id
#### docker rm -f container_id    # Xóa container đang chạy

## Xem log container
#### docker logs container_id
#### docker logs -f container_id  # Theo dõi log realtime

## Build Docker image
#### docker build -t myapp-be:1.0 ./BE
#### docker build -t myapp-fe:1.0 ./FE

## Xem & xóa image
#### docker images
#### docker rmi image_id
#### docker rmi image_name:tag

# 6️⃣ Docker Compose (nếu nhiều service)
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
      - "1433:1433"
    volumes:
      - sql_data:/var/opt/mssql

  backend:
    build: ./BE
    ports:
      - "5000:5000"
    depends_on:
      - sqlserver

  frontend:
    build: ./FE
    ports:
      - "5001:5001"
    depends_on:
      - backend

volumes:
  sql_data:
EOF

# Chạy tất cả service
#### docker-compose up -d --build

# Kiểm tra
#### docker-compose ps
#### docker-compose logs -f
