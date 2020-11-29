using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrypterCore
{
    public class EnglishAlphabet : IAlphabet
    {
        private char[] data;

        public EnglishAlphabet()
        {
            data = new char['z' - 'a' + 1];

            for (var i = 0; i < data.Length; ++i)
            {
                data[i] = (char) ('a' + i);
            }
        }

        public char[] Chars()
        {
            return data;
        }

        public bool Contains(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        public int Position(char c)
        {
            return (c >= 'a' && c <= 'z')
                ? c - 'a'
                : (c >= 'A' && c <= 'Z')
                    ? c - 'A'
                    : -1;
        }

        public override string ToString()
        {
            return "English";
        }
    }
}
