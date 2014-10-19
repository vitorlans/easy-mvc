using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Easy.Models
{
    public class Usuario
    {
        public int IdUser { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Senha1 { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Cep { get; set; }
        public string Telefone { get; set; }
        public bool UsuarioSistema { get; set; }
        public bool LiberaConvite { get; set; }
        public bool Status { get; set; }


        public List<Usuario> ContatosLTotal()
        {
            List<Usuario> lista = new List<Usuario>();

            try
            {
                var x = 0;
               while(x < 3)
                {
                    lista.Add
                        (
                        new Usuario
                        {
                            UsuarioSistema = true,
                            Nome = "Vitor Santos",
                            Sobrenome = "Silva",
                            Telefone = "19 3582-5664",
                            Email = "vitor_hs@live.com",
                            Endereco = "Mauricio Affonso Moreno",
                            Cidade = "Santa Rita do Passa Quatro",
                            Bairro = "Vila Norte",
                            Cep = "13670000",
                            LiberaConvite = true
                        }
                        );
                    x++;
                }
               lista[1].IdUser = 1;
               lista[1].Nome = "Sebastião Purcini";

               lista[2].IdUser = 2;
               lista[2].Nome = "Nathan Bernardes";


            }
            catch { }

            return lista;
        }

    }
}