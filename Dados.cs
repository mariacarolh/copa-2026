static class Dados {
    // Limites
    public const int MAX_SELECOES = 48;
    public const int MAX_ESTADIOS = 16;
    public const int MAX_JOGOS    = 200; // fase grupos + mata-mata

    public static Selecao[] selecoes = new Selecao[MAX_SELECOES];
    public static Estadio[] estadios = new Estadio[MAX_ESTADIOS];
    public static Jogo[]    jogos    = new Jogo[MAX_JOGOS];

    public static int totalSelecoes = 0;
    public static int totalEstadios = 0;
    public static int totalJogos    = 0;

    // Matriz de classificação
    // Linhas: índice da seleção no vetor
    // Colunas: 0=Jogos 1=Vitórias 2=Empates 3=Derrotas 4=GolsPro 5=GolsContra 6=SaldoGols 7=Pontos
    public static int[,] tabela = new int[MAX_SELECOES, 8];

    public static readonly string[] GRUPOS_VALIDOS = { "A","B","C","D","E","F","G","H","I","J","K","L" };

    public static readonly string[] FASES_MATA = { "32avos","Oitavas","Quartas","Semifinal","3Lugar","Final" };

    public static int proximoIdSelecao = 1;
    public static int proximoIdEstadio = 1;
    public static int proximoIdJogo    = 1;
}
