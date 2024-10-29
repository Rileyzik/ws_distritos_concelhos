using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ws_distritos_concelhos
{
    /// <summary>
    /// Summary description for ws_distritos_concelhos
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ws_distritos_concelhos : System.Web.Services.WebService
    {
        private static List<Concelho> LerConcelhos()
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/ficheiros/concelhos.txt");
            string[] lines = System.IO.File.ReadAllLines(path);
            List<Concelho> concelhos = new List<Concelho>();

            foreach (var line in lines)
            {
                var dados = line.Split('*');
                if (dados.Length >= 1)
                {
                    short codDistrito, codConcelho;
                    double area;
                    int populacao;

                    if (short.TryParse(dados[0], out codDistrito) &&
                        short.TryParse(dados[1], out codConcelho) &&
                        double.TryParse(dados[3], out area) &&
                        int.TryParse(dados[4], out populacao))
                    {
                        concelhos.Add(new Concelho
                        {
                            CodigoDistrito = codDistrito,
                            CodigoConcelho = codConcelho,
                            Nome = dados[2],
                            Area = area,
                            Populacao = populacao
                        });
                    }
                }
            }

            return concelhos;
        }

        [WebMethod]
        public List<Concelho> ListaConcelhos()
        {
            List<Concelho> concelhos = LerConcelhos();
            return concelhos.OrderBy(c => c.Nome).ToList();
        }

        [WebMethod]
        public List<Concelho> ListaConcelhosDistrito(string idDistrito)
        {
            List<Concelho> concelhos = LerConcelhos();
            return concelhos.Where(c => c.CodigoDistrito.ToString() == idDistrito).OrderBy(c => c.Nome).ToList();
        }


        static int GetID(string Nome, List<Concelho> concelhos)
        {
            Nome = Nome.ToLower();

            var concelho = concelhos.FirstOrDefault(d => d.CodigoConcelho.ToString().ToLower() == Nome);

            return concelhos != null ? concelho.CodigoConcelho : -1;
        }

        [WebMethod]
        public double MediaPopulacaoDistrito(string nomeDistrito)
        {
            List<Concelho> concelhos = LerConcelhos();

            var concelhosDoDistrito = concelhos.Where(c => c.CodigoDistrito == GetID(nomeDistrito, concelhos)).ToList();

            if (concelhosDoDistrito.Count == 0)
                return 0;

            double mediaPopulacao = concelhosDoDistrito.Average(c => c.Populacao);

            return Math.Round(mediaPopulacao,2);
        }

        [WebMethod]
        public List<Concelho> ConcelhosPorArea(double areaMaxima)
        {
            List<Concelho> concelhos = LerConcelhos();

           
            var concelhosFiltrados = concelhos
                .Where(c => c.Area < areaMaxima)
                .OrderByDescending(c => c.Area)
                .ToList();

            return concelhosFiltrados;
        }

        [WebMethod]
        public object StatzPopulacionais()
        {
            List<Concelho> concelhos = LerConcelhos();

            double populacaoTotal = concelhos.Sum(c => c.Populacao);
            double areaTotal = concelhos.Sum(c => c.Area);

            double densidadePopulacional = populacaoTotal / areaTotal;

            double mediaPopulacao = concelhos.Average(c => c.Populacao);

            var concelhoMaisPopuloso = concelhos.OrderByDescending(c => c.Populacao).First();

            var concelhoMenosPopuloso = concelhos.OrderBy(c => c.Populacao).First();

            return new
            {
                DensidadePopulacional = densidadePopulacional,
                MediaPopulacao = mediaPopulacao,
                ConcelhoMaisPopuloso = new { concelhoMaisPopuloso.Nome, concelhoMaisPopuloso.Populacao },
                ConcelhoMenosPopuloso = new { concelhoMenosPopuloso.Nome, concelhoMenosPopuloso.Populacao }
            };
        }


    }

}
