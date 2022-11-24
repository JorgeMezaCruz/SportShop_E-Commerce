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
    public class CiudadesMntController : Controller
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["conex"].ConnectionString);


        public ActionResult CiudadesMntIndex()
        {
            //Mostrar lista de productos
            return View(ciudades());
        }

        //Crear un create tipo get
        public ActionResult CiudadesMntCreate()
        {
            return View(new Ciudades());

        }

        //Crear un create tipo post
        [HttpPost]
        public ActionResult CiudadesMntCreate(Ciudades reg)
        {
            List<SqlParameter> lista = new List<SqlParameter>()
            {
                //Definir parametros
                new SqlParameter() { ParameterName = "@id_ciudad", SqlDbType = SqlDbType.VarChar, Value = reg.id_ciudad },
                new SqlParameter() { ParameterName = "@nombre_ciudad", SqlDbType = SqlDbType.VarChar, Value = reg.nombre_ciudad }

            };

            //Proceso de ejecucion llamando al metodo CRUD(recibe 2 parametros)
            ViewBag.mensaje = CRUD("sp_agregar_ciudad", lista);

            //la pagina sera refrescada, reenvia la lista de proveedores, categorias y elemento seleccionado
            return View(reg);
        }

        IEnumerable<Ciudades>ciudades()
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


        public ActionResult CiudadesMntEdit(string id)
        {
            Ciudades reg = ciudades().Where(x => x.id_ciudad == id).FirstOrDefault();
            return View(reg);
        }


        [HttpPost]
        public ActionResult CiudadesMntEdit(Ciudades reg)
        {
            //Crear lista de parametros
            List<SqlParameter> lista = new List<SqlParameter>()
            {
                //Definir parametros
                new SqlParameter() { ParameterName = "@id_ciudad", SqlDbType = SqlDbType.VarChar, Value = reg.id_ciudad },
                new SqlParameter() { ParameterName = "@nombre_ciudad", SqlDbType = SqlDbType.VarChar, Value = reg.nombre_ciudad }

            };

            //Ejecutar 
            ViewBag.mensaje = CRUD("sp_actualizar_ciudad", lista);

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