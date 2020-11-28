namespace CrypterCore
{
    public interface ICrypter
    {
        string Encrypt(string input);

        string Decrypt(string input);
    }
}