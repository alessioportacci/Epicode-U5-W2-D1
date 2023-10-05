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
                        Aggiornamento = sqlDataReader["Aggiornamento"].ToString(),
                    });
                return PartialView(aggiornamentiList);
            }
            catch 
            { }
            finally
            {
                conn.Close();
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
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

        [Authorize]
        [HttpPost]
        public ActionResult AggiungiAggiornamento(AddAggiornamentoModel agg) 
        {
            if(ModelState.IsValid)
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

            return RedirectToAction("Index", new { id = agg.FkSpedizione });
        }

        [Authorize]
        public ActionResult Spedizioni () 
        {
            return View();
        }

        [Authorize]
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

        [Authorize]
        public ActionResult AggiungiSpedizione()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult AggiungiSpedizione(SpedizioneModel spedizione)
        {
            if(ModelState.IsValid)
                try
                {
                    string appoggio = string.Empty;

                    conn.Open();

                    //Prendo l'id del cliente
                    SqlCommand cmd = new SqlCommand("SELECT IdCliente, Username FROM T_Clienti WHERE Username = '" + User.Identity.Name + "'", conn);
                    SqlDataReader sqlDataReader = cmd.ExecuteReader();
                    while (sqlDataReader.Read())
                        appoggio += sqlDataReader["IdCliente"].ToString();
                
                    conn.Close();
                    conn.Open();
                
                    //Inserisco la spedizione
                    cmd = new SqlCommand("INSERT INTO T_Spedizioni VALUES(@Destinatario, @CFDestinatario, @Peso, @Destinazione, @DataSpedizione, @DataPrevista, @FkCliente)", conn);
                    cmd.Parameters.AddWithValue("Destinatario", spedizione.NomeDestinatario);
                    cmd.Parameters.AddWithValue("CFDestinatario", spedizione.CFDestinatario);
                    cmd.Parameters.AddWithValue("Peso", spedizione.Peso);
                    cmd.Parameters.AddWithValue("Destinazione", spedizione.Destinazione);
                    cmd.Parameters.AddWithValue("DataSpedizione", DateTime.Parse(spedizione.DataSpedizione));
                    cmd.Parameters.AddWithValue("DataPrevista", DateTime.Parse(spedizione.DataSpedizione));
                    cmd.Parameters.AddWithValue("FkCliente", appoggio);
                    cmd.ExecuteNonQuery();

                    conn.Close();
                    conn.Open();

                    //Prendo il Pk della spedizione
                    cmd = new SqlCommand("SELECT TOP 1 PkSpedizione FROM T_Spedizioni ORDER BY PkSpedizione DESC", conn);
                    sqlDataReader = cmd.ExecuteReader();
                    while (sqlDataReader.Read())
                        appoggio = sqlDataReader["PkSpedizione"].ToString();

                    conn.Close();
                    conn.Open();

                    //Inserisco la nuova spedizione
                    cmd = new SqlCommand(String.Concat("INSERT INTO T_Aggiornamento VALUES('", DateTime.Now.Date,"', 'Ordine registrato', '_', 1, @FkSpedizione)"), conn);
                    cmd.Parameters.AddWithValue("FkSpedizione", appoggio);
                    cmd.ExecuteNonQuery();

                }
                catch
                { }
                finally
                {
                    conn.Close();
                }
            return RedirectToAction("Spedizioni");
        }

    }
}