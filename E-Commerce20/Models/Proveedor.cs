using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Commerce20.Models
{
    public class Proveedor
    {
        [Display(Name = "RUC de Proveedor")]
        [StringLength(11, MinimumLength = 8,
        ErrorMessage = "El Código de Proveedor debe tener 11 caracteres como máximo y 8 caracteres como minimo (DNI)")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Ingrese solo numeros")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El RUC es obligatorio")]
        public string idProveedor { get; set; }

        [Display(Name = "Nombre de Empresa")]
        [RegularExpression(@"^[a-zA-Z  ]+$", ErrorMessage = "Ingrese solo letras")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string nombreCia { get; set; }


        [Display(Name = "Representante Legal")]
        [RegularExpression(@"^[a-zA-Z  ]+$", ErrorMessage = "Ingrese solo letras")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string nombreContacto { get; set; }

        [Display(Name = "Direccion")]
        [StringLength(60, MinimumLength = 10,
        ErrorMessage = "La {0} debe tener {1} caracteres de máximo y {2} de mínimo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string direccion { get; set; }




        [Display(Name = "Ciudad")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El código de ciudad es obligatorio")]
        public string id_ciudad { get; set; }


        [Display(Name = "Telefono")]
        [RegularExpression(@"^[0-9 ]+$", ErrorMessage = "Ingrese solo numeros")]
        [StringLength(15, MinimumLength = 6,
        ErrorMessage = "El {0} debe tener {1} caracteres de máximo y {2} de mínimo")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número de telefono es obligatorio")]
        public string telefono { get; set; }
    }
}