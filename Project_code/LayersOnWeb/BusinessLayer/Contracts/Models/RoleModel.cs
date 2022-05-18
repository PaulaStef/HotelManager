
namespace BusinessLayer.Contracts
{
    public class RoleModel
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public RoleTypeEnum RoleType { get; set; }
    }

    public enum RoleTypeEnum
    {
        Receptionist = 0,
        Administrator = 1
    }
}
