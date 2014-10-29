using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Easy.Models
{
    public class Comentarios
    {
        public int IdComent { get; set; }
        public Tarefas IdTarefa { get; set; }
        public string Comentario { get; set; }
    }
}