
using MySqlConnector;

namespace InterfaceBlog.data;

public class Conexao
{
    public MySqlConnection CriarConexao()
    {
        string host = LerVariavel("DB_HOST");
        string porta = LerVariavel("DB_PORT");
        string banco = LerVariavel("DB_NAME");
        string usuario = LerVariavel("DB_USER");
        string senha = LerVariavel("DB_PASSWORD");

        var construtor = new MySqlConnectionStringBuilder
        {
            Server = host,
            Port = uint.Parse(porta),
            Database = banco,
            UserID = usuario,
            Password = senha
        };

        return new MySqlConnection(construtor.ConnectionString);
    }

    private string LerVariavel(string nome)
    {
        return Environment.GetEnvironmentVariable(nome)
            ?? throw new InvalidOperationException(
                $"{nome} não configurado."
            );
    }
}
