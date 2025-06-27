using System.Diagnostics;
using System.Numerics;

namespace SIMDRPG;

class Program
{
    static void Main()
    {
        bool temSuporteSIMD = Vector.IsHardwareAccelerated;
        Console.WriteLine(temSuporteSIMD);

        const int tamanhoExercito = 500_000; // Meio milhão de personagens!

        Personagem[] atacantes = SimuladorCombate.GerarExercito(tamanhoExercito, "atacante");
        Personagem[] defensores = SimuladorCombate.GerarExercito(tamanhoExercito, "defensor");
        PersonagemVetorizado atacantesVetorizados = new PersonagemVetorizado(atacantes);
        PersonagemVetorizado defensoresVetorizados = new PersonagemVetorizado(defensores);

        Console.WriteLine("=== SIMULAÇÃO DE BATALHA ÉPICA ===");
        Console.WriteLine($"Exércitos: {tamanhoExercito:N0} vs {tamanhoExercito:N0}");

        Stopwatch cronometro = Stopwatch.StartNew();
        int danoTotalRodada = SimuladorCombate.SimularRodadaCombate(atacantes, defensores);
        cronometro.Stop();

        Console.WriteLine($"Dano total causado: {danoTotalRodada:N0}");
        Console.WriteLine($"Tempo sem SIMD: {cronometro.ElapsedMilliseconds}ms");
        Console.WriteLine($"DPS (danos por segundo): {danoTotalRodada * 1000 / Math.Max(1, cronometro.ElapsedMilliseconds):N0}");


        Stopwatch cronometro2 = Stopwatch.StartNew();
        int danoTotalRodadaSIMD = SimuladorCombate.SimularRodadaCombateSIMD(atacantesVetorizados, defensoresVetorizados);
        cronometro2.Stop();

        Console.WriteLine($"Dano total causado: {danoTotalRodadaSIMD:N0}");
        Console.WriteLine($"Tempo com SIMD: {cronometro2.ElapsedMilliseconds}ms");
        Console.WriteLine($"DPS (danos por segundo): {danoTotalRodadaSIMD * 1000 / Math.Max(1, cronometro2.ElapsedMilliseconds):N0}");
    }
}