using Spectre.Console;
using static System.Enum;

namespace SisOp_TP2;

public static class Program
{
    private const string InputFolder = "Input";

    public static void Main(string[] args)
    {
        var listaArquivos = Directory.GetFiles($"{InputFolder}/");
        for (var i = 0; i < listaArquivos.Length; i++)
        {
            listaArquivos[i] = listaArquivos[i][(InputFolder.Length + 1)..];
        }

        var arquivo = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"Qual o [green]arquivo desejado[/]? (listando arquivos da pasta {InputFolder})")
                .AddChoices(listaArquivos)
        );
        arquivo = $"{InputFolder}/{arquivo}";
        AnsiConsole.MarkupLine($"Arquivo selecionado: [blue]{arquivo}[/]");

        var requisicoes = Util.CarregarArquivo(arquivo);

        var tamanhoMemoria = AnsiConsole.Prompt(
            new TextPrompt<uint>("Qual o [green]tamanho da memoria[/]?")
                .ValidationErrorMessage("[red]O tamanho da memoria deve ser uma potencia de 2.[/]")
                .Validate(memoria => memoria > 0 && (memoria & (memoria - 1)) == 0
                    ? ValidationResult.Success()
                    : ValidationResult.Error()
                )
        );
        AnsiConsole.MarkupLine($"Tamanho de memoria selecionado: [blue]{tamanhoMemoria}[/]");

        var tipoParticao = Parse<TipoParticao>(AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Qual o [green]tipo de particao[/]?")
                .AddChoices("Fixa", "Variavel", "Buddy")
        ));
        AnsiConsole.MarkupLine($"Tipo de particao selecionado: [blue]{tipoParticao}[/]");

        switch (tipoParticao)
        {
            case TipoParticao.Fixa:
            {
                var tamanhoParticao = AnsiConsole.Prompt(
                    new TextPrompt<uint>("Qual o [green]tamanho da particao[/]?")
                        .ValidationErrorMessage(
                            "[red]O tamanho da particao deve ser uma potencia de 2 menor ou igual ao tamanho total da memória.[/]")
                        .Validate(particao =>
                            particao > 0 && (particao & (particao - 1)) == 0 && particao <= tamanhoMemoria
                                ? ValidationResult.Success()
                                : ValidationResult.Error()
                        )
                );
                AnsiConsole.MarkupLine($"Tamanho de partição selecionado: [blue]{tamanhoParticao}[/]");
                var gerenciadorFixo = new GerenciadorFixo(tamanhoMemoria, tamanhoParticao);
                gerenciadorFixo.Rodar(requisicoes);
                break;
            }
            case TipoParticao.Variavel:
            {
                var politicaAlocacao = Parse<PoliticaAlocacao>(AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Qual a [green]politica de alocacao[/]?")
                        .AddChoices("BestFit", "WorstFit")
                ));
                AnsiConsole.MarkupLine($"Politica de alocacao selecionada: [blue]{politicaAlocacao}[/]");
                var gerenciadorVariavel = new GerenciadorVariavel(tamanhoMemoria, politicaAlocacao);
                gerenciadorVariavel.Rodar(requisicoes);
                break;
            }
            case TipoParticao.Buddy:
                var gerenciadorBuddy = new GerenciadorBuddy(tamanhoMemoria);
                gerenciadorBuddy.Rodar(requisicoes);
                break;
        }
    }

    private static void PromptUsuario() { }

    public static void PrintFabiano()
    {
        var image = new CanvasImage($"{InputFolder}/fabiano_passuelo_hessel.png");
        AnsiConsole.Write(image);
    }
}
