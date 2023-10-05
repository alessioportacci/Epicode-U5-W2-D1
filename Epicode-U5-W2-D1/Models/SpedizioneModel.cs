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
        [Required]
        [Display(Name = "Nome destinatario")]
        public string NomeDestinatario { get; set; }
        [Required]
        [Display(Name = "Codice Fiscale destinatario")]
        public string CFDestinatario { get; set; }
        [Required]
        public double Peso {  get; set; }
        [Required]
        public string Destinazione { get; set; }
        [Required]
        [Display(Name = "Data di spedizione")]
        public string DataSpedizione { get; set; }
        [Required]
        [Display(Name = "Data di arrivo prevista")]
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