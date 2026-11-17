using MySqlConnector;
using InterfaceBlog.data;
using InterfaceBlog.models;

namespace InterfaceBlog.services;

public class PublicacaoService
{
    private readonly Conexao _conexao;
    private readonly SessaoUsuario _sessao;

    public PublicacaoService(Conexao conexao, SessaoUsuario sessao)
    {
        _conexao = conexao;
        _sessao = sessao;
    }

    public void Criar(string titulo, string texto)
    {
        if (!_sessao.EstaAutenticado || !_sessao.UsuarioId.HasValue)
        {
            throw new InvalidOperationException("É necessário estar autenticado para criar uma publicação.");
        }

        if (string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(texto))
        {
            throw new ArgumentException("Título e texto não podem ser vazios.");
        }

        using var conexao = _conexao.CriarConexao();
        conexao.Open();

        const string sql = """
            INSERT INTO publicacao (titulo, texto, data_publicacao, id_autor)
            VALUES (@titulo, @texto, NOW(), @autor);
            """;

        using var comando = new MySqlCommand(sql, conexao);
        comando.Parameters.AddWithValue("@titulo", titulo.Trim());
        comando.Parameters.AddWithValue("@texto", texto.Trim());
        comando.Parameters.AddWithValue("@autor", _sessao.UsuarioId.Value);

        comando.ExecuteNonQuery();
    }

    public Publicacao? ObterPorId(int id)
    {
        using var conexao = _conexao.CriarConexao();
        conexao.Open();

        const string sql = """
            SELECT id_publicacao, titulo, texto, data_publicacao, id_autor
            FROM publicacao
            WHERE id_publicacao = @id;
            """;

        using var comando = new MySqlCommand(sql, conexao);
        comando.Parameters.AddWithValue("@id", id);

        using var leitor = comando.ExecuteReader();

        if (leitor.Read())
        {
            return new Publicacao
            {
                Id = leitor.GetInt32("id_publicacao"),
                Titulo = leitor.GetString("titulo"),
                Texto = leitor.GetString("texto"),
                DataCriacao = leitor.GetDateTime("data_publicacao"),
                UsuarioId = leitor.GetInt32("id_autor")
            };
        }

        return null;
    }

    public string ObterNomeAutor(int idAutor)
    {
        using var conexao = _conexao.CriarConexao();
        conexao.Open();

        const string sql = "SELECT nome FROM usuario WHERE id_usuario = @idAutor;";

        using var comando = new MySqlCommand(sql, conexao);
        comando.Parameters.AddWithValue("@idAutor", idAutor);

        var resultado = comando.ExecuteScalar();
        return resultado?.ToString() ?? "Autor desconhecido";
    }

    public List<Publicacao> ListarRecentes()
    {
        var publicacoes = new List<Publicacao>();

        using var conexao = _conexao.CriarConexao();
        conexao.Open();

        const string sql = """
            SELECT id_publicacao, titulo, texto, data_publicacao, id_autor
            FROM publicacao
            ORDER BY data_publicacao DESC;
            """;

        using var comando = new MySqlCommand(sql, conexao);
        using var leitor = comando.ExecuteReader();

        while (leitor.Read())
        {
            publicacoes.Add(new Publicacao
            {
                Id = leitor.GetInt32("id_publicacao"),
                Titulo = leitor.GetString("titulo"),
                Texto = leitor.GetString("texto"),
                DataCriacao = leitor.GetDateTime("data_publicacao"),
                UsuarioId = leitor.GetInt32("id_autor")
            });
        }

        return publicacoes;
    }

    public void Atualizar(int id, string titulo, string texto)
    {
        using var conexao = _conexao.CriarConexao();
        conexao.Open();

        const string sql = """
            UPDATE publicacao
            SET titulo = @titulo,
                texto = @texto
            WHERE id_publicacao = @id;
            """;

        using var comando = new MySqlCommand(sql, conexao);
        comando.Parameters.AddWithValue("@titulo", titulo);
        comando.Parameters.AddWithValue("@texto", texto);
        comando.Parameters.AddWithValue("@id", id);

        int linhasAfetadas = comando.ExecuteNonQuery();
        if (linhasAfetadas == 0)
        {
            throw new Exception("Publicação não encontrada.");
        }
    }

    public void Excluir(int id)
{
    // 1. Valida se o usuário está logado
    if (!_sessao.EstaAutenticado || !_sessao.UsuarioId.HasValue)
    {
        throw new InvalidOperationException("Você precisa estar autenticado para excluir uma publicação.");
    }

    // 2. Busca a publicação para checar a autoria
    var publicacao = ObterPorId(id);

    if (publicacao == null)
    {
        throw new Exception("Publicação não encontrada.");
    }

    // 3. Valida se o usuário da sessão é o mesmo que criou a publicação
    if (publicacao.UsuarioId != _sessao.UsuarioId.Value)
    {
        throw new InvalidOperationException("Você não tem permissão para excluir uma publicação de outro autor.");
    }

    // 4. Executa a exclusão no banco
    using var conexao = _conexao.CriarConexao();
    conexao.Open();

    const string sql = "DELETE FROM publicacao WHERE id_publicacao = @id;";

    using var comando = new MySqlCommand(sql, conexao);
    comando.Parameters.AddWithValue("@id", id);

    comando.ExecuteNonQuery();
}
}