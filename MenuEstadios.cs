using System;

static class MenuEstadios {
    public static void Exibir() {
        int op;
        do {
            Helpers.Titulo("Gerenciar Estádios");
            Console.WriteLine("  1 - Cadastrar estádio");
            Console.WriteLine("  2 - Listar estádios");
            Console.WriteLine("  3 - Alterar estádio");
            Console.WriteLine("  4 - Excluir estádio");
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
        Helpers.Titulo("Cadastrar Estádio");

        if (Dados.totalEstadios >= Dados.MAX_ESTADIOS) {
            Console.WriteLine("Limite de estádios atingido!");
            Helpers.Pausar();
            return;
        }

        Estadio e;
        e.Id         = Dados.proximoIdEstadio++;
        e.Nome       = Helpers.LerString("Nome do estádio: ");
        e.Cidade     = Helpers.LerString("Cidade: ");
        e.Pais       = Helpers.LerString("País: ");
        e.Capacidade = Helpers.LerInteiroPositivo("Capacidade: ");
        e.Ativo      = true;

        Dados.estadios[Dados.totalEstadios] = e;
        Dados.totalEstadios++;

        Console.WriteLine($"\nEstádio '{e.Nome}' cadastrado com ID {e.Id}!");
        CsvHelper.SalvarTodos();
        Helpers.Pausar();
    }

    public static void Listar() {
        Helpers.Titulo("Lista de Estádios");
        Console.WriteLine($"  {"ID",-4} {"Nome",-32} {"Cidade",-20} {"País",-15} {"Cap.",-8}");
        Console.WriteLine("  " + new string('-', 82));

        bool achou = false;
        for (int i = 0; i < Dados.totalEstadios; i++) {
            Estadio e = Dados.estadios[i];
            if (!e.Ativo) { 
                continue; 
            }

            Console.WriteLine($"  {e.Id,-4} {e.Nome,-32} {e.Cidade,-20} {e.Pais,-15} {e.Capacidade,-8}");
            achou = true;
        }

        if (!achou) Console.WriteLine("Nenhum estádio cadastrado.");
        Helpers.Pausar();
    }

    public static void Alterar() {
        Helpers.Titulo("Alterar Estádio");
        Console.Write("ID do estádio a alterar: ");
        int id  = Helpers.LerInteiro();
        int idx = Helpers.BuscarEstadio(id);

        if (idx < 0) {
            Console.WriteLine("Estádio não encontrado!");
            Helpers.Pausar();
            return;
        }

        Console.WriteLine($"Estádio atual: {Dados.estadios[idx].Nome} - {Dados.estadios[idx].Cidade}");

        Dados.estadios[idx].Nome       = Helpers.LerString("Novo nome: ");
        Dados.estadios[idx].Cidade     = Helpers.LerString("Nova cidade: ");
        Dados.estadios[idx].Pais       = Helpers.LerString("Novo país: ");
        Dados.estadios[idx].Capacidade = Helpers.LerInteiroPositivo("Nova capacidade: ");

        Console.WriteLine("Estádio alterado com sucesso!");
        CsvHelper.SalvarTodos();
        Helpers.Pausar();
    }

    public static void Excluir() {
        Helpers.Titulo("Excluir Estádio");
        Console.Write("ID do estádio a excluir: ");
        int id  = Helpers.LerInteiro();
        int idx = Helpers.BuscarEstadio(id);

        if (idx < 0) {
            Console.WriteLine("Estádio não encontrado!");
            Helpers.Pausar();
            return;
        }

        Console.Write($"Confirma exclusão de '{Dados.estadios[idx].Nome}'? (S/N): ");
        string conf = Console.ReadLine()?.Trim().ToUpper() ?? "N";
        if (conf == "S") {
            Dados.estadios[idx].Ativo = false;
            Console.WriteLine("Estádio excluído.");
            CsvHelper.SalvarTodos();
        } else { 
            Console.WriteLine("Operação cancelada.");
        }
        Helpers.Pausar();
    }
}
