using Spectre.Console;
using static System.Enum;

namespace SisOp_TP2;

public static class Program
{
    public static void Main(string[] args)
    {
        var requisicoes = new Requisicoes("Input/Ex1.txt");
        var tamanhoMemoria = AnsiConsole.Prompt(
            new TextPrompt<uint>("Qual o [green]tamanho da memoria[/]?")
                .ValidationErrorMessage("[red]O tamanho da memoria deve ser uma potencia de 2.[/]")
                .Validate(memoria => memoria > 0 && (memoria & (memoria - 1)) == 0
                    ? ValidationResult.Success()
                    : ValidationResult.Error()
                )
        );
        Console.WriteLine($"Tamanho selecionado: {tamanhoMemoria}");

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
                break;
            }
            case TipoParticao.Buddy:
                break;
        }
    }

    public static void PrintFabiano()
    {
        var image = new CanvasImage("Input/fabiano_passuelo_hessel.png");
        AnsiConsole.Write(image);
        Console.ReadKey();
    }
}