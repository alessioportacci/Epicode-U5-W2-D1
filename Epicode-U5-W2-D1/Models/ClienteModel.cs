﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Epicode_U5_W2_D1.Models
{
    public class ClienteModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Display(Name = "Nome Privato o dell'azienda")]
        public string Nome { get; set; }
        public bool Privato { get; set; }
        [Required]
        [Display(Name = "Codice Fiscale / Partita IVA")]
        public string CF_PIVA { get; set; }
        [Required]
        public string Ruolo { get; set; }
    }

    public class CredenzialiClienteModel
    {
        [Display(Name = "Inserire Username:")]
        [Required(ErrorMessage = "Username obbligatorio")]
        public string username { get; set; }

        [Display(Name = "Inserire Password:")]  
        [Required(ErrorMessage = "Password obbligatoria")]
        public string password { get; set; }
    }
}