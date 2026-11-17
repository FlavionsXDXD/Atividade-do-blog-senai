namespace InterfaceBlog.services;

public class SessaoUsuario
{
    public int? UsuarioId { get; private set; }
    public string? NomeUsuario { get; private set; }

    public bool EstaAutenticado => UsuarioId.HasValue;

    public void Iniciar(int usuarioId, string nomeUsuario)
    {
        UsuarioId = usuarioId;
        NomeUsuario = nomeUsuario;
    }

    public void Encerrar()
    {
        UsuarioId = null;
        NomeUsuario = null;
    }
}