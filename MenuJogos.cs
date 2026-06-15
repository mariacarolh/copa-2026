using System;

static class MenuJogos {
    public static void Exibir() {
        int op;
        do {
            Helpers.Titulo("Gerenciar Jogos");
            Console.WriteLine("  1 - Cadastrar jogo");
            Console.WriteLine("  2 - Listar jogos");
            Console.WriteLine("  3 - Alterar jogo");
            Console.WriteLine("  4 - Excluir jogo");
            Console.WriteLine("  0 - Voltar");
            Console.Write("\nOpção: ");
            op = Helpers.LerInteiro();

            switch (op) {
                case 1: 
                    Cadastrar();
                    break;
                case 2: 
                    Listar();   
                    break;
                case 3:
                    Alterar();  
                    break;
                case 4:
                    Excluir(); 
                    break;
                case 0:
                    break;
                default:
                    Console.WriteLine("Opção inválida!");
                    Helpers.Pausar();
                    break;
            }
        } while (op != 0);
    }

    public static void Cadastrar() {
        Helpers.Titulo("Cadastrar Jogo");

        if (Dados.totalJogos >= Dados.MAX_JOGOS) {
            Console.WriteLine("Limite de jogos atingido!");
            Helpers.Pausar();
            return;
        }

        // Fase
        Console.WriteLine("Fases disponíveis: Grupo, 32avos, Oitavas, Quartas, Semifinal, 3Lugar, Final");
        string fase = Helpers.LerString("Fase: ");

        string grupo = "";
        if (fase.ToLower() == "grupo") {
            grupo = Helpers.LerGrupo("Grupo (A-L): ");
        }

        // Data
        string data = Helpers.LerData("Data do jogo");

        // Estádio
        int idxEst = Helpers.SelecionarEstadio("Estádio");
        if (idxEst < 0) {
            Console.WriteLine("Operação cancelada.");
            Helpers.Pausar();
            return;
        }
        int idEstadio = Dados.estadios[idxEst].Id;

        // Seleções
        int idxA = Helpers.SelecionarSelecao("Time A");
        if (idxA < 0) {
            Console.WriteLine("Operação cancelada.");
            Helpers.Pausar();
            return;
        }

        int idxB = Helpers.SelecionarSelecao("Time B");
        if (idxB < 0) {
            Console.WriteLine("Operação cancelada.");
            Helpers.Pausar();
            return;
        }

        if (idxA == idxB) {
            Console.WriteLine("Um time não pode jogar contra ele mesmo!");
            Helpers.Pausar();
            return;
        }

        int idA = Dados.selecoes[idxA].Id;
        int idB = Dados.selecoes[idxB].Id;

        Jogo j;
        j.Id                  = Dados.proximoIdJogo++;
        j.Fase                = fase;
        j.Grupo               = grupo;
        j.Data                = data;
        j.IdEstadio           = idEstadio;
        j.IdTimeA             = idA;
        j.IdTimeB             = idB;
        j.GolsA               = 0;
        j.GolsB               = 0;
        j.Realizado           = false;
        j.IdVencedorPenaltis  = 0;
        j.Ativo               = true;

        Dados.jogos[Dados.totalJogos] = j;
        Dados.totalJogos++;

        Console.WriteLine($"\nJogo cadastrado com ID {j.Id}!");
        CsvHelper.SalvarTodos();
        Helpers.Pausar();
    }

    public static void Listar() {
        Helpers.Titulo("Lista de Jogos");

        bool achou = false;
        for (int i = 0; i < Dados.totalJogos; i++) {
            Jogo j = Dados.jogos[i];
            if (!j.Ativo) {
                continue;
            }

            string nomeA     = Helpers.NomeSelecao(j.IdTimeA);
            string nomeB     = Helpers.NomeSelecao(j.IdTimeB);
            string estadio   = Helpers.NomeEstadio(j.IdEstadio);
            string resultado = j.Realizado ? $"{j.GolsA} x {j.GolsB}" : "A realizar";

            string extra = "";
            if (j.Realizado && j.IdVencedorPenaltis > 0) {
                extra = $" (Pên: {Helpers.NomeSelecao(j.IdVencedorPenaltis)})";
            }

            Console.WriteLine($"[{j.Id,3}] {j.Fase,-10} {j.Grupo,-4} {j.Data,-12} " +
                              $"{nomeA,-20} {resultado,-7} {nomeB,-20} {estadio,-30}{extra}");
            achou = true;
        }

        if (!achou) Console.WriteLine("Nenhum jogo cadastrado.");
        Helpers.Pausar();
    }

    // Listar por fase (sem pausar — usado internamente)
    public static void ListarPorFase(string fase) {
        for (int i = 0; i < Dados.totalJogos; i++) {
            Jogo j = Dados.jogos[i];
            if (!j.Ativo || j.Fase != fase) { 
                continue; 
            }

            string nomeA   = Helpers.NomeSelecao(j.IdTimeA);
            string nomeB   = Helpers.NomeSelecao(j.IdTimeB);
            string estadio = Helpers.NomeEstadio(j.IdEstadio);
            string placar  = j.Realizado ? $"{j.GolsA} x {j.GolsB}" : "x";

            string extra = "";
            if (j.Realizado && j.IdVencedorPenaltis > 0) {
                extra = $" (Pên: {Helpers.NomeSelecao(j.IdVencedorPenaltis)})";
            }

            Console.WriteLine($"  Jogo {j.Id,3}: {nomeA,-20} {placar,-7} {nomeB,-20}" +
                              $"  {estadio,-30} {j.Data}{extra}");
        }
    }

    public static void Alterar() {
        Helpers.Titulo("Alterar Jogo");
        Listar();
        Console.Write("ID do jogo a alterar: ");
        int id  = Helpers.LerInteiro();
        int idx = Helpers.BuscarJogo(id);

        if (idx < 0) {
            Console.WriteLine("Jogo não encontrado!");
            Helpers.Pausar();
            return;
        }

        if (Dados.jogos[idx].Realizado) {
            Console.WriteLine("Jogo já realizado. Use 'Registrar Resultado' para alterar o placar.");
            Helpers.Pausar();
            return;
        }

        Dados.jogos[idx].Data = Helpers.LerData("Nova data");

        // Estádio: busca por nome, ENTER para manter
        Console.Write("Novo estádio (ENTER para manter): ");
        string termo = Console.ReadLine()?.Trim() ?? "";
        if (termo != "") {
            int idxEst = Helpers.SelecionarEstadio("Nome do novo estádio");
            if (idxEst >= 0) {
                Dados.jogos[idx].IdEstadio = Dados.estadios[idxEst].Id;
            }
        }

        Console.WriteLine("Jogo alterado com sucesso!");
        CsvHelper.SalvarTodos();
        Helpers.Pausar();
    }

    public static void Excluir() {
        Helpers.Titulo("Excluir Jogo");
        Listar();
        Console.Write("ID do jogo a excluir: ");
        int id  = Helpers.LerInteiro();
        int idx = Helpers.BuscarJogo(id);

        if (idx < 0) {
            Console.WriteLine("Jogo não encontrado!");
            Helpers.Pausar();
            return;
        }

        Console.Write($"Confirma exclusão do jogo {id}? (S/N): ");
        string conf = Console.ReadLine()?.Trim().ToUpper() ?? "N";
        if (conf == "S") {
            Dados.jogos[idx].Ativo = false;
            Console.WriteLine("Jogo excluído.");
            CsvHelper.SalvarTodos();
        } else {
            Console.WriteLine("Operação cancelada.");
        }
        Helpers.Pausar();
    }
}


static class JogoHelper {
    public static void RegistrarResultado() {
        Helpers.Titulo("Registrar Resultado");
        MenuJogos.Listar();

        Console.Write("ID do jogo: ");
        int id  = Helpers.LerInteiro();
        int idx = Helpers.BuscarJogo(id);

        if (idx < 0) {
            Console.WriteLine("Jogo não encontrado!");
            Helpers.Pausar();
            return;
        }

        Jogo j = Dados.jogos[idx];
        string nomeA = Helpers.NomeSelecao(j.IdTimeA);
        string nomeB = Helpers.NomeSelecao(j.IdTimeB);

        Console.WriteLine($"\n{nomeA} x {nomeB}");
        int golsA = Helpers.LerInteiroPositivo($"Gols de {nomeA}: ");
        int golsB = Helpers.LerInteiroPositivo($"Gols de {nomeB}: ");

        Dados.jogos[idx].GolsA     = golsA;
        Dados.jogos[idx].GolsB     = golsB;
        Dados.jogos[idx].Realizado = true;

        // Se for mata-mata e empate, pede pênaltis
        bool ehMataMata = j.Fase != "Grupo";
        if (ehMataMata && golsA == golsB) {
            Console.WriteLine("\nEmpate no mata-mata! Quem venceu nos pênaltis?");
            Console.WriteLine($"  1 - {nomeA}");
            Console.WriteLine($"  2 - {nomeB}");
            Console.Write("Escolha: ");
            int pens = Helpers.LerInteiro();
            if (pens == 1) { 
                Dados.jogos[idx].IdVencedorPenaltis = j.IdTimeA;
            } else if (pens == 2) { 
                Dados.jogos[idx].IdVencedorPenaltis = j.IdTimeB;
            } else {
                Console.WriteLine("Opção inválida. Escolha 1 ou 2.");
                Helpers.Pausar();
                return;
            }
        }

        Console.WriteLine($"\nResultado registrado: {nomeA} {golsA} x {golsB} {nomeB}");
        CsvHelper.SalvarTodos();
        Helpers.Pausar();
    }
}