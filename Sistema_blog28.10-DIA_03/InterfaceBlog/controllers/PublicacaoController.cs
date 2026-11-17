using InterfaceBlog.services;
using InterfaceBlog.views;

namespace InterfaceBlog.controllers;

public class PublicacaoController
{
    private readonly PublicacaoService _service;
    private readonly PublicacaoView _view;
    private readonly SessaoUsuario _sessao;

    public PublicacaoController(PublicacaoService service, PublicacaoView view, SessaoUsuario sessao)
    {
        _service = service;
        _view = view;
        _sessao = sessao;
    }

    // Criar publicação usando as novas chamadas do Dia 3
    public void Criar()
    {
        try
        {
            if (!_sessao.EstaAutenticado)
            {
                _view.ExibirMensagem("É necessário estar autenticado para criar uma publicação.");
                return;
            }

            var (titulo, texto) = _view.SolicitarDadosPublicacao();
            _service.Criar(titulo, texto);

            _view.ExibirMensagem("Publicação criada com sucesso!");
        }
        catch (Exception ex)
        {
            _view.ExibirMensagem($"Erro ao criar publicação: {ex.Message}");
        }
    }

    public void Visualizar()
    {
        try
        {
            int id = _view.SolicitarId();

            if (id <= 0)
            {
                _view.ExibirMensagem("ID inválido.");
                return;
            }

            var publicacao = _service.ObterPorId(id);

            if (publicacao == null)
            {
                _view.ExibirMensagem("Publicação não encontrada.");
                return;
            }

            string nomeAutor = _service.ObterNomeAutor(publicacao.UsuarioId);
            _view.ExibirDetalhes(publicacao, nomeAutor);
            
        }
        catch (Exception ex)
        {
            _view.ExibirMensagem($"Erro ao visualizar publicação: {ex.Message}");
        }
    }

    public void Listar()
    {
        try
        {
            var publicacoes = _service.ListarRecentes();
            _view.ExibirPublicacoes(publicacoes);
        }
        catch (Exception ex)
        {
            _view.ExibirMensagem($"Erro ao carregar publicações: {ex.Message}");
        }
    }

    public void Atualizar()
    {
        try
        {
            int id = _view.SolicitarId();
            if (id <= 0)
            {
                _view.ExibirMensagem("ID inválido.");
                return;
            }

            var (titulo, texto) = _view.SolicitarDadosPublicacao();
            _service.Atualizar(id, titulo, texto);

            _view.ExibirMensagem("Publicação atualizada com sucesso!");
        }
        catch (Exception ex)
        {
            _view.ExibirMensagem($"Erro ao atualizar publicação: {ex.Message}");
        }
    }

    public void Excluir()
    {
        try
        {
            int id = _view.SolicitarId();
            if (id <= 0)
            {
                _view.ExibirMensagem("ID inválido.");
                return;
            }

            Console.Write("Tem certeza que deseja excluir? (s/n): ");
            string resposta = Console.ReadLine() ?? "";

            if (resposta.Trim().ToLower() != "s")
            {
                _view.ExibirMensagem("Exclusão cancelada.");
                return;
            }

            _service.Excluir(id);
            _view.ExibirMensagem("Publicação excluída com sucesso!");
        }
        catch (Exception ex)
        {
            _view.ExibirMensagem($"Erro ao excluir publicação: {ex.Message}");
        }
    }
}