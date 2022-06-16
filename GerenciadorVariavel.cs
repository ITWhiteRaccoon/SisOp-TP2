namespace SisOp_TP2;

public class GerenciadorVariavel
{
    private bool[] memoria;

    public GerenciadorVariavel(uint tamanhoMemoria, PoliticaAlocacao politicaAlocacao)
    {
        memoria = new bool[tamanhoMemoria];
    }

    public void Rodar(List<Requisicao> requisicoes)
    {
        
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
