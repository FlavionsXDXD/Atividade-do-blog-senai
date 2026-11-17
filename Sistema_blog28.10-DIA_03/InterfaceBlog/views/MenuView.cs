using InterfaceBlog.services;

namespace InterfaceBlog.views;

public class MenuView
{
    public int ExibirMenu(SessaoUsuario sessao)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("===== BLOG =====");

            if (sessao.EstaAutenticado)
            {
                Console.WriteLine($"[Sessão Ativa: {sessao.NomeUsuario}]");
                Console.WriteLine("1 - Listar publicações");
                Console.WriteLine("2 - Abrir publicação");
                Console.WriteLine("5 - Criar publicação");
                Console.WriteLine("6 - Excluir publicação");
                Console.WriteLine("9 - Encerrar Sessão (Logout)");
            }
            else
            {
                Console.WriteLine("[Visitante - Não Autenticado]");
                Console.WriteLine("1 - Listar publicações");
                Console.WriteLine("2 - Abrir publicação");
                Console.WriteLine("3 - Criar conta");
                Console.WriteLine("4 - Fazer login");
                Console.WriteLine("5 - Criar publicação");
                Console.WriteLine("6 - Excluir publicação");
                Console.WriteLine("9 - Encerrar Sessão (Logout)");
            }

            Console.WriteLine("0 - Sair");
            Console.Write("Escolha uma opção: ");

            string? entrada = Console.ReadLine();

            if (int.TryParse(entrada, out int opcao))
            {
                return opcao;
            }

            Console.WriteLine("Opção inválida.");
        }
    }
}