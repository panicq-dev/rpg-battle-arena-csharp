namespace RPGBattleArena.Classes;

internal class Protagonista : Personagem
{
    public Protagonista(string nome, ClassesProtagonista classe) : base(nome)
    {
        this.Classe = classe.Nome;
        this.Nome = nome;
        if (classe.classes.TryGetValue(classe.Nome, out List<Habilidades> habilidades))
        {
            int quantidadeDeHabilidades = habilidades.Count;
            if (quantidadeDeHabilidades > 0) 
            { 
                foreach (var habilidade in habilidades)
                {
                    this.Habilidades.Add(habilidade);
                }
            }
        }

    }

    public string Classe { get; }
}
