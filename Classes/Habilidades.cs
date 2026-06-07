namespace RPGBattleArena.Classes;
internal class Habilidades
{
    public Habilidades(string nome, int dano, int manaNecessaria)
    {
        Nome = nome;
        Dano = dano;
        ManaNecessaria = manaNecessaria;
    }

    public string Nome { get; set; }
    public int Dano { get; }
    public int ManaNecessaria { get; }
    

}
