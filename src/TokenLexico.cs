namespace MicroC;

public class TokenLexico
{
    public int Linea { get; set; }
    public string Lexema { get; set; }
    public int Token { get; set; }
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
        string lexemaVisible = Lexema
            .Replace("\r", "\\r")
            .Replace("\n", "\\n");

        return $"Linea: {Linea}    Lexema: {lexemaVisible}    Token: {Token}    Tipo: {Tipo}";
    }
}
