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
    public class ProveedoresMntController : Controller
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["conex"].ConnectionString);


        public ActionResult ProveedoresMntIndex()
        {
            //Mostrar lista de productos
            return View(proveedores());
        }

        //Crear un create tipo get
        public ActionResult ProveedoresMntCreate()
        {
            //Enviar lista de ciudades
            ViewBag.ciudades = new SelectList(ciudades(), "id_ciudad", "nombre_ciudad");
            return View(new Proveedor());

        }

        //Crear un create tipo post
        [HttpPost]
        public ActionResult ProveedoresMntCreate(Proveedor reg)
        {
            List<SqlParameter> lista = new List<SqlParameter>()
            {
                //Definir parametros
                new SqlParameter() { ParameterName = "@id", SqlDbType = SqlDbType.VarChar, Value = reg.idProveedor },

                new SqlParameter() { ParameterName = "@nombreCia", SqlDbType = SqlDbType.VarChar, Value = reg.nombreCia },

                new SqlParameter() { ParameterName = "@nombreContacto", SqlDbType = SqlDbType.VarChar, Value = reg.nombreContacto },

                new SqlParameter() { ParameterName = "@direccion", SqlDbType = SqlDbType.VarChar, Value = reg.direccion},

                new SqlParameter() { ParameterName = "@ciudad", SqlDbType = SqlDbType.VarChar, Value = reg.id_ciudad},

                new SqlParameter() { ParameterName = "@telefono", SqlDbType = SqlDbType.VarChar, Value = reg.telefono}


            };

            //Proceso de ejecucion llamando al metodo CRUD(recibe 2 parametros)
            ViewBag.mensaje = CRUD("sp_agregar_proveedor", lista);

            //la pagina sera refrescada, reenvia la lista de ciudades
            ViewBag.ciudades = new SelectList(ciudades(), "id_ciudad", "nombre_ciudad",reg.id_ciudad);

            return View(reg);
        }


        IEnumerable<Proveedor> proveedores()
        {
            //Crear lista temporal
            List<Proveedor> temporal = new List<Proveedor>();

            //Abrir conexion
            cn.Open();

            //Comando sql para poder manipular sentencias sql
            SqlCommand cmd = new SqlCommand("sp_Proveedor", cn);
            //Indicar comando que se va a manejar
            cmd.CommandType = CommandType.StoredProcedure;
            //Data readar para almacenar datos 
            SqlDataReader dr = cmd.ExecuteReader();
            //Luego de ejecutar cmd, leer datos mediante bucle
            while (dr.Read())
            {
                Proveedor reg = new Proveedor();
                reg.idProveedor = dr.GetString(0);
                reg.nombreCia = dr.GetString(1);
                reg.nombreContacto = dr.GetString(2);
                reg.direccion = dr.GetString(3);
                reg.id_ciudad = dr.GetString(4);
                reg.telefono = dr.GetString(5);

                //Forma alternativa -> reg.nombre = dr[""].ToString();
                temporal.Add(reg);
            }

            //Cerrar conexiones
            dr.Close();
            cn.Close();
            return temporal;
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


        public ActionResult ProveedoresMntEdit(string id)
        {
            Proveedor reg = proveedores().Where(x => x.idProveedor == id).FirstOrDefault();
            ViewBag.ciudades = new SelectList(ciudades(), "id_ciudad", "nombre_ciudad", reg.id_ciudad);

            return View(reg);
        }


        [HttpPost]
        public ActionResult ProveedoresMntEdit(Proveedor reg)
        {
            //Crear lista de parametros
            List<SqlParameter> lista = new List<SqlParameter>()
            {
                //Definir parametros
                new SqlParameter() { ParameterName = "@id", SqlDbType = SqlDbType.VarChar, Value = reg.idProveedor },

                new SqlParameter() { ParameterName = "@nombreCia", SqlDbType = SqlDbType.VarChar, Value = reg.nombreCia },

                new SqlParameter() { ParameterName = "@nombreContacto", SqlDbType = SqlDbType.VarChar, Value = reg.nombreContacto },

                new SqlParameter() { ParameterName = "@direccion", SqlDbType = SqlDbType.VarChar, Value = reg.direccion},

                new SqlParameter() { ParameterName = "@ciudad", SqlDbType = SqlDbType.VarChar, Value = reg.id_ciudad},

                new SqlParameter() { ParameterName = "@telefono", SqlDbType = SqlDbType.VarChar, Value = reg.telefono}

            };

            //Ejecutar 
            ViewBag.mensaje = CRUD("sp_actualizar_proveedor", lista);

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
    }
}