using System.Runtime.Serialization;

namespace Matta.Core.Application.AppEnums
{
    public enum Roles
    {
        [EnumMember(Value = "Super Admin")]
        SuperAdmin = 0,

        [EnumMember(Value = "Branch Admin")]
        BranchAdmin = 1,

        [EnumMember(Value = "Teller")]
        Teller = 2,


    }
}
