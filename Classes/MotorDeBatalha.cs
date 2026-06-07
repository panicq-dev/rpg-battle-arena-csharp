using RPGBattleArena.Classes;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;

internal static class MotorDeBatalha 
{

    public static Personagem OrdemDeAtaque(Protagonista player, Inimigo inimigo)
    {
       if (player.Velocidade > inimigo.Velocidade)
        {
            return player;
        }
        return inimigo;
    }

    public static double Dano(Personagem personagem, string nomeDaHabilidade)
    {
        if (personagem is Protagonista)
        {

            var habilidadeUsada = personagem.Habilidades.FirstOrDefault(h => h.Nome.Equals(nomeDaHabilidade));
            if (habilidadeUsada is not null)
            {
                double danoDaHabilidade = habilidadeUsada.Dano * 0.7;
                double danoTotal = personagem.Ataque * 0.3 + danoDaHabilidade;
                return danoTotal;
            }
            else
            {
                Display.Escrever("[ERRO] ESSA HABILIDADE NÃO EXISTE");
                return 0;
            }
            
        }
        else 
        {
            double danoDeAtaque = personagem.Ataque * 0.5;
            return danoDeAtaque;
        }
        
    }



}
