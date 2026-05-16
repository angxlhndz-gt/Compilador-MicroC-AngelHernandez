using System.Collections.Generic;

namespace MicroC;

public class UnidadesLexicas
{
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
        { "|", 109 }
    };

    private readonly Dictionary<string, int> palabrasBasicas = new()
    {
        // Preparado para agregar palabras reservadas en fases siguientes.
    };

    public int GetTokenPalabra(string lexema)
    {
        if (palabrasBasicas.TryGetValue(lexema, out int token))
        {
            return token;
        }

        return 300;
    }

    public int GetTokenSimbolo(string lexema)
    {
        if (simbolosSimples.TryGetValue(lexema, out int token))
        {
            return token;
        }

        return -1;
    }
}
