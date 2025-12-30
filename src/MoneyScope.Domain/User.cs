using MoneyScope.Core.Enums.User;
using MoneyScope.Domain.AccessProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Domain
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Password { get; set; }
        public string? ImageUrl { get; set; }
        public string? ProviderId { get; set; }
        public EUserStatus Status { get; set; } = EUserStatus.Ativo;

        // Navigation properties
        public ICollection<ProfileUser> ProfilesUsers { get; set; } = [];
        public ICollection<Transaction> Transactions { get; set; } = [];
    }
}
