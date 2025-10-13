namespace BE.Model
{
    public class User
    {
        public int Id { get; set; }           // Primary key
        public string UserName { get; set; }  // Tên người dùng
        public string Email { get; set; }     // Email
        public string Password { get; set; }  // Mật khẩu (hash sau này)
        public string Role { get; set; }      // Vai trò (Admin, Customer, v.v.)
    }
}
