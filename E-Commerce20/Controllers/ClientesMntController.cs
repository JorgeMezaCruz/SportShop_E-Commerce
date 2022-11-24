using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using E_Commerce20.Models;

namespace E_Commerce20.Controllers
{
    public class ClientesMntController : Controller
    {

        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["conex"].ConnectionString);



        public ActionResult ClientesMntIndex()
        {
            return View(clientes());
        }

        //Crear un create tipo get
        public ActionResult ClientesMntCreate()
        {
            ViewBag.ciudades = new SelectList(ciudades(), "id_ciudad", "nombre_ciudad");
            return View(new Clientes());

        }

        //Crear un create tipo post
        [HttpPost]
        public ActionResult ClientesMntCreate(Clientes reg)
        {
            List<SqlParameter> lista = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@dni_cliente", SqlDbType = SqlDbType.VarChar, Value = reg.DniCliente },

                new SqlParameter() { ParameterName = "@nombre_cliente", SqlDbType = SqlDbType.VarChar, Value = reg.NombreCliente },

                new SqlParameter() { ParameterName = "@apellido_cliente", SqlDbType = SqlDbType.VarChar, Value = reg.ApellidosCliente },

                new SqlParameter() { ParameterName = "@direccion_cliente", SqlDbType = SqlDbType.VarChar, Value = reg.DireccionCliente},

                new SqlParameter() { ParameterName = "@id_ciudad", SqlDbType = SqlDbType.VarChar, Value = reg.id_ciudad},

                new SqlParameter() { ParameterName = "@telefono_cliente", SqlDbType = SqlDbType.VarChar, Value = reg.TelefonoCliente},

                new SqlParameter() { ParameterName = "@correo_cliente", SqlDbType = SqlDbType.VarChar, Value = reg.CorreoCliente},

                new SqlParameter() { ParameterName = "@password_cliente", SqlDbType = SqlDbType.VarChar, Value = reg.PasswordCliente}
            };

            //Proceso de ejecucion llamando al metodo CRUD(recibe 2 parametros)
            ViewBag.mensaje = CRUD("sp_agregar_cliente", lista);

            ViewBag.ciudades = new SelectList(ciudades(), "id_ciudad", "nombre_ciudad", reg.id_ciudad);
            return View(reg);
        }

        IEnumerable<Ciudades> ciudades()
        {
            //Crear lista temporal
            List<Ciudades> temporal = new List<Ciudades>();

            //Abrir conexion
            cn.Open();

            //Comando sql para poder manipular sentencias sql
            SqlCommand cmd = new SqlCommand("sp_Ciudad", cn);
            //Indicar comando que se va a manejar
            cmd.CommandType = CommandType.StoredProcedure;
            //Data readar para almacenar datos 
            SqlDataReader dr = cmd.ExecuteReader();
            //Luego de ejecutar cmd, leer datos mediante bucle
            while (dr.Read())
            {
                Ciudades reg = new Ciudades();
                reg.id_ciudad = dr.GetString(0);
                reg.nombre_ciudad = dr.GetString(1);
                temporal.Add(reg);
            }

            //Cerrar conexiones
            dr.Close();
            cn.Close();
            return temporal;
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


        public ActionResult ClientesMntEdit(string id)
        {
            Clientes reg = clientes().Where(x => x.DniCliente == id).FirstOrDefault();
            ViewBag.ciudades = new SelectList(ciudades(), "id_ciudad", "nombre_ciudad", reg.id_ciudad);
            return View(reg);
        }


        [HttpPost]
        public ActionResult ClientesMntEdit(Clientes reg)
        {
            //Crear lista de parametros
            List<SqlParameter> lista = new List<SqlParameter>()
            {
                //Definir parametros
                new SqlParameter() { ParameterName = "@dni_cliente", SqlDbType = SqlDbType.VarChar, Value = reg.DniCliente },

                new SqlParameter() { ParameterName = "@nombre_cliente", SqlDbType = SqlDbType.VarChar, Value = reg.NombreCliente },

                new SqlParameter() { ParameterName = "@apellido_cliente", SqlDbType = SqlDbType.VarChar, Value = reg.ApellidosCliente },

                new SqlParameter() { ParameterName = "@direccion_cliente", SqlDbType = SqlDbType.VarChar, Value = reg.DireccionCliente},

                new SqlParameter() { ParameterName = "@id_ciudad", SqlDbType = SqlDbType.VarChar, Value = reg.id_ciudad},

                new SqlParameter() { ParameterName = "@telefono_cliente", SqlDbType = SqlDbType.VarChar, Value = reg.TelefonoCliente},

                new SqlParameter() { ParameterName = "@correo_cliente", SqlDbType = SqlDbType.VarChar, Value = reg.CorreoCliente},

                new SqlParameter() { ParameterName = "@password_cliente", SqlDbType = SqlDbType.VarChar, Value = reg.PasswordCliente}
            };

            //Ejecutar 
            ViewBag.mensaje = CRUD("sp_actualizar_cliente", lista);

            ViewBag.ciudades = new SelectList(ciudades(), "id_ciudad", "nombre_ciudad", reg.id_ciudad);



            //Envio vendedor a la vista
            return View(reg);
        }

        string CRUD(string proceso, List<SqlParameter> pars)
        {
            string mensaje;
            cn.Open();
            SqlCommand cmd = new SqlCommand(proceso, cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(pars.ToArray());
            try
            {
                //n va a recibir el estado de ejecucion de la consulta
                int n = cmd.ExecuteNonQuery();
                mensaje = n + "Registro Actualizado";

            }
            catch (SqlException ex)
            {
                mensaje = ex.Message;
            }
            finally
            {
                cn.Close();
            }
            return mensaje;
        }

        public ActionResult ClientesMntDelete(string id)
        {

            string mensaje = "";
            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("Delete FROM Clientes where DniCliente = @dni_cliente", cn);
                cmd.Parameters.AddWithValue("@dni_Cliente", id);
                cmd.ExecuteNonQuery();
                mensaje = "Registro eliminado";
            }
            catch (SqlException ex)
            {
                mensaje = ex.Message;
            }
            finally
            {
                cn.Close();
            }


            //Envio vendedor a la vista
            return RedirectToAction("ClientesMntIndex");
        }

    }
}