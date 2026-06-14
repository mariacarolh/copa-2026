using System;

static class MataMata {
    // Verifica se a fase de grupos está concluída
    private static bool GruposConcluidos()  {
        // Todo grupo deve ter 6 jogos realizados (4 seleções, cada uma joga 3 vezes)
        // Verificação simplificada: todos os jogos da fase "Grupo" devem estar realizados
        for (int i = 0; i < Dados.totalJogos; i++) {
            Jogo j = Dados.jogos[i];
            if (j.Ativo && j.Fase == "Grupo" && !j.Realizado) { 
                return false;
            }
        }
        // Precisa ter ao menos 1 jogo de grupo cadastrado
        for (int i = 0; i < Dados.totalJogos; i++)
            if (Dados.jogos[i].Ativo && Dados.jogos[i].Fase == "Grupo") { 
                return true;
            }

        return false;
    }

    private static bool ExisteFase(string fase) {
        for (int i = 0; i < Dados.totalJogos; i++)
            if (Dados.jogos[i].Ativo && Dados.jogos[i].Fase == fase)
                return true;
        return false;
    }

    private static int ObterVencedorJogo(int idJogo) {
        int idx = Helpers.BuscarJogo(idJogo);
        if (idx < 0) return -1;

        Jogo j = Dados.jogos[idx];
        if (!j.Realizado) return -1;

        if (j.GolsA > j.GolsB) return j.IdTimeA;
        if (j.GolsB > j.GolsA) return j.IdTimeB;
        // Empate → pênaltis
        return j.IdVencedorPenaltis;
    }

    private static void CriarJogoMata(string fase, int idA, int idB, string data, int idEstadio) {
        if (Dados.totalJogos >= Dados.MAX_JOGOS) {
            Console.WriteLine("AVISO: Limite de jogos atingido!");
            return;
        }

        Jogo j;
        j.Id = Dados.proximoIdJogo++;
        j.Fase = fase;
        j.Grupo = "";
        j.Data = data;
        j.IdEstadio = idEstadio;
        j.IdTimeA = idA;
        j.IdTimeB = idB;
        j.GolsA = 0;
        j.GolsB = 0;
        j.Realizado = false;
        j.IdVencedorPenaltis = 0;
        j.Ativo = true;

        Dados.jogos[Dados.totalJogos] = j;
        Dados.totalJogos++;
    }

    // Gerar fase de 32
    public static void Gerar() {
        Helpers.Titulo("Gerar Mata-Mata");

        if (!GruposConcluidos()) {
            Console.WriteLine("A fase de grupos ainda não foi concluída!");
            Console.WriteLine("Todos os jogos da fase de grupos precisam ter resultado.");
            Helpers.Pausar();
            return;
        }

        if (ExisteFase("32avos")) {
            Console.WriteLine("O mata-mata já foi gerado!");
            Console.WriteLine("Use a opção 'Registrar Resultados Mata-Mata' para avançar as fases.");
            Helpers.Pausar();
            return;
        }

        // Obter 32 classificados (índices no vetor selecoes)
        int[] cl = Classificacao.ObterClassificados32();

        // Pegar primeiro estádio disponível como padrão
        int idEst = Dados.totalEstadios > 0 ? Dados.estadios[0].Id : 1;

        // Criar 16 jogos da fase de 32
        // Chaveamento: 1º A x 3ºmelhor, 1ºB x 3ºmelhor, etc. (simplificado por posição)
        string dataBase = "28/06/2026";
        for (int i = 0; i < 16; i++) {
            int idxA = cl[i * 2];
            int idxB = cl[i * 2 + 1];
            // Usa estádios em rodízio
            int estIdx = i % Dados.totalEstadios;
            int estId  = Dados.totalEstadios > 0 ? Dados.estadios[estIdx].Id : 1;
            CriarJogoMata("32avos",
                Dados.selecoes[idxA].Id,
                Dados.selecoes[idxB].Id,
                dataBase, estId);
        }

        Console.WriteLine("16 jogos da Fase de 32 gerados com sucesso!");
        Console.WriteLine("Registre os resultados e depois avance para as Oitavas.");
        CsvHelper.SalvarTodos();
        Helpers.Pausar();
    }

    // Avançar fase — gera próxima rodada
    private static void AvancarFase(string faseAtual, string proxFase, string dataSugerida) {
        // Verifica se todos os jogos da fase atual foram realizados
        for (int i = 0; i < Dados.totalJogos; i++) {
            Jogo j = Dados.jogos[i];
            if (!j.Ativo || j.Fase != faseAtual) { 
                continue; 
            }
            if (!j.Realizado) {
                Console.WriteLine($"Ainda há jogos de {faseAtual} sem resultado!");
                return;
            }
            if (j.GolsA == j.GolsB && j.IdVencedorPenaltis == 0) {
                Console.WriteLine($"Jogo {j.Id} empatado sem vencedor nos pênaltis!");
                return;
            }
        }

        if (ExisteFase(proxFase)) {
            Console.WriteLine($"Fase {proxFase} já foi gerada.");
            return;
        }

        // Coletar vencedores em ordem
        int[] vencedores = new int[32];
        int totalV = 0;
        for (int i = 0; i < Dados.totalJogos; i++) {
            Jogo j = Dados.jogos[i];
            if (!j.Ativo || j.Fase != faseAtual) { 
                continue; 
            }
            vencedores[totalV++] = ObterVencedorJogo(j.Id);
        }

        // Criar jogos com pares consecutivos
        for (int i = 0; i + 1 < totalV; i += 2) {
            int estIdx = (i / 2) % Dados.totalEstadios;
            int estId  = Dados.totalEstadios > 0 ? Dados.estadios[estIdx].Id : 1;
            CriarJogoMata(proxFase, vencedores[i], vencedores[i+1], dataSugerida, estId);
        }

        Console.WriteLine($"Fase {proxFase} gerada com {totalV/2} jogos!");
        CsvHelper.SalvarTodos();
    }

    // Registrar resultados e avançar fases
    public static void RegistrarResultados() {
        Helpers.Titulo("Registrar Resultados - Mata-Mata");

        // Mostra jogos pendentes do mata-mata
        bool temPendente = false;
        for (int i = 0; i < Dados.totalJogos; i++) {
            Jogo j = Dados.jogos[i];
            if (!j.Ativo || j.Fase == "Grupo" || j.Realizado) { 
                continue; 
            }
            string nomeA  = Helpers.NomeSelecao(j.IdTimeA);
            string nomeB  = Helpers.NomeSelecao(j.IdTimeB);
            Console.WriteLine($"  [{j.Id,3}] {j.Fase,-10} {nomeA,-22} x {nomeB,-22} {j.Data}");
            temPendente = true;
        }

        if (!temPendente) {
            // Verificar qual fase avançar
            if (!ExisteFase("32avos")) {
                Console.WriteLine("Gere o mata-mata primeiro (opção 7).");
            } else if (!ExisteFase("Oitavas")) {
                AvancarFase("32avos", "Oitavas", "04/07/2026");
                Console.WriteLine("Oitavas de final geradas!");
            }  else if (!ExisteFase("Quartas")) {
                AvancarFase("Oitavas", "Quartas", "09/07/2026");
                Console.WriteLine("Quartas de final geradas!");
            } else if (!ExisteFase("Semifinal")) {
                AvancarFase("Quartas", "Semifinal", "14/07/2026");
                Console.WriteLine("Semifinais geradas!");
            } else if (!ExisteFase("3Lugar") && !ExisteFase("Final")) {
                GerarFinalESemiTerceiro();
            } else {
                Console.WriteLine("Todos os jogos do mata-mata já foram realizados!");
                Console.WriteLine("Use a opção 9 para ver o campeão.");
            }
            Helpers.Pausar();
            return;
        }

        // Registrar resultado de um jogo específico
        Console.Write("\nID do jogo para registrar resultado (0 para cancelar): ");
        int id = Helpers.LerInteiro();
        if (id == 0) { 
            return;
        }

        JogoHelper.RegistrarResultado();

        // Após registrar, verifica se deve avançar fase
        VerificarAvancoFase();
    }

    private static void VerificarAvancoFase() {
        string[] fases = { "32avos", "Oitavas", "Quartas", "Semifinal" };
        string[] proximas = { "Oitavas", "Quartas", "Semifinal", "3Lugar" };
        string[] datas    = { "04/07/2026","09/07/2026","14/07/2026","18/07/2026" };

        for (int f = 0; f < fases.Length; f++) {
            if (!ExisteFase(fases[f])) { 
                continue; 
            }
            if (ExisteFase(proximas[f])) {
                continue; 
            }

            // Checa se todos realizados
            bool todosProntos = true;
            bool temJogos = false;
            for (int i = 0; i < Dados.totalJogos; i++) {
                Jogo j = Dados.jogos[i];
                if (!j.Ativo || j.Fase != fases[f]) {
                    continue;
                }

                temJogos = true;
                if (!j.Realizado || (j.GolsA == j.GolsB && j.IdVencedorPenaltis == 0)) {
                    todosProntos = false;
                    break;
                }
            }

            if (temJogos && todosProntos) {
                Console.Write($"\nTodos os jogos de {fases[f]} concluídos! Gerar {proximas[f]}? (S/N): ");
                string r = Console.ReadLine()?.Trim().ToUpper() ?? "N";
                if (r == "S") {
                    if (fases[f] == "Semifinal")
                        GerarFinalESemiTerceiro();
                    else
                        AvancarFase(fases[f], proximas[f], datas[f]);
                }
            }
        }
    }

    private static void GerarFinalESemiTerceiro() {
        // Pega os 2 perdedores da semifinal → disputa 3º lugar
        // Pega os 2 vencedores → final
        int[] perdedores  = new int[2];
        int[] vencedores  = new int[2];
        int pIdx = 0, vIdx = 0;

        for (int i = 0; i < Dados.totalJogos; i++) {
            Jogo j = Dados.jogos[i];
            if (!j.Ativo || j.Fase != "Semifinal" || !j.Realizado) {
                continue; 
            }

            int venc = ObterVencedorJogo(j.Id);
            int perd = (venc == j.IdTimeA) ? j.IdTimeB : j.IdTimeA;

            if (vIdx < 2) { 
                vencedores[vIdx++] = venc; 
            }
            if (pIdx < 2) { 
                perdedores[pIdx++] = perd; 
            }
        }

        int estTerceiro = Dados.totalEstadios > 0 ? Dados.estadios[0].Id : 1;
        int estFinal    = Dados.totalEstadios > 1 ? Dados.estadios[Dados.totalEstadios-1].Id : estTerceiro;

        if (pIdx >= 2)
            CriarJogoMata("3Lugar", perdedores[0], perdedores[1], "18/07/2026", estTerceiro);
        if (vIdx >= 2)
            CriarJogoMata("Final", vencedores[0], vencedores[1], "19/07/2026", estFinal);

        Console.WriteLine("Disputa de 3º lugar e Final geradas!");
        CsvHelper.SalvarTodos();
    }

    public static void MostrarChave() {
        Helpers.Titulo("Chave do Mata-Mata");
        string[] fases = { "32avos","Oitavas","Quartas","Semifinal","3Lugar","Final" };

        foreach (string fase in fases) {
            if (!ExisteFase(fase)) { 
                continue; 
            }

            string titulo = fase == "3Lugar" ? "DISPUTA DE 3º LUGAR" : fase.ToUpper();
            Console.WriteLine($"\n  ── {titulo} ──");
            MenuJogos.ListarPorFase(fase);
        }
    }

    public static void MostrarCampeao() {
        Helpers.Titulo("Campeão da Copa 2026");

        int idFinal = -1;
        for (int i = 0; i < Dados.totalJogos; i++) {
            Jogo j = Dados.jogos[i];
            if (j.Ativo && j.Fase == "Final") {
                idFinal = j.Id;
                break;
            }
        }

        if (idFinal < 0) {
            Console.WriteLine("A final ainda não foi disputada.");
            Helpers.Pausar();
            return;
        }

        int idx = Helpers.BuscarJogo(idFinal);
        Jogo final = Dados.jogos[idx];

        if (!final.Realizado) {
            Console.WriteLine("O resultado da final ainda não foi registrado.");
            Helpers.Pausar();
            return;
        }

        int idCampeao = ObterVencedorJogo(idFinal);
        int idVice    = (idCampeao == final.IdTimeA) ? final.IdTimeB : final.IdTimeA;

        // Busca terceiro colocado
        int idTerceiro = -1;
        for (int i = 0; i < Dados.totalJogos; i++) {
            Jogo j = Dados.jogos[i];
            if (j.Ativo && j.Fase == "3Lugar" && j.Realizado) {
                idTerceiro = ObterVencedorJogo(j.Id);
                break;
            }
        }

        Console.WriteLine();
        Console.WriteLine("  ╔══════════════════════════════════════╗");
        Console.WriteLine($"  ║   CAMPEÃO: {Helpers.NomeSelecao(idCampeao),-24} ║");
        Console.WriteLine($"  ║   2º LUGAR:    {Helpers.NomeSelecao(idVice),-24} ║");
        if (idTerceiro > 0)
        Console.WriteLine($"  ║   3º LUGAR:{Helpers.NomeSelecao(idTerceiro),-23} ║");
        Console.WriteLine("  ╚══════════════════════════════════════╝");
        Console.WriteLine();
        Console.WriteLine($"  Final: {Helpers.NomeSelecao(final.IdTimeA)} {final.GolsA} x {final.GolsB} {Helpers.NomeSelecao(final.IdTimeB)}");

        Helpers.Pausar();
    }
}
