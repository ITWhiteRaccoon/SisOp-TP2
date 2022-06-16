namespace SisOp_TP2;

public class GerenciadorFixo
{
    private bool[] memoria;
    private uint tamanhoParticao;

    public GerenciadorFixo(uint tamanhoMemoria, uint tamanhoParticao)
    {
        memoria = new bool[tamanhoMemoria];
        tamanhoParticao = this.tamanhoParticao;
    }

    public void Rodar(List<Requisicao> requisicoes)
    {
        
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
