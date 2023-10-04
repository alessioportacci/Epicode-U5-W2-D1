using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Epicode_U5_W2_D1.Models
{
    public class SpedizioneModel
    {
        public int PkSpedizione { get; set; }
        public string NomeDestinatario { get; set; }
        public string CFDestinatario { get; set; }
        public double Peso {  get; set; }
        public string Destinazione { get; set; }
        public string DataSpedizione { get; set; }
        public string DataPrevista { get; set; }
        public string NomeMittente {  get; set; }
        public string Stato { get; set; }
        public string username { get; set; }
    }

    public class GetSpedizioneModel
    {
        [Required(ErrorMessage = "Codice identificativo obbligatorio")]
        [Display(Name = "Cod. identificativo spedizione")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Codice fiscale obbligatorio")]
        [Display (Name = "Codice Fiscale")]
        public string CF { get; set; }
    }
}