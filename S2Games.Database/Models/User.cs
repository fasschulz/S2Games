using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S2Games.Database.Models
{
    public class User : Model
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        public string Email { get; set; }

        public bool Enabled { get; set; }
    }
}
