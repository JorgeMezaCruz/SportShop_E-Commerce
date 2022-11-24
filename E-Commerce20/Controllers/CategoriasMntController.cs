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
    public class CategoriasMntController : Controller
    {

        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["conex"].ConnectionString);


        public ActionResult CategoriasMntIndex()
        {
            //Mostrar lista de categorias
            return View(categorias());
        }

        //Crear un create tipo get
        public ActionResult CategoriasMntCreate()
        {
            return View(new Categorias());

        }

        //Crear un create tipo post
        [HttpPost]
        public ActionResult CategoriasMntCreate(Categorias reg)
        {
            List<SqlParameter> lista = new List<SqlParameter>()
            {
                //Definir parametros
                new SqlParameter() { ParameterName = "@idCategoria", SqlDbType = SqlDbType.VarChar, Value = reg.idCategoria },
                new SqlParameter() { ParameterName = "@nombreCategoria", SqlDbType = SqlDbType.VarChar, Value = reg.nombreCategoria }
            };

            //Proceso de ejecucion llamando al metodo CRUD(recibe 2 parametros)
            ViewBag.mensaje = CRUD("sp_agregar_categoria", lista);

            //la pagina sera refrescada, reenvia la lista de proveedores, categorias y elemento seleccionado
            return View(reg);
        }

        IEnumerable<Categorias> categorias()
        {
            //Crear lista temporal
            List<Categorias> temporal = new List<Categorias>();

            //Abrir conexion
            cn.Open();

            //Comando sql para poder manipular sentencias sql
            SqlCommand cmd = new SqlCommand("sp_Categorias", cn);
            //Indicar comando que se va a manejar
            cmd.CommandType = CommandType.StoredProcedure;
            //Data readar para almacenar datos 
            SqlDataReader dr = cmd.ExecuteReader();
            //Luego de ejecutar cmd, leer datos mediante bucle
            while (dr.Read())
            {
                Categorias reg = new Categorias();
                reg.idCategoria = dr.GetString(0);
                reg.nombreCategoria = dr.GetString(1);
                temporal.Add(reg);
            }

            //Cerrar conexiones
            dr.Close();
            cn.Close();
            return temporal;
        }


        public ActionResult CategoriasMntEdit(string id)
        {
            Categorias reg = categorias().Where(x => x.idCategoria == id).FirstOrDefault();
            return View(reg);
        }


        [HttpPost]
        public ActionResult CategoriasMntEdit(Categorias reg)
        {
            //Crear lista de parametros
            List<SqlParameter> lista = new List<SqlParameter>()
            {
                //Definir parametros
                new SqlParameter() { ParameterName = "@idCategoria", SqlDbType = SqlDbType.VarChar, Value = reg.idCategoria },
                new SqlParameter() { ParameterName = "@nombreCategoria", SqlDbType = SqlDbType.VarChar, Value = reg.nombreCategoria }
            };

            //Ejecutar 
            ViewBag.mensaje = CRUD("sp_actualizar_categoria", lista);

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