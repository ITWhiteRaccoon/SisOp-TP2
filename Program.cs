using Spectre.Console;
using static System.Enum;

namespace SisOp_TP2;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== Ex1 ===");
        Util.CarregarArquivo("Input/Ex1.txt");
        Console.WriteLine("===     ===");
        Console.WriteLine();

        Console.WriteLine("=== Ex2 ===");
        Util.CarregarArquivo("Input/Ex2.txt");
        Console.WriteLine("===     ===");
        Console.WriteLine();

        Console.WriteLine("=== ExT ===");
        Util.CarregarArquivo("Input/ExT.txt");
        Console.WriteLine("===     ===");
        Console.WriteLine();
    }

    private static void PromptUsuario()
    {
        var tamanhoMemoria = AnsiConsole.Prompt(
            new TextPrompt<uint>("Qual o [green]tamanho da memória[/]?")
                .ValidationErrorMessage("[red]O tamanho da memória deve ser uma potência de 2.[/]")
                .Validate(memoria => memoria > 0 && (memoria & (memoria - 1)) == 0
                    ? ValidationResult.Success()
                    : ValidationResult.Error())
        );
        Console.WriteLine($"Tamanho selecionado: {tamanhoMemoria}");

        var tipoParticao = Parse<TipoParticao>(AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Qual o [green]tipo de partição[/]?")
                .AddChoices("Fixa", "Variavel", "Buddy")
        ));
        Console.WriteLine($"Tipo de partição selecionado: {tipoParticao}");

        if (tipoParticao == TipoParticao.Variavel)
        {
            var politicaAlocacao = Parse<PoliticaAlocacao>(AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Qual a [green]política de alocação[/]?")
                    .AddChoices("BestFit", "WorstFit")
            ));
            Console.WriteLine($"Política de alocação selecionada: {politicaAlocacao}");
        }
    }

    public static void PrintFabiano()
    {
        var image = new CanvasImage("Input/fabiano_passuelo_hessel.png");
        AnsiConsole.Write(image);
        Console.ReadKey();
    }
}
