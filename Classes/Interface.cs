namespace RPGBattleArena.Classes;

internal class Interface
{
    public static void Limpar() => Console.Clear();

    public static string SolicitarDados(string mensagem)
    {
        Console.WriteLine(mensagem);
        return Console.ReadLine();
    }

    public static void ExibirMensagemTemporizada(string mensagem, int delay)
    {
        Console.WriteLine(mensagem);
        Thread.Sleep(delay);
    }
}
