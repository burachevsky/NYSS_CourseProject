using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrypterCore
{
    public class RussianAlphabet : IAlphabet
    {
        private char[] chars = 
        {
            'а', 'б', 'в', 'г', 'д',
            'е', 'ё', 'ж', 'з', 'и',
            'й', 'к', 'л', 'м', 'н',
            'о', 'п', 'р', 'с', 'т',
            'у', 'ф', 'х', 'ц', 'ч',
            'ш', 'щ', 'ъ', 'ы', 'ь',
            'э', 'ю', 'я'
        };

        public char[] Chars()
        {
            return chars;
        }

        public bool Contains(char c)
        {
            return (c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я') || c == 'ё' || c == 'Ё';
        }

        public int Position(char c)
        {
            if (c >= 'а' && c <= 'е')
            {
                return c - 'а';
            }
            
            if (c >= 'ж' && c <= 'я')
            {
                return c - 'а' + 1;
            }

            if (c >= 'А' && c <= 'Е')
            {
                return c - 'А';
            }

            if (c >= 'Ж' && c <= 'Я')
            {
                return c - 'А' + 1;
            }

            if (c == 'ё' || c == 'Ё')
            {
                return 6;
            }

            return -1;
        }

        public override string ToString()
        {
            return "Русский";
        }
    }
}
