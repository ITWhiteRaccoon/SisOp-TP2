using System.Text;

namespace SisOp_TP2;

public class BuddyTree
{
    private BuddyTreeNode _root;

    public BuddyTree(uint tamanho)
    {
        _root = new BuddyTreeNode(null, 0, tamanho);
    }

    public void Inserir(string processo, uint tamanho)
    {
        var nodo = EncontrarDisponivel(_root, tamanho);
        if (nodo == null)
        {
            throw new OutOfMemoryException(
                $"Não há espaço suficiente para o processo {processo} de tamanho {tamanho}.");
        }

        nodo.Processo = processo;
        nodo.TamanhoOcupado = tamanho;
    }

    private static BuddyTreeNode? EncontrarDisponivel(BuddyTreeNode inicio, uint tamanho)
    {
        if (inicio.Processo == null && inicio.Tamanho >= tamanho) //Se o nodo não está ocupado e cabe o que quero
        {
            if (inicio.Dividido)
            {
                return EncontrarDisponivel(inicio.Esquerda, tamanho) ?? EncontrarDisponivel(inicio.Direita, tamanho);
            }

            var nodo = inicio;
            var metade = nodo.Tamanho / 2;
            while (metade >= tamanho)
            {
                nodo.Esquerda = new BuddyTreeNode(null, nodo.Inicio, metade) { Pai = nodo };
                nodo.Direita = new BuddyTreeNode(null, metade, metade) { Pai = nodo };
                nodo.Dividido = true;
                nodo = nodo.Esquerda;
                metade = nodo.Tamanho / 2;
            }

            return nodo;
        }

        return null;
    }

    public void Remover(string processo)
    {
        var processoRemover = EncontrarProcesso(_root, processo);
        if (processoRemover != null)
        {
            var sup = processoRemover.Pai;
            processoRemover.Processo = null;
            processoRemover.TamanhoOcupado = 0;

            while (sup != null)
            {
                if (sup.Esquerda.Processo != null || sup.Esquerda.Dividido ||
                    sup.Direita.Processo != null || sup.Direita.Dividido)
                {
                    return; //Se subir um nivel e houver conteudo em algum dos filhos, para de tentar mesclar
                }

                sup.Esquerda = null;
                sup.Direita = null;
                sup.Dividido = false;
                sup = sup.Pai;
            }
        }
    }

    private static BuddyTreeNode? EncontrarProcesso(BuddyTreeNode inicio, string processo)
    {
        if (inicio.Processo != null && inicio.Processo == processo)
        {
            return inicio;
        }

        if (inicio.Processo == null && inicio.Dividido)
        {
            return EncontrarProcesso(inicio.Esquerda, processo) ?? EncontrarProcesso(inicio.Direita, processo);
        }

        return null;
    }

    private static List<BuddyTreeNode> GetLeafNodes(BuddyTreeNode inicio, List<BuddyTreeNode> folhas)
    {
        if (inicio.Esquerda == null && inicio.Direita == null)
        {
            folhas.Add(inicio);
            return folhas;
        }

        if (inicio.Esquerda != null)
        {
            GetLeafNodes(inicio.Esquerda, folhas);
        }

        if (inicio.Direita != null)
        {
            GetLeafNodes(inicio.Direita, folhas);
        }

        return folhas;
    }

    public override string ToString()
    {
        return $"|{string.Join('|', GetLeafNodes(_root, new List<BuddyTreeNode>()))}|";
    }

    private class BuddyTreeNode
    {
        public string? Processo { get; set; }
        public uint Inicio { get; }
        public uint Tamanho { get; }
        public bool Dividido { get; set; }
        public uint TamanhoOcupado { get; set; }
        public BuddyTreeNode? Pai { get; set; }
        public BuddyTreeNode? Esquerda { get; set; }
        public BuddyTreeNode? Direita { get; set; }

        public BuddyTreeNode(string? processo, uint inicio, uint tamanho)
        {
            Processo = processo;
            Inicio = inicio;
            Tamanho = tamanho;
            Dividido = false;
        }

        public override string ToString()
        {
            if (Processo != null)
            {
                return Tamanho == TamanhoOcupado
                    ? $" [yellow]{Processo}[/] "
                    : $" [yellow]{Processo}[/] [red]{Tamanho - TamanhoOcupado}[/] ";
            }

            return $" [green]{Tamanho}[/] ";
        }
    }
}
