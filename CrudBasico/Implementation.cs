using CrudBasico.ClassesNegocio;
using CrudBasico.DTO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudBasico.InterfacesDAL.Implementation
{
    public class MensagemUsuarioViewDTODAL : IMensagemUsuarioViewDTODAL
    {
        private DbConnection cnn;

        public MensagemUsuarioViewDTODAL(DbConnection cnn)
        {
            // TODO: Complete member initialization
            this.cnn = cnn;
        }

        public List<MensagemUsuarioViewDTO> Buscar(string query, params DbParameter[] parametros)
        {

            //Esta parte deverá montar os inner joins que vão trazer os dados para preenchimento das propriedades

            throw new NotImplementedException();
        }
    }

    public class MensagemDAL : IMensagemDAL
    {
        private DbConnection cnn;

        public MensagemDAL(DbConnection cnn)
        {
            // TODO: Complete member initialization
            this.cnn = cnn;
        }

        public DbParameter getpDescricao(DbCommand cmd, object value)
        {
            DbParameter pDescricao = cmd.CreateParameter();
            pDescricao.DbType = System.Data.DbType.String;
            pDescricao.ParameterName = "@Descricao";
            pDescricao.Value = value;
            return pDescricao;
        }

        public DbParameter getpDtCriacao(DbCommand cmd, object value)
        {
            var pDtCriacao = cmd.CreateParameter();
            pDtCriacao.DbType = System.Data.DbType.DateTime;
            pDtCriacao.ParameterName = "@DtCriacao";
            pDtCriacao.Value = value;
            return pDtCriacao;
        }

        public DbParameter getpMensageID(DbCommand cmd, object value)
        {
            var pMensageID = cmd.CreateParameter();
            pMensageID.DbType = System.Data.DbType.Int32;
            pMensageID.ParameterName = "@MensagemID";
            pMensageID.Value = value;

            if (value == null)
                pMensageID.Direction = System.Data.ParameterDirection.Output;

            return pMensageID;
        }

        public void Inserir(Mensagem obj)
        {
            try
            {
                using (DbCommand cmd = cnn.CreateCommand())
                {
                    //caso queira procedure fica CommandText = "Nome da Stored Procedure"
                    cmd.CommandText = "insert into mensagem (Descricao,DtCriacao) values (@Descricao,@DtCriacao); set @MensagemID = Scope_Identity()";
                    //não esquecer de alterar caso queira stored procedure
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandTimeout = 10;
                    cmd.Connection = cnn;

                    var pMensageID = getpMensageID(cmd, null);

                    //A ordem dos parâmetros não influencia na inserção dos dados
                    cmd.Parameters.Add(getpDescricao(cmd, obj.Descricao));
                    cmd.Parameters.Add(getpDtCriacao(cmd, obj.DtCriacao));
                    cmd.Parameters.Add(pMensageID);

                    //Faz a compilação do comando antes de executar(Torna a execução mais rápida)
                    cmd.Prepare();

                    //Retorna a quantidade de operações executadas no banco de dados
                    var result = cmd.ExecuteNonQuery();

                    //pMensagemID foi atribuído no BD pela propriedade Scope_Identity considerando auto imcremento da PK
                    obj.MensagemID = Convert.ToInt32(pMensageID.Value);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }
        }

        //A atualização funciona similar à inclusão, não utilizar Direction no parâmetro, deixar o valor padrão
        public void Atualizar(Mensagem obj)
        {
            throw new NotImplementedException();
        }

        public Mensagem BuscarPelaChave(int ID)
        {
            using (var cmd = cnn.CreateCommand())
            {
                cmd.CommandText = "select * from Mensagem where MensagemID = @MensagemID";
                cmd.Connection = cnn;

                DbParameter pMensageID = getpMensageID(cmd, ID);

                cmd.Parameters.Add(pMensageID);

                cmd.Prepare();

                using (var reader = cmd.ExecuteReader())
                {
                    var dalMensagemUsuario = new MensagemUsuarioDAL(cnn);
                    while (reader.Read())
                    {
                        //Este objeto vai encapsular o LazyLoading caso queira carregar uma propriedade objeto
                        MensagemProxy obj = new MensagemProxy(dalMensagemUsuario);
                        obj.Descricao = reader["Descricao"].ToString();
                        obj.DtCriacao = DateTime.Parse(reader["DtCriacao"].ToString());
                        obj.MensagemID = int.Parse(reader["MensagemID"].ToString());

                        return obj;
                    }

                    //caso não tenha nada retorna nulo
                    return null;
                }
            }
        }
    }

    public class MensagemUsuarioDAL : IMensagemUsuarioDAL
    {
        private DbConnection cnn;

        public MensagemUsuarioDAL(DbConnection cnn)
        {
            // TODO: Complete member initialization
            this.cnn = cnn;
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

                    DbParameter pDescricao = cmd.CreateParameter();
                    pDescricao.DbType = System.Data.DbType.String;
                    pDescricao.ParameterName = "@Destinatario";
                    pDescricao.Value = obj.Destinatario;
                    //para campos de texto é obrigatório informar o tamanho.
                    pDescricao.Size = 40;

                    DbParameter pUsuarioCriacao = cmd.CreateParameter();
                    pUsuarioCriacao.DbType = System.Data.DbType.String;
                    pUsuarioCriacao.ParameterName = "@UsuarioCriacao";
                    pUsuarioCriacao.Value = obj.UsuarioCriacao;
                    //para campos de texto é obrigatório informar o tamanho.
                    pUsuarioCriacao.Size = 40;

                    DbParameter pDtCriacao = cmd.CreateParameter();
                    pDtCriacao.DbType = System.Data.DbType.DateTime;
                    pDtCriacao.ParameterName = "@DtCriacao";
                    pDtCriacao.Value = obj.DtCriacao;

                    DbParameter pMensageID = cmd.CreateParameter();
                    pMensageID.DbType = System.Data.DbType.Int32;
                    pMensageID.ParameterName = "@MensagemID";
                    pMensageID.Value = obj.MensagemID;

                    DbParameter pMensageUsuarioID = cmd.CreateParameter();
                    pMensageUsuarioID.DbType = System.Data.DbType.Int32;
                    pMensageUsuarioID.ParameterName = "@pMensageUsuarioID";
                    //O tipo output informa que a query do BD fará a atribuição do parâmetro para ser lido pela aplicação. 
                    pMensageUsuarioID.Direction = System.Data.ParameterDirection.Output;

                    //A ordem dos parâmetros não influencia na inserção dos dados
                    cmd.Parameters.Add(pDtCriacao);
                    cmd.Parameters.Add(pDescricao);
                    cmd.Parameters.Add(pMensageID);
                    cmd.Parameters.Add(pUsuarioCriacao);
                    cmd.Parameters.Add(pMensageUsuarioID);

                    //Faz a compilação do comando antes de executar(Torna a execução mais rápida)
                    cmd.Prepare();

                    //Retorna a quantidade de operações executadas no banco de dados
                    var result = cmd.ExecuteNonQuery();

                    //pMensagemID foi atribuído no BD pela propriedade Scope_Identity considerando auto imcremento da PK
                    obj.MensagemID = Convert.ToInt32(pMensageID.Value);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnn.Close();
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
