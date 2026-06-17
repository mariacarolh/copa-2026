struct Selecao {
    public int Id;
    public string Nome;
    public string Grupo; // A até L
    public bool Ativo;
}

struct Estadio {
    public int Id;
    public string Nome;
    public string Cidade;
    public string Pais;
    public int Capacidade;
    public bool Ativo;
}

struct Jogo {
    public int Id;
    public string Fase; // Grupo, 16avos, Oitavas, Quartas, Semifinal, 3Lugar, Final
    public string Grupo; // A-L (somente fase de grupos)
    public string Data; // dd/MM/yyyy
    public int IdEstadio;
    public int IdTimeA;
    public int IdTimeB;
    public int GolsA;
    public int GolsB;
    public bool Realizado;
    public int IdVencedorPenaltis; // 0 = nenhum
    public bool Ativo;
}
