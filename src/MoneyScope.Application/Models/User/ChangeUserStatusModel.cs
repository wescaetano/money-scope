﻿using MoneyScope.Core.Enums.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Models.User
{
    public class ChangeUserStatusModel
    {
        public long Id { get; set; }
        public EUserStatus Status { get; set; }
    }
}
