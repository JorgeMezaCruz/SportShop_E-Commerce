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
    public class ProductosMntController : Controller
    {

        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["conex"].ConnectionString);


        IEnumerable<Producto> Listado(string nombre, List<SqlParameter> parameters = null)
        {
            List<Producto> temporal = new List<Producto>();

            SqlCommand cmd = new SqlCommand(nombre, cn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
                cmd.Parameters.AddRange(parameters.ToArray());
            cn.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Producto reg = new Producto();
                reg.idProducto = dr.GetInt32(0);
                reg.nomProducto = dr.GetString(1);
                reg.idProveedor = dr.GetString(2);
                reg.idCategoria = dr.GetString(3);
                reg.precioUnidad = dr.GetDecimal(4);
                reg.stockProducto = dr.GetInt16(5);
                temporal.Add(reg);
            }

            dr.Close();
            cn.Close();
            return temporal;
        }

        public ActionResult ProductosReporte(string nombre = "", int p = 0)
        {
            List<SqlParameter> pars = new List<SqlParameter>()
            {
                new SqlParameter{ParameterName="@nombre",SqlDbType=SqlDbType.VarChar,Value=nombre}
            };

            IEnumerable<Producto> temporal = Listado("usp_Producto_Nombre", pars);
            int filas = 4;
            int n = temporal.Count();
            int pags = n % filas > 0 ? n / filas + 1 : n / filas;
            ViewBag.pags = pags;
            ViewBag.p = p;

            return View(temporal.Skip(p * filas).Take(filas));
        }




        public ActionResult ProductosMntIndex()
        {
            //Mostrar lista de productos
            return View(Productos());
        }

        //Crear un create tipo get
        public ActionResult ProductosMntCreate()
        {

            ViewBag.proveedores = new SelectList(proveedores(), "idProveedor", "nombreCia");
            ViewBag.categorias = new SelectList(categorias(), "idCategoria", "nombreCategoria");


            return View(new Producto());

        }

        //Crear un create tipo post
        [HttpPost]
        public ActionResult ProductosMntCreate(Producto reg)
        {
            List<SqlParameter> lista = new List<SqlParameter>()
            {
                //Definir parametros

                new SqlParameter() { ParameterName = "@id", SqlDbType = SqlDbType.Int, Value = reg.idProducto },

                new SqlParameter() { ParameterName = "@nombre", SqlDbType = SqlDbType.VarChar, Value = reg.nomProducto },

                new SqlParameter() { ParameterName = "@proveedor", SqlDbType = SqlDbType.VarChar, Value = reg.idProveedor },

                new SqlParameter() { ParameterName = "@categoria", SqlDbType = SqlDbType.VarChar, Value = reg.idCategoria},

                new SqlParameter() { ParameterName = "@precio", SqlDbType = SqlDbType.Decimal, Value = reg.precioUnidad},

                new SqlParameter() { ParameterName = "@stock", SqlDbType = SqlDbType.SmallInt, Value = reg.stockProducto}


            };

            //Proceso de ejecucion llamando al metodo CRUD(recibe 2 parametros)
            ViewBag.mensaje = CRUD("sp_agregar_producto", lista);

            //la pagina sera refrescada, reenvia la lista de proveedores, categorias y elemento seleccionado
            ViewBag.proveedores = new SelectList(proveedores(), "idProveedor", "nombreCia", reg.idProveedor);
            ViewBag.categorias = new SelectList(categorias(), "idCategoria", "nombreCategoria", reg.idCategoria);
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

                //Forma alternativa -> reg.nombre = dr["nombrePais"].ToString();
                temporal.Add(reg);
            }

            //Cerrar conexiones
            dr.Close();
            cn.Close();
            return temporal;

        }


        IEnumerable<Producto> Productos()
        {
            //Crear lista temporal
            List<Producto> temporal = new List<Producto>();

            //Abrir conexion
            cn.Open();

            //Comando sql para poder manipular sentencias sql
            SqlCommand cmd = new SqlCommand("sp_Productos", cn);
            //Indicar comando que se va a manejar
            cmd.CommandType = CommandType.StoredProcedure;
            //Data readar para almacenar datos 
            SqlDataReader dr = cmd.ExecuteReader();
            //Luego de ejecutar cmd, leer datos mediante bucle
            while (dr.Read())
            {
                Producto reg = new Producto();
                reg.idProducto = dr.GetInt32(0);
                reg.nomProducto = dr.GetString(1);
                reg.idProveedor = dr.GetString(2);
                reg.idCategoria = dr.GetString(3);
                reg.precioUnidad = dr.GetDecimal(4);
                reg.stockProducto = dr.GetInt16(5);
                temporal.Add(reg);
            }

            //Cerrar conexiones
            dr.Close();
            cn.Close();
            return temporal;
        }


        public ActionResult ProductosMntEdit(int id)
        {
            Producto reg = Productos().Where(x => x.idProducto == id).FirstOrDefault();
            ViewBag.proveedores = new SelectList(proveedores(), "idProveedor", "nombreCia", reg.idProveedor);
            ViewBag.categorias = new SelectList(categorias(), "idCategoria", "nombreCategoria", reg.idCategoria);
            return View(reg);
        }


        [HttpPost]
        public ActionResult ProductosMntEdit(Producto reg)
        {
            //Crear lista de parametros
            List<SqlParameter> lista = new List<SqlParameter>()
            {
                //Definir parametros
                new SqlParameter() { ParameterName = "@id", SqlDbType = SqlDbType.Int, Value = reg.idProducto },

                new SqlParameter() { ParameterName = "@nombre", SqlDbType = SqlDbType.VarChar, Value = reg.nomProducto },

                new SqlParameter() { ParameterName = "@proveedor", SqlDbType = SqlDbType.VarChar, Value = reg.idProveedor },

                new SqlParameter() { ParameterName = "@categoria", SqlDbType = SqlDbType.VarChar, Value = reg.idCategoria},

                new SqlParameter() { ParameterName = "@precio", SqlDbType = SqlDbType.Decimal, Value = reg.precioUnidad},

                new SqlParameter() { ParameterName = "@stock", SqlDbType = SqlDbType.SmallInt, Value = reg.stockProducto}

            };

            //Ejecutar 
            ViewBag.mensaje = CRUD("sp_actualizar_producto", lista);

            ViewBag.proveedores = new SelectList(proveedores(), "idProveedor", "nombreCia", reg.idProveedor);
            ViewBag.categorias = new SelectList(categorias(), "idCategoria", "nombreCategoria", reg.idCategoria);

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

        public ActionResult ProductosMntDelete(string id)
        {

            string mensaje = "";
            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("Delete FROM ec_productos where idProducto = @id", cn);
                cmd.Parameters.AddWithValue("@id", id);
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

            return RedirectToAction("ProductosMntIndex");
        }
    }
}