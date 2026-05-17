using System.Collections.Generic;

namespace MicroC;

public class UnidadesLexicas
{
    // Tabla de simbolos validos.
    private readonly Dictionary<string, int> simbolosSimples = new()
    {
        { "(", 75 },
        { ")", 76 },
        { "{", 77 },
        { "}", 78 },
        { "[", 79 },
        { "]", 80 },
        { ",", 91 },
        { ";", 92 },
        { ".", 93 },
        { "+", 100 },
        { "-", 101 },
        { "*", 102 },
        { "/", 103 },
        { "=", 104 },
        { "<", 105 },
        { ">", 106 },
        { "!", 107 },
        { "&", 108 },
        { "|", 109 },
        { "++", 110 },
        { "--", 111 },
        { "==", 112 },
        { "!=", 113 },
        { "<=", 114 },
        { ">=", 115 },
        { "&&", 116 },
        { "||", 117 },
        { "+=", 118 },
        { "-=", 119 },
        { "*=", 120 },
        { "/=", 121 }
    };

    // Tabla de palabras reservadas.
    private readonly Dictionary<string, int> palabrasReservadas = new()
    {
        { "auto", 1 },
        { "break", 2 },
        { "case", 3 },
        { "char", 4 },
        { "const", 5 },
        { "continue", 6 },
        { "default", 7 },
        { "do", 8 },
        { "double", 9 },
        { "else", 10 },
        { "enum", 11 },
        { "extern", 12 },
        { "float", 13 },
        { "for", 14 },
        { "goto", 15 },
        { "if", 16 },
        { "int", 17 },
        { "long", 18 },
        { "register", 19 },
        { "return", 20 },
        { "short", 21 },
        { "signed", 22 },
        { "sizeof", 23 },
        { "static", 24 },
        { "struct", 25 },
        { "switch", 26 },
        { "typedef", 27 },
        { "union", 28 },
        { "unsigned", 29 },
        { "void", 30 },
        { "volatile", 31 },
        { "while", 32 },
        { "include", 40 },
        { "define", 41 },
        { "using", 42 },
        { "namespace", 43 },
        { "class", 44 },
        { "public", 45 },
        { "private", 46 },
        { "protected", 47 },
        { "new", 48 },
        { "delete", 49 },
        { "true", 50 },
        { "false", 51 },
        { "cout", 52 },
        { "cin", 53 },
        { "endl", 54 }
    };

    // Buscar palabra reservada; si no existe, es identificador.
    public int GetTokenPalabra(string lexema)
    {
        if (palabrasReservadas.TryGetValue(lexema, out int token))
        {
            return token;
        }

        return 300;
    }

    // Buscar simbolo; si no existe, es invalido.
    public int GetTokenSimbolo(string lexema)
    {
        if (simbolosSimples.TryGetValue(lexema, out int token))
        {
            return token;
        }

        return -1;
    }
}
