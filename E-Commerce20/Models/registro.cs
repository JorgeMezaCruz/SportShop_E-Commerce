using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace E_Commerce20.Models
{
    public class registro
    {

        [Display(Name = "Codigo")]
        public int idProducto { set; get; }


        [Display(Name = "Nombre")]
        public string nomProducto { set; get; }


        [Display(Name = "Precio Unidad")]
        public decimal precioUnidad { set; get; }


        [Display(Name = "Stock")]
        public int stockProducto { set; get; }


        [Display(Name = "Cantidad")]
        public double cantidad { set; get; }


        [Display(Name = "Monto")]
        public decimal monto { set; get; }












    }
}