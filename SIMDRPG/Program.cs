using System.Diagnostics;

class Program
{
    static void Main()
    {
        const int tamanhoExercito = 500_000; // Meio milhão de personagens!

        Personagem[] atacantes = SimuladorCombate.GerarExercito(tamanhoExercito, "atacante");
        Personagem[] defensores = SimuladorCombate.GerarExercito(tamanhoExercito, "defensor");

        Console.WriteLine("=== SIMULAÇÃO DE BATALHA ÉPICA ===");
        Console.WriteLine($"Exércitos: {tamanhoExercito:N0} vs {tamanhoExercito:N0}");

        Stopwatch cronometro = Stopwatch.StartNew();
        int danoTotalRodada = SimuladorCombate.SimularRodadaCombate(atacantes, defensores);
        cronometro.Stop();

        Console.WriteLine($"Dano total causado: {danoTotalRodada:N0}");
        Console.WriteLine($"Tempo sem SIMD: {cronometro.ElapsedMilliseconds}ms");
        Console.WriteLine($"DPS (danos por segundo): {danoTotalRodada * 1000 / Math.Max(1, cronometro.ElapsedMilliseconds):N0}");
    }
}