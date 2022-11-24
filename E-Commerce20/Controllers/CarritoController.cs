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
    public class CarritoController : Controller
    {

        // GET: Carrito
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["conex"].ConnectionString);


        public ActionResult IndexCarrito()
        {
            //Sesion
            if (Session["carrito"] == null)
            {
                List<registro> detalle = new List<registro>();
                Session["carrito"] = detalle;
            }
            return View(productos());
        }


        //GET : SELECCIONAR
        public ActionResult Seleccionar(int? id = null)
        {
            return View(productos().Where(x => x.idProducto.Equals(id)).FirstOrDefault());
        }

        //POST : SELECCIONAR
        [HttpPost]
        public ActionResult Seleccionar(int id)
        {
            Producto reg = productos().Where(x => x.idProducto == id).FirstOrDefault();

            registro it = new registro();
            it.idProducto = reg.idProducto;
            it.nomProducto = reg.nomProducto;
            it.precioUnidad = reg.precioUnidad;
            it.stockProducto = reg.stockProducto;
            it.cantidad = 1;
            it.monto = reg.precioUnidad;

            List<registro> detalle = (List<registro>)Session["carrito"];
            //Vaciar el carrito a una lista local
            detalle.Add(it);
            Session["carrito"] = detalle;
            return RedirectToAction("IndexCarrito");
        }
        public ActionResult Comprar()
        {
            List<registro> detalle = (List<registro>)Session["carrito"]; //Vaciar el carrito a una lista local
            decimal mt = 0;
            foreach (registro it in detalle)
            {
                mt += it.monto;
            }

            ViewBag.mt = mt;
            return View(detalle);

        }


        public ActionResult Elimina(int? id = null)
        {
            List<registro> detalle = (List<registro>)Session["carrito"];
            foreach (registro it in detalle)
            {
                if (it.idProducto.Equals(id))
                {
                    detalle.Remove(it);
                    break;
                }
            }
            Session["carrito"] = detalle;
            return RedirectToAction("Comprar");
        }


        public ActionResult Pago()
        {
            List<registro> detalle = (List<registro>)Session["carrito"];
            decimal mt = 0;
            foreach (registro it in detalle) { mt += it.monto; }
            ViewBag.mt = mt;
            return View(detalle);
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

        [HttpPost]
        public ActionResult Pago(string dni, string nom)
        {
            int id = Autogenerar();
            cn.Open();
            SqlTransaction tr = cn.BeginTransaction(IsolationLevel.Serializable);
            try
            {
                SqlCommand cmd = new SqlCommand(
                    "Insert into tb_pedido Values(@id,@f,@dni,@nom,@monto)", cn, tr);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@f", SqlDbType.DateTime).Value = DateTime.Today;
                cmd.Parameters.Add("@dni", SqlDbType.VarChar, 8).Value = dni;
                cmd.Parameters.Add("@nom", SqlDbType.VarChar, 50).Value = nom;
                cmd.Parameters.Add("@monto", SqlDbType.Decimal).Value = Monto();
                cmd.ExecuteNonQuery();

                //Detalle del pedido
                List<registro> detalle = (List<registro>)Session["carrito"];
                foreach (registro it in detalle)
                {
                    cmd = new SqlCommand("Insert into tb_detapedido values (@id,@prod,@pre,@q)", cn, tr);
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@prod", SqlDbType.VarChar).Value = it.idProducto;
                    cmd.Parameters.Add("@pre", SqlDbType.Decimal).Value = it.precioUnidad;
                    cmd.Parameters.Add("@q", SqlDbType.Int).Value = it.cantidad;
                    cmd.ExecuteNonQuery();
                }
                tr.Commit();

            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                tr.Rollback();
            }
            finally
            {
                cn.Close();
            }

            return RedirectToAction("IndexCarrito");
        }



        int Autogenerar()
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand("sp_autogenera", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            int n = Int32.Parse(cmd.ExecuteScalar().ToString());
            cn.Close();
            return n;
        }

        decimal Monto()
        {
            List<registro> detalle = (List<registro>)Session["carrito"];
            decimal mt = 0;
            foreach (registro it in detalle) { mt += it.monto; }
            return mt;
        }


        IEnumerable<Producto> productos()
        {

            List<Producto> temporal = new List<Producto>();
            cn.Open();
            SqlCommand cmd = new SqlCommand("sp_ProductosT", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Producto reg = new Producto();
                reg.idProducto = dr.GetInt32(0);
                reg.nomProducto = dr.GetString(1);
                reg.precioUnidad = dr.GetDecimal(2);
                reg.stockProducto = dr.GetInt16(3);
                temporal.Add(reg);
            }
            dr.Close();
            cn.Close();

            return temporal;
        }



        IEnumerable<pedidos> Listado(string nombre, List<SqlParameter> parameters = null)
        {
            List<pedidos> temporal = new List<pedidos>();

            SqlCommand cmd = new SqlCommand(nombre, cn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
                cmd.Parameters.AddRange(parameters.ToArray());
            cn.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                pedidos reg = new pedidos();
                reg.idpedido = dr.GetInt32(0);
                reg.fecha = dr.GetDateTime(1);
                reg.dnicli = dr.GetString(2);
                reg.cliente = dr.GetString(3);
                reg.direccion = dr.GetString(4);
                reg.monto = dr.GetDecimal(5);
                temporal.Add(reg);
            }

            dr.Close();
            cn.Close();
            return temporal;
        }

        public ActionResult PedidosYear(int y = 0, int p = 0)
        {
            List<SqlParameter> pars = new List<SqlParameter>()
            {
                new SqlParameter{ParameterName="@y",SqlDbType=SqlDbType.Int,Value=y}
            };

            IEnumerable<pedidos> temporal = Listado("usp_Pedido_Year", pars);
            int filas = 6;
            int n = temporal.Count();
            int pags = n % filas > 0 ? n / filas + 1 : n / filas;
            ViewBag.pags = pags;
            ViewBag.p = p;

            return View(temporal.Skip(p * filas).Take(filas));

        }

    }
}