using InterfaceBlog.services;
using InterfaceBlog.views;

namespace InterfaceBlog.controllers;

public class AutenticacaoController
{
    private readonly UsuarioService _usuarioService;
    private readonly AutenticacaoView _view;
    private readonly SessaoUsuario _sessao;

    public AutenticacaoController(
        UsuarioService usuarioService,
        AutenticacaoView view,
        SessaoUsuario sessao)
    {
        _usuarioService = usuarioService;
        _view = view;
        _sessao = sessao;
    }

    public void Cadastrar()
    {
        
if (_sessao.EstaAutenticado)
    {
        _view.ExibirMensagem($"Você já está logado como '{_sessao.NomeUsuario}'. Faça logout para registrar uma nova conta.");
        return;
    }

        try
        {
            var (nome, email, senha) = _view.SolicitarDadosCadastro();
            _usuarioService.Cadastrar(nome, email, senha);
            _view.ExibirMensagem("Conta criada com sucesso! Agora pode entrar.");
        }
        catch (Exception ex)
        {
            _view.ExibirMensagem($"Erro ao cadastrar: {ex.Message}");
        }
    }

    public void Entrar()
    {
if (_sessao.EstaAutenticado)
    {
        _view.ExibirMensagem($"Você já está autenticado como '{_sessao.NomeUsuario}'. Encerre a sessão atual para entrar com outra conta.");
        return;
    }

        try
        {
            var (email, senha) = _view.SolicitarCredenciais();
            var usuario = _usuarioService.Autenticar(email, senha);

            _sessao.Iniciar(usuario.Id, usuario.Nome);
            _view.ExibirMensagem($"Bem-vindo(a), {usuario.Nome}!");
        }
        catch (Exception ex)
        {
            _view.ExibirMensagem($"Erro ao entrar: {ex.Message}");
        }
    }

    public void Sair()
    {
        if (_sessao.EstaAutenticado)
        {
            _sessao.Encerrar();
            _view.ExibirMensagem("Sessão encerrada com sucesso.");
        }
        else
        {
            _view.ExibirMensagem("Nenhum utilizador está autenticado no momento.");
        }
    }
}