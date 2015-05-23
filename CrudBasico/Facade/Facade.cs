using CrudBasico.DAL.Implementation;
using CrudBasico.DAL.Interfaces;
using CrudBasico.DTO;
using CrudBasico.Negocio;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;

namespace CrudBasico.Facade
{
    public class FacadeMensagem : IDisposable
    {
        IMensagemDAL mensagemDAL = null;
        IMensagemUsuarioDAL mensagemUsuarioDAL = null;
        IMensagemUsuarioViewDTODAL mensagemDtoDAL = null;
        private DbConnection cnn;

        public FacadeMensagem(DbConnection cnn)
        {
            this.cnn = cnn;
            //Pode ser criado sob demanda ou seja em cada método que for utilizar
            //Coloquei aqui para ficar mais simples
            mensagemDAL = new MensagemDAL(cnn);
            mensagemUsuarioDAL = new MensagemUsuarioDAL(cnn);
            mensagemDtoDAL = new MensagemUsuarioViewDTODAL(cnn);
        }

        public void SalvarMensagem(MensagemCriacaoDTO obj, string UsuarioCriacao)
        {
            try
            {
                //garante que se alguma das operações falharem, será feito rollback das operações

                using (TransactionScope transaction = new TransactionScope())
                {
                    if (cnn.State == System.Data.ConnectionState.Closed)
                        cnn.Open();

                    Mensagem objMensagem = new Mensagem();
                    objMensagem.Descricao = obj.Descricao;
                    objMensagem.DtCriacao = DateTime.Now;

                    mensagemDAL.Inserir(objMensagem);

                    foreach (var item in obj.Destinatarios)
                    {
                        MensagemUsuario mu = new MensagemUsuario();
                        mu.Destinatario = item;
                        mu.UsuarioCriacao = UsuarioCriacao;
                        mu.DtCriacao = DateTime.Now;
                        //MensagemID será atribuído pelo DAL
                        mu.MensagemID = objMensagem.MensagemID;

                        mensagemUsuarioDAL.Inserir(mu);
                    }

                    //faz o commit
                    transaction.Complete();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //sempre fecha a conexão como  banco de dados
                cnn.Close();
            }
        }

        public List<MensagemUsuarioViewDTO> RecuperarMensagens(string destinatario)
        {
            try
            {
                if (cnn.State == System.Data.ConnectionState.Closed)
                    cnn.Open();

                MensagemUsuarioViewDTODAL dal = new MensagemUsuarioViewDTODAL(cnn);

                DbParameter pDescricao = new SqlParameter();
                pDescricao.DbType = System.Data.DbType.String;
                pDescricao.ParameterName = "@Destinatario";
                pDescricao.Value = destinatario;
                //para campos de texto é obrigatório informar o tamanho.
                pDescricao.Size = 50;


                var result = dal.Buscar("mu.Destinatario = @Destinatario", pDescricao);
                return result;
            }
            catch (Exception)
            {
                throw;
            }

            cnn.Close();
        }

        public void Dispose()
        {
            //Descarta os elementos da memória
            mensagemDAL = null;
            mensagemUsuarioDAL = null;
            mensagemDtoDAL = null;
            cnn.Close();
            cnn = null;
        }
    }
}






