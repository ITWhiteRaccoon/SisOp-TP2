using System.Text;
using Spectre.Console;

namespace SisOp_TP2;

public class GerenciadorBuddy
{
    private List<Espaco> _mapa;

    public GerenciadorBuddy(uint tamanhoMemoria)
    {
        _mapa = new List<Espaco> { new(null, 0, tamanhoMemoria) };
    }

    public void Rodar(List<Requisicao> requisicoes)
    {
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
                    Inserir(requisicao.Processo, requisicao.Espaco);
                }
                catch (OutOfMemoryException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Remover(requisicao.Processo);
            }

            table.AddRow(requisicao.ToString(), ToString());
        }

        AnsiConsole.Write(table);
    }

    private void Inserir(string processoInserido, uint tamanhoInserido)
    {
        int? indiceEscolhido = null;
        uint? tamanhoEscolhido = null;
        for (var i = 0; i < _mapa.Count; i++)
        {
            if (_mapa[i].Processo == null && //Quer dizer que marca espaço livre, não processo
                _mapa[i].Tamanho >= tamanhoInserido && //Cabe o processo
                (_mapa[i].Tamanho < tamanhoEscolhido || tamanhoEscolhido == null)) //Best-fit
            {
                indiceEscolhido = i;
                tamanhoEscolhido = _mapa[i].Tamanho;
            }
        }

        if (indiceEscolhido != null && tamanhoEscolhido != null) //Se algum espaco foi encontrado
        {
            var metade = _mapa[indiceEscolhido.Value].Tamanho / 2;
            if (metade >= tamanhoInserido)
            {
                
            }
        }

        throw new OutOfMemoryException(
            $"Não há espaço suficiente para o processo {processoInserido} de tamanho {tamanhoInserido}.");
    }

    private void Remover(string processoRemovido)
    {
        //TODO: Change to fit buddy
        for (var i = 0; i < _mapa.Count; i++)
        {
            if (_mapa[i].Processo != null && _mapa[i].Processo == processoRemovido)
            {
                _mapa[i].Processo = null;
                if (i + 1 < _mapa.Count && _mapa[i + 1].Processo == null)
                {
                    _mapa[i].Tamanho += _mapa[i + 1].Tamanho;
                    _mapa.RemoveAt(i + 1);
                }

                if (i >= 1 && _mapa[i - 1].Processo == null)
                {
                    _mapa[i].Tamanho += _mapa[i - 1].Tamanho;
                    _mapa.RemoveAt(i - 1);
                }

                return;
            }
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder("|");
        foreach (var espaco in _mapa)
        {
            if (espaco.Processo == null)
            {
                sb.Append($" [green]{espaco.Tamanho}[/] |");
            }
            else
            {
                sb.Append($" [red]{espaco.Processo}[/] |");
            }
        }

        return sb.ToString();
    }
}
