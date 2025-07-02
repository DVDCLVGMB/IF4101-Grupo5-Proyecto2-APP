namespace Steady_Management_App.Models
{
    public class User
    {
        public int UserId { get; private set; }
        public string Username { get; private set; } = string.Empty;
        public int RoleId { get; private set; }
        public bool IsAuthenticated { get; private set; }

        /// <summary>
        /// Llena la sesión con los datos recibidos.
        /// </summary>
        public void SetSession(int userId, string username, int roleId)
        {
            UserId = userId;
            Username = username;
            RoleId = roleId;
            IsAuthenticated = true;
        }

        /// <summary>
        /// Limpia la sesión (logout).
        /// </summary>
        public void ClearSession()
        {
            UserId = 0;
            Username = string.Empty;
            RoleId = 0;
            IsAuthenticated = false;
        }
    }
}

