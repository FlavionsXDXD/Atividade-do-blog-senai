using MySqlConnector;
using InterfaceBlog.data;
using InterfaceBlog.models;

namespace InterfaceBlog.services;

public class UsuarioService
{
    private readonly Conexao _conexao;
    private readonly SenhaService _senhaService;

    public UsuarioService(Conexao conexao, SenhaService senhaService)
    {
        _conexao = conexao;
        _senhaService = senhaService;
    }

    public void Cadastrar(string nome, string email, string senha)
    {
        if (string.IsNullOrWhiteSpace(nome) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(senha))
        {
            throw new ArgumentException("Todos os campos são obrigatórios.");
        }

        string emailNormalizado = email.Trim().ToLower();

        using var conexao = _conexao.CriarConexao();
        conexao.Open();

        // Verificar se e-mail já existe
        const string sqlVerifica = "SELECT COUNT(*) FROM usuario WHERE email = @email;";
        using var cmdVerifica = new MySqlCommand(sqlVerifica, conexao);
        cmdVerifica.Parameters.AddWithValue("@email", emailNormalizado);

        long existe = (long)cmdVerifica.ExecuteScalar()!;
        if (existe > 0)
        {
            throw new InvalidOperationException("Este e-mail já está cadastrado.");
        }

        // Gerar hash com Argon2 (preserva espaços da senha)
        string senhaHash = _senhaService.GerarHash(senha);

        const string sqlInsert = """
            INSERT INTO usuario (nome, email, senha_hash)
            VALUES (@nome, @email, @senhaHash);
            """;

        using var cmdInsert = new MySqlCommand(sqlInsert, conexao);
        cmdInsert.Parameters.AddWithValue("@nome", nome.Trim());
        cmdInsert.Parameters.AddWithValue("@email", emailNormalizado);
        cmdInsert.Parameters.AddWithValue("@senhaHash", senhaHash);

        cmdInsert.ExecuteNonQuery();
    }

    public Usuario Autenticar(string email, string senha)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
        {
            throw new ArgumentException("E-mail e senha são obrigatórios.");
        }

        string emailNormalizado = email.Trim().ToLower();

        using var conexao = _conexao.CriarConexao();
        conexao.Open();

        const string sql = """
            SELECT id_usuario, nome, email, senha_hash
            FROM usuario
            WHERE email = @email;
            """;

        using var comando = new MySqlCommand(sql, conexao);
        comando.Parameters.AddWithValue("@email", emailNormalizado);

        using var leitor = comando.ExecuteReader();

        if (!leitor.Read())
        {
            throw new InvalidOperationException("E-mail ou senha incorretos.");
        }

        string hashArmazenado = leitor.GetString("senha_hash");

        if (!_senhaService.Verificar(senha, hashArmazenado))
        {
            throw new InvalidOperationException("E-mail ou senha incorretos.");
        }

        return new Usuario
        {
            Id = leitor.GetInt32("id_usuario"),
            Nome = leitor.GetString("nome"),
            Email = leitor.GetString("email"),
            SenhaHash = hashArmazenado
        };
    }
}