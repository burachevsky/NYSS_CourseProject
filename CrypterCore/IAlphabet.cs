using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrypterCore
{
    public interface IAlphabet
    {
        char[] Chars();

        bool Contains(char c);

        int Position(char c);
    }
}
