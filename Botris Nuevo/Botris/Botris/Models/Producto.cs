using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Botris.Models
{
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string CodigoBarras { get; set;}
        public DateTime FechaCreacion { get; set;}
        public string SKU { get; set;}
        public string Marca { get; set;}
        public string RFID { get; set;}
        public string NombreSucursal { get; set;}
        public string Codigo { get; set;}
    }
}
