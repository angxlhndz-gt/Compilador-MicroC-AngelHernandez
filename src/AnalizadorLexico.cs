using System.Collections.Generic;

namespace MicroC;

public class AnalizadorLexico
{
    public List<TokenLexico> Lista { get; private set; } = new();
    public int cont = 0;
    public int linea = 1;

    private readonly UnidadesLexicas unidadesLexicas = new();

    public int GetAlfabetoAlfanumerico(char c)
    {
        if (char.IsLetterOrDigit(c) || c == '_')
        {
            return 1;
        }

        return -1;
    }

    public int GetAlfabetoNumero(char c)
    {
        if (char.IsDigit(c))
        {
            return 1;
        }

        return -1;
    }

    public int GetAlfabetoSimbolo(char c)
    {
        return unidadesLexicas.GetTokenSimbolo(c.ToString());
    }

    public void IdentificadorPalabraReservada(string archivo)
    {
        int inicio = cont;

        while (cont < archivo.Length && GetAlfabetoAlfanumerico(archivo[cont]) == 1)
        {
            cont++;
        }

        string lexema = archivo.Substring(inicio, cont - inicio);
        int token = unidadesLexicas.GetTokenPalabra(lexema);
        string tipo = token == 300 ? "Identificador" : "PalabraReservada";

        Lista.Add(new TokenLexico(linea, lexema, token, tipo));
    }

    public void EnteroReal(string archivo)
    {
        int inicio = cont;

        while (cont < archivo.Length && GetAlfabetoNumero(archivo[cont]) == 1)
        {
            cont++;
        }

        if (cont + 1 < archivo.Length && archivo[cont] == '.' && char.IsDigit(archivo[cont + 1]))
        {
            cont++;

            while (cont < archivo.Length && GetAlfabetoNumero(archivo[cont]) == 1)
            {
                cont++;
            }
        }

        string lexema = archivo.Substring(inicio, cont - inicio);
        Lista.Add(new TokenLexico(linea, lexema, 301, "Numero"));
    }

    public void AutomataComentario(string archivo)
    {
        if (cont + 1 >= archivo.Length || archivo[cont] != '/')
        {
            return;
        }

        char siguiente = archivo[cont + 1];

        if (siguiente == '/')
        {
            cont += 2;

            while (cont < archivo.Length && archivo[cont] != '\n')
            {
                cont++;
            }

            return;
        }

        if (siguiente == '*')
        {
            int lineaInicio = linea;
            cont += 2;
            bool comentarioCerrado = false;

            while (cont < archivo.Length)
            {
                if (archivo[cont] == '\n')
                {
                    linea++;
                }

                if (cont + 1 < archivo.Length && archivo[cont] == '*' && archivo[cont + 1] == '/')
                {
                    cont += 2;
                    comentarioCerrado = true;
                    break;
                }

                cont++;
            }

            if (!comentarioCerrado)
            {
                Lista.Add(new TokenLexico(lineaInicio, "Comentario sin cerrar", -1, "ErrorLexico"));
            }
        }
    }

    public List<TokenLexico> AnalisisLexico(string archivo)
    {
        Lista = new List<TokenLexico>();
        cont = 0;
        linea = 1;

        while (cont < archivo.Length)
        {
            char c = archivo[cont];

            if (c == '\n')
            {
                linea++;
                cont++;
                continue;
            }

            if (c == ' ' || c == '\t' || c == '\r' || char.IsWhiteSpace(c))
            {
                cont++;
                continue;
            }

            if (char.IsLetter(c) || c == '_')
            {
                IdentificadorPalabraReservada(archivo);
                continue;
            }

            if (char.IsDigit(c))
            {
                EnteroReal(archivo);
                continue;
            }

            if (c == '/' && cont + 1 < archivo.Length && (archivo[cont + 1] == '/' || archivo[cont + 1] == '*'))
            {
                AutomataComentario(archivo);
                continue;
            }

            int tokenSimbolo = GetAlfabetoSimbolo(c);

            if (tokenSimbolo != -1)
            {
                Lista.Add(new TokenLexico(linea, c.ToString(), tokenSimbolo, "Simbolo"));
            }
            else
            {
                Lista.Add(new TokenLexico(linea, c.ToString(), -1, "ErrorLexico"));
            }

            cont++;
        }

        return Lista;
    }
}
