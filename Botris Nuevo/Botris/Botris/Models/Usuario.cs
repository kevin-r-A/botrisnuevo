using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Botris.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? id { get; set; }
        public string? Nombre { get; set; }
        public string? User { get; set; }
        public string? Password { get; set; }    
        public string? Rol { get; set; }
        public string? Token { get; set; }
        public string? Email { get; set; }
    }
}
