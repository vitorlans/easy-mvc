using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Easy.Models
{
    public class Tarefas
    {
        public int IdTarefa { get; set; }

        [Required(ErrorMessage = "Deve ser informada a Descrição da Tarefa.")]
        [Display(Name = "Descrição")]
        public string   Descricao   { get; set; }

        [Required(ErrorMessage = "A Data de Início da Tarefa deve ser informada.")]
        [Display(Name = "Data de Início")]
        public string   DtInicio    { get; set; }

        [Display(Name = "Data de Término")]
        public string   DtFim       { get; set; }

        public string   Prioridade  { get; set; }
        public string   Status      { get; set; }
        public Usuario  Criador     { get; set; }
        
        [Remote("ValidaContatos", "Tarefas", ErrorMessage = "Este email não faz parte da sua lista de Contatos.")]
        public string  Relacionado { get; set; }
        public Empresas Empresa     { get; set; }//ALTERAR PARA CLASSE EMPRESA
    }
}