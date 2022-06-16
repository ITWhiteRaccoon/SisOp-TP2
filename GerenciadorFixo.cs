using System.Text;

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
        foreach (var requisicao in requisicoes)
        {
            if (requisicao.TipoRequisicao == TipoRequisicao.IN)
            {
                
            }
            else
            {
                
            }
        }
    }

    private bool Inserir(string processo, uint espaco)
    {
        
    }

    public override string ToString()
    {
        var sb = new StringBuilder("|");
    }
}
