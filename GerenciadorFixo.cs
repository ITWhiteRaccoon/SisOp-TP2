using System.Text;

namespace SisOp_TP2;

public class GerenciadorFixo
{
    private List<Mapeador> _mapa;
    private uint _tamanhoParticao;

    public GerenciadorFixo(uint tamanhoMemoria, uint tamanhoParticao)
    {
        _mapa = new List<Mapeador> { new(null, 0, tamanhoMemoria) };
        _tamanhoParticao = tamanhoParticao;
    }

    public void Rodar(List<Requisicao> requisicoes)
    {
        foreach (var requisicao in requisicoes)
        {
            if (requisicao.TipoRequisicao == TipoRequisicao.IN) { }
            else { }
        }
    }

    private bool Inserir(string processo, uint espaco)
    {
        return false;
    }

    public override string ToString()
    {
        var sb = new StringBuilder("|");
        foreach (var espaco in _mapa)
        {
            if (espaco.Processo == null)
            {
                sb.Append($" [green]{espaco.Tamanho}[/] |");
            }
            else
            {
                sb.Append($" [red]{espaco.Processo}[/] |");
            }
        }

        return sb.ToString();
    }
}
