using System;

static class Classificacao {
    // Gerar/recalcular tabela dos grupos
    public static void GerarTabela() {
        // Zerar a matriz inteira
        for (int i = 0; i < Dados.MAX_SELECOES; i++)
            for (int c = 0; c < 8; c++)
                Dados.tabela[i, c] = 0;

        // Percorrer jogos realizados da fase de grupos
        for (int k = 0; k < Dados.totalJogos; k++) {
            Jogo j = Dados.jogos[k];
            if (!j.Ativo || !j.Realizado || j.Fase != "Grupo") { 
                continue;
            }

            int ia = EncontrarLinha(j.IdTimeA);
            int ib = EncontrarLinha(j.IdTimeB);
            if (ia < 0 || ib < 0) { 
                continue; 
            }

            // col 0 = jogos
            Dados.tabela[ia, 0]++;
            Dados.tabela[ib, 0]++;

            // col 4 = gols pró, col 5 = gols contra
            Dados.tabela[ia, 4] += j.GolsA;
            Dados.tabela[ia, 5] += j.GolsB;
            Dados.tabela[ib, 4] += j.GolsB;
            Dados.tabela[ib, 5] += j.GolsA;

            // vitória, empate ou derrota
            if (j.GolsA > j.GolsB) {
                Dados.tabela[ia, 1]++; // vitória A
                Dados.tabela[ib, 3]++; // derrota B
            } else if (j.GolsA < j.GolsB) {
                Dados.tabela[ib, 1]++; // vitória B
                Dados.tabela[ia, 3]++; // derrota A
            } else {
                Dados.tabela[ia, 2]++; // empate A
                Dados.tabela[ib, 2]++; // empate B
            }
        }

        // Calcular saldo e pontos para cada seleção ativa
        for (int i = 0; i < Dados.totalSelecoes; i++) {
            if (!Dados.selecoes[i].Ativo) { 
                continue; 
            }
            Dados.tabela[i, 6] = Dados.tabela[i, 4] - Dados.tabela[i, 5]; // saldo
            Dados.tabela[i, 7] = Dados.tabela[i, 1] * 3 + Dados.tabela[i, 2]; // pontos
        }

        // Exibir tabela por grupo
        Helpers.Titulo("Tabela de Classificação por Grupo");

        foreach (string grupo in Dados.GRUPOS_VALIDOS) {
            // Pega índices das seleções desse grupo
            int[] indices = new int[4];
            int total = 0;
            for (int i = 0; i < Dados.totalSelecoes; i++) {
                if (Dados.selecoes[i].Ativo && Dados.selecoes[i].Grupo == grupo) {
                    indices[total++] = i;
                }
            }

            if (total == 0) { 
                continue;
            }

            Ordenar(indices, total);

            Console.WriteLine($"\n  GRUPO {grupo}");
            Console.WriteLine($"  {"Pos",-5}{"Seleção",-22}{"J",3}{"V",4}{"E",4}{"D",4}{"GP",5}{"GC",5}{"SG",5}{"PTS",5}");
            Console.WriteLine("  " + new string('-', 57));

            for (int p = 0; p < total; p++) {
                int i    = indices[p];
                string   pos = p == 0 ? "1º" : p == 1 ? "2º" : p == 2 ? "3º" : "4º";
               
                Console.WriteLine(
                    $"  {pos,-5}{Dados.selecoes[i].Nome,-22}" +
                    $"{Dados.tabela[i,0],3}{Dados.tabela[i,1],4}" +
                    $"{Dados.tabela[i,2],4}{Dados.tabela[i,3],4}" +
                    $"{Dados.tabela[i,4],5}{Dados.tabela[i,5],5}" +
                    $"{Dados.tabela[i,6],5}{Dados.tabela[i,7],5}");
            }
        }

        Helpers.Pausar();
    }

    // Melhores terceiros colocados
    public static void MelhoresTerceiros() {
        Helpers.Titulo("Melhores Terceiros Colocados");

        // Regenera tabela silenciosamente para garantir dados atualizados
        GerarTabelaSilenciosa();

        int[] terceiros   = new int[12];
        int   totalTerceiros = 0;

        foreach (string grupo in Dados.GRUPOS_VALIDOS) {
            int[] indices = new int[4];
            int total = 0;

            for (int i = 0; i < Dados.totalSelecoes; i++)
                if (Dados.selecoes[i].Ativo && Dados.selecoes[i].Grupo == grupo)
                    indices[total++] = i;

            if (total < 3) { 
                continue; 
            }

            Ordenar(indices, total);
            terceiros[totalTerceiros++] = indices[2]; // posição 2 = 3º lugar
        }

        if (totalTerceiros == 0) {
            Console.WriteLine("Nenhum terceiro colocado encontrado.");
            Helpers.Pausar();
            return;
        }

        // Ordena os terceiros entre si
        Ordenar(terceiros, totalTerceiros);

        Console.WriteLine($"\n  {"Pos",-5}{"Seleção",-22}{"Grupo",6}{"PTS",5}{"V",4}{"SG",5}{"GP",5}");
        Console.WriteLine("  " + new string('-', 53));

        for (int p = 0; p < totalTerceiros; p++) {
            int i   = terceiros[p];
            string cl = p < 8 ? "✓ CLASSIFICADO" : "";
            Console.WriteLine(
                $"  {p+1}º   {Dados.selecoes[i].Nome,-22}" +
                $"{Dados.selecoes[i].Grupo,6}" +
                $"{Dados.tabela[i,7],5}{Dados.tabela[i,1],4}" +
                $"{Dados.tabela[i,6],5}{Dados.tabela[i,4],5}  {cl}");
        }

        Helpers.Pausar();
    }

    // Ordenar por critérios de desempate
    public static void Ordenar(int[] indices, int total) {
        // Bubble sort (simples e didático)
        for (int i = 0; i < total - 1; i++) {
            for (int j = 0; j < total - i - 1; j++) {
                if (Comparar(indices[j], indices[j+1]) > 0) {
                    int tmp = indices[j];
                    indices[j]   = indices[j+1];
                    indices[j+1] = tmp;
                }
            }
        }
    }

    // Retorna > 0 se A deve ficar ATRÁS de B (B é melhor)
    private static int Comparar(int a, int b) {
        // 1º pontos
        int dif = Dados.tabela[b, 7] - Dados.tabela[a, 7];
        if (dif != 0) return dif;
        // 2º vitórias
        dif = Dados.tabela[b, 1] - Dados.tabela[a, 1];
        if (dif != 0) return dif;
        // 3º saldo
        dif = Dados.tabela[b, 6] - Dados.tabela[a, 6];
        if (dif != 0) return dif;
        // 4º gols pró
        dif = Dados.tabela[b, 4] - Dados.tabela[a, 4];
        if (dif != 0) return dif;
        // 5º alfabético
        return string.Compare(
            Dados.selecoes[a].Nome,
            Dados.selecoes[b].Nome,
            StringComparison.OrdinalIgnoreCase);
    }

    // Retorna o índice (no vetor) de uma seleção pelo seu ID
    public static int EncontrarLinha(int idSelecao) {
        for (int i = 0; i < Dados.totalSelecoes; i++)
            if (Dados.selecoes[i].Id == idSelecao && Dados.selecoes[i].Ativo)
                return i;
        return -1;
    }

    // Versão silenciosa (sem exibir) para uso interno
    public static void GerarTabelaSilenciosa() {
        for (int i = 0; i < Dados.MAX_SELECOES; i++)
            for (int c = 0; c < 8; c++)
                Dados.tabela[i, c] = 0;

        for (int k = 0; k < Dados.totalJogos; k++) {
            Jogo j = Dados.jogos[k];
            if (!j.Ativo || !j.Realizado || j.Fase != "Grupo") continue;

            int ia = EncontrarLinha(j.IdTimeA);
            int ib = EncontrarLinha(j.IdTimeB);
            if (ia < 0 || ib < 0) {
                continue; 
            }

            Dados.tabela[ia, 0]++; Dados.tabela[ib, 0]++;
            Dados.tabela[ia, 4] += j.GolsA; Dados.tabela[ia, 5] += j.GolsB;
            Dados.tabela[ib, 4] += j.GolsB; Dados.tabela[ib, 5] += j.GolsA;

            if (j.GolsA > j.GolsB)      { Dados.tabela[ia,1]++; Dados.tabela[ib,3]++; }
            else if (j.GolsA < j.GolsB) { Dados.tabela[ib,1]++; Dados.tabela[ia,3]++; }
            else                         { Dados.tabela[ia,2]++; Dados.tabela[ib,2]++; }
        }

        for (int i = 0; i < Dados.totalSelecoes; i++) {
            if (!Dados.selecoes[i].Ativo) { 
                continue; 
            }
            Dados.tabela[i, 6] = Dados.tabela[i, 4] - Dados.tabela[i, 5];
            Dados.tabela[i, 7] = Dados.tabela[i, 1] * 3 + Dados.tabela[i, 2];
        }
    }

    // Retorna os 32 classificados para o mata-mata
    // 0-23: 1º e 2º de cada grupo  |  24-31: 8 melhores terceiros
    public static int[] ObterClassificados32() {
        GerarTabelaSilenciosa();

        int[] classificados = new int[32];
        int pos = 0;

        // 1º e 2º de cada grupo
        foreach (string grupo in Dados.GRUPOS_VALIDOS) {
            int[] indices = new int[4];
            int total = 0;
            for (int i = 0; i < Dados.totalSelecoes; i++)
                if (Dados.selecoes[i].Ativo && Dados.selecoes[i].Grupo == grupo) { 
                    indices[total++] = i;
                }

            if (total < 2) continue;
            Ordenar(indices, total);
            classificados[pos++] = indices[0]; // 1º
            classificados[pos++] = indices[1]; // 2º
        }

        // 8 melhores terceiros
        int[] terceiros      = new int[12];
        int   totalTerceiros = 0;
        foreach (string grupo in Dados.GRUPOS_VALIDOS) {
            int[] indices = new int[4];
            int total = 0;
            for (int i = 0; i < Dados.totalSelecoes; i++)
                if (Dados.selecoes[i].Ativo && Dados.selecoes[i].Grupo == grupo)
                    indices[total++] = i;

            if (total < 3) continue;
            Ordenar(indices, total);
            terceiros[totalTerceiros++] = indices[2];
        }
        Ordenar(terceiros, totalTerceiros);
        for (int t = 0; t < 8 && t < totalTerceiros; t++)
            classificados[pos++] = terceiros[t];

        return classificados;
    }
}
