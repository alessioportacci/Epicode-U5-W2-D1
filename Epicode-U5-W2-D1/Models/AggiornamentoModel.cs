using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace Epicode_U5_W2_D1.Models
{
    public class AggiornamentoModel
    {
        public int PkSpedizione { get; set; }
        public string CittaCorrente { get; set; }
        public string Stato { get; set; }
        public DateTime DataAggiornamento { get; set; }
        public string Aggiornamento { get; set; }
    }

    public class AddAggiornamentoModel
    {
        [Required]
        public string Aggiornamento { get; set; }
        [Required]
        [Display(Name = "Città corrente")]
        public string CittaCorrente { get; set; }
        [Display (Name = "Stato")]
        [Required]
        public int FkStato { get; set; }
        public int FkSpedizione { get; set; }
    }

    public class StatiDropdownModel
    {
        public int value { get; set; }
        public string text { get; set; }
    }
}