﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrypterCore
{
    public class Alphabets
    {
        public static IAlphabet RUSSIAN => new RussianAlphabet();
        public static IAlphabet ENGLISH => new EnglishAlphabet();

        public static List<IAlphabet> AlphabetList()
        {
            return new List<IAlphabet> { RUSSIAN, ENGLISH };
        }
    }
}
