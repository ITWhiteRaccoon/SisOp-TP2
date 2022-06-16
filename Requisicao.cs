namespace SisOp_TP2;

public class Requisicao
{
    public TipoRequisicao TipoRequisicao { get; }
    public uint Espaco { get; }
    public string Processo { get; }

    public Requisicao(TipoRequisicao tipoRequisicao, string processo)
    {
        if (tipoRequisicao != TipoRequisicao.OUT)
        {
            throw new ArgumentException($"{tipoRequisicao} requires more parameters");
        }

        TipoRequisicao = tipoRequisicao;
        Processo = processo;
    }

    public Requisicao(TipoRequisicao tipoRequisicao, string processo, uint espaco)
    {
        if (tipoRequisicao != TipoRequisicao.IN)
        {
            throw new ArgumentException($"{tipoRequisicao} takes only one parameter");
        }

        TipoRequisicao = tipoRequisicao;
        Processo = processo;
        Espaco = espaco;
    }
}
