using CrudBasico.DTO;
using CrudBasico.Facade;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudBasico.UI
{
    public static class Program
    {
        //Interface do usuário
        public static void Main(string[] args)
        {
            //Cria a conexão com o Banco de dados            
            using (DbConnection cnn = ConnectionFactory.ConnectionFactory.CreateConnection())
            {
                //ou
                //SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["CNN"].ConnectionString);

                using (var fachada = new FacadeMensagem(cnn))
                {
                    //Necessário ler as propriedades da tela do usuário
                    var cliente = new MensagemCriacaoDTO();

                    cliente.Descricao = "MensagemTeste";
                    cliente.Destinatarios.Add("User1");
                    cliente.Destinatarios.Add("User2");

                    fachada.SalvarMensagem(cliente, "ADM");
                }
            }
        }
    }
}
