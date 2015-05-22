using CrudBasico.ClassesNegocio;
using CrudBasico.DTO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudBasico.InterfacesDAL
{
    public interface IMensagemDAL
    {
        void Inserir(Mensagem obj);
        void Atualizar(Mensagem obj);
        //Retorna somente o objeto sem realizar joins entre tabelas
        Mensagem BuscarPelaChave(int ID);
    }

    public interface IMensagemUsuarioDAL
    {
        void Inserir(MensagemUsuario obj);
        void Atualizar(MensagemUsuario obj);
        //Retorna somente o objeto sem realizar joins entre tabelas
        MensagemUsuario BuscarPelaChave(int ID);

        List<MensagemUsuario> BuscarPelaChaveMensagemID(int p);
    }

    public interface IMensagemUsuarioViewDTODAL
    {
        //Não concatene queries, utilize parâmetros para evitar injeção SQL
        List<MensagemUsuarioViewDTO> Buscar(string query, params DbParameter[] parametros);
    }
}
