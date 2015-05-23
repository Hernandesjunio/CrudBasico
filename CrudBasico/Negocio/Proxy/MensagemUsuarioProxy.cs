using CrudBasico.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudBasico.Negocio.Proxy
{
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
