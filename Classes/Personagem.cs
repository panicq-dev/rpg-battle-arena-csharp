namespace RPGBattleArena.Classes;

internal class Personagem
{
    public Personagem(string nome, double vida = 100, int ataque = 100, int defesa = 100)
    {
        Nome = nome;
        VidaMaxima = vida;
        Vida = vida;
        _ataque = ataque;
        _defesa = defesa;
    }

    public string Nome { get; protected set; }
    public double VidaMaxima { get; protected set; }
    public double Vida { get; set; }
    protected int _ataque;
    protected int _defesa;
    public int Ataque => _ataque;
    public int Defesa => _defesa;
    public double Velocidade { get; protected set; } = 2.0;

    public List<Habilidades> Habilidades { get; } = new()
    {
        new Habilidades("Soco", 10, 5),
        new Habilidades("Chute", 15, 10),
    };

    public void AplicarBonusAtaque(int bonus) => _ataque += bonus;
    public void AplicarBonusVida(double bonus)
    {
        VidaMaxima += bonus;
        Vida = Math.Min(VidaMaxima, Vida + bonus);
    }

    public virtual double Atacar(string habilidadeDoAtacante, Personagem personagemAtacado)
    {
        double danoCausado = MotorDeBatalha.Dano(this, habilidadeDoAtacante);
        personagemAtacado.Vida = Math.Max(0, personagemAtacado.Vida - danoCausado);
        return danoCausado;
    }
}