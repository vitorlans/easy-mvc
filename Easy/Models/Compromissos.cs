using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Easy.Models
{
    public class Compromissos
    {
        public int IdComp {get;set;}
        [Required(ErrorMessage= "O Título do Compromisso deve ser informado.")]
        public string Titulo {get;set;}
        [Required(ErrorMessage= "A Descrição do Compromisso deve ser informada.")]
        public string Descricao {get;set;}
        [Required(ErrorMessage = "A Data de Início do Compromisso deve ser informada.")]
        public string DataInicio {get;set;}
        [Required(ErrorMessage = "A Data de Termino do Compromisso deve ser informada.")]
        public string DataTermino {get;set;}
        //Status será inserido como (T)erminado, (C)ancelado ou em (A)ndamento
        public string Status {get;set;}
        public Usuario Usuario {get;set;}
        public List<Usuario> Participantes { get; set; }
        public Empresas Empresa { get; set; }


        public static string FormataTexto(string c) {

            char primeira = char.ToUpper(c[0]);
            c = primeira + c.Substring(1);
            return c;
        }
        public static Compromissos SplitParticipantes(Compromissos Comp, string participantes)//Como separar/dividir uma string em array/variáveis usando C# (C Sharp)
        {
            char[] strSep1 = { ';' }; //Caracter usado para separar o texto
            string[] strArray = participantes.Split(strSep1);//Onde ficará o resultado da separação

            for (int count = 0; count <= strArray.Length - 1; count++)//Mostra os valores existentes no array
            {
                DAOUsuario dUser = new DAOUsuario();
                Comp.Participantes.Add(new Usuario { Email = strArray[count] });
                if (dUser.RecuperaUsuarioEmail(Comp.Participantes[count].Email) != null)
                    Comp.Participantes[count] = dUser.RecuperaUsuarioEmail(Comp.Participantes[count].Email);

            }
            return Comp;
        }
    }
}