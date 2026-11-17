using DotNetEnv;
using InterfaceBlog.controllers;
using InterfaceBlog.data;
using InterfaceBlog.services;
using InterfaceBlog.views;

string caminhoEnv = Path.Combine(AppContext.BaseDirectory, "projeto.env");

// 1. Carrega as variáveis do arquivo .env
DotNetEnv.Env.Load(caminhoEnv);

// 2. Menu de Pré-Validação da Conexão com MySQL
var configService = new ConfiguracaoService(caminhoEnv);
var configView = new ConfiguracaoView();

while (!configService.TestarConexao(out string erro))
{
    configView.ExibirErroConexao(erro);
    int opcaoConfig = configView.ExibirMenuTrocaPorta();

    if (opcaoConfig == 1)
    {
        string novaPorta = configView.SolicitarNovaPorta();

        if (uint.TryParse(novaPorta, out _))
        {
            configService.AtualizarPortaEnv(novaPorta);
            configView.ExibirMensagem($"Porta alterada para {novaPorta}. Re-testando conexão...");
        }
        else
        {
            configView.ExibirMensagem("Porta inválida. Digite apenas números.");
        }
    }
    else if (opcaoConfig == 2)
    {
        string novoUsuario = configView.SolicitarNovoUsuario();
        if (!string.IsNullOrWhiteSpace(novoUsuario))
        {
            configService.AtualizarUsuarioEnv(novoUsuario);
            configView.ExibirMensagem($"Usuário alterado para '{novoUsuario}'. Re-testando conexão...");
        }
        else
        {
            configView.ExibirMensagem("O usuário não pode ser vazio.");
        }
    }
    else if (opcaoConfig == 3)
    {
        string novaSenha = configView.SolicitarNovaSenha();
        configService.AtualizarSenhaEnv(novaSenha);
        configView.ExibirMensagem("Senha alterada. Re-testando conexão...");
    }
    else if (opcaoConfig == 4)
    {
        configView.ExibirMensagem("Re-testando conexão...");
    }
    else if (opcaoConfig == 0)
    {
        Console.WriteLine("Aplicação encerrada.");
        return;
    }
    else
    {
        configView.ExibirMensagem("Opção inválida.");
    }

    Thread.Sleep(1500);
}

Console.Clear();
Console.WriteLine("✅ Conexão com o banco de dados estabelecida com sucesso!");
Console.WriteLine("\nPressione qualquer tecla para entrar no menu principal...");
Console.ReadKey();

// --- APLICAÇÃO PRINCIPAL (Injeção de dependências do Dia 3) ---
var conexao = new Conexao();
var sessao = new SessaoUsuario(); // Sessão compartilhada

var senhaService = new SenhaService();
var usuarioService = new UsuarioService(conexao, senhaService);
var autenticacaoView = new AutenticacaoView();
var autenticacaoController = new AutenticacaoController(usuarioService, autenticacaoView, sessao);

// Service e Controller recebem a sessão compartilhada
var publicacaoService = new PublicacaoService(conexao, sessao);
var publicacaoView = new PublicacaoView();
var publicacaoController = new PublicacaoController(publicacaoService, publicacaoView, sessao);

var menu = new MenuView();

bool executando = true;

while (executando)
{
    Console.Clear();

    int opcao = menu.ExibirMenu(sessao);
   
    Console.Clear();
    switch (opcao)
    {
        case 1:
            publicacaoController.Listar();
            Pausar();
            break;

        case 2:
            publicacaoController.Visualizar(); // Opção 2: Abrir publicação
            Pausar();
            break;

        case 3:
            autenticacaoController.Cadastrar(); // Criar conta
            Pausar();
            break;

        case 4:
            autenticacaoController.Entrar(); // Fazer login
            Pausar();
            break;

        case 5:
            publicacaoController.Criar(); // Opção 5: Criar publicação
            Pausar();
            break;

        case 6:
            publicacaoController.Excluir();
            Pausar();
            break;

        case 9:
            autenticacaoController.Sair();
            Pausar();
            break;

        case 0:
            executando = false;
            Console.WriteLine("Aplicação encerrada.");
            break;

        default:
            Console.WriteLine("Opção inválida!");
            Pausar();
            break;
    }
}

static void Pausar()
{
    Console.WriteLine("\nPressione qualquer tecla para continuar...");
    Console.ReadKey();
}