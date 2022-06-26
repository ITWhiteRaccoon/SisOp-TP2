using System.Text;
using Spectre.Console;

namespace SisOp_TP2;

public class GerenciadorBuddy
{
    private BuddyTree _mapa;

    public GerenciadorBuddy(uint tamanhoMemoria)
    {
        _mapa = new BuddyTree(tamanhoMemoria);
    }

    public void Rodar(List<Requisicao> requisicoes)
    {
        var excecoes = new StringBuilder();
        var table = new Table().RoundedBorder();
        table.AddColumns(
            new TableColumn("[bold underline]Comando[/]").Centered().NoWrap(),
            new TableColumn("[bold underline]Memoria[/]").Centered().NoWrap()
        );

        table.AddRow("", ToString());
        foreach (var requisicao in requisicoes)
        {
            if (requisicao.TipoRequisicao == TipoRequisicao.IN)
            {
                try
                {
                    _mapa.Inserir(requisicao.Processo, requisicao.Espaco);
                }
                catch (OutOfMemoryException e)
                {
                    excecoes.AppendLine($"[red]{e.Message}[/]");
                }
            }
            else
            {
                _mapa.Remover(requisicao.Processo);
            }

            table.AddRow(requisicao.ToString(), ToString());
            if (excecoes.Length > 0)
            {
                AnsiConsole.Markup(excecoes.ToString());
            }
        }

        AnsiConsole.Write(table);
    }

    public override string ToString()
    {
        return _mapa.ToString();
    }
}
