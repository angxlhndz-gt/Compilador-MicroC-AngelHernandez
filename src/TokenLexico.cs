namespace MicroC;

public class TokenLexico
{
    // Linea donde aparece el token.
    public int Linea { get; set; }
    // Texto exacto encontrado.
    public string Lexema { get; set; }
    // Numero de token.
    public int Token { get; set; }
    // Tipo de token.
    public string Tipo { get; set; }

    public TokenLexico(int linea, string lexema, int token, string tipo)
    {
        Linea = linea;
        Lexema = lexema;
        Token = token;
        Tipo = tipo;
    }

    public override string ToString()
    {
        // Mostrar saltos de linea como texto visible.
        string lexemaVisible = Lexema
            .Replace("\r", "\\r")
            .Replace("\n", "\\n");

        return $"Linea: {Linea}    Lexema: {lexemaVisible}    Token: {Token}    Tipo: {Tipo}";
    }
}
