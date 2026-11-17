using MySqlConnector;

namespace InterfaceBlog.data;

public class ConfiguracaoService
{
    private readonly string _caminhoEnv;

    public ConfiguracaoService(string caminhoEnv)
    {
        _caminhoEnv = caminhoEnv;
    }

    public bool TestarConexao(out string mensagemErro)
    {
        try
        {
            var conexao = new Conexao();
            using var conn = conexao.CriarConexao();
            conn.Open();
            mensagemErro = string.Empty;
            return true;
        }
        catch (Exception ex)
        {
            mensagemErro = ex.Message;
            return false;
        }
    }

    public void AtualizarPortaEnv(string novaPorta)
    {
        AtualizarChaveEnv("DB_PORT", novaPorta);
    }

    public void AtualizarUsuarioEnv(string novoUsuario)
    {
        AtualizarChaveEnv("DB_USER", novoUsuario);
    }

    public void AtualizarSenhaEnv(string novaSenha)
    {
        AtualizarChaveEnv("DB_PASSWORD", novaSenha);
    }

    private void AtualizarChaveEnv(string chave, string novoValor)
    {
        if (!File.Exists(_caminhoEnv))
        {
            throw new FileNotFoundException("Arquivo projeto.env não encontrado.");
        }

        string[] linhas = File.ReadAllLines(_caminhoEnv);
        bool chaveEncontrada = false;

        for (int i = 0; i < linhas.Length; i++)
        {
            if (linhas[i].StartsWith($"{chave}="))
            {
                linhas[i] = $"{chave}={novoValor}";
                chaveEncontrada = true;
                break;
            }
        }

        if (!chaveEncontrada)
        {
            var listaLinhas = linhas.ToList();
            listaLinhas.Add($"{chave}={novoValor}");
            linhas = listaLinhas.ToArray();
        }

        File.WriteAllLines(_caminhoEnv, linhas);

        // Atualiza a variável na memória do processo atual
        Environment.SetEnvironmentVariable(chave, novoValor);
    }
}