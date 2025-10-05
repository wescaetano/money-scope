﻿using MoneyScope.Core.Enums.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Domain.AccessProfile
{
    public class Profile : BaseEntity
    {
        public string Name { get; set; } = null!;
        public EProfileStatus Status { get; set; }
        public virtual ICollection<ProfileUser> ProfilesUsers { get; private set; } = [];
        public virtual ICollection<ProfileModule> ProfilesModules { get; set; } = [];
    }
}
