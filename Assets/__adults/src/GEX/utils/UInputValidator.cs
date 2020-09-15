using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;


namespace GEX.utils
{
	public class UInputValidator : MonoBehaviour
	{
        public InputField inputField;
        [Tooltip("include this characters")]
        public String include = "";
        [Tooltip("include this characters decoded from charcodes")]
        public int [] includeCharCodes = null;

        [Tooltip("ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        public bool upperCase =     true;
        [Tooltip("abcdefghijklmnopqrstuvwxyz")]
        public bool lowerCase =     true;
        [Tooltip("0123456789")]
        public bool numerals =      true;
        [Tooltip("áéíóúñ")]
        public bool latin =         true;
        [Tooltip(".,:;\"'`´")]
        public bool punctuation_1 =   true;
        [Tooltip("¿?¡!@#$%^&*()_-+={}[]|<>/\\~")]
        public bool punctuation_2 =     true;
        [Tooltip("space character")]
        public bool space=          true;
        
        private const String char_upper_case =      "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const String char_lower_case =      "abcdefghijklmnopqrstuvwxyz";
        private const String char_numerals=         "0123456789";
        private const String char_latin =           "áéíóúñ";
        private const String char_punctuation_1 =   ".,:;\"'`´";
        private const String char_punctuation_2 =   "¿?¡!@#$%^&*()_-+={}[]|<>/\\~";
        private const String char_space =           " ";

        private List<char> allowedChars;

        public void Awake()
        {
            int a;

            allowedChars = new List<char>();

            inputField.onValidateInput += new InputField.OnValidateInput(onValidateInput);

            if (include != null)
                _add_chars(include);

            if (includeCharCodes != null)
            {
                for (a = 0; a < includeCharCodes.Length; a++ )
                    allowedChars.Add((char)includeCharCodes[a]);
            }

            if (upperCase)
                _add_chars(char_upper_case);
            if (lowerCase)
                _add_chars(char_lower_case);
            if (numerals)
                _add_chars(char_numerals);
            if (latin)
                _add_chars(char_latin);
            if (punctuation_1)
                _add_chars(char_punctuation_1);
            if (punctuation_1)
                _add_chars(char_punctuation_2);
            if (space)
                _add_chars(char_space);
        }

        private void _add_chars(String new_chars)
        {
            char [] chars = new_chars.ToCharArray();
            for (int a = 0; a < chars.Length; a++)
                allowedChars.Add(chars[a]);
        }

        private char onValidateInput(String str, int i, Char c)
        {
            for (int a = 0; a < allowedChars.Count; a++ )
            {
                if (c == allowedChars[a])
                    return c;
            }

            return '\0';
        }
	}
}
