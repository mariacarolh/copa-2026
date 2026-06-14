using System;

static class MenuRelatorios {
    public static void Exibir() {
        int op;
        do {
            Helpers.Titulo("Relatórios");
            Console.WriteLine("  1 - Tabela dos grupos (tela)");
            Console.WriteLine("  2 - Jogos da fase de grupos (tela)");
            Console.WriteLine("  3 - Melhores terceiros (tela)");
            Console.WriteLine("  4 - Mata-mata (tela)");
            Console.WriteLine("  5 - Gerar relatorio_final.csv");
            Console.WriteLine("  6 - Gerar mata_mata.csv");
            Console.WriteLine("  0 - Voltar");
            Console.Write("\nOpção: ");
            op = Helpers.LerInteiro();

            switch (op) {
                case 1: 
                    Classificacao.GerarTabela();              
                    break;
                case 2: 
                    RelatorioJogosGrupo();                   
                    break;
                case 3:
                    Classificacao.MelhoresTerceiros();   
                    break;
                case 4: 
                    MataMata.MostrarChave(); Helpers.Pausar();
                    break;
                case 5:
                    CsvHelper.SalvarRelatorioFinal();
                    Helpers.Pausar();
                    break;
                case 6:
                    CsvHelper.SalvarMataMata();
                    Helpers.Pausar();
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

    private static void RelatorioJogosGrupo() {
        Helpers.Titulo("Jogos da Fase de Grupos");

        foreach (string grupo in Dados.GRUPOS_VALIDOS) {
            bool temJogo = false;

            for (int i = 0; i < Dados.totalJogos; i++)
                if (Dados.jogos[i].Ativo && Dados.jogos[i].Fase == "Grupo" && Dados.jogos[i].Grupo == grupo) {
                    temJogo = true;
                }

            if (!temJogo) {
                continue; 
            }

            Console.WriteLine($"\n  GRUPO {grupo}:");
            for (int i = 0; i < Dados.totalJogos; i++) {
                Jogo j = Dados.jogos[i];
                if (!j.Ativo || j.Fase != "Grupo" || j.Grupo != grupo) { 
                    continue; 
                }

                string nomeA   = Helpers.NomeSelecao(j.IdTimeA);
                string nomeB   = Helpers.NomeSelecao(j.IdTimeB);
                string estadio = Helpers.NomeEstadio(j.IdEstadio);
                string placar  = j.Realizado ? $"{j.GolsA} x {j.GolsB}" : "A realizar";

                Console.WriteLine($"  {j.Data}  {nomeA,-22} {placar,-7} {nomeB,-22}  {estadio}");
            }
        }

        Helpers.Pausar();
    }
}
