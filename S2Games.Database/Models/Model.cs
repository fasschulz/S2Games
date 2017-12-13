using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S2Games.Database.Models
{
    public class Model
    {
        [Key]
        public int Id { get; set; }

        [NotMapped]
        [ScaffoldColumn(false)] //Não deixa fazer "scaffold" (gerar automático) do campo no controller e na view
        public static string Key //Campo utilizado para criptografia
        {
            get
            {
                return "9&J2Rz@n|Cao<>2+fgnU_W#M%RJZ%o*RdL:?C)C>l}-t9}KSiZS*O*VM_x";
            }
        }

        [Timestamp]
        public byte[] RowVersion { get; set; } //Campo utilizado para controlar transações no banco
    }
}
