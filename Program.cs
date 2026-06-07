using RPGBattleArena.Classes;

ExibirMenuPrincipal();

void ExibirMenuPrincipal()
{
    bool executando = true;
    while (executando)
    {
        Console.Clear();
        string titulo = " ARENA DE BATALHA RPG ";
        string moldura = new string('=', titulo.Length);

        Display.Escrever(moldura, ConsoleColor.Cyan);
        Display.Escrever(titulo, ConsoleColor.Cyan);
        Display.Escrever(moldura, ConsoleColor.Cyan);

        Console.WriteLine("\n[1]. Novo Jogo");
        Console.WriteLine("[2]. Continuar");
        Console.WriteLine("[3]. Estatísticas");
        Console.WriteLine("[4]. Créditos");
        Console.WriteLine("[0]. Sair");

        Console.Write("\nEscolha uma opção: ");
        string opcaoSelecionada = Console.ReadLine();

        switch (opcaoSelecionada)
        {
            case "1":
                IniciarNovoJogo();
                break;
            case "2":
                break;
            case "3":
                break;
            case "4":
                break;
            case "0":
                Interface.Limpar();
                Interface.ExibirMensagemTemporizada("Finalizando...", 1000);
                Interface.Limpar();
                Display.Escrever("Jogo finalizado. Até a próxima!", ConsoleColor.Yellow);
                executando = false;
                break;
            default:
                Display.Escrever("\nOpção inválida! Pressione qualquer tecla para tentar novamente.", ConsoleColor.Red);
                Console.ReadKey();
                break;
        }
    }
}

void IniciarNovoJogo()
{
    Interface.Limpar();
    string nome = Interface.SolicitarDados("Para começar, escolha o nome do seu personagem:");

    Interface.ExibirMensagemTemporizada("Criando personagem...", 800);
    Interface.ExibirMensagemTemporizada("Personagem criado!", 800);

    string classe = Interface.SolicitarDados("\nAgora, escolha a sua classe:\n\t - Mago\n\t - Paladino\n\t - Guerreiro");

    Interface.Limpar();
    Interface.ExibirMensagemTemporizada($"Criando personagem do tipo {classe}...", 800);
    Interface.Limpar();

    var protagonista = new Protagonista(nome, new ClassesProtagonista(classe));
    var xp = new SistemaXP();

    Display.Escrever("Carregando narrador...", ConsoleColor.DarkGray);
    string narracao = NarradorIA.Narrar(
        $"Um herói chamado {nome}, da classe {classe}, entra na Arena de Batalha RPG pela primeira vez. Ele está prestes a enfrentar perigos desconhecidos.",
        "Introdução do jogo"
    );
    NarradorIA.ExibirNarracao(narracao);

    ExibirResumo(protagonista);
    ExecutarProgressao(protagonista, xp);
}

void ExibirResumo(Protagonista p)
{
    Display.Escrever($"\nSeja bem-vindo, {p.Nome}! Seu personagem foi criado com sucesso.", ConsoleColor.Green);
    Console.WriteLine("\nSuas Habilidades Iniciais:");
    p.Habilidades.ForEach(h =>
        Console.WriteLine($"\t - {h.Nome.PadRight(10)} | Dano: {h.Dano} | Mana: {h.ManaNecessaria}"));

    Display.Escrever("\nPressione qualquer tecla para entrar na Arena...", ConsoleColor.DarkYellow);
    Console.ReadKey();
}


void ExecutarProgressao(Protagonista protagonista, SistemaXP xp)
{
    var estagios = Progressaojogo.Estagios;
    int rodadasNormais = estagios.Count(e => !e.IsBoss);

    for (int i = 0; i < estagios.Count; i++)
    {
        var estagio = estagios[i];
        int rodadaAtual = i + 1;

        Progressaojogo.ExibirAnuncioInimigo(estagio, rodadaAtual, estagios.Count);

        // Narrar início da batalha
        string narrInicio = NarradorIA.Narrar(
            $"{protagonista.Nome} (nível {xp.Nivel}, classe {protagonista.Classe}) enfrenta {estagio.NomeInimigo}.",
            estagio.IsBoss ? "Boss fight final" : $"Rodada {rodadaAtual}"
        );
        NarradorIA.ExibirNarracao(narrInicio);

        var inimigo = Progressaojogo.CriarInimigo(estagio);
        bool venceu = MenuCombate(protagonista, inimigo, xp, estagio.IsBoss);

        if (!venceu)
        {
            // Derrota — narrar e acabar
            string narrDerrota = NarradorIA.Narrar(
                $"{protagonista.Nome} foi derrotado por {estagio.NomeInimigo}. Fim da jornada.",
                "Derrota"
            );
            NarradorIA.ExibirNarracao(narrDerrota);
            Display.Escrever("\n☠️  GAME OVER — Sua jornada termina aqui.", ConsoleColor.DarkRed);
            Console.ReadKey();
            return;
        }

        // Vitória: ganhar XP
        bool subiuDeNivel = xp.GanharXP(estagio.XPRecompensa);
        Display.Escrever($"\n+{estagio.XPRecompensa} XP ganhos!", ConsoleColor.Yellow);
        xp.ExibirStatus();
        Console.ReadKey();

        // Loot (apenas se o inimigo tem item)
        if (!string.IsNullOrEmpty(estagio.ItemLoot))
        {
            string narrLoot = NarradorIA.Narrar(
                $"{protagonista.Nome} encontrou um item: {estagio.ItemLoot} após derrotar {estagio.NomeInimigo}.",
                "Loot"
            );
            NarradorIA.ExibirNarracao(narrLoot);
            Progressaojogo.ExibirLoot(estagio.ItemLoot);
            protagonista.AplicarBonusAtaque(5); 
        }

        // Level up
        if (subiuDeNivel)
        {
            xp.MenuUpgrade(protagonista);
        }
        if (!estagio.IsBoss && i < estagios.Count - 1)
        {
            double cura = protagonista.VidaMaxima * 0.4;
            protagonista.Vida = Math.Min(protagonista.VidaMaxima, protagonista.Vida + cura);
            Display.Escrever($"\n💊 Você descansou e recuperou {cura:F0} de vida.", ConsoleColor.Green);
            Console.ReadKey();
        }

        if (estagio.IsBoss)
        {
            ExibirFimDeJogo(protagonista, xp);
            return;
        }
    }
}

void ExibirFimDeJogo(Protagonista protagonista, SistemaXP xp)
{
    Console.Clear();
    string narrFinal = NarradorIA.Narrar(
        $"{protagonista.Nome}, o lendário {protagonista.Classe} de nível {xp.Nivel}, derrotou Anghorn, o Terrível, e se tornou o campeão da Arena.",
        "Vitória final épica"
    );

    Display.Escrever("╔══════════════════════════════════════╗", ConsoleColor.Yellow);
    Display.Escrever("║     🏆 VOCÊ VENCEU A ARENA! 🏆      ║", ConsoleColor.Yellow);
    Display.Escrever("╚══════════════════════════════════════╝", ConsoleColor.Yellow);
    NarradorIA.ExibirNarracao(narrFinal);
    xp.ExibirStatus();
    Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
    Console.ReadKey();
}

bool MenuCombate(Protagonista p, Inimigo inimigo, SistemaXP xp, bool isBoss)
{
    bool emCombate = true;
    bool venceu = false;

    while (emCombate)
    {
        Console.Clear();

        string painelTurno = $"--- CAMPOS DE BATALHA: {p.Nome} vs {inimigo.Nome} ---";
        Display.Escrever(new string('=', painelTurno.Length), ConsoleColor.Red);
        Display.Escrever(painelTurno, ConsoleColor.Red);
        Display.Escrever(new string('=', painelTurno.Length), ConsoleColor.Red);

        // Barras de vida 
        Console.Write("\n[VOCÊ]    ");
        ExibirBarraDeVida(p.Vida, p.VidaMaxima, ConsoleColor.Green);
        Display.Escrever($"  {p.Nome} ({p.Classe}) | VIDA: {p.Vida:F0}/{p.VidaMaxima:F0}", ConsoleColor.Green);

        Console.Write("[INIMIGO] ");
        ExibirBarraDeVida(inimigo.Vida, inimigo.VidaMaxima, ConsoleColor.DarkRed);
        Display.Escrever($"  {inimigo.Nome} | VIDA: {inimigo.Vida:F0}/{inimigo.VidaMaxima:F0}\n", ConsoleColor.DarkRed);

        xp.ExibirStatus();

        Console.WriteLine("\n[1]. Atacar");
        Console.WriteLine("[2]. Abrir Inventário");
        if (!isBoss) Console.WriteLine("[3]. Fugir");

        Console.Write("\nEscolha sua ação: ");
        string acao = Console.ReadLine();

        switch (acao)
        {
            case "1":
                ProcessarTurnoAtaque(p, inimigo);
                break;
            case "2":
                Interface.Limpar();
                Display.Escrever("--- INVENTÁRIO ---", ConsoleColor.Blue);
                Console.WriteLine("O inventário está vazio.");
                Console.WriteLine("\n[0]. Voltar");
                Console.ReadKey();
                break;
            case "3" when !isBoss:
                Interface.Limpar();
                string narrFuga = NarradorIA.Narrar($"{p.Nome} recuou covardemente da batalha contra {inimigo.Nome}.");
                NarradorIA.ExibirNarracao(narrFuga);
                Display.Escrever("Você fugiu do combate...", ConsoleColor.Yellow);
                emCombate = false;
                venceu = false; 
                Console.ReadKey();
                break;
            default:
                Display.Escrever("\nOpção inválida! Você se atrapalhou e perdeu o turno.", ConsoleColor.Magenta);
                EsperarInimigoAtacar(p, inimigo);
                break;
        }

        // Verificação de fim de combate
        if (inimigo.Vida <= 0)
        {
            string narrVitoria = NarradorIA.Narrar(
                $"{p.Nome} derrotou {inimigo.Nome} em batalha épica.",
                isBoss ? "Boss derrotado" : "Inimigo derrotado"
            );
            NarradorIA.ExibirNarracao(narrVitoria);
            Display.Escrever($"\n🏆 VITÓRIA! Você derrotou {inimigo.Nome}!", ConsoleColor.Green);
            emCombate = false;
            venceu = true;
            Console.ReadKey();
        }
        else if (p.Vida <= 0)
        {
            string narrDerrota = NarradorIA.Narrar($"{p.Nome} caiu diante de {inimigo.Nome}.", "Derrota em combate");
            NarradorIA.ExibirNarracao(narrDerrota);
            Display.Escrever("\n☠️  GAME OVER... Você foi derrotado.", ConsoleColor.DarkRed);
            emCombate = false;
            venceu = false;
            Console.ReadKey();
        }
    }

    return venceu;
}

void ProcessarTurnoAtaque(Protagonista p, Inimigo inimigo)
{
    Interface.Limpar();
    Display.Escrever("--- SUAS HABILIDADES ---", ConsoleColor.Green);

    for (int i = 0; i < p.Habilidades.Count; i++)
    {
        Console.WriteLine($"\t[{i + 1}]. {p.Habilidades[i].Nome.PadRight(10)} | DANO: {p.Habilidades[i].Dano}");
    }

    Console.Write("\nEscolha sua habilidade (ou qualquer outra tecla para voltar): ");
    string opcaoMenuAtaque = Console.ReadLine();

    if (int.TryParse(opcaoMenuAtaque, out int idx) && idx > 0 && idx <= p.Habilidades.Count)
    {
        Interface.Limpar();

        if (p.Vida > 0)
        {
            string habilidadeSelecionada = p.Habilidades[idx - 1].Nome;
            double danoCausado = p.Atacar(habilidadeSelecionada, inimigo);

            // Narrar ataque do jogador
            Task.Run(async () =>
            {
                string narr = await NarradorIA.NarrarAsync(
                    $"{p.Nome} usa {habilidadeSelecionada} e causa {danoCausado:F0} de dano em {inimigo.Nome}.",
                    $"{inimigo.Nome} com {inimigo.Vida:F0} de vida restante"
                );
                if (!string.IsNullOrWhiteSpace(narr))
                    NarradorIA.ExibirNarracao(narr);
            });

            Display.Escrever($"» Você usou {habilidadeSelecionada}!", ConsoleColor.Green);
            Display.Escrever($"» {inimigo.Nome} sofreu {danoCausado:F0} de dano.", ConsoleColor.DarkRed);
            Console.WriteLine($"» Vida restante do Inimigo: {inimigo.Vida:F0}");
        }
        else
        {
            Display.Escrever("» Você morreu!", ConsoleColor.Red);
        }

        if (inimigo.Vida > 0)
        {
            EsperarInimigoAtacar(p, inimigo);
        }
        else
        {
            Display.Escrever($"» {inimigo.Nome} morreu!", ConsoleColor.Red);
        }
    }
}

void EsperarInimigoAtacar(Protagonista p, Inimigo inimigo)
{
    Console.WriteLine("\n------------------------------------------------");
    Display.Escrever($"O turno agora é do {inimigo.Nome}...", ConsoleColor.Yellow);
    Console.ReadKey();

    double danoSofrido = inimigo.Atacar(p);

    Display.Escrever($"\n» {inimigo.Nome} contra-atacou!", ConsoleColor.DarkRed);
    Display.Escrever($"» Você sofreu {danoSofrido:F0} de dano.", ConsoleColor.Red);
    Console.WriteLine($"» Sua vida restante: {p.Vida:F0}");

    Console.WriteLine("\nPressione qualquer tecla para continuar...");
    Console.ReadKey();
}

// UTIL

void ExibirBarraDeVida(double vidaAtual, double vidaMaxima, ConsoleColor cor)
{
    int total = 20;
    int preenchido = (int)Math.Round((vidaAtual / vidaMaxima) * total);
    preenchido = Math.Max(0, Math.Min(total, preenchido));

    Console.Write("[");
    Console.ForegroundColor = cor;
    Console.Write(new string('█', preenchido));
    Console.ResetColor();
    Console.Write(new string('░', total - preenchido));
    Console.Write("] ");
}