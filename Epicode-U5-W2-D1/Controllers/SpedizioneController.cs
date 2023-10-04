using Epicode_U5_W2_D1.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Epicode_U5_W2_D1.Controllers
{
    public class SpedizioneController : Controller
    {
        static SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionDb"].ConnectionString.ToString());

        public ActionResult Index(int id)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM [V_Spedizioni] " +
                                                "WHERE PkSpedizione = " + id, conn);
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                while (sqlDataReader.Read())
                    return View(new SpedizioneModel()
                                {
                                    PkSpedizione = Int32.Parse(sqlDataReader["PkSpedizione"].ToString()),
                                    NomeDestinatario = sqlDataReader["NomeDestinatario"].ToString(),
                                    CFDestinatario = sqlDataReader["CFDestinatario"].ToString(),
                                    Peso = Double.Parse(sqlDataReader["Peso"].ToString()),
                                    Destinazione = sqlDataReader["Destinazione"].ToString(),
                                    DataSpedizione = sqlDataReader["DataSpedizione"].ToString(),
                                    DataPrevista = sqlDataReader["DataPrevista"].ToString(),
                                    NomeMittente = sqlDataReader["Nome"].ToString(),
                                    Stato = sqlDataReader["Stato"].ToString(),
                                    username = sqlDataReader["Username"].ToString(),
                                });
            }
            catch 
            { }
            finally 
            { 
                conn.Close();
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Aggiornamenti(int id) 
        {
            List<AggiornamentoModel> aggiornamentiList = new List<AggiornamentoModel>();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM [V_Aggiornamenti]" +
                                                "WHERE PkSpedizione = " + id, conn);
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                while (sqlDataReader.Read())
                    aggiornamentiList.Add(new AggiornamentoModel
                    {
                        PkSpedizione = Int32.Parse(sqlDataReader["PkSpedizione"].ToString()),
                        CittaCorrente = sqlDataReader["CittaCorrente"].ToString(),
                        Stato = sqlDataReader["Stato"].ToString(),
                        DataAggiornamento = DateTime.Parse(sqlDataReader["DataAggiornamento"].ToString()),
                        Aggiornamento = sqlDataReader["DataAggiornamento"].ToString(),
                    });
                return View(aggiornamentiList);
            }
            catch 
            { }
            finally
            {
                conn.Close();
            }

            return RedirectToAction("Index", "Home");
        }


        public ActionResult AggiungiAggiornamento(int id) 
        {
            List<StatiDropdownModel> statiList = new List<StatiDropdownModel>();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM [T_Stati]", conn);
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                while (sqlDataReader.Read())
                     statiList.Add(new StatiDropdownModel(){
                        value = Int32.Parse(sqlDataReader["PkStato"].ToString()),
                        text = sqlDataReader["Stato"].ToString(),
                    });
            }
            catch { }
            finally 
            {
                ViewBag.Stati = statiList;    
                conn.Close(); 
            }

            return View();
        }

        [HttpPost]
        public ActionResult AggiungiAggiornamento(AddAggiornamentoModel agg) 
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO T_Aggiornamento VALUES(@Data, @Aggiornamento, @Citta, @Stato, @Spedizione)", conn);
                cmd.Parameters.AddWithValue("Data", DateTime.Now);
                cmd.Parameters.AddWithValue("Aggiornamento", agg.Aggiornamento);
                cmd.Parameters.AddWithValue("Citta", agg.CittaCorrente);
                cmd.Parameters.AddWithValue("Stato", agg.FkStato);
                cmd.Parameters.AddWithValue("Spedizione", agg.FkSpedizione);
                cmd.ExecuteNonQuery();
            }
            catch
            { }
            finally
            {
                conn.Close();
            }

            return RedirectToAction("Aggiornamenti", new { id = agg.FkSpedizione });
        }

        [Authorize]
        public ActionResult Spedizioni () 
        {
            return View();
        }

        public JsonResult SpedizioniFilter(string User, string Citta, string Cliente)
        {
            List<SpedizioneModel> spedizioniList = new List<SpedizioneModel>();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM [V_Spedizioni] WHERE 1=1", conn);
                if (User != "")
                    cmd.CommandText += String.Concat("AND username = '", User, "'");
                if (Citta != "") 
                    cmd.CommandText += String.Concat("AND Destinazione = '" , Citta, "'");
                if (Cliente != "")
                    cmd.CommandText += String.Concat("AND Nome = '", Cliente, "'");

                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                while (sqlDataReader.Read())
                    spedizioniList.Add(new SpedizioneModel
                    {
                        PkSpedizione = Int32.Parse(sqlDataReader["PkSpedizione"].ToString()),
                        NomeDestinatario = sqlDataReader["NomeDestinatario"].ToString(),
                        CFDestinatario = sqlDataReader["CFDestinatario"].ToString(),
                        Peso = Double.Parse(sqlDataReader["Peso"].ToString()),
                        Destinazione = sqlDataReader["Destinazione"].ToString(),
                        DataSpedizione = sqlDataReader["DataSpedizione"].ToString(),
                        DataPrevista = sqlDataReader["DataPrevista"].ToString(),
                        NomeMittente = sqlDataReader["Nome"].ToString(),
                        Stato = sqlDataReader["Stato"].ToString(),
                    });
            }
            catch
            { }
            finally
            {
                conn.Close();
            }

            return Json(spedizioniList, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult AggiungiSpedizione()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AggiungiSpedizione(SpedizioneModel spedizione)
        {
            return RedirectToAction("Spedizioni");
        }

    }
}