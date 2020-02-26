using GetHtmlFromPortal.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GetHtmlFromPortal.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Consulta()
        {
            try
            {
                var informes = new List<Retorno>();

                var url = "http://www.nfe.fazenda.gov.br/portal/informe.aspx?ehCTG=false";
                var htmlTag = "div";
                var id = false;
                var classNameOrId = "divInforme";

                string result;
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 5000;
                using (var getResponse = request.GetResponse())
                using (var stream = getResponse.GetResponseStream())
                {
                    if (stream == null) informes.Add(new Retorno { Mensagem = "Não foi possivel recuperar as informações do portal nacional da NF-e." });
                    using (var reader = new StreamReader(stream, Encoding.GetEncoding("iso-8859-1"))) result = reader.ReadToEnd();
                }

                var doc = new HtmlDocument();
                using (var sr = new StringReader(result))
                {
                    doc.Load(sr);
                    var links = doc.DocumentNode.Descendants(htmlTag).Where(a => (a.Attributes[id ? "id" : "class"] == null ? "" : a.Attributes[id ? "id" : "class"].Value) == classNameOrId).Select(x => x).ToList();

                    links.ForEach(a =>
                    {
                        var informe = a.Element("p");
                        var mensagem = a.InnerHtml.Replace(informe.InnerHtml, "");
                        var split = Regex.Split(informe.InnerText, " -");
                        var data = split[0];
                        var titulo = "";
                        for (int j = 1; j < split.Length; j++)
                        {
                            titulo += $" {split[j]}";
                        }
                        informes.Add(new Retorno { Data = data, Titulo = titulo, Mensagem = mensagem });
                    });

                    return Json(informes, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> ConsultaDisponibilidade()
        {
            try
            {
                var url = "http://www.nfe.fazenda.gov.br/portal/disponibilidade.aspx?versao=2.00";
                var htmlTag = "table";
                var id = false;
                var classNameOrId = "tabelaListagemDados";
                var disponibilidade = new List<DiponibilidadeModel>();
                string result;
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 5000;
                using (var getResponse = request.GetResponse())
                using (var stream = getResponse.GetResponseStream())
                {
                    if (stream == null) disponibilidade.Add(new DiponibilidadeModel { Mensagem = "Não foi possivel recuperar as informações do portal nacional da NF-e." });
                    using (var reader = new StreamReader(stream, Encoding.GetEncoding("iso-8859-1"))) result = reader.ReadToEnd();
                }

                var doc = new HtmlDocument();
                using (var sr = new StringReader(result))
                {
                    doc.Load(sr);
                    var tables = doc.DocumentNode.Descendants(htmlTag).Where(a => (a.Attributes[id ? "id" : "class"] == null ? "" : a.Attributes[id ? "id" : "class"].Value) == classNameOrId).Select(x => x).ToList();                    

                    tables.ForEach(t => 
                    {
                        for (int i = 3; i < 17 ; i++)
                        {
                            var sefazAutorizadora = t.ChildNodes[i].ChildNodes[1].InnerHtml;
                            var autorizacao = GetDisponibilidade(t.ChildNodes[i].ChildNodes[2].InnerHtml);
                            var retAutorizacao = GetDisponibilidade(t.ChildNodes[i].ChildNodes[3].InnerHtml);
                            var inutilizacao = GetDisponibilidade(t.ChildNodes[i].ChildNodes[4].InnerHtml);
                            var consultaProtocolo = GetDisponibilidade(t.ChildNodes[i].ChildNodes[5].InnerHtml);
                            var statusServico = GetDisponibilidade(t.ChildNodes[i].ChildNodes[6].InnerHtml);
                            var tempoMedio = t.ChildNodes[i].ChildNodes[7].InnerHtml;
                            var consultaCadastro = GetDisponibilidade(t.ChildNodes[i].ChildNodes[8].InnerHtml);
                            var recepcaoEvento = GetDisponibilidade(t.ChildNodes[i].ChildNodes[9].InnerHtml);

                            disponibilidade.Add(new DiponibilidadeModel
                            {
                                SefazAutorizadora = sefazAutorizadora,
                                Autorizacao = autorizacao,
                                RetornoAutorizacao = retAutorizacao,
                                Inutilizacao = inutilizacao,
                                ConsultaProtocolo = consultaProtocolo,
                                StatusServico = statusServico,
                                TempoMedio = tempoMedio,
                                ConsultaCadastro = consultaCadastro,
                                RecepcaoEvento = recepcaoEvento
                            });
                        }
                    });

                    return Json(disponibilidade, JsonRequestBehavior.AllowGet);

                }

            }
            catch (Exception ex)
            {
                //Todo Colocar um envio de email com erro para logar possiveis problemas
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        public static string GetDisponibilidade(string innerHTML)
        {
            if (innerHTML.Contains("verde")) return "1" ;
            if (innerHTML.Contains("vermelho")) return "2" ;
            if (innerHTML.Contains("amarela")) return "3";
            return "0";            
        }
    }
}