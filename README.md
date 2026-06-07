# RPG Battle Arena

RPG Battle Arena é um jogo de RPG com combate por turnos desenvolvido como uma aplicação console em C#. O jogador cria um personagem, escolhe uma classe e avança por uma sequência fixa de batalhas contra inimigos progressivamente mais difíceis, culminando em uma luta contra um chefe. O jogo conta com um narrador alimentado por inteligência artificial que gera comentários dramáticos e contextuais para cada evento em tempo real, utilizando a API do Google Gemini.

## Gameplay

O jogo segue uma progressão linear de três estágios:

- **Rodada 1 — Goblin:** Um encontro introdutório projetado para familiarizar o jogador com o sistema de combate. Derrotar o Goblin concede um item de loot que aumenta permanentemente o ataque do personagem.
- **Rodada 2 — Monstro do Gelo:** Um encontro intermediário com vida e dano significativamente maiores, exigindo que o jogador aplique a experiência e os upgrades obtidos na primeira rodada.
- **Boss Fight — Anghorn, o Terrível:** O confronto final. A opção de fuga é desativada. Anghorn possui os maiores atributos do jogo e exige que o jogador entre na batalha em plena forma.

Entre as batalhas, o jogador recupera uma parte da vida e pode subir de nível, escolhendo um entre três upgrades de atributos para personalizar sua build.

## Sistema de Combate

A cada turno, o jogador escolhe uma ação: atacar, abrir o inventário ou fugir (indisponível durante o boss fight). Ao atacar, o jogador seleciona uma das habilidades disponíveis. O dano é calculado pelo motor de batalha `MotorDeBatalha`, que pondera o dano base da habilidade e o atributo de ataque do personagem de forma diferente dependendo se o atacante é o jogador ou um inimigo. Após a ação do jogador, o inimigo contra-ataca automaticamente utilizando uma habilidade escolhida aleatoriamente.

O combate termina quando a vida de um dos combatentes chega a zero. Uma barra de vida visual é exibida para ambos os lados no início de cada turno.

## Progressão e Níveis

O sistema de experiência rastreia o XP obtido ao derrotar inimigos. Ao atingir o limite do nível atual, o jogador é apresentado a uma tela de level up com três opções de melhoria:

- Bônus elevado de ataque
- Bônus elevado de vida máxima, com restauração parcial imediata
- Bônus equilibrado dividido entre ataque e vida máxima

Os upgrades são aplicados diretamente nos atributos do personagem e persistem pelo restante da partida.

## Narrador com IA

O narrador é alimentado pelo modelo Gemini 2.0 Flash via API do Google Generative Language. Ele produz narrações curtas e dramáticas em português para os momentos principais: criação do personagem, início e fim de cada batalha, ataques do jogador, descoberta de loot e a tela de vitória ou derrota final. A narração é não bloqueante durante o combate — ela é executada de forma assíncrona para que o tempo de resposta da API não interrompa o fluxo do jogo. Caso a API esteja indisponível ou a chave não esteja configurada, o narrador falha silenciosamente e o jogo continua normalmente.

## Classes

O jogador seleciona uma das três classes no início do jogo — Mago, Paladino ou Guerreiro — cada uma fornecendo um conjunto distinto de habilidades iniciais, além das duas habilidades base compartilhadas por todos os personagens (Soco e Chute).

## Requisitos

- .NET 10 ou superior
- Uma chave válida da API do Google Gemini configurada como variável de ambiente `GEMINI_API_KEY`

## Executando o Projeto

```powershell
$env:GEMINI_API_KEY = "sua-chave-aqui"
dotnet run --project RPGBattleArena
```

A chave da API deve ser definida na mesma sessão do terminal antes de executar o projeto. Sem ela, o jogo funciona normalmente, porém sem a narração por IA.

## Observação

Este projeto está em desenvolvimento e não se encontra em sua versão final. A presença de arquivos incompletos, classes sem implementação ou funcionalidades ainda não utilizadas é esperada e faz parte do processo de construção do jogo.
