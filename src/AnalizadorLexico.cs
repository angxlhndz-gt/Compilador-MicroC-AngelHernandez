using System.Collections.Generic;

namespace MicroC;

public class AnalizadorLexico
{
    // Lista final de tokens encontrados.
    public List<TokenLexico> Lista { get; private set; } = new();
    // Posicion actual dentro del archivo.
    public int cont = 0;
    // Contador de linea actual.
    public int linea = 1;

    // Tabla de palabras reservadas y simbolos.
    private readonly UnidadesLexicas unidadesLexicas = new();

    // Letra o guion bajo: inicio de identificador o palabra reservada.
    private static bool EsInicioIdentificador(char c)
    {
        return char.IsLetter(c) || c == '_';
    }

    // Parte de numero mal formado: posible error lexico.
    private static bool EsParteNumeroMalFormado(char c)
    {
        return char.IsLetterOrDigit(c) || c == '_' || c == '.';
    }

    // Caracter valido para seguir formando identificador.
    public int GetAlfabetoAlfanumerico(char c)
    {
        if (char.IsLetterOrDigit(c) || c == '_')
        {
            return 1;
        }

        return -1;
    }

    // Caracter valido para formar numero.
    public int GetAlfabetoNumero(char c)
    {
        if (char.IsDigit(c))
        {
            return 1;
        }

        return -1;
    }

    // Busca si el caracter pertenece a la tabla de simbolos.
    public int GetAlfabetoSimbolo(char c)
    {
        return unidadesLexicas.GetTokenSimbolo(c.ToString());
    }

    // Automata de palabras reservadas e identificadores.
    public void IdentificadorPalabraReservada(string archivo)
    {
        // Inicio del lexema.
        int inicio = cont;

        cont++;

        // Obtener lexema alfanumerico.
        while (cont < archivo.Length && GetAlfabetoAlfanumerico(archivo[cont]) == 1)
        {
            cont++;
        }

        string lexema = archivo.Substring(inicio, cont - inicio);
        // Buscar en tabla de palabras reservadas.
        int token = unidadesLexicas.GetTokenPalabra(lexema);
        // Si no esta en tabla, queda como identificador.
        string tipo = token == 300 ? "Identificador" : "PalabraReservada";

        // Agregar token a la lista.
        Lista.Add(new TokenLexico(linea, lexema, token, tipo));
    }

    // Automata de numeros enteros y reales.
    public void EnteroReal(string archivo)
    {
        // Inicio del lexema numerico.
        int inicio = cont;
        bool esReal = false;
        bool esError = false;

        // Consumir digitos del entero.
        while (cont < archivo.Length && GetAlfabetoNumero(archivo[cont]) == 1)
        {
            cont++;
        }

        // Punto decimal: posible numero real.
        if (cont < archivo.Length && archivo[cont] == '.')
        {
            // Hay digito despues del punto: real valido por ahora.
            if (cont + 1 < archivo.Length && char.IsDigit(archivo[cont + 1]))
            {
                esReal = true;
                cont++;

                while (cont < archivo.Length && GetAlfabetoNumero(archivo[cont]) == 1)
                {
                    cont++;
                }

                // Letras o segundo punto despues del real: error lexico.
                if (cont < archivo.Length && (archivo[cont] == '.' || EsInicioIdentificador(archivo[cont])))
                {
                    esError = true;

                    // Consumir numero mal formado.
                    while (cont < archivo.Length && EsParteNumeroMalFormado(archivo[cont]))
                    {
                        cont++;
                    }
                }
            }
            else
            {
                // Punto sin digito despues: error lexico.
                cont++;
                esError = true;

                // Consumir numero mal formado.
                while (cont < archivo.Length && EsParteNumeroMalFormado(archivo[cont]))
                {
                    cont++;
                }
            }
        }
        // Numero seguido de letra: error lexico.
        else if (cont < archivo.Length && EsInicioIdentificador(archivo[cont]))
        {
            esError = true;

            // Consumir numero mal formado.
            while (cont < archivo.Length && EsParteNumeroMalFormado(archivo[cont]))
            {
                cont++;
            }
        }

        string lexema = archivo.Substring(inicio, cont - inicio);

        if (esError)
        {
            // Error lexico numerico.
            Lista.Add(new TokenLexico(linea, lexema, -1, "ErrorLexico"));
            return;
        }

        if (esReal)
        {
            // Numero real: token 302.
            Lista.Add(new TokenLexico(linea, lexema, 302, "NumeroReal"));
            return;
        }

        // Numero entero: token 301.
        Lista.Add(new TokenLexico(linea, lexema, 301, "NumeroEntero"));
    }

    // Automata de comentarios.
    public void AutomataComentario(string archivo)
    {
        // No es comentario.
        if (cont + 1 >= archivo.Length || archivo[cont] != '/')
        {
            return;
        }

        // Revisar si es // o /*.
        char siguiente = archivo[cont + 1];

        if (siguiente == '/')
        {
            // Comentario de linea.
            int inicio = cont;
            int lineaInicio = linea;
            cont += 2;

            // Consumir hasta salto de linea.
            while (cont < archivo.Length && archivo[cont] != '\n' && archivo[cont] != '\r')
            {
                cont++;
            }

            string lexema = archivo.Substring(inicio, cont - inicio);
            // Token de comentario de linea.
            Lista.Add(new TokenLexico(lineaInicio, lexema, 400, "ComentarioLinea"));
            return;
        }

        if (siguiente == '*')
        {
            // Comentario de bloque.
            int inicio = cont;
            int lineaInicio = linea;
            cont += 2;
            bool comentarioCerrado = false;

            while (cont < archivo.Length)
            {
                // Cierre de comentario de bloque.
                if (cont + 1 < archivo.Length && archivo[cont] == '*' && archivo[cont + 1] == '/')
                {
                    cont += 2;
                    comentarioCerrado = true;
                    break;
                }

                // Salto de linea dentro del comentario.
                if (archivo[cont] == '\n')
                {
                    linea++;
                }

                cont++;
            }

            string lexema = archivo.Substring(inicio, cont - inicio);

            if (!comentarioCerrado)
            {
                // Comentario sin cerrar: error lexico.
                Lista.Add(new TokenLexico(lineaInicio, lexema, -1, "ErrorLexico"));
                return;
            }

            // Token de comentario de bloque.
            Lista.Add(new TokenLexico(lineaInicio, lexema, 401, "ComentarioBloque"));
        }
    }

    // Inicio del analisis lexico.
    public List<TokenLexico> AnalisisLexico(string archivo)
    {
        // Inicializar variables de control.
        Lista = new List<TokenLexico>();
        cont = 0;
        linea = 1;

        // Mientras no sea fin de archivo.
        while (cont < archivo.Length)
        {
            // Leer siguiente caracter.
            char c = archivo[cont];

            if (c == '\n')
            {
                // Salto de linea: incrementar linea.
                linea++;
                cont++;
                continue;
            }

            if (c == ' ' || c == '\t' || c == '\r' || char.IsWhiteSpace(c))
            {
                // Espacio/tab/retorno: ignorar caracter.
                cont++;
                continue;
            }

            if (EsInicioIdentificador(c))
            {
                // Letra o guion bajo: identificador o palabra reservada.
                IdentificadorPalabraReservada(archivo);
                continue;
            }

            if (char.IsDigit(c))
            {
                // Digito: entero o real.
                EnteroReal(archivo);
                continue;
            }

            if (c == '/' && cont + 1 < archivo.Length && (archivo[cont + 1] == '/' || archivo[cont + 1] == '*'))
            {
                // Diagonal con / o *: comentario.
                AutomataComentario(archivo);
                continue;
            }

            if (cont + 1 < archivo.Length)
            {
                // Probar simbolo compuesto de dos caracteres.
                string simboloCompuesto = archivo.Substring(cont, 2);
                int tokenCompuesto = unidadesLexicas.GetTokenSimbolo(simboloCompuesto);

                if (tokenCompuesto != -1)
                {
                    // Simbolo compuesto valido.
                    Lista.Add(new TokenLexico(linea, simboloCompuesto, tokenCompuesto, "Simbolo"));
                    cont += 2;
                    continue;
                }
            }

            // Buscar simbolo simple.
            int tokenSimbolo = GetAlfabetoSimbolo(c);

            if (tokenSimbolo != -1)
            {
                // Simbolo simple valido.
                Lista.Add(new TokenLexico(linea, c.ToString(), tokenSimbolo, "Simbolo"));
            }
            else
            {
                // Simbolo invalido: error lexico.
                Lista.Add(new TokenLexico(linea, c.ToString(), -1, "ErrorLexico"));
            }

            // Avanzar al siguiente caracter.
            cont++;
        }

        // Fin: regresar lista de tokens.
        return Lista;
    }
}
