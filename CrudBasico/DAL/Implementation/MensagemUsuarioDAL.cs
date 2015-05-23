using CrudBasico.DAL.Interfaces;
using CrudBasico.Negocio;
using CrudBasico.Negocio.Proxy;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace CrudBasico.DAL.Implementation
{
    public class MensagemUsuarioDAL : IMensagemUsuarioDAL
    {
        private DbConnection cnn;

        public MensagemUsuarioDAL(DbConnection cnn)
        {
            // TODO: Complete member initialization
            this.cnn = cnn;
        }

        public DbParameter getpDestinatario(DbCommand cmd, object Value)
        {
            DbParameter pDescricao = cmd.CreateParameter();
            pDescricao.DbType = System.Data.DbType.String;
            pDescricao.ParameterName = "@Destinatario";
            pDescricao.Value = Value;
            //para campos de texto é obrigatório informar o tamanho.
            pDescricao.Size = 50;

            return pDescricao;
        }

        public DbParameter getpUsuarioCriacao(DbCommand cmd, object Value)
        {
            DbParameter pUsuarioCriacao = cmd.CreateParameter();
            pUsuarioCriacao.DbType = System.Data.DbType.String;
            pUsuarioCriacao.ParameterName = "@UsuarioCriacao";
            pUsuarioCriacao.Value = Value;
            //para campos de texto é obrigatório informar o tamanho.
            pUsuarioCriacao.Size = 50;
            return pUsuarioCriacao;
        }

        public DbParameter getpDtCriacao(DbCommand cmd, object Value)
        {
            DbParameter pDtCriacao = cmd.CreateParameter();
            pDtCriacao.DbType = System.Data.DbType.DateTime;
            pDtCriacao.ParameterName = "@DtCriacao";
            pDtCriacao.Value = Value;
            return pDtCriacao;
        }

        public DbParameter getpMensageID(DbCommand cmd, object Value)
        {
            DbParameter pMensageID = cmd.CreateParameter();
            pMensageID.DbType = System.Data.DbType.Int32;
            pMensageID.ParameterName = "@MensagemID";
            pMensageID.Value = Value;
            return pMensageID;
        }

        public DbParameter getpMensageUsuarioID(DbCommand cmd, object Value)
        {
            DbParameter pMensageUsuarioID = cmd.CreateParameter();
            pMensageUsuarioID.DbType = System.Data.DbType.Int32;
            pMensageUsuarioID.ParameterName = "@MensagemUsuarioID";
            pMensageUsuarioID.Value = Value;
            //O tipo output informa que a query do BD fará a atribuição do parâmetro para ser lido pela aplicação. 
            if (Value == null)                
                pMensageUsuarioID.Direction = System.Data.ParameterDirection.Output;
            return pMensageUsuarioID;
        }

        public void Inserir(MensagemUsuario obj)
        {
            try
            {
                using (DbCommand cmd = cnn.CreateCommand())
                {
                    //caso queira procedure fica CommandText = "Nome da Stored Procedure"
                    cmd.CommandText = "insert into MensagemUsuario (MensagemID, Destinatario, UsuarioCriacao,DtCriacao) " +
                        "values (@MensagemID, @Destinatario, @UsuarioCriacao,@DtCriacao); set @MensagemUsuarioID = Scope_Identity()";
                    //não esquecer de alterar caso queira stored procedure
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandTimeout = 10;
                    cmd.Connection = cnn;
                    
                    DbParameter pMensageUsuarioID = getpMensageUsuarioID(cmd, null);

                    //A ordem dos parâmetros não influencia na inserção dos dados
                    cmd.Parameters.Add(getpDtCriacao(cmd, obj.DtCriacao));
                    cmd.Parameters.Add(getpDestinatario(cmd, obj.Destinatario));
                    cmd.Parameters.Add(getpMensageID(cmd, obj.MensagemID));
                    cmd.Parameters.Add(getpUsuarioCriacao(cmd, obj.UsuarioCriacao));
                    cmd.Parameters.Add(pMensageUsuarioID);

                    //Faz a compilação do comando antes de executar(Torna a execução mais rápida)
                    cmd.Prepare();

                    //Retorna a quantidade de operações executadas no banco de dados
                    var result = cmd.ExecuteNonQuery();

                    //pMensagemID foi atribuído no BD pela propriedade Scope_Identity considerando auto imcremento da PK
                    obj.MensagemID = Convert.ToInt32(pMensageUsuarioID.Value);
                }
            }
            catch (Exception)
            {
                throw;
            }            
        }

        public void Atualizar(MensagemUsuario obj)
        {
            throw new NotImplementedException();
        }

        public MensagemUsuario BuscarPelaChave(int MensagemUsuarioID)
        {
            using (var cmd = cnn.CreateCommand())
            {
                cmd.CommandText = "select * from MensagemUsuario where MensagemUsuarioID = @MensagemUsuarioID";
                cmd.Connection = cnn;

                DbParameter pMensageUsuarioID = cmd.CreateParameter();
                pMensageUsuarioID.DbType = System.Data.DbType.Int32;
                pMensageUsuarioID.ParameterName = "@MensagemUsuarioID";
                pMensageUsuarioID.Value = MensagemUsuarioID;

                cmd.Parameters.Add(pMensageUsuarioID);

                cmd.Prepare();

                using (var reader = cmd.ExecuteReader())
                {
                    var dalMensagem = new MensagemDAL(cnn);

                    while (reader.Read())
                    {
                        //Este objeto vai encapsular o LazyLoading caso queira carregar uma propriedade objeto
                        MensagemUsuarioProxy obj = new MensagemUsuarioProxy(dalMensagem);
                        obj.Destinatario = reader["Destinatario"].ToString();
                        obj.DtCriacao = DateTime.Parse(reader["DtCriacao"].ToString());
                        obj.MensagemID = int.Parse(reader["MensagemID"].ToString());

                        obj.MensagemUsuarioID = int.Parse(reader["MensagemUsuarioID"].ToString());
                        obj.UsuarioCriacao = reader["UsuarioCriacao"].ToString();

                        return obj;
                    }

                    //caso não tenha nada retorna nulo
                    return null;
                }
            }
        }

        public List<MensagemUsuario> BuscarPelaChaveMensagemID(int MensagemID)
        {
            //retorna todas as mensagens baseadas na FK MensagemID
            using (var cmd = cnn.CreateCommand())
            {
                cmd.CommandText = "select * from MensagemUsuario where MensagemID = @MensagemID";
                cmd.Connection = cnn;

                DbParameter pMensageID = cmd.CreateParameter();
                pMensageID.DbType = System.Data.DbType.Int32;
                pMensageID.ParameterName = "@MensagemID";
                pMensageID.Value = MensagemID;

                cmd.Parameters.Add(pMensageID);

                cmd.Prepare();

                using (var reader = cmd.ExecuteReader())
                {
                    var dalMensagem = new MensagemDAL(cnn);
                    List<MensagemUsuario> lst = new List<MensagemUsuario>();

                    while (reader.Read())
                    {
                        //Este objeto vai encapsular o LazyLoading caso queira carregar uma propriedade objeto
                        MensagemUsuarioProxy obj = new MensagemUsuarioProxy(dalMensagem);
                        obj.Destinatario = reader["Destinatario"].ToString();
                        obj.DtCriacao = DateTime.Parse(reader["DtCriacao"].ToString());
                        obj.MensagemID = int.Parse(reader["MensagemID"].ToString());

                        obj.MensagemUsuarioID = int.Parse(reader["MensagemUsuarioID"].ToString());
                        obj.UsuarioCriacao = reader["UsuarioCriacao"].ToString();

                        //Por causa do Princípio da Substuição de Liskov é permitido uma super classe referenciar uma subclasse
                        lst.Add(obj);
                    }

                    return lst;
                }
            }
        }
    }
}
