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
        Console.WriteLine($"Arquivo selecionado: {arquivo}");

        var requisicoes = Util.CarregarArquivo(arquivo);

        var tamanhoMemoria = AnsiConsole.Prompt(
            new TextPrompt<uint>("Qual o [green]tamanho da memoria[/]?")
                .ValidationErrorMessage("[red]O tamanho da memoria deve ser uma potencia de 2.[/]")
                .Validate(memoria => memoria > 0 && (memoria & (memoria - 1)) == 0
                    ? ValidationResult.Success()
                    : ValidationResult.Error()
                )
        );
        Console.WriteLine($"Tamanho de memoria selecionado: {tamanhoMemoria}");

        var tipoParticao = Parse<TipoParticao>(AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Qual o [green]tipo de particao[/]?")
                .AddChoices("Fixa", "Variavel", "Buddy")
        ));
        Console.WriteLine($"Tipo de particao selecionado: {tipoParticao}");

        switch (tipoParticao)
        {
            case TipoParticao.Fixa:
            {
                var tamanhoParticao = AnsiConsole.Prompt(
                    new TextPrompt<uint>("Qual o [green]tamanho da particao[/]?")
                        .ValidationErrorMessage("[red]O tamanho da particao deve ser uma potencia de 2.[/]")
                        .Validate(particao => particao > 0 && (particao & (particao - 1)) == 0
                            ? ValidationResult.Success()
                            : ValidationResult.Error()
                        )
                );
                Console.WriteLine($"Tamanho de partição selecionado: {tamanhoParticao}");
                GerenciadorFixo.Iniciar(tamanhoMemoria, tamanhoParticao);
                break;
            }
            case TipoParticao.Variavel:
            {
                var politicaAlocacao = Parse<PoliticaAlocacao>(AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Qual a [green]politica de alocacao[/]?")
                        .AddChoices("BestFit", "WorstFit")
                ));
                Console.WriteLine($"Politica de alocacao selecionada: {politicaAlocacao}");
                GerenciadorVariavel.Iniciar(tamanhoMemoria, politicaAlocacao);
                break;
            }
            case TipoParticao.Buddy:
                GerenciadorBuddy.Iniciar(tamanhoMemoria);
                break;
        }
    }

    private static void TesteArquivos()
    {
        Console.WriteLine("=== Ex1 ===");
        Util.CarregarArquivo($"{InputFolder}/Ex1.txt");
        Console.WriteLine("===     ===");
        Console.WriteLine();

        Console.WriteLine("=== Ex2 ===");
        Util.CarregarArquivo($"{InputFolder}/Ex2.txt");
        Console.WriteLine("===     ===");
        Console.WriteLine();

        Console.WriteLine("=== ExT ===");
        Util.CarregarArquivo($"{InputFolder}/ExT.txt");
        Console.WriteLine("===     ===");
        Console.WriteLine();
    }

    private static void PromptUsuario() { }

    public static void PrintFabiano()
    {
        var image = new CanvasImage($"{InputFolder}/fabiano_passuelo_hessel.png");
        AnsiConsole.Write(image);
        Console.ReadKey();
    }
}
