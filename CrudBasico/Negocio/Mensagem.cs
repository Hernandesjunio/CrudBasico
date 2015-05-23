using System;
using System.Collections.Generic;

namespace CrudBasico.Negocio
{
    public class Mensagem
    {
        public int MensagemID { get; set; }
        public string Descricao { get; set; }
        public DateTime DtCriacao { get; set; }

        public virtual List<MensagemUsuario> MensagensUsuarios { get; set; }
    }
}
