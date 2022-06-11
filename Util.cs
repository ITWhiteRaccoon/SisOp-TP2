using System.Text;
using System.Text.RegularExpressions;

namespace SisOp_TP2;

public class Util
{
    public static List<Requisicao> CarregarArquivo(string nomeArquivo)
    {
        var requisicoes = new List<Requisicao>();
        var linhas = File.ReadAllLines(nomeArquivo);
        for (var i = 0; i < linhas.Length; i++)
        {
            var conteudo = Regex.Match(linhas[i].Trim().ToUpper(),
                @"^(?<comando>[IN|OUT]{2,3})\(\s*(?<processo>[a-zA-Z]+)\s*(,\s*(?<espacos>\d+))?\s*\)");
            if (!conteudo.Success)
            {
                throw new InvalidDataException($"Invalid code: {linhas[i]} at line {i}");
            }

            var comando = Enum.Parse<TipoRequisicao>(conteudo.Groups["comando"].Value);
            var processo = conteudo.Groups["processo"].Value;
            var sb = new StringBuilder($"\u2713 {comando}\t{conteudo.Groups["processo"]}");

            if (comando == TipoRequisicao.IN)
            {
                if (!conteudo.Groups["espacos"].Success)
                {
                    throw new InvalidDataException(
                        $"Invalid code: {linhas[i]} at line {i}. IN requires process name and space ocuppied");
                }

                var espacos = Convert.ToUInt32(conteudo.Groups["espacos"].Value);
                sb.Append($"\t{conteudo.Groups["espacos"]}");
                requisicoes.Add(new Requisicao(comando, processo, espacos));
            }
            else
            {
                requisicoes.Add(new Requisicao(comando, processo));
            }

            Console.WriteLine(sb);
        }

        return requisicoes;
    }
}
