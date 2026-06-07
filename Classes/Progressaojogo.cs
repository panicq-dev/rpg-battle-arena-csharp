namespace RPGBattleArena.Classes;

internal static class Progressaojogo
{
    public record EstagioBatalha(
        string NomeInimigo,
        double VidaInimigo,
        int AtaqueInimigo,
        int DefesaInimigo,
        int XPRecompensa,
        bool IsBoss,
        string? ItemLoot = null
    );

    public static readonly List<EstagioBatalha> Estagios = new()
    {
        new EstagioBatalha(
            NomeInimigo:  "Goblin",
            VidaInimigo:  80,
            AtaqueInimigo: 70,
            DefesaInimigo: 60,
            XPRecompensa: 80,
            IsBoss:       false,
            ItemLoot:     "Adaga Enferrujada (+5 ATK)"
        ),
        new EstagioBatalha(
            NomeInimigo:  "Monstro do Gelo",
            VidaInimigo:  110,
            AtaqueInimigo: 75,
            DefesaInimigo: 65,
            XPRecompensa: 120,
            IsBoss:       false,
            ItemLoot:     null
        ),
        new EstagioBatalha(
            NomeInimigo:  "Anghorn, o Terrível",
            VidaInimigo:  220,
            AtaqueInimigo: 90,
            DefesaInimigo: 110,
            XPRecompensa: 250,
            IsBoss:       true,
            ItemLoot:     null
        ),
    };

    public static Inimigo CriarInimigo(EstagioBatalha estagio)
    {
        return new Inimigo(
            nome: estagio.NomeInimigo,
            vida: estagio.VidaInimigo,
            ataque: estagio.AtaqueInimigo,
            defesa: estagio.DefesaInimigo
        );
    }

    public static void ExibirLoot(string item)
    {
        Console.Clear();
        Display.Escrever("╔══════════════════════════════╗", ConsoleColor.Yellow);
        Display.Escrever("║          LOOT ENCONTRADO!    ║", ConsoleColor.Yellow);
        Display.Escrever("╚══════════════════════════════╝", ConsoleColor.Yellow);
        Console.WriteLine($"\n  Você encontrou: {item}");
        Console.WriteLine("\n  O item foi adicionado ao seu equipamento.");
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public static void ExibirAnuncioInimigo(EstagioBatalha estagio, int rodada, int totalRodadas)
    {
        Console.Clear();
        string cor = estagio.IsBoss ? "🔴" : "⚔️";
        string tipo = estagio.IsBoss ? " ★ BOSS FIGHT ★" : $" Rodada {rodada}/{totalRodadas}";

        Display.Escrever($"\n{cor}{tipo}", estagio.IsBoss ? ConsoleColor.Red : ConsoleColor.Cyan);
        Display.Escrever($"\n  Um {estagio.NomeInimigo} surge das sombras!", ConsoleColor.DarkYellow);
        Console.WriteLine($"\n  VIDA: {estagio.VidaInimigo} | ATAQUE: {estagio.AtaqueInimigo} | DEFESA: {estagio.DefesaInimigo}");

        if (estagio.IsBoss)
        {
            Display.Escrever("\n  ⚠️  Esta é a batalha final. Não há como fugir!", ConsoleColor.Red);
        }

        Console.WriteLine("\nPressione qualquer tecla para iniciar o combate...");
        Console.ReadKey();
    }
}