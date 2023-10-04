﻿using Epicode_U5_W2_D1.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Epicode_U5_W2_D1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClientiController : Controller
    {

        static SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionDb"].ConnectionString.ToString());

        public ActionResult Index()
        {
            List<ClienteModel> clientiList = new List<ClienteModel>();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM [T_Clienti]", conn);
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                while (sqlDataReader.Read())
                    clientiList.Add(new ClienteModel
                    {
                        Username = sqlDataReader["Username"].ToString(),
                        Password = sqlDataReader["Password"].ToString(),
                        Nome = sqlDataReader["Nome"].ToString(),
                        Privato = Boolean.Parse(sqlDataReader["Privato"].ToString()),
                        CF_PIVA = sqlDataReader["CF_PIVA"].ToString(),
                        Ruolo = sqlDataReader["Username"].ToString()
                    });                
            }
            catch { }
            finally 
            { 
                conn.Close();
            }

            return View(clientiList);
        }

    }
}
