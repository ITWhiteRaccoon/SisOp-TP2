using System.Configuration;
using Spectre.Console;
using static System.Enum;

namespace SisOp_TP2;

public static class Program
{
    private static string _inputFolder = "Input";

    public static void Main(string[] args)
    {
        _inputFolder = ConfigurationManager.AppSettings.Get("filesLocation") ?? "Input";
        while (true)
        {
            var opcao = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("Selecione a opcao desejada:")
                    .AddChoices(1, 2, 0).UseConverter(i =>
                    {
                        return i switch
                        {
                            0 => "0. Sair",
                            1 => "1. Executar gerenciador de memoria",
                            2 => "2. Selecionar caminho dos arquivos",
                            _ => "Erro"
                        };
                    })
            );

            switch (opcao)
            {
                case 0:
                    return;
                case 1:
                    GerenciadorMemoria();
                    break;
                case 2:
                    SelecionarCaminho("filesLocation");
                    break;
            }
        }
    }

    private static void SelecionarCaminho(string key)
    {
        var novoLocal = AnsiConsole.Prompt(
            new TextPrompt<string>("Qual o novo caminho desejado?"));
        try
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            if (settings[key] == null)
            {
                settings.Add(key, novoLocal);
            }
            else
            {
                settings[key].Value = novoLocal;
            }

            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            _inputFolder = novoLocal;
        }
        catch (ConfigurationErrorsException)
        {
            Console.WriteLine("Erro salvando configuracoes");
        }
    }

    private static void GerenciadorMemoria()
    {
        var listaArquivos = Directory.GetFiles($"{_inputFolder}");
        for (var i = 0; i < listaArquivos.Length; i++)
        {
            listaArquivos[i] = listaArquivos[i][(_inputFolder.Length + 1)..];
        }

        var arquivo = AnsiConsole.Prompt<string>(
            new SelectionPrompt<string>()
                .Title("Qual o [green]arquivo desejado[/]?")
                .AddChoiceGroup($"Listando arquivos da pasta {_inputFolder}", listaArquivos)
                .AddChoices("Voltar ao menu principal")
        );
        if (arquivo == "Voltar ao menu principal")
        {
            return;
        }

        arquivo = $"{_inputFolder}/{arquivo}";
        AnsiConsole.MarkupLine($"Arquivo selecionado: [blue]{arquivo}[/]");
        var requisicoes = Util.CarregarArquivo(arquivo);

        var tipoParticao = Parse<TipoParticao>(AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Qual o [green]tipo de particao[/]?")
                .AddChoices("Fixa", "Variavel", "Buddy")
        ));
        Console.Clear();
        AnsiConsole.MarkupLine($"Tipo de particao selecionado: [blue]{tipoParticao}[/]");

        var tamanhoMemoria = AnsiConsole.Prompt(
            new TextPrompt<uint>("Qual o [green]tamanho da memoria[/]?")
                .ValidationErrorMessage("[red]O tamanho da memoria deve ser uma potencia de 2.[/]")
                .Validate(memoria => memoria > 0 && (memoria & (memoria - 1)) == 0
                    ? ValidationResult.Success()
                    : ValidationResult.Error()
                )
        );
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        AnsiConsole.MarkupLine($"Tamanho de memoria selecionado: [blue]{tamanhoMemoria}[/]");

        switch (tipoParticao)
        {
            case TipoParticao.Fixa:
            {
                var tamanhoParticao = AnsiConsole.Prompt(
                    new TextPrompt<uint>("Qual o [green]tamanho da particao[/]?")
                        .ValidationErrorMessage(
                            "[red]O tamanho da particao deve ser uma potencia de 2 menor ou igual ao tamanho total da memoria.[/]")
                        .Validate(particao =>
                            particao > 0 && (particao & (particao - 1)) == 0 && particao <= tamanhoMemoria
                                ? ValidationResult.Success()
                                : ValidationResult.Error()
                        )
                );
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                AnsiConsole.MarkupLine($"Tamanho de particao selecionado: [blue]{tamanhoParticao}[/]");
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

    public static void PrintFabiano()
    {
        var image = new CanvasImage($"{_inputFolder}/fabiano_passuelo_hessel.png");
        AnsiConsole.Write(image);
    }
}