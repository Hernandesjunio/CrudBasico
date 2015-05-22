using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudBasico.DTO
{
    public class MensagemCriacaoDTO
    {
        public MensagemCriacaoDTO()
        {
            Destinatarios = new List<string>();
        }

        public string Descricao { get; set; }
        public List<string> Destinatarios { get; set; }
    }

    public class MensagemUsuarioViewDTO
    {
        private DbConnection cnn;

        public MensagemUsuarioViewDTO(DbConnection cnn)
        {
            // TODO: Complete member initialization
            this.cnn = cnn;
        }
        public string Descricao { get; set; }
        public int ID { get; set; }
        public string Destinatario { get; set; }
        public DateTime DtCriacao { get; set; }
        public string UsuarioCriacao { get; set; }
    }
}