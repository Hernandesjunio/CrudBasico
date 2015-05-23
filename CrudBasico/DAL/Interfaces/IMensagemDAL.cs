using CrudBasico.Negocio;

namespace CrudBasico.DAL.Interfaces
{
    public interface IMensagemDAL
    {
        void Inserir(Mensagem obj);
        void Atualizar(Mensagem obj);
        //Retorna somente o objeto sem realizar joins entre tabelas
        Mensagem BuscarPelaChave(int ID);
    }
}
