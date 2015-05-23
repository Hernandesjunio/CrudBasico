using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudBasico.Negocio
{
    public class MensagemUsuario
    {
        public int MensagemUsuarioID { get; set; }
        public int MensagemID { get; set; }
        public string Destinatario { get; set; }
        public DateTime DtCriacao { get; set; }
        public string UsuarioCriacao { get; set; }

        public virtual Mensagem Mensagem { get; set; }
    }
}
