namespace SisOp_TP2;

public class GerenciadorVariavel
{
    private List<Mapeador> _mapa;
    private PoliticaAlocacao _politicaAlocacao;

    public GerenciadorVariavel(uint tamanhoMemoria, PoliticaAlocacao politicaAlocacao)
    {
        _mapa = new List<Mapeador> { new(null, 0, tamanhoMemoria) };
        _politicaAlocacao = politicaAlocacao;
    }

    public void Rodar(List<Requisicao> requisicoes)
    {
        foreach (var requisicao in requisicoes)
        {
            if (requisicao.TipoRequisicao == TipoRequisicao.IN)
            {
                Inserir(requisicao.Processo, requisicao.Espaco);
            }
            else
            {
                Remover(requisicao.Processo);
            }
        }
    }

    private void Inserir(string processoInserido, uint tamanhoInserido)
    {
        int? indiceEscolhido = null;
        uint? tamanhoEscolhido = null;
        if (_politicaAlocacao == PoliticaAlocacao.BestFit)
        {
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
        }
        else if (_politicaAlocacao == PoliticaAlocacao.WorstFit)
        {
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

    private void Remover(string processoRemovido)
    {
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
        return base.ToString();
    }
}
