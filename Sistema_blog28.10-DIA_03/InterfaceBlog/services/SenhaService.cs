using MySqlConnector;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using InterfaceBlog.data;
using InterfaceBlog.models;

namespace InterfaceBlog.services;

public class SenhaService
{

private byte[] GerarSalt()
{
    return  RandomNumberGenerator.GetBytes(16);
}

 
public string GerarHash(string senha)
{
    if (string.IsNullOrWhiteSpace(senha))
    {
        throw new ArgumentException(
            "A senha não pode ser vazia."
        );
    }

    byte[] salt = GerarSalt();

    byte[] hash = CalcularHash(senha, salt);

    string saltBase64 = Convert.ToBase64String(salt);
    string hashBase64 = Convert.ToBase64String(hash);

    return $"{saltBase64}:{hashBase64}";
}


public bool Verificar(string senha, string hashArmazenado)
{
    if (string.IsNullOrEmpty(senha) ||
        string.IsNullOrEmpty(hashArmazenado))
    {
        return false;
    }

    string[] partes = hashArmazenado.Split(':');

    if (partes.Length != 2)
    {
        return false;
    }

try
{
    byte[] salt = Convert.FromBase64String(partes[0]);
    byte[] hashOriginal = Convert.FromBase64String(partes[1]);
    if (salt.Length != 16 || hashOriginal.Length != 32)
{
    return false;
}
    byte[] hashInformado = CalcularHash(senha, salt);

    return CryptographicOperations.FixedTimeEquals(
    hashOriginal,
    hashInformado
);
}
catch (FormatException)
{
    return false;
}

}

    private byte[] CalcularHash(string senha, byte[] salt)
{
    byte[] senhaEmBytes = System.Text.Encoding.UTF8.GetBytes(senha);

    using var argon2 = new Argon2id(senhaEmBytes)
    {
        Salt = salt,
        MemorySize = 19456,
        Iterations = 2,
        DegreeOfParallelism = 1
    };

    return argon2.GetBytes(32);
}


}