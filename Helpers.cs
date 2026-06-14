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

            foreach (string g in Dados.GRUPOS_VALIDOS)
                if (g == val) { 
                    ok = true; 
                    break; 
                }
            if (!ok) {
                Console.WriteLine("Grupo inválido. Use A até L.");
            }
            else { 
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

    public static void Titulo(string texto) {
        Console.Clear();
        Console.WriteLine(new string('═', 45));
        Console.WriteLine("  " + texto.ToUpper());
        Console.WriteLine(new string('═', 45));
    }

    public static int BuscarSelecao(int id) {
        for (int i = 0; i < Dados.totalSelecoes; i++)
            if (Dados.selecoes[i].Id == id && Dados.selecoes[i].Ativo)
                return i;
        return -1;
    }

    public static int BuscarEstadio(int id) {
        for (int i = 0; i < Dados.totalEstadios; i++)
            if (Dados.estadios[i].Id == id && Dados.estadios[i].Ativo)
                return i;
        return -1;
    }

    public static int BuscarJogo(int id) {
        for (int i = 0; i < Dados.totalJogos; i++)
            if (Dados.jogos[i].Id == id && Dados.jogos[i].Ativo)
                return i;
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
        for (int i = 0; i < Dados.totalSelecoes; i++)
            if (Dados.selecoes[i].Ativo && Dados.selecoes[i].Grupo == grupo)
                count++;
        return count;
    }
}
