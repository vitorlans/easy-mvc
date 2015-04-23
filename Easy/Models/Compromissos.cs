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
        public static Compromissos SplitParticipantes(Compromissos Comp, string _participantes)//Como separar/dividir uma string em array/variáveis usando C# (C Sharp)
        {
            int _iParticipantes = _participantes.Length;
            string _caracter = _participantes.Substring(_iParticipantes - 1, 1);

            if (_caracter == ";")
                _participantes = _participantes.Substring(0, _iParticipantes - 1);

            string[] _strArray;
            DAOUsuario dUser = new DAOUsuario();
            if (_participantes.Contains(";") || _participantes.Contains(" "))
            {
                char[] strSep1 = { ';' }; //Caracter usado para separar o texto
                _strArray = _participantes.Split(strSep1);//Onde ficará o resultado da separação


                for (int _count = 0; _count < _strArray.Length; _count++)
                {
                    Comp.Participantes.Add(new Usuario { Email = _strArray[_count] });
                    if (dUser.RecuperaUsuarioEmail(Comp.Participantes[_count].Email) != null)
                        Comp.Participantes[_count] = dUser.RecuperaUsuarioEmail(Comp.Participantes[_count].Email);
                }
            }
            else
            {
                Comp.Participantes.Add(new Usuario { Email = _participantes });
                Comp.Participantes[0] = dUser.RecuperaUsuarioEmail(Comp.Participantes[0].Email);
            }
            return Comp;
        }
    }
}