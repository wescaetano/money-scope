using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Models.User
{
    public class CreateUserModel
    {
        [Required(ErrorMessage = "O campo 'name' é obrigatório!")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "O campo 'email' é obrigatório!")]
        public string Email { get; set; } = null!;
        public string? ImageBase64 { get; set; }
        [Required(ErrorMessage = "O campo 'accessProfile' é obrigatório!")]
        public long AccessProfile { get; set; }
    }
}
