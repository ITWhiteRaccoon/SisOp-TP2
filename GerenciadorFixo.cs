using System.Text;
using Spectre.Console;

namespace SisOp_TP2;

public class GerenciadorFixo
{
    private List<Mapeador> _mapa;
    private uint _tamanhoParticao;

    public GerenciadorFixo(uint tamanhoMemoria, uint tamanhoParticao)
    {
        _mapa = new List<Mapeador> { new(null, 0, tamanhoMemoria) };
        _tamanhoParticao = tamanhoParticao;

        //TODO: CHECAR SE O TAMANHO DA PARTICAO É SEMPRE DIVISOR DO TAMANHO DA MEMORIA
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

        //TODO: Implementar lógica de inserção
        for (var i = 0; i < _mapa.Count; i++)
        {
            if (_mapa[i].Processo == null) //Quer dizer que marca espaço livre, não processo
            {
                if (_mapa[i].Tamanho >= tamanhoInserido &&
                    (_mapa[i].Tamanho < tamanhoEscolhido || tamanhoEscolhido == null))
                {
                    indiceEscolhido = i;
                    tamanhoEscolhido = _mapa[i].Tamanho;
                }
            }
        }

        for (var i = 0; i < _mapa.Count; i++)
        {
            if (_mapa[i].Processo == null) //Quer dizer que marca espaço livre, não processo
            {
                if (_mapa[i].Tamanho >= tamanhoInserido &&
                    (_mapa[i].Tamanho > tamanhoEscolhido || tamanhoEscolhido == null))
                {
                    indiceEscolhido = i;
                    tamanhoEscolhido = _mapa[i].Tamanho;
                }
            }
        }


        if (indiceEscolhido != null && tamanhoEscolhido != null)
        {
            if (tamanhoInserido == tamanhoEscolhido)
            {
                _mapa[indiceEscolhido.Value].Processo = processoInserido;
                return;
            }

            if (tamanhoInserido < tamanhoEscolhido)
            {
                var espacoNovo = new Mapeador(processoInserido, _mapa[indiceEscolhido.Value].Inicio, tamanhoInserido);
                _mapa[indiceEscolhido.Value].Inicio += tamanhoInserido;
                _mapa[indiceEscolhido.Value].Tamanho -= tamanhoInserido;
                _mapa.Insert(indiceEscolhido.Value, espacoNovo);
                return;
            }
        }

        throw new OutOfMemoryException(
            $"Não há espaço suficiente para o processo {processoInserido} de tamanho {tamanhoInserido}.");
    }

    private bool Remover(string processo)
    {
        //TODO: Implementar lógica de remoção
        return false;
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
