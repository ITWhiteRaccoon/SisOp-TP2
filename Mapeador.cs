namespace SisOp_TP2;

public class Mapeador
{
    public string? Processo { get; set; }
    public uint Inicio { get; set; }
    public uint Tamanho { get; set; }

    public Mapeador(string? processo, uint inicio, uint tamanho)
    {
        Processo = processo;
        Inicio = inicio;
        Tamanho = tamanho;
    }
}
