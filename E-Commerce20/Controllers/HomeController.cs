using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using E_Commerce20.Models;

namespace E_Commerce20.Controllers
{
    public class HomeController : Controller
    {
        // GET: ECommerce
        // Cadena de conexion
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["conex"].ConnectionString);

        public ActionResult IndexLogin(string message = "")
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                var user = clientes().FirstOrDefault(e => e.CorreoCliente == email && e.PasswordCliente == password);
                var admin = trabajadores().FirstOrDefault(e => e.CorreoTrab == email && e.PasswordTrab == password);


                //Si usuario es diferente de null
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.CorreoCliente, true);
                    return RedirectToAction("IndexCarrito", "Carrito");
                }
                else if(admin != null){
                    FormsAuthentication.SetAuthCookie(admin.CorreoTrab, true);
                    return RedirectToAction("ProductosMntIndex", "ProductosMnt");

                }
                else
                {
                    return RedirectToAction("IndexLogin", new { message = "No encontramos tus datos" });
                }

            }
            else
            {
                return RedirectToAction("IndexLogin", new { message="Llena los campos para poder iniciar sesion"});
            }

        }

        //Cerrar Sesion
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("IndexLogin");
        }


        IEnumerable<Clientes> clientes()
        {
            List<Clientes> temporal = new List<Clientes>();
            cn.Open();
            SqlCommand cmd = new SqlCommand("sp_Clientes", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Clientes reg = new Clientes();
                reg.DniCliente = dr.GetString(0);
                reg.NombreCliente = dr.GetString(1);
                reg.ApellidosCliente = dr.GetString(2);
                reg.DireccionCliente = dr.GetString(3);
                reg.id_ciudad = dr.GetString(4);
                reg.TelefonoCliente = dr.GetString(5);
                reg.CorreoCliente = dr.GetString(6);
                reg.PasswordCliente = dr.GetString(7);
               
                temporal.Add(reg);
            }
            dr.Close();
            cn.Close();

            return temporal;
        }


        IEnumerable<Trabajadores> trabajadores()
        {

            List<Trabajadores> temporal = new List<Trabajadores>();
            cn.Open();
            SqlCommand cmd = new SqlCommand("sp_Trabajadores", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Trabajadores reg = new Trabajadores();
                reg.DniTrab = dr.GetString(0);
                reg.NombreTrab = dr.GetString(1);
                reg.ApellidosTrab = dr.GetString(2);
                reg.DireccionTrab = dr.GetString(3);
                reg.id_ciudad = dr.GetString(4);
                reg.TelefonoTrab = dr.GetString(5);
                reg.CorreoTrab = dr.GetString(6);
                reg.PasswordTrab = dr.GetString(7);
                temporal.Add(reg);
            }
            dr.Close();
            cn.Close();

            return temporal;
        }


    }
}

