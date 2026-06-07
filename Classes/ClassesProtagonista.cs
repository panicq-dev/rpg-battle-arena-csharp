namespace RPGBattleArena.Classes;
internal class ClassesProtagonista
{
    public ClassesProtagonista(string nome)
    {
        Nome = nome;
    }

    public string Nome {  get; }
    public Dictionary<string, List<Habilidades>> classes { get;  } = new()
    {
        { "Mago",  [new Habilidades("Bola de Fogo", 35, 15), new Habilidades("Flechas Divinidas", 75, 40)]},
        { "Guerreiro",  [new Habilidades("Ataque perfurante", 55, 35), new Habilidades("Cortes rápidos", 35, 10)]},
        { "Paladino",  [new Habilidades("Lança Divina", 40, 15), new Habilidades("Lança Normal Dupla", 35, 10)]}

    };

}
