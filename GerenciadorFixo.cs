using System.Text;
using Spectre.Console;

namespace SisOp_TP2;

public class GerenciadorFixo
{
    private Espaco[] _memoria;
    private uint _tamanhoParticao;

    public GerenciadorFixo(uint tamanhoMemoria, uint tamanhoParticao)
    {
        _memoria = new Espaco[tamanhoMemoria / tamanhoParticao];
        for (var i = 0; i < _memoria.Length; i++)
        {
            _memoria[i] = new Espaco(null, (uint)(i * tamanhoParticao), tamanhoParticao);
        }

        _tamanhoParticao = tamanhoParticao;
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
                    Inserir(requisicao.Processo, requisicao.Espaco);
                }
                catch (OutOfMemoryException e)
                {
                    excecoes.AppendLine($"[red]{e.Message}[/]");
                }
            }
            else
            {
                Remover(requisicao.Processo);
            }

            table.AddRow(requisicao.ToString(), ToString());
            if (excecoes.Length > 0)
            {
                AnsiConsole.Markup(excecoes.ToString());
            }
        }

        AnsiConsole.Write(table);
    }

    private void Inserir(string processoInserido, uint tamanhoInserido)
    {
        var indiceInicial = -1;
        uint espacoLivre = 0;
        for (var i = 0; i < _memoria.Length; i++)
        {
            if (_memoria[i].Processo == null)
            {
                if (indiceInicial == -1)
                {
                    indiceInicial = i;
                }

                espacoLivre += _memoria[i].Tamanho;
                if (espacoLivre >= tamanhoInserido)
                {
                    var restaInserir = (int)tamanhoInserido;
                    var indice = indiceInicial;
                    while (restaInserir > 0)
                    {
                        _memoria[indice].Processo = processoInserido;
                        _memoria[indice].TamanhoOcupado = restaInserir > _tamanhoParticao
                            ? _tamanhoParticao
                            : (uint)restaInserir;
                        restaInserir -= (int)_tamanhoParticao;
                        indice++;
                    }

                    return;
                }
            }
            else
            {
                espacoLivre = 0;
                indiceInicial = -1;
            }
        }

        throw new OutOfMemoryException(
            $"Nao ha espaco suficiente para o processo {processoInserido} de tamanho {tamanhoInserido}.");
    }

    private void Remover(string processo)
    {
        foreach (var espaco in _memoria)
        {
            if (espaco.Processo == processo)
            {
                espaco.Processo = null;
                espaco.TamanhoOcupado = null;
            }
        }
    }

    public override string ToString()
    {
        return $"|{string.Join<Espaco>('|', _memoria)}|";
    }
}
