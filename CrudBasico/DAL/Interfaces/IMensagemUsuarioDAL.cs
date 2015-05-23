using CrudBasico.Negocio;
using System.Collections.Generic;

namespace CrudBasico.DAL.Interfaces
{
    public interface IMensagemUsuarioDAL
    {
        void Inserir(MensagemUsuario obj);
        void Atualizar(MensagemUsuario obj);
        //Retorna somente o objeto sem realizar joins entre tabelas
        MensagemUsuario BuscarPelaChave(int ID);

        List<MensagemUsuario> BuscarPelaChaveMensagemID(int p);
    }
}
