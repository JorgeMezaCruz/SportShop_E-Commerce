using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//Referencia para validaciones
using System.ComponentModel.DataAnnotations;


namespace E_Commerce20.Models
{
    public class Trabajadores
    {
        [Display(Name = "Dni")]
        [StringLength(8, MinimumLength = 8,
        ErrorMessage = "El DNI debe tener 8 caracteres")]
        [Range(0, int.MaxValue, ErrorMessage = "Ingrese solo numeros")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El DNI es obligatorio")]
        public string DniTrab { set; get; }

        [Display(Name = "Nombres")]
        [RegularExpression(@"^[a-zA-Z  ]+$", ErrorMessage = "Ingrese solo letras")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string NombreTrab { set; get; }


        [Display(Name = "Apellidos")]
        [RegularExpression(@"^[a-zA-Z  ]+$", ErrorMessage = "Ingrese solo letras")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string ApellidosTrab { get; set; }


        [Display(Name = "Direccion")]
        [StringLength(60, MinimumLength = 10,
        ErrorMessage = "La {0} debe tener {1} caracteres de máximo y {2} de mínimo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string DireccionTrab { get; set; }


        //[StringLength(3, MinimumLength = 3,
        //ErrorMessage = "El código de ciudad debe tener solo 3 caracteres")]
        //[Range(0, int.MaxValue, ErrorMessage = "Ingrese solo numeros")]
        [Display(Name = "Ciudad")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El código de ciudad es obligatorio")]
        public string id_ciudad { get; set; }


        [Display(Name = "Telefono")]
        [RegularExpression(@"^[0-9 ]+$", ErrorMessage = "Ingrese solo numeros")]
        [StringLength(15, MinimumLength = 6,
        ErrorMessage = "El {0} debe tener {1} caracteres de máximo y {2} de mínimo")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número de telefono es obligatorio")]
        public string TelefonoTrab { set; get; }

        [Display(Name = "Correo Electronico")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(30, ErrorMessage = "El tamaño maximo del {0} es de {1} caracteres")]
        [EmailAddress(ErrorMessage = "El campo {0} debe de un correo valido")]
        public string CorreoTrab { set; get; }


        [Display(Name = "Contraseña")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(8, ErrorMessage = "{0} Tiene que tener mínimo {1} caracteres")]
        public string PasswordTrab { set; get; }


        [Display(Name = "Repetir Contraseña")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "La verificacion de contraseña es obligatoria")]
        [Compare("PasswordTrab", ErrorMessage = "Las contraseñas deben coincidir")]
        [MinLength(8, ErrorMessage = "{0} Tiene que tener mínimo {1} caracteres")]
        public string PasswordTrabConfirmacion { get; set; }



    }
}