using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Botris.Models
{
    public class Inventario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int Cabecera { get; set; }
        public string Nombre { get; set; }
        public string CodigoBarras { get; set; }
        public DateTime FechaCreacion { get; set;}
        public DateTime? FechaInventario { get; set; }
        public DateTime? FechaInventarioManual { get; set; }
        public string SKU { get; set; }
        public string RFID { get; set; }
        public string Marca { get; set; }
        public string NombreSucursal { get; set; }
        public string CodigoSucursal {get; set;}
        public bool? EstadoInventario { get; set; } = false;
        public string? Comentario { get; set; }
        public bool? Estado_IFinalizar { get; set; } = false;

    }
}
