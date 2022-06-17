namespace SisOp_TP2;

public class Espaco
{
    public string? Processo { get; set; }
    public uint Inicio { get; set; }
    public uint Tamanho { get; set; }

    public Espaco(string? processo, uint inicio, uint tamanho)
    {
        Processo = processo;
        Inicio = inicio;
        Tamanho = tamanho;
    }
}
