using RPGBattleArena.Classes;

internal interface IObservadorDaBatalha
{
    void Mensagem(string mensagem);
    void MensagemFinalBatalha(string nomeDoVencedor);
    void VidaMudou(Personagem personagem);
}
