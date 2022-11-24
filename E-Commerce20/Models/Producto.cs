using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//Referencia para validaciones
using System.ComponentModel.DataAnnotations;


namespace E_Commerce20.Models
{
    public class Producto
    {
        [Display(Name = "Codigo de Producto")]
        [Range(0, int.MaxValue, ErrorMessage = "Ingrese solo numeros")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int idProducto { set; get; }


        [Display(Name = "Nombre de Producto")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string nomProducto { set; get; }


        [Display(Name = "Código de Proveedor")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string idProveedor { get; set; }

        [Display(Name = "Código de Categoria")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string idCategoria { get; set; }



        [Display(Name = "Precio de Producto")]
        [Range(0, double.MaxValue, ErrorMessage = "Ingrese solo numeros")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El precio es obligatorio")]
        public decimal precioUnidad { set; get; }


        [Display(Name = "Stock")]
        [Range(0, int.MaxValue, ErrorMessage = "Ingrese solo numeros")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El stock de producto es obligatorio")]
        public int stockProducto { set; get; }

    }
}