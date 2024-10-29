using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

namespace ws_distritos_concelhos
{
    /// <summary>
    /// Summary description for distritos_service
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class distritos_service : System.Web.Services.WebService
    {
        public  List<Distrito> LerDistrito()
        {
            string path = Server.MapPath("/ficheiros/distritos.txt");
            List<Distrito> distritos = new List<Distrito>();
            string linha = "";
            StreamReader ficheiro = new StreamReader(path, Encoding.UTF8);
            if (ficheiro != null)
            {
                while ((linha = ficheiro.ReadLine()) != null)
                {
                    string[] dados = linha.Split('*');
                   
                        int id;
                    if (dados.Length > 1 && int.TryParse(dados[0], out id))
                    {
                        distritos.Add(new Distrito()
                        {
                            id = id,
                            Nome = dados[1]
                        });
                    }
                    
                }
                ficheiro.Close();
            }
            return distritos;
        }

        [WebMethod]
        public List<Distrito> ListaDistritos()
        {
            return LerDistrito().OrderBy(d => d.Nome).ToList();
        }

        [WebMethod]
        public List<Distrito> ProcuraDistrito(string nome)
        {
    
            return LerDistrito().Where(d => d.Nome.Contains(nome)).ToList();
        }


        static int GetID(string Nome, List<Distrito> distritos)
        {
            Nome = Nome.ToLower();
            var distrito = distritos.FirstOrDefault(d => d.Nome.ToLower() == Nome);
            return distrito != null ? distrito.id : -1;
        }



    }




}
