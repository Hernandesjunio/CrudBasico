using System;
using System.Collections.Generic;
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
}
