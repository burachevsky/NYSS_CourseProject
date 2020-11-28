using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrypterCore
{
    public class VigenereTable
    {
        private IAlphabet alphabet;
        private CyclicArray<char> cyclicAlphabet;

        public VigenereTable(IAlphabet alphabet)
        {
            this.alphabet = alphabet;
            cyclicAlphabet = new CyclicArray<char>(alphabet.Chars());
        }

        public char EncryptChar(char value, char key)
        {
            return cyclicAlphabet[alphabet.Position(value) + alphabet.Position(key)];
        }

        public char DecryptChar(char value, char key)
        {
            return cyclicAlphabet[alphabet.Position(value) - alphabet.Position(key)];
        }
    }
}
