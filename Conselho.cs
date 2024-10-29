using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ws_distritos_concelhos
{
    public class Concelho
    {
        public int CodigoDistrito { get; set; }
        public int CodigoConcelho { get; set; }
        public string Nome { get; set; }
        public double Area { get; set; }
        public int Populacao { get; set; }
    }
}