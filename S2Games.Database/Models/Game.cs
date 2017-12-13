using S2Games.Database.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S2Games.Database.Models
{
    public enum Genre
    {
        [Display(Name = "Action", ResourceType = typeof(GameStrings))]
        Action,
        [Display(Name = "Adventure", ResourceType = typeof(GameStrings))]
        Adventure,
        [Display(Name = "Shooter", ResourceType = typeof(GameStrings))]
        Shooter,
        [Display(Name = "RPG", ResourceType = typeof(GameStrings))]
        RPG,
        [Display(Name = "Simulation", ResourceType = typeof(GameStrings))]
        Simulation,
        [Display(Name = "Others", ResourceType = typeof(GameStrings))]
        Others
    }

    public class Game : Model
    {
        [Required]
        public string Label { get; set; }

        public Genre Genre { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public int? LentForId { get; set; }

        public virtual Friend LentFor { get; set; }
    }
}
