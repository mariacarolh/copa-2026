using System;

static class MenuSelecoes {
    public static void Exibir() {
        int op;
        do {
            Helpers.Titulo("Gerenciar Seleções");
            Console.WriteLine("  1 - Cadastrar seleção");
            Console.WriteLine("  2 - Listar seleções");
            Console.WriteLine("  3 - Alterar seleção");
            Console.WriteLine("  4 - Excluir seleção");
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
        Helpers.Titulo("Cadastrar Seleção");

        if (Dados.totalSelecoes >= Dados.MAX_SELECOES) {
            Console.WriteLine("Limite de 48 seleções atingido!");
            Helpers.Pausar();
            return;
        }

        string nome  = Helpers.LerString("Nome da seleção: ");
        string grupo = Helpers.LerGrupo("Grupo (A-L): ");

        if (Helpers.ContarSelecoesPorGrupo(grupo) >= 4) {
            Console.WriteLine($"Grupo {grupo} já tem 4 seleções cadastradas!");
            Helpers.Pausar();
            return;
        }

        Selecao s;
        s.Id    = Dados.proximoIdSelecao++;
        s.Nome  = nome;
        s.Grupo = grupo;
        s.Ativo = true;

        Dados.selecoes[Dados.totalSelecoes] = s;
        Dados.totalSelecoes++;

        Console.WriteLine($"\nSeleção '{nome}' cadastrada com ID {s.Id}!");
        CsvHelper.SalvarTodos();
        Helpers.Pausar();
    }

    public static void Listar() {
        Helpers.Titulo("Lista de Seleções");

        bool achou = false;
        for (int g = 0; g < Dados.GRUPOS_VALIDOS.Length; g++) {
            string grupo = Dados.GRUPOS_VALIDOS[g];
            bool temGrupo = false;

            for (int i = 0; i < Dados.totalSelecoes; i++) {
                if (Dados.selecoes[i].Ativo && Dados.selecoes[i].Grupo == grupo) {
                    temGrupo = true;
                }
            }

            if (!temGrupo) {
                continue; 
            }

            Console.WriteLine($"\n  GRUPO {grupo}:");
            Console.WriteLine($"  {"ID",-5} {"Nome",-25}");
            Console.WriteLine("  " + new string('-', 32));

            for (int i = 0; i < Dados.totalSelecoes; i++) {
                Selecao s = Dados.selecoes[i];
                if (s.Ativo && s.Grupo == grupo) {
                    Console.WriteLine($"  {s.Id,-5} {s.Nome,-25}");
                    achou = true;
                }
            }
        }

        if (!achou) Console.WriteLine("Nenhuma seleção cadastrada.");
        Helpers.Pausar();
    }

    public static void Alterar() {
        Helpers.Titulo("Alterar Seleção");
        int idx = Helpers.SelecionarSelecao("Nome da seleção a alterar");

        if (idx < 0) {
            Console.WriteLine("Operação cancelada.");
            Helpers.Pausar();
            return;
        }

        Console.WriteLine($"Seleção atual: {Dados.selecoes[idx].Nome} - Grupo {Dados.selecoes[idx].Grupo}");
        string novoNome = Helpers.LerString("Novo nome: ");

        // Lê o grupo explicitamente (sem "manter", para simplificar)
        Console.Write("Novo grupo A-L (ENTER para manter): ");
        string entrada = Console.ReadLine()?.Trim().ToUpper() ?? "";
        string novoGrupo = entrada == "" ? Dados.selecoes[idx].Grupo : entrada;

        bool grupoValido = false;
        foreach (string g in Dados.GRUPOS_VALIDOS) {
            if (g == novoGrupo) {
                grupoValido = true;
                break;
            }
        }

        if (!grupoValido) {
            Console.WriteLine("Grupo inválido.");
            Helpers.Pausar();
            return;
        }

        // Verifica limite do grupo apenas se mudou de grupo
        if (novoGrupo != Dados.selecoes[idx].Grupo && Helpers.ContarSelecoesPorGrupo(novoGrupo) >= 4) {
            Console.WriteLine($"Grupo {novoGrupo} já tem 4 seleções!");
            Helpers.Pausar();
            return;
        }

        Selecao s = Dados.selecoes[idx];
        s.Nome  = novoNome;
        s.Grupo = novoGrupo;
        Dados.selecoes[idx] = s;

        Console.WriteLine("Seleção alterada com sucesso!");
        CsvHelper.SalvarTodos();
        Helpers.Pausar();
    }

    public static void Excluir() {
        Helpers.Titulo("Excluir Seleção");
        int idx = Helpers.SelecionarSelecao("Nome da seleção a excluir");

        if (idx < 0) {
            Console.WriteLine("Operação cancelada.");
            Helpers.Pausar();
            return;
        }

        Console.Write($"Confirma exclusão de '{Dados.selecoes[idx].Nome}'? (S/N): ");
        string conf = Console.ReadLine()?.Trim().ToUpper() ?? "N";
        if (conf == "S") {
            Selecao s = Dados.selecoes[idx];
            s.Ativo = false;
            Dados.selecoes[idx] = s;

            // compacta o array removendo a posição inativa
            for (int i = idx; i < Dados.totalSelecoes - 1; i++) {
                Dados.selecoes[i] = Dados.selecoes[i + 1];
            }
            Dados.totalSelecoes--;

            Console.WriteLine("Seleção excluída.");
            CsvHelper.SalvarTodos();
        } else {
            Console.WriteLine("Operação cancelada.");
        }
        Helpers.Pausar();
    }
}