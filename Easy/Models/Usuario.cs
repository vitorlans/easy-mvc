using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public string UsuarioSistema { get; set; }
        public string LiberaConvite { get; set; }
        public string Status { get; set; }
        public string DataCriacao {get;set;}
        public string Imagem { get; set; }


        //Definido variável com os caracteres utilizados na geração da senha
        private const string SenhaCaracteresValidos = "abcdefghijklmnopqrstuvwxyz1234567890@#!?";

        public static string GerarSenha()
        {
            //Aqui eu defino o número de caracteres que a senha terá
            int tamanho = 8;

            //Aqui pego o valor máximo de caracteres para gerar a senha
            int valormaximo = SenhaCaracteresValidos.Length;

            //Criamos um objeto do tipo randon
            Random random = new Random(DateTime.Now.Millisecond);

            //Criamos a string que montaremos a senha
            StringBuilder senha = new StringBuilder(tamanho);

            //Fazemos um for adicionando os caracteres a senha
            for (int i = 0; i < tamanho; i++)
                senha.Append(SenhaCaracteresValidos[random.Next(0, valormaximo)]);

            //retorna a senha
            return senha.ToString();
        }

    }
}