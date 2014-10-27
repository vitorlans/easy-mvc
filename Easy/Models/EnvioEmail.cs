using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Net;
using System.Text;

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

                //MailAddress remetente = new MailAddress("easypeoplespi@gmail.com");
                //MailAddress destinatario = new MailAddress(dr["destinatario"].ToString());

                SmtpClient cliente = new SmtpClient("smtp.gmail.com", 587);

                cliente.EnableSsl = true;

                MailMessage msg = new MailMessage("easystemspi@gmail.com", dr["destinatario"].ToString(), "Teste", "TEstando envio de Email..");

                msg.Priority = MailPriority.High;

                cliente.Credentials = new NetworkCredential("easystemspi@gmail.com", "chavemestra");


                cliente.Send(msg);
            }
        }
    }
}