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
####  Swagger: http://98.95.20.86:5000/swagger
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

# 13/ Chạy FE container (FE gọi BE API: http://98.95.20.86:5000)
#### docker build -t myapp-fe:1.0 ./FE
#### docker run -d -p 5001:5001 myapp-fe:1.0

# 14/ Truy cập Frontend
#### http://13.223.107.213:5001
