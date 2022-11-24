using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Commerce20.Models
{
    public class Categorias
    {
        [Display(Name = "Código de Categoria")]
        [StringLength(3, MinimumLength = 3,
        ErrorMessage = "El código de categoria debe tener solo 3 caracteres")]
        [Range(0, int.MaxValue, ErrorMessage = "Ingrese solo numeros")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Código de Categoria es obligatorio")]
        public string idCategoria { set; get; }


        [Display(Name = "Nombres de Categoria")]
        [RegularExpression(@"^[a-zA-Z  ]+$", ErrorMessage = "Ingrese solo letras")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string nombreCategoria { set; get; }
    }
}