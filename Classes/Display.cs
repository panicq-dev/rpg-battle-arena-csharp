public static class Display
{
    public static void Escrever(string texto, ConsoleColor cor = ConsoleColor.White)
    {
        Console.ForegroundColor = cor;
        Console.WriteLine(texto);
        Console.ResetColor();
    }

}
