using InterfaceBlog.models;

namespace InterfaceBlog.views;

public class PublicacaoView
{
    public (string Titulo, string Texto) SolicitarDadosPublicacao()
    {
        Console.Write("Digite o título: ");
        string titulo = Console.ReadLine() ?? "";

        Console.Write("Digite o texto: ");
        string texto = Console.ReadLine() ?? "";

        return (titulo, texto);
    }

    public int SolicitarId()
    {
        Console.Write("Digite o ID da publicação: ");
        string entrada = Console.ReadLine() ?? "";

        if (int.TryParse(entrada, out int id))
        {
            return id;
        }

        return -1;
    }

    public void ExibirDetalhes(Publicacao publicacao, string nomeAutor)
    {
        Console.WriteLine();
        Console.WriteLine("===== DETALHES DA PUBLICAÇÃO =====");
        Console.WriteLine($"ID: {publicacao.Id}");
        Console.WriteLine($"Título: {publicacao.Titulo}");
        Console.WriteLine($"Autor: {nomeAutor}");
        Console.WriteLine($"Data: {publicacao.DataCriacao:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"Texto: {publicacao.Texto}");
        Console.WriteLine("----------------------------------");
        
        Console.WriteLine("Comentários:");
        var comentarios = new List<string>();
        if (comentarios.Count == 0)
        {
            Console.WriteLine("Nenhum comentário cadastrado.");
        }
        Console.WriteLine("==================================");
    }

    public void ExibirPublicacoes(List<Publicacao> publicacoes)
    {
        if (publicacoes.Count == 0)
        {
            ExibirMensagem("Não há publicações cadastradas.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("===== PUBLICAÇÕES =====");

        foreach (Publicacao publicacao in publicacoes)
        {
            Console.WriteLine($"ID: {publicacao.Id} | Título: {publicacao.Titulo} | Data: {publicacao.DataCriacao:dd/MM/yyyy HH:mm}");
            Console.WriteLine("-----------------------");
        }
    }

    public void ExibirMensagem(string mensagem)
    {
        Console.WriteLine(mensagem);
    }
}