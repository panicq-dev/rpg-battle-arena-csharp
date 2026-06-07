namespace RPGBattleArena.Classes;

internal class Inimigo : Personagem
{
    public Inimigo(string nome = null, double vida = 100, int ataque = 100, int defesa = 100)
        : base(nome ??= RandomizarInimigo(), vida, ataque, defesa)
    {
        Nome = nome;
    }

    public static string[] Inimigos { get; } = { "Goblin", "Monstro do Gelo", "Rato Mutante", "Anghorn, o Terrível" };

    public new string Nome { get; set; }

    public static string RandomizarInimigo()
    {
        int indiceAleatorio = Random.Shared.Next(Inimigos.Length);
        return Inimigos[indiceAleatorio];
    }

    public string RandomizarHabilidade()
    {
        int indiceAleatorioDaHabilidade = Random.Shared.Next(Habilidades.Count);
        return Habilidades[indiceAleatorioDaHabilidade].Nome;
    }

    public double Atacar(Personagem personagemAtacado)
    {
        return this.Atacar(string.Empty, personagemAtacado);
    }
}