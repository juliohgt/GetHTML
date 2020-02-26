using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GetHtmlFromPortal.Models
{
    public class Retorno
    {
        public string Data { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
    }

    public class Retorno<T>
    {
        [DataMember]
        public bool Sucesso { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<string> Mensagens { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public T Entidade { get; set; }
    }

    public class DiponibilidadeModel
    {
        public string SefazAutorizadora { get; set; }
        public string Autorizacao { get; set; }
        public string RetornoAutorizacao { get; set; }
        public string Inutilizacao { get; set; }
        public string ConsultaProtocolo { get; set; }
        public string StatusServico { get; set; }
        public string TempoMedio { get; set; }
        public string ConsultaCadastro { get; set; }
        public string RecepcaoEvento { get; set; }
        public string Mensagem { get; set; }
    }
}