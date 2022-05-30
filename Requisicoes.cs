using System.Text.RegularExpressions;

namespace SisOp_TP2;

public class Requisicoes
{
    public Requisicoes(string caminhoArquivo)
    {
        var comandos = File.ReadAllLines(caminhoArquivo);
        foreach (var line in comandos)
        {
            var lineMatch = Regex.Match(line.Trim().ToLower(), @"\s*(?<comando>in|out)\((?<parametros>\w|\w,\s*\d)\)");
            if (lineMatch.Success)
            {
                var comando = lineMatch.Groups["comando"];
                var parametros = lineMatch.Groups["parametros"];
            }
        }
    }
}