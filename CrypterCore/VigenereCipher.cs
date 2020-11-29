using System;
using System.Linq;

namespace CrypterCore
{
    public class VigenereCipher : ICrypter
    {
        public CyclicArray<char> Key { get; }

        public string KeyAsString => new string(Key.ToArray());

        private readonly VigenereTable table;

        public IAlphabet Alphabet { get; }

        public VigenereCipher(string key, IAlphabet alphabet)
        {
            if (key == null || alphabet == null)
            {
                throw new ArgumentException($"key = {key}, alph = {alphabet}");
            }

            Key = new CyclicArray<char>(key.Where(alphabet.Contains).ToArray());

            if (Key.Length == 0)
            {
                throw new ArgumentException();
            }

            table = new VigenereTable(alphabet);
            Alphabet = alphabet;
        }

        public string Decrypt(string input)
        {
            return Crypt(input, table.DecryptChar);
        }

        public string Encrypt(string input)
        {
            return Crypt(input, table.EncryptChar);
        }

        private string Crypt(string input, Func<char, char, char> crypt)
        {
            var result = new char[input.Length];

            for (int i = 0, keyI = 0; i < input.Length; ++i)
            {
                var value = input[i];

                result[i] = Alphabet.Contains(value) ? crypt(value, Key[keyI++]) : value;
            }

            return new string(result);
        }
    }
}