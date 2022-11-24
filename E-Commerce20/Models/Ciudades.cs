using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Commerce20.Models
{
    public class Ciudades
    {

        [Display(Name = "Código de Ciudad")]
        [StringLength(3, MinimumLength = 3,
        ErrorMessage = "El código debe tener solo 3 caracteres")]
        [Range(0, int.MaxValue, ErrorMessage = "Ingrese solo numeros")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Código de ciudad es obligatorio")]
        public string id_ciudad { set; get; }


        [Display(Name = "Nombre de Ciudad")]
        [RegularExpression(@"^[a-zA-Z  ]+$", ErrorMessage = "Ingrese solo letras")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string nombre_ciudad { set; get; }
    }
}