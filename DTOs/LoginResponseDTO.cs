namespace Steady_Management_App.DTOs
{
    public class LoginResponseDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }
}