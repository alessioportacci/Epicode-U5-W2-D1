using Epicode_U5_W2_D1.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Epicode_U5_W2_D1.Controllers
{
    public class HomeController : Controller
    {

        static SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionDb"].ConnectionString.ToString());

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetSpedizione()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetSpedizione(GetSpedizioneModel infoSpedizione)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT [PkSpedizione] FROM [T_Spedizioni] " +
                                                "WHERE @Id = PkSpedizione AND @CF = CFDestinatario", conn);
                cmd.Parameters.AddWithValue("Id", infoSpedizione.Id);
                cmd.Parameters.AddWithValue("CF", infoSpedizione.CF);
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.HasRows)
                    while (sqlDataReader.Read())
                        return RedirectToAction("Index","Spedizione", new {id = sqlDataReader["PkSpedizione"] });
            }
            catch 
            {
                ViewBag.Message = "C'è stato un errore nella ricerca del pacco";
                return View();
            }
            finally
            {
                conn.Close();
            }

            ViewBag.Message = "Non è stato possibile trovare il pacco con ID e CF specificati";
            return View();
        }

        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(CredenzialiClienteModel credenziali)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Username, Password FROM [T_Clienti] " +
                                                "WHERE @Username = Username AND @Password = Password", conn);
                cmd.Parameters.AddWithValue("Username", credenziali.username);
                cmd.Parameters.AddWithValue("Password", credenziali.password);
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    FormsAuthentication.SetAuthCookie(credenziali.username, false);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                ViewBag.Message = "C'è stato un errore con il login";
                return View();
            }
            finally
            {
                conn.Close();
            }

            ViewBag.Message = "Credenziali non valide";
            return View();
        }


        public ActionResult Logout() 
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}