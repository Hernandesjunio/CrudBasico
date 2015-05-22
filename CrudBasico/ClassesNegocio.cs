using CrudBasico.InterfacesDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudBasico.ClassesNegocio
{
    public class Mensagem 
    {
        public int MensagemID { get; set; }
        public string Descricao { get; set; }
        public DateTime DtCriacao { get; set; }

        public virtual List<MensagemUsuario> MensagensUsuarios { get; set; }
    }

    public class MensagemUsuario
    {
        public int MensagemUsuarioID { get; set; }
        public int MensagemID { get; set; }
        public string Destinatario { get; set; }
        public DateTime DtCriacao { get; set; }
        public string UsuarioCriacao { get; set; }

        public virtual Mensagem Mensagem { get; set; }
    }


    public class MensagemProxy : Mensagem
    {
        public MensagemProxy(IMensagemUsuarioDAL dalUsuario)
        {
            this.MensagensUsuarios = new List<MensagemUsuario>();
            this.dalUsuario = dalUsuario;
        }

        private IMensagemUsuarioDAL dalUsuario = null;

        //Carrega propriedade sob demanda (Lazy Loading)
        public override List<MensagemUsuario> MensagensUsuarios
        {
            get
            {
                if (this.MensagemID > 0 && base.MensagensUsuarios.Count == 0)
                    base.MensagensUsuarios = dalUsuario.BuscarPelaChaveMensagemID(this.MensagemID);

                return base.MensagensUsuarios;
            }
            set
            {
                base.MensagensUsuarios = value;
            }
        }
    }

    public class MensagemUsuarioProxy : MensagemUsuario
    {
        public MensagemUsuarioProxy(IMensagemDAL dalMensagem)
        {
            this.dalMensagem = dalMensagem;
        }

        private IMensagemDAL dalMensagem = null;

        //Carrega propriedade sob demanda (Lazy Loading)
        public override Mensagem Mensagem
        {
            get
            {
                if (this.MensagemID > 0)
                    this.Mensagem = dalMensagem.BuscarPelaChave(this.MensagemID);

                return base.Mensagem;
            }
            set
            {
                base.Mensagem = value;
            }
        }
    }
}