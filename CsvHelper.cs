using System;
using System.IO;

static class CsvHelper {
    private static readonly string DIR = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dados");

    private static string Caminho(string arquivo) {
        return Path.Combine(DIR, arquivo);
    }

    public static void SalvarTodos() {
        try {
            Directory.CreateDirectory(DIR);
            SalvarSelecoes();
            SalvarEstadios();
            SalvarJogos();
            SalvarClassificacao();
        } catch (Exception ex) {
            Console.WriteLine($"[AVISO] Erro ao salvar dados: {ex.Message}");
        }
    }

    public static void CarregarTodos() {
        try {
            if (!Directory.Exists(DIR)) { 
                return; 
            }

            CarregarSelecoes();
            CarregarEstadios();
            CarregarJogos();
        } catch (Exception ex) {
            Console.WriteLine($"[AVISO] Erro ao carregar dados: {ex.Message}");
        }
    }

    private static void SalvarSelecoes() {
        using StreamWriter sw = new StreamWriter(Caminho("selecoes.csv"), false, System.Text.Encoding.UTF8);
        sw.WriteLine("id;nome;grupo;ativo");
        for (int i = 0; i < Dados.totalSelecoes; i++) {
            Selecao s = Dados.selecoes[i];
            sw.WriteLine($"{s.Id};{s.Nome};{s.Grupo};{s.Ativo.ToString().ToLower()}");
        }
    }

    private static void CarregarSelecoes() {
        string path = Caminho("selecoes.csv");
        if (!File.Exists(path)) {
            return;
        }

        Dados.totalSelecoes    = 0;
        Dados.proximoIdSelecao = 1;

        string[] linhas = File.ReadAllLines(path, System.Text.Encoding.UTF8);

        // pula cabeçalho
        for (int i = 1; i < linhas.Length; i++) {
            if (string.IsNullOrWhiteSpace(linhas[i])) { 
                continue; 
            }
            string[] p = linhas[i].Split(';');
            if (p.Length < 4) { 
                continue; 
            }

            Selecao s;
            s.Id    = int.Parse(p[0]);
            s.Nome  = p[1];
            s.Grupo = p[2];
            s.Ativo = p[3].ToLower() == "true";

            if (Dados.totalSelecoes < Dados.MAX_SELECOES) {
                Dados.selecoes[Dados.totalSelecoes++] = s;
                if (s.Id >= Dados.proximoIdSelecao) { 
                    Dados.proximoIdSelecao = s.Id + 1;
                }
            }
        }
    }

    private static void SalvarEstadios() {
        using StreamWriter sw = new StreamWriter(Caminho("estadios.csv"), false, System.Text.Encoding.UTF8);
        sw.WriteLine("id;nome;cidade;pais;capacidade;ativo");
        for (int i = 0; i < Dados.totalEstadios; i++) {
            Estadio e = Dados.estadios[i];
            sw.WriteLine($"{e.Id};{e.Nome};{e.Cidade};{e.Pais};{e.Capacidade};{e.Ativo.ToString().ToLower()}");
        }
    }

    private static void CarregarEstadios() {
        string path = Caminho("estadios.csv");
        if (!File.Exists(path)) { 
            return; 
        }

        Dados.totalEstadios    = 0;
        Dados.proximoIdEstadio = 1;

        string[] linhas = File.ReadAllLines(path, System.Text.Encoding.UTF8);
        for (int i = 1; i < linhas.Length; i++) {
            if (string.IsNullOrWhiteSpace(linhas[i])) { 
                continue; 
            }
            string[] p = linhas[i].Split(';');
            if (p.Length < 6) { 
                continue; 
            }

            Estadio e;
            e.Id         = int.Parse(p[0]);
            e.Nome       = p[1];
            e.Cidade     = p[2];
            e.Pais       = p[3];
            e.Capacidade = int.Parse(p[4]);
            e.Ativo      = p[5].ToLower() == "true";

            if (Dados.totalEstadios < Dados.MAX_ESTADIOS) {
                Dados.estadios[Dados.totalEstadios++] = e;
                if (e.Id >= Dados.proximoIdEstadio) { 
                    Dados.proximoIdEstadio = e.Id + 1;
                }
            }
        }
    }

    private static void SalvarJogos() {
        using StreamWriter sw = new StreamWriter(Caminho("jogos.csv"), false,System.Text.Encoding.UTF8);
        sw.WriteLine("id;fase;grupo;data;idEstadio;idTimeA;idTimeB;golsA;golsB;realizado;idVencedorPenaltis;ativo");
        for (int i = 0; i < Dados.totalJogos; i++) {
            Jogo j = Dados.jogos[i];

            sw.WriteLine(
                $"{j.Id};{j.Fase};{j.Grupo};{j.Data};{j.IdEstadio};" +
                $"{j.IdTimeA};{j.IdTimeB};{j.GolsA};{j.GolsB};" +
                $"{j.Realizado.ToString().ToLower()};{j.IdVencedorPenaltis};" +
                $"{j.Ativo.ToString().ToLower()}");
        }
    }

    private static void CarregarJogos() {
        string path = Caminho("jogos.csv");
        if (!File.Exists(path)) { 
            return; 
        }

        Dados.totalJogos    = 0;
        Dados.proximoIdJogo = 1;

        string[] linhas = File.ReadAllLines(path, System.Text.Encoding.UTF8);
        for (int i = 1; i < linhas.Length; i++) {
            if (string.IsNullOrWhiteSpace(linhas[i])) {
                continue; 
            }

            string[] p = linhas[i].Split(';');
            if (p.Length < 12) {
                continue; 
            }

            Jogo j;
            j.Id = int.Parse(p[0]);
            j.Fase = p[1];
            j.Grupo = p[2];
            j.Data = p[3];
            j.IdEstadio = int.Parse(p[4]);
            j.IdTimeA = int.Parse(p[5]);
            j.IdTimeB = int.Parse(p[6]);
            j.GolsA = int.Parse(p[7]);
            j.GolsB = int.Parse(p[8]);
            j.Realizado = p[9].ToLower() == "true";
            j.IdVencedorPenaltis = int.Parse(p[10]);
            j.Ativo = p[11].ToLower() == "true";

            if (Dados.totalJogos < Dados.MAX_JOGOS) {
                Dados.jogos[Dados.totalJogos++] = j;
                if (j.Id >= Dados.proximoIdJogo) {
                    Dados.proximoIdJogo = j.Id + 1;
                }
            }
        }
    }

    private static void SalvarClassificacao() {
        Classificacao.GerarTabelaSilenciosa();

        using StreamWriter sw = new StreamWriter(Caminho("classificacao.csv"), false, System.Text.Encoding.UTF8);
        sw.WriteLine("id;nome;grupo;jogos;vitorias;empates;derrotas;golsPro;golsContra;saldo;pontos");

        for (int i = 0; i < Dados.totalSelecoes; i++) {
            if (!Dados.selecoes[i].Ativo) { 
                continue; 
            }

            Selecao s = Dados.selecoes[i];
            sw.WriteLine(
                $"{s.Id};{s.Nome};{s.Grupo};" +
                $"{Dados.tabela[i,0]};{Dados.tabela[i,1]};{Dados.tabela[i,2]};" +
                $"{Dados.tabela[i,3]};{Dados.tabela[i,4]};{Dados.tabela[i,5]};" +
                $"{Dados.tabela[i,6]};{Dados.tabela[i,7]}");
        }
    }

    public static void SalvarRelatorioFinal() {
        try {
            Directory.CreateDirectory(DIR);
            using StreamWriter sw = new StreamWriter(Caminho("relatorio_final.csv"), false, System.Text.Encoding.UTF8);
            sw.WriteLine("fase;grupo;data;estadio;timeA;golsA;golsB;timeB;vencedor");

            for (int i = 0; i < Dados.totalJogos; i++) {
                Jogo j = Dados.jogos[i];
                if (!j.Ativo || !j.Realizado) {
                    continue; 
                }

                string nomeA   = Helpers.NomeSelecao(j.IdTimeA);
                string nomeB   = Helpers.NomeSelecao(j.IdTimeB);
                string estadio = Helpers.NomeEstadio(j.IdEstadio);

                string vencedor;

                if (j.GolsA > j.GolsB) {
                    vencedor = nomeA;
                }
                else if (j.GolsB > j.GolsA) {
                    vencedor = nomeB;
                }
                else if (j.IdVencedorPenaltis > 0) {
                    vencedor = Helpers.NomeSelecao(j.IdVencedorPenaltis) + " (pen)";
                } else { 
                    vencedor = "Empate"; 
                }

                sw.WriteLine($"{j.Fase};{j.Grupo};{j.Data};{estadio};{nomeA};{j.GolsA};{j.GolsB};{nomeB};{vencedor}");
            }

            Console.WriteLine("Relatório final salvo em 'dados/relatorio_final.csv'!");
        } catch (Exception ex) {
            Console.WriteLine($"Erro ao salvar relatório: {ex.Message}");
        }
    }

    public static void SalvarMataMata() {
        try {
            Directory.CreateDirectory(DIR);
            using StreamWriter sw = new StreamWriter(Caminho("mata_mata.csv"), false,
                System.Text.Encoding.UTF8);
            sw.WriteLine("fase;data;estadio;timeA;golsA;golsB;timeB;vencedor");

            string[] fases = { "16avos","Oitavas","Quartas","Semifinal","3Lugar","Final" };
            foreach (string fase in fases) {
                for (int i = 0; i < Dados.totalJogos; i++) {
                    Jogo j = Dados.jogos[i];
                    if (!j.Ativo || j.Fase != fase) { 
                        continue; 
                    }

                    string nomeA   = Helpers.NomeSelecao(j.IdTimeA);
                    string nomeB   = Helpers.NomeSelecao(j.IdTimeB);
                    string estadio = Helpers.NomeEstadio(j.IdEstadio);

                    string venc = "A realizar";
                    if (j.Realizado) {
                        if (j.GolsA > j.GolsB) {
                            venc = nomeA;
                        } else if (j.GolsB > j.GolsA) {
                            venc = nomeB;
                        } else if (j.IdVencedorPenaltis > 0) {
                            venc = Helpers.NomeSelecao(j.IdVencedorPenaltis) + " (pen)";
                        }
                    }

                    sw.WriteLine($"{fase};{j.Data};{estadio};{nomeA};{j.GolsA};{j.GolsB};{nomeB};{venc}");
                }
            }

            Console.WriteLine("Mata-mata salvo em 'dados/mata_mata.csv'!");
        } catch (Exception ex) {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }
}
