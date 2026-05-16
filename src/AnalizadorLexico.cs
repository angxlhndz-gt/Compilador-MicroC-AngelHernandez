using System.Collections.Generic;

namespace MicroC;

public class AnalizadorLexico
{
    public List<TokenLexico> Lista { get; private set; } = new();
    public int cont = 0;
    public int linea = 1;

    private readonly UnidadesLexicas unidadesLexicas = new();

    private static bool EsInicioIdentificador(char c)
    {
        return char.IsLetter(c) || c == '_';
    }

    private static bool EsParteNumeroMalFormado(char c)
    {
        return char.IsLetterOrDigit(c) || c == '_' || c == '.';
    }

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

        cont++;

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
        bool esReal = false;
        bool esError = false;

        while (cont < archivo.Length && GetAlfabetoNumero(archivo[cont]) == 1)
        {
            cont++;
        }

        if (cont < archivo.Length && archivo[cont] == '.')
        {
            if (cont + 1 < archivo.Length && char.IsDigit(archivo[cont + 1]))
            {
                esReal = true;
                cont++;

                while (cont < archivo.Length && GetAlfabetoNumero(archivo[cont]) == 1)
                {
                    cont++;
                }

                if (cont < archivo.Length && (archivo[cont] == '.' || EsInicioIdentificador(archivo[cont])))
                {
                    esError = true;

                    while (cont < archivo.Length && EsParteNumeroMalFormado(archivo[cont]))
                    {
                        cont++;
                    }
                }
            }
            else
            {
                cont++;
                esError = true;

                while (cont < archivo.Length && EsParteNumeroMalFormado(archivo[cont]))
                {
                    cont++;
                }
            }
        }
        else if (cont < archivo.Length && EsInicioIdentificador(archivo[cont]))
        {
            esError = true;

            while (cont < archivo.Length && EsParteNumeroMalFormado(archivo[cont]))
            {
                cont++;
            }
        }

        string lexema = archivo.Substring(inicio, cont - inicio);

        if (esError)
        {
            Lista.Add(new TokenLexico(linea, lexema, -1, "ErrorLexico"));
            return;
        }

        if (esReal)
        {
            Lista.Add(new TokenLexico(linea, lexema, 302, "NumeroReal"));
            return;
        }

        Lista.Add(new TokenLexico(linea, lexema, 301, "NumeroEntero"));
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
            int inicio = cont;
            int lineaInicio = linea;
            cont += 2;

            while (cont < archivo.Length && archivo[cont] != '\n' && archivo[cont] != '\r')
            {
                cont++;
            }

            string lexema = archivo.Substring(inicio, cont - inicio);
            Lista.Add(new TokenLexico(lineaInicio, lexema, 400, "ComentarioLinea"));
            return;
        }

        if (siguiente == '*')
        {
            int inicio = cont;
            int lineaInicio = linea;
            cont += 2;
            bool comentarioCerrado = false;

            while (cont < archivo.Length)
            {
                if (cont + 1 < archivo.Length && archivo[cont] == '*' && archivo[cont + 1] == '/')
                {
                    cont += 2;
                    comentarioCerrado = true;
                    break;
                }

                if (archivo[cont] == '\n')
                {
                    linea++;
                }

                cont++;
            }

            string lexema = archivo.Substring(inicio, cont - inicio);

            if (!comentarioCerrado)
            {
                Lista.Add(new TokenLexico(lineaInicio, lexema, -1, "ErrorLexico"));
                return;
            }

            Lista.Add(new TokenLexico(lineaInicio, lexema, 401, "ComentarioBloque"));
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

            if (EsInicioIdentificador(c))
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

            if (cont + 1 < archivo.Length)
            {
                string simboloCompuesto = archivo.Substring(cont, 2);
                int tokenCompuesto = unidadesLexicas.GetTokenSimbolo(simboloCompuesto);

                if (tokenCompuesto != -1)
                {
                    Lista.Add(new TokenLexico(linea, simboloCompuesto, tokenCompuesto, "Simbolo"));
                    cont += 2;
                    continue;
                }
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
