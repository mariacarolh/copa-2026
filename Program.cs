using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Tenta carregar dados do CSV ao iniciar
        CsvHelper.CarregarTodos();

        int opcao;
        do
        {
            Console.Clear();
            ExibirMenuPrincipal();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\n   Escolha uma opção: ");
            Console.ResetColor();

            opcao = Helpers.LerInteiro();

            switch (opcao)
            {
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
                case 11:
                    Helpers.Creditos();
                    break;
                case 0:
                    CsvHelper.SalvarTodos();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n   Saindo... Até a próxima Copa!");
                    Console.ResetColor();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n   Opção inválida!");
                    Console.ResetColor();
                    Helpers.Pausar();
                    break;
            }
        } while (opcao != 0);
    }

    // =========================================================
    //  Apenas visual — monta a caixa do menu principal.
    //  Nenhuma lógica do programa foi alterada nesta função.
    // =========================================================
    static void ExibirMenuPrincipal()
    {
        const int largura = 46;

        // Funções locais
        void Borda(char esquerda, char meio, char direita)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(esquerda + new string(meio, largura) + direita);
            Console.ResetColor();
        }

        void Conteudo(string texto, ConsoleColor cor, bool centralizar = false)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("║");

            string corpo = centralizar
                ? Centralizar(texto, largura)
                : (" " + texto).PadRight(largura);

            Console.ForegroundColor = cor;
            Console.Write(corpo);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("║");
            Console.ResetColor();
        }

     

        // ===== BORDA SUPERIOR ESTILIZADA (Bandeira do Brasil) =====
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("╔");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(new string('═', 15));
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(new string('═', 16));
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(new string('═', 15));
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("╗");
        Console.ResetColor();

        // ===== TÍTULO =====
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("║");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(" " + Centralizar("🏆 COPA DO MUNDO 2026 🏆", largura - 2));
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("║");
        Console.ResetColor();

        // ===== DIVISÓRIA AZUL =====
        Borda('╠', '═', '╣');
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("║" + new string('═', largura) + "║");
        Console.ResetColor();

        // ===== OPÇÕES DO MENU =====
        Conteudo("", ConsoleColor.Gray);
        Conteudo(" 1  -  Gerenciar Seleções", ConsoleColor.White);
        Conteudo(" 2  -  Gerenciar Estádios", ConsoleColor.White);
        Conteudo(" 3  -  Gerenciar Jogos", ConsoleColor.White);
        Conteudo(" 4  -  Registrar Resultado", ConsoleColor.White);
        Conteudo(" 5  -  Gerar Tabela dos Grupos", ConsoleColor.White);
        Conteudo(" 6  -  Mostrar Melhores Terceiros", ConsoleColor.White);
        Conteudo(" 7  -  Gerar Mata-Mata", ConsoleColor.White);
        Conteudo(" 8  -  Registrar Resultados Mata-Mata", ConsoleColor.White);
        Conteudo(" 9  -  Mostrar Campeão", ConsoleColor.White);
        Conteudo(" 10 -  Relatórios", ConsoleColor.White);
        Conteudo(" 11 -  Créditos", ConsoleColor.White);
        Conteudo("", ConsoleColor.Gray);

        // ===== DIVISÓRIA AMARELA =====
        Borda('╠', '═', '╣');
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("║" + new string('═', largura) + "║");
        Console.ResetColor();

        // ===== SAIR =====
        Conteudo(" 0  -  Sair", ConsoleColor.White);

        // ===== BORDA INFERIOR =====
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("╚");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(new string('═', 15));
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(new string('═', 16));
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(new string('═', 15));
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("╝");
        Console.ResetColor();
    }

    

    static string Centralizar(string texto, int largura)
    {
        int espacos = largura - texto.Length;
        if (espacos <= 0) return texto;
        int esquerda = espacos / 2;
        int direita = espacos - esquerda;
        return new string(' ', esquerda) + texto + new string(' ', direita);
    }
}