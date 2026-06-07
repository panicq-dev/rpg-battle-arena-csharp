namespace RPGBattleArena.Classes;

internal class SistemaXP
{
    public int XPAtual { get; private set; } = 0;
    public int Nivel { get; private set; } = 1;
    public int XPParaProximoNivel => Nivel * 100;

    // BONUS acumulado
    public int BonusAtaque { get; private set; } = 0;
    public int BonusDefesa { get; private set; } = 0;
    public double BonusVida { get; private set; } = 0;

    public bool GanharXP(int quantidade)
    {
        XPAtual += quantidade;
        if (XPAtual >= XPParaProximoNivel)
        {
            XPAtual -= XPParaProximoNivel;
            Nivel++;
            return true; 
        }
        return false;
    }

    public void ExibirStatus()
    {
        Display.Escrever($"\n⭐ NÍVEL: {Nivel} | XP: {XPAtual}/{XPParaProximoNivel}", ConsoleColor.Yellow);
        if (BonusAtaque > 0 || BonusDefesa > 0 || BonusVida > 0)
        {
            Console.WriteLine($"   Bônus acumulados — ATK: +{BonusAtaque} | DEF: +{BonusDefesa} | VIDA: +{BonusVida}");
        }
    }

    public void MenuUpgrade(Protagonista protagonista)
    {
        Console.Clear();
        Display.Escrever("══════════════════════════════", ConsoleColor.Yellow);
        Display.Escrever("   ✨ SUBIU DE NÍVEL! ✨", ConsoleColor.Yellow);
        Display.Escrever($"   Você chegou ao Nível {Nivel}!", ConsoleColor.Yellow);
        Display.Escrever("══════════════════════════════", ConsoleColor.Yellow);
        Console.WriteLine("\nEscolha 1 melhoria para seu personagem:\n");

        var opcoes = GerarOpcoes();

        for (int i = 0; i < opcoes.Count; i++)
        {
            Console.WriteLine($"  [{i + 1}]. {opcoes[i].Descricao}");
        }

        Console.Write("\nSua escolha: ");
        string input = Console.ReadLine() ?? "1";

        if (!int.TryParse(input, out int idx) || idx < 1 || idx > opcoes.Count)
            idx = 1;

        var escolha = opcoes[idx - 1];
        escolha.Aplicar(protagonista, this);

        Display.Escrever($"\n✅ {escolha.Descricao} aplicado!", ConsoleColor.Green);
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    private List<OpcaoUpgrade> GerarOpcoes()
    {
        // 3 op
        return new List<OpcaoUpgrade>
        {
            new OpcaoUpgrade(
                "⚔️  Força Bruta  — +20 de Ataque",
                (p, s) => { s.BonusAtaque += 20; p.AplicarBonusAtaque(20); }
            ),
            new OpcaoUpgrade(
                "🛡️  Resistência — +30 de Vida máxima (restaura também)",
                (p, s) => { s.BonusVida += 30; p.Vida = Math.Min(p.VidaMaxima, p.Vida + 30); p.AplicarBonusVida(30); }
            ),
            new OpcaoUpgrade(
                "🌀  Equilíbrio  — +10 de Ataque e +15 de Vida máxima",
                (p, s) => { s.BonusAtaque += 10; s.BonusVida += 15; p.AplicarBonusAtaque(10); p.AplicarBonusVida(15); }
            ),
        };
    }

    private class OpcaoUpgrade
    {
        public string Descricao { get; }
        private readonly Action<Protagonista, SistemaXP> _aplicar;

        public OpcaoUpgrade(string descricao, Action<Protagonista, SistemaXP> aplicar)
        {
            Descricao = descricao;
            _aplicar = aplicar;
        }

        public void Aplicar(Protagonista p, SistemaXP s) => _aplicar(p, s);
    }
}