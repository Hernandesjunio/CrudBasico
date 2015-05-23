using CrudBasico.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudBasico.Negocio.Proxy
{
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
}
