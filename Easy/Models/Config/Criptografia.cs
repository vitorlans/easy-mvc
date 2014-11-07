using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Easy.Models
{
    public class Criptografia
    {
        private static byte[] chave = { };
        private static byte[] iv = { 12, 34, 56, 78, 90, 102, 114, 126 };

        private static string chaveCriptografia = "VitorSebastiaoNathan";

        //Criptografa o Cookie
        public static string Criptografar(string valor)
        {
            DESCryptoServiceProvider des;
            MemoryStream ms;
            CryptoStream cs; byte[] input;

            try
            {
                using (des = new DESCryptoServiceProvider())
                {

                    using (ms = new MemoryStream())
                    {

                        input = Encoding.UTF8.GetBytes(valor);
                        chave = Encoding.UTF8.GetBytes(chaveCriptografia.Substring(0, 8));

                        using (cs = new CryptoStream(ms, des.CreateEncryptor(chave, iv), CryptoStreamMode.Write))
                        {
                            cs.Write(input, 0, input.Length);
                            cs.FlushFinalBlock();
                        }

                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Descriptografa o cookie
        public static string Descriptografar(string valor)
        {
        
            DESCryptoServiceProvider des;
            MemoryStream ms;
            CryptoStream cs; byte[] input;

            try
            {
                des = new DESCryptoServiceProvider();
                ms = new MemoryStream();

                input = new byte[valor.Length];
                input = Convert.FromBase64String(valor.Replace(" ", "+"));

                chave = Encoding.UTF8.GetBytes(chaveCriptografia.Substring(0, 8));

                cs = new CryptoStream(ms, des.CreateDecryptor(chave, iv), CryptoStreamMode.Write);
                cs.Write(input, 0, input.Length);
                cs.FlushFinalBlock();

                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return null;
            }
        }


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