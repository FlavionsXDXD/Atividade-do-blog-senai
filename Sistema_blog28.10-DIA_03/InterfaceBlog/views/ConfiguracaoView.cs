namespace InterfaceBlog.views;

public class ConfiguracaoView
{
    public void ExibirErroConexao(string detalheErro)
    {
        Console.Clear();
        Console.WriteLine("==================================================");
        Console.WriteLine("⚠️ FALHA DE CONEXÃO COM O BANCO DE DADOS (MySQL) ⚠️");
        Console.WriteLine("==================================================");
        Console.WriteLine($"Erro retornado: {detalheErro}\n");
        Console.WriteLine($"Porta atual:   {Environment.GetEnvironmentVariable("DB_PORT")}");
        Console.WriteLine($"Usuário atual: {Environment.GetEnvironmentVariable("DB_USER")}");
              Console.WriteLine($"Senha atual: {Environment.GetEnvironmentVariable("DB_PASSWORD")}");
    }

    public int ExibirMenuTrocaPorta()
    {
        Console.WriteLine("\nO que você deseja fazer?");
        Console.WriteLine("1 - Alterar porta do MySQL (DB_PORT)");
        Console.WriteLine("2 - Alterar usuário do MySQL (DB_USER)");
        Console.WriteLine("3 - Alterar senha do MySQL (DB_PASSWORD)");
        Console.WriteLine("4 - Tentar conectar novamente com os dados atuais");
        Console.WriteLine("0 - Sair da aplicação");
        Console.Write("Escolha uma opção: ");

        if (int.TryParse(Console.ReadLine(), out int opcao))
        {
            return opcao;
        }
        return -1;
    }

    public string SolicitarNovaPorta()
    {
        Console.Write("\nDigite a nova porta do MySQL (ex: 3306, 3309): ");
        return Console.ReadLine() ?? "";
    }

    public string SolicitarNovoUsuario()
    {
        Console.Write("\nDigite o novo usuário do MySQL (ex: root): ");
        return Console.ReadLine() ?? "";
    }

    public string SolicitarNovaSenha()
    {
        Console.Write("\nDigite a nova senha do MySQL: ");
        return Console.ReadLine() ?? "";
    }

    public void ExibirMensagem(string mensagem)
    {
        Console.WriteLine($"\n> {mensagem}");
    }
}