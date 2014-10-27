using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Net;

namespace Easy.Models
{
    public class EnvioEmail
    {
        public void EnviarTarefaAtualizada(Tarefas tar)
        {
            //REALIZAR ALTERACAO APÓS TER O LOGIN DO USUARIO, PASSANDO PELOS DADOS DA TAREFA
            SqlCommand busca = new SqlCommand
            (
                "SELECT remetente = (SELECT EMAIL FROM TBUSUARIOS WHERE IDUSER = 1), "
                +"destinatario = (SELECT EMAIL FROM TBUSUARIOS WHERE IDUSER = 2)", Connection.Conectar()
            );

            SqlDataReader dr = busca.ExecuteReader();

            while (dr.Read())
            {

                MailAddress remetente = new MailAddress("easypeoplespi@gmail.com");
                MailAddress destinatario = new MailAddress(dr["destinatario"].ToString());

                MailMessage msg = new MailMessage(remetente, destinatario);

                msg.Priority = MailPriority.Normal;

                msg.IsBodyHtml = true;

                msg.Subject = "Alteração Realizada em Tarefa";

                msg.Body = "Foram realizadas alteraçõoes em Tarefa. Segue abaixo as mudanças:";

                msg.SubjectEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                msg.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                NetworkCredential cred = new NetworkCredential("easypeoplespi@gmail.com", "chavemestra4");
                smtp.Credentials = cred;

                smtp.Host = "localhost";
                smtp.Port = 25;

                smtp.EnableSsl = true;

                try
                {
                    smtp.Send(msg);
                }
                catch (Exception e)
                {
                }
            }
        }
    }
}