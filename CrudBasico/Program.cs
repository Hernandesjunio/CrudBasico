using CrudBasico.Connection;
using CrudBasico.DTO;
using CrudBasico.Facade;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CrudBasico.UI
{
    public static class Program
    {
        //Interface do usuário
        public static void Main(string[] args)
        {
            //Cria a conexão com o Banco de dados            
            using (DbConnection cnn = ConnectionFactory.CreateConnection())
            {
                //ou
                //SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["CNN"].ConnectionString);
                using (TransactionScope transaction = new TransactionScope())
                {
                    using (var fachada = new FacadeMensagem(cnn))
                    {                                                
                        for (int i = 0; i < 50; i++)
                        {
                            //Necessário ler as propriedades da tela do usuário
                            var cliente = new MensagemCriacaoDTO();
                            cliente.Descricao = "MensagemTeste_" + i.ToString("00");
                            cliente.Destinatarios.Add("User1");
                            cliente.Destinatarios.Add("User2");
                            fachada.SalvarMensagem(cliente, "ADM");
                        }

                        var result = fachada.RecuperarMensagens("User1");
                        return;
                    }
                }
            }
        }
    }
}
