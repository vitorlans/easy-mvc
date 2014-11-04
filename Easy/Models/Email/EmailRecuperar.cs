using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Postal;

namespace Easy.Models
{
    public class EmailRecuperar : Email
    {

        public string Para { get; set; }
        public string Assunto { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Quando { get; set; }
        public string Mensagem { get; set; }
        public string UrlAceita { get; set; }
        public string UrlRejeita { get; set; }
        public string Login { get; set; }  
        public string Senha { get; set; }

    }
}