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
    public class TrabajadoresMntController : Controller
    {

        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["conex"].ConnectionString);



        public ActionResult TrabajadoresMntIndex()
        {
            return View(trabajadores());
        }

        //Crear un create tipo get
        public ActionResult TrabajadoresMntCreate()
        {
            ViewBag.ciudades = new SelectList(ciudades(), "id_ciudad", "nombre_ciudad");
            return View(new Trabajadores());

        }

        //Crear un create tipo post
        [HttpPost]
        public ActionResult TrabajadoresMntCreate(Trabajadores reg)
        {
            List<SqlParameter> lista = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@dni_trab", SqlDbType = SqlDbType.VarChar, Value = reg.DniTrab },

                new SqlParameter() { ParameterName = "@nombre_trab", SqlDbType = SqlDbType.VarChar, Value = reg.NombreTrab },

                new SqlParameter() { ParameterName = "@apellido_trab", SqlDbType = SqlDbType.VarChar, Value = reg.ApellidosTrab },

                new SqlParameter() { ParameterName = "@direccion_trab", SqlDbType = SqlDbType.VarChar, Value = reg.DireccionTrab},

                new SqlParameter() { ParameterName = "@id_ciudad", SqlDbType = SqlDbType.VarChar, Value = reg.id_ciudad},

                new SqlParameter() { ParameterName = "@telefono_trab", SqlDbType = SqlDbType.VarChar, Value = reg.TelefonoTrab},

                new SqlParameter() { ParameterName = "@correo_trab", SqlDbType = SqlDbType.VarChar, Value = reg.CorreoTrab},

                new SqlParameter() { ParameterName = "@password_trab", SqlDbType = SqlDbType.VarChar, Value = reg.PasswordTrab}
            };

            //Proceso de ejecucion llamando al metodo CRUD(recibe 2 parametros)
            ViewBag.mensaje = CRUD("sp_agregar_trabajador", lista);

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


        public ActionResult TrabajadoresMntEdit(string id)
        {
            Trabajadores reg = trabajadores().Where(x => x.DniTrab == id).FirstOrDefault();
            ViewBag.ciudades = new SelectList(ciudades(), "id_ciudad", "nombre_ciudad", reg.id_ciudad);
            return View(reg);
        }


        [HttpPost]
        public ActionResult TrabajadoresMntEdit(Trabajadores reg)
        {
            //Crear lista de parametros
            List<SqlParameter> lista = new List<SqlParameter>()
            {
                //Definir parametros
                new SqlParameter() { ParameterName = "@dni_trab", SqlDbType = SqlDbType.VarChar, Value = reg.DniTrab },

                new SqlParameter() { ParameterName = "@nombre_trab", SqlDbType = SqlDbType.VarChar, Value = reg.NombreTrab },

                new SqlParameter() { ParameterName = "@apellido_trab", SqlDbType = SqlDbType.VarChar, Value = reg.ApellidosTrab },

                new SqlParameter() { ParameterName = "@direccion_trab", SqlDbType = SqlDbType.VarChar, Value = reg.DireccionTrab},

                new SqlParameter() { ParameterName = "@id_ciudad", SqlDbType = SqlDbType.VarChar, Value = reg.id_ciudad},

                new SqlParameter() { ParameterName = "@telefono_trab", SqlDbType = SqlDbType.VarChar, Value = reg.TelefonoTrab},

                new SqlParameter() { ParameterName = "@correo_trab", SqlDbType = SqlDbType.VarChar, Value = reg.CorreoTrab},

                new SqlParameter() { ParameterName = "@password_trab", SqlDbType = SqlDbType.VarChar, Value = reg.PasswordTrab}
            };

            //Ejecutar 
            ViewBag.mensaje = CRUD("sp_actualizar_trabajador", lista);

            ViewBag.ciudades = new SelectList(ciudades(), "id_ciudad", "nombre_ciudad", reg.id_ciudad);

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


        public ActionResult TrabajadoresMntDelete(string id)
        {

            string mensaje = "";
            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("Delete FROM Trabajadores where DniTrab = @dni_trab", cn);
                cmd.Parameters.AddWithValue("@dni_trab", id);
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
            return RedirectToAction("TrabajadoresMntIndex");
        }



    }
}