using CrudBasico.DTO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudBasico.DAL.Interfaces
{
    public interface IMensagemUsuarioViewDTODAL
    {
        //Não concatene queries, utilize parâmetros para evitar injeção SQL
        List<MensagemUsuarioViewDTO> Buscar(string query, params DbParameter[] parametros);
    }
}
