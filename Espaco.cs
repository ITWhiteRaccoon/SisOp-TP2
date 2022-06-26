namespace SisOp_TP2;

public class Espaco
{
    public string? Processo { get; set; }
    public uint Inicio { get; set; }
    public uint Tamanho { get; set; }
    public uint? TamanhoOcupado { get; set; }

    public Espaco(string? processo, uint inicio, uint tamanho)
    {
        Processo = processo;
        Inicio = inicio;
        Tamanho = tamanho;
    }

    public Espaco(string? processo, uint inicio, uint tamanho, uint tamanhoOcupado)
    {
        Processo = processo;
        Inicio = inicio;
        Tamanho = tamanho;
        TamanhoOcupado = tamanhoOcupado;
    }

    public override string ToString()
    {
        if (Processo != null)
        {
            return Tamanho == TamanhoOcupado || TamanhoOcupado == null
                ? $" [yellow]{Processo}[/] "
                : $" [yellow]{Processo}[/] [red]{Tamanho - TamanhoOcupado}[/] ";
        }

        return $" [green]{Tamanho}[/] ";
    }
}
