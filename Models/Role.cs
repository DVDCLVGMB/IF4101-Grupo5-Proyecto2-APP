namespace Steady_Management.Domain
{
    public class Role
    {
        public Role()
        {
            // Constructor vacío para serialización
        }

        public Role(int roleId, string roleName)
        {
            RoleId = roleId;
            RoleName = roleName;
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public override string ToString()
        {
            return RoleName;
        }
    }
}