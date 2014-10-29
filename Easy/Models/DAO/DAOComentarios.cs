using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Easy.Models
{
    public class DAOComentarios
    {

        public List<Comentarios> ListaComentarios(int id)
        {
            List<Comentarios> list = new List<Comentarios>();

            list.Add
                (
                    new Comentarios
                    {
                        IdComent = 1,
                        IdTarefa = new Tarefas { IdTarefa = id } ,
                        Comentario = "Testnado Comentatio"
                    }
                );

            return list;
        }
    }
}