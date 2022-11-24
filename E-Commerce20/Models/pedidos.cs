using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Commerce20.Models
{
    public class pedidos
    {
        [Display(Name = "Código de Pedido")]
        public int idpedido { set; get; }

        [Display(Name = "Fecha")]
        public DateTime fecha { set; get; }

        [Display(Name = "DNI")]
        public string dnicli { set; get; }

        [Display(Name = "Cliente")]
        public string cliente { set; get; }

        [Display(Name = "Direccion")]
        public string direccion { set; get; }

        [Display(Name = "Monto")]
        public decimal monto { set; get; }
    }
}