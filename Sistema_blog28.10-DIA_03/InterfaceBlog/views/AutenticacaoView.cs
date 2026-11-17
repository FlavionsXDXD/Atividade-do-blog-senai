namespace InterfaceBlog.views;

public class AutenticacaoView
{
    public (string nome, string email, string senha) SolicitarDadosCadastro()
    {
        Console.WriteLine("\n--- CRIAR CONTA ---");
        Console.Write("Digite o seu nome: ");
        string nome = Console.ReadLine() ?? "";

        Console.Write("Digite o seu e-mail: ");
        string email = Console.ReadLine() ?? "";

        Console.Write("Digite a sua senha: ");
        string senha = Console.ReadLine() ?? "";

        return (nome, email, senha);
    }

    public (string email, string senha) SolicitarCredenciais()
    {
        Console.WriteLine("\n--- ENTRAR ---");
        Console.Write("Digite o seu e-mail: ");
        string email = Console.ReadLine() ?? "";

        Console.Write("Digite a sua senha: ");
        string senha = Console.ReadLine() ?? "";

        return (email, senha);
    }

    public void ExibirMensagem(string mensagem)
    {
        Console.WriteLine($"\n> {mensagem}");
    }
}