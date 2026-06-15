using System;
using System.IO;

class Program {
    static void Main(string[] args) {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Title = "Sistema Copa do Mundo 2026";

        // Tenta carregar dados do CSV ao iniciar
        CsvHelper.CarregarTodos();

        int opcao;
        do {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════════╗");
            Console.WriteLine("║     SISTEMA COPA DO MUNDO 2026        ║");
            Console.WriteLine("╠═══════════════════════════════════════╣");
            Console.WriteLine("║  1 - Gerenciar Seleções               ║");
            Console.WriteLine("║  2 - Gerenciar Estádios               ║");
            Console.WriteLine("║  3 - Gerenciar Jogos                  ║");
            Console.WriteLine("║  4 - Registrar Resultado              ║");
            Console.WriteLine("║  5 - Gerar Tabela dos Grupos          ║");
            Console.WriteLine("║  6 - Mostrar Melhores Terceiros       ║");
            Console.WriteLine("║  7 - Gerar Mata-Mata                  ║");
            Console.WriteLine("║  8 - Registrar Resultados Mata-Mata   ║");
            Console.WriteLine("║  9 - Mostrar Campeão                  ║");
            Console.WriteLine("║  10 - Relatórios                      ║");
            Console.WriteLine("║  0 - Sair                             ║");
            Console.WriteLine("╚═══════════════════════════════════════╝");
            Console.Write("\nEscolha uma opção: ");

            opcao = Helpers.LerInteiro();

            switch (opcao) {
                case 1:  
                    MenuSelecoes.Exibir();           
                    break;
                case 2: 
                    MenuEstadios.Exibir();       
                    break;
                case 3:  
                    MenuJogos.Exibir();           
                    break;
                case 4: 
                    JogoHelper.RegistrarResultado(); 
                    break;
                case 5:  
                    Classificacao.GerarTabela();    
                    break;
                case 6: 
                    Classificacao.MelhoresTerceiros();
                    break;
                case 7: 
                    MataMata.Gerar();               
                    break;
                case 8: 
                    MataMata.RegistrarResultados(); 
                    break;
                case 9: 
                    MataMata.MostrarCampeao(); 
                    break;
                case 10: 
                    MenuRelatorios.Exibir();  
                    break;
                case 0:
                    CsvHelper.SalvarTodos();
                    Console.WriteLine("\nSaindo... Até a próxima Copa!");
                    break;
                default:
                    Console.WriteLine("\nOpção inválida!");
                    Helpers.Pausar();
                    break;
            }

        } while (opcao != 0);
    }
}
