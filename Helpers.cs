using System;

static class Helpers {
    public static int LerInteiro() {
        int val;

        while (!int.TryParse(Console.ReadLine(), out val)) {
            Console.Write("Valor inválido. Digite um número inteiro: ");
        }
        return val;
    }

    public static int LerInteiroPositivo(string prompt) {
        int val;

        do {
            Console.Write(prompt);
            while (!int.TryParse(Console.ReadLine(), out val)) {
                Console.Write("Valor inválido. " + prompt);
            }
            if (val < 0) {
                Console.WriteLine("Valor não pode ser negativo.");
            }
        } while (val < 0);
        return val;
    }

    public static string LerString(string prompt) {
        string val;

        do {
            Console.Write(prompt);
            val = Console.ReadLine()?.Trim() ?? "";
            if (val == "") Console.WriteLine("Campo obrigatório. Tente novamente.");
        } while (val == "");
        return val;
    }

    public static string LerGrupo(string prompt) {
        string val;

        do {
            Console.Write(prompt);
            val = (Console.ReadLine()?.Trim() ?? "").ToUpper();
            bool ok = false;

            foreach (string g in Dados.GRUPOS_VALIDOS) {
                if (g == val) {
                    ok = true;
                    break;
                }
            }
            if (!ok) {
                Console.WriteLine("Grupo inválido. Use A até L.");
            } else {
                return val;
            }
        } while (true);
    }

    public static string LerData(string prompt) {
        string val;
        do {
            Console.Write(prompt + " (dd/MM/yyyy): ");
            val = Console.ReadLine()?.Trim() ?? "";
            if (val.Length == 10) return val; // validação básica de formato
            Console.WriteLine("Formato inválido. Use dd/MM/yyyy.");
        } while (true);
    }

    public static void Pausar() {
        Console.WriteLine("\nPressione ENTER para continuar...");
        Console.ReadLine();
    }

    public static void Titulo(string texto)
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("╔══════════════════════════════════════════════╗");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"║ {texto.ToUpper().PadRight(42)} ║");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("╚══════════════════════════════════════════════╝");

        Console.ResetColor();
    }

    public static int BuscarSelecao(int id) {
        for (int i = 0; i < Dados.totalSelecoes; i++) {
            if (Dados.selecoes[i].Id == id && Dados.selecoes[i].Ativo) {
                return i;
            }
        }
        return -1;
    }



    public static int BuscarEstadio(int id) {
        for (int i = 0; i < Dados.totalEstadios; i++) {
            if (Dados.estadios[i].Id == id && Dados.estadios[i].Ativo) {
                return i;
            }
        }
        return -1;
    }

    public static int BuscarJogo(int id) {
        for (int i = 0; i < Dados.totalJogos; i++) {
            if (Dados.jogos[i].Id == id && Dados.jogos[i].Ativo) {
                return i;
            }
        }
        return -1;
    }

    public static string NomeSelecao(int id) {
        int idx = BuscarSelecao(id);
        return idx >= 0 ? Dados.selecoes[idx].Nome : "?";
    }

    public static string NomeEstadio(int id) {
        int idx = BuscarEstadio(id);
        return idx >= 0 ? Dados.estadios[idx].Nome : "?";
    }

    public static int ContarSelecoesPorGrupo(string grupo) {
        int count = 0;
        for (int i = 0; i < Dados.totalSelecoes; i++) {
            if (Dados.selecoes[i].Ativo && Dados.selecoes[i].Grupo == grupo) {
                count++;
            }
        }
        return count;
    }

    public static int SelecionarSelecao(string prompt = "Nome da seleção") {
        while (true) {
            Console.Write($"{prompt}: ");
            string termo = Console.ReadLine()?.Trim() ?? "";
            if (termo == "") return -1;

            int[] matches = new int[Dados.MAX_SELECOES];
            int total = 0;
            for (int i = 0; i < Dados.totalSelecoes; i++) {
                if (Dados.selecoes[i].Ativo &&
                    Dados.selecoes[i].Nome.ToLower().Contains(termo.ToLower())) {
                    matches[total++] = i;
                }
            }

            if (total == 0) {
                Console.WriteLine("Nenhuma seleção encontrada. Tente novamente.");
                continue;
            }
            if (total == 1) {
                Console.WriteLine($"  → {Dados.selecoes[matches[0]].Nome}");
                return matches[0];
            }

            for (int i = 0; i < total; i++) {
                Console.WriteLine($"  {i + 1} - {Dados.selecoes[matches[i]].Nome} (Grupo {Dados.selecoes[matches[i]].Grupo})");
            }
            Console.Write("Escolha o número (0 = cancelar): ");
            int op = LerInteiro();
            if (op == 0) return -1;
            if (op >= 1 && op <= total) return matches[op - 1];
            Console.WriteLine("Opção inválida.");
        }
    }

    public static int SelecionarEstadio(string prompt = "Nome do estádio") {
        while (true) {
            Console.Write($"{prompt}: ");
            string termo = Console.ReadLine()?.Trim() ?? "";
            if (termo == "") return -1;

            int[] matches = new int[Dados.MAX_ESTADIOS];
            int total = 0;
            for (int i = 0; i < Dados.totalEstadios; i++) {
                if (Dados.estadios[i].Ativo &&
                    Dados.estadios[i].Nome.ToLower().Contains(termo.ToLower())) {
                    matches[total++] = i;
                }
            }

            if (total == 0) {
                Console.WriteLine("Nenhum estádio encontrado. Tente novamente.");
                continue;
            }
            if (total == 1) {
                Console.WriteLine($"  → {Dados.estadios[matches[0]].Nome}");
                return matches[0];
            }

            for (int i = 0; i < total; i++) {
                Console.WriteLine($"  {i + 1} - {Dados.estadios[matches[i]].Nome} ({Dados.estadios[matches[i]].Cidade})");
            }
            Console.Write("Escolha o número (0 = cancelar): ");
            int op = LerInteiro();
            if (op == 0) return -1;
            if (op >= 1 && op <= total) return matches[op - 1];
            Console.WriteLine("Opção inválida.");
        }
    }

    public static void Creditos() {
        Helpers.Titulo("Créditos");

        Console.WriteLine("  INTEGRANTES");
        Console.WriteLine("  " + new string('-', 42));
        Console.WriteLine("  Cesar Augusto Silva RA: 2026109330");
        Console.WriteLine("  Maria Carolina Pereira RA: 2022202695");
        Console.WriteLine("  Sara Hadassa B. Carvalho RA: 2022101542");

        Console.WriteLine();
        Console.WriteLine("  LINGUAGEM");
        Console.WriteLine("  " + new string('-', 42));
        Console.WriteLine("  C# (.NET)");

        Console.WriteLine();
        Console.WriteLine("  RECURSOS UTILIZADOS");
        Console.WriteLine("  " + new string('-', 42));
        Console.WriteLine("  Structs — Selecao, Estadio, Jogo");
        Console.WriteLine("  Matrizes — arrays fixos de structs");
        Console.WriteLine("  Classes static — menus, helpers, CSV");
        Console.WriteLine("  Arquivos CSV — persistência de dados");
        Console.WriteLine("  String format — tabelas alinhadas no console");

        Helpers.Pausar();
    }
}