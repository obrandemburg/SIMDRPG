using SIMDRPG;
using System.Numerics;

public class SimuladorCombate
{
    private static Random gerador = new Random(42);
    const int tamanhoExercito = 10_000_000;


    static int[] vetorEhCritico = new int[tamanhoExercito];

    public static void PreGerarResultadosCriticos()
    {
        Console.WriteLine("Pré-gerando resultados de acertos críticos...");
        for (int i = 0; i < tamanhoExercito; i++)
        {
            vetorEhCritico[i] = gerador.Next(0, 100);
        }
        Console.WriteLine("Resultados gerados.");
    }


    public static long SimularRodadaCombateSIMD(PersonagemVetorizado atacantes, PersonagemVetorizado defensores)
    {
        long danoTotalAcumulado = 0;
        int i = 0;
        int bloco = Vector<int>.Count;
        Vector<int> cem = new Vector<int>(100);
        Vector<int> vBase = new Vector<int>(0);


        for (; i <= atacantes.Ataque.Length - bloco; i += bloco)
        {
            var vAtaque = new Vector<int>(atacantes.Ataque, i);
            var vChanceCritico = new Vector<int>(atacantes.ChanceCritico, i);
            var vDefesa = new Vector<int>(defensores.Defesa, i);
            var vMultCritico = new Vector<int>(atacantes.MultCritico, i);
            var vNumerosAleatorios = new Vector<int>(vetorEhCritico, i);

            var vDanoBase = Vector.Max(Vector<int>.One, vAtaque - vDefesa);
            var vMascaraCritico = Vector.LessThan(vNumerosAleatorios, vChanceCritico);
            var vDanoCritico = Vector.Divide(Vector.Multiply(vDanoBase, vMultCritico), cem);
            var vDanoFinal = Vector.ConditionalSelect(vMascaraCritico, vDanoCritico, vDanoBase);

            vBase += vDanoFinal;

        }
        danoTotalAcumulado += Vector.Sum(vBase);

        for (; i < atacantes.Ataque.Length; i++)
        {
            int danoBase = Math.Max(1, atacantes.Ataque[i] - defensores.Defesa[i]);
            if (vetorEhCritico[i] < atacantes.ChanceCritico[i])
            {
                danoBase = (danoBase * atacantes.MultCritico[i]) / 100;
            }
            danoTotalAcumulado += danoBase;
        }

        return danoTotalAcumulado;
    }

    public static long SimularRodadaCombate(Personagem[] atacantes, Personagem[] defensores)
    {
        long danoTotal = 0;

        // BUG 1 CORRIGIDO: A lógica de cálculo foi movida para cá para usar o índice 'i' diretamente,
        // garantindo consistência com a versão SIMD.
        for (int i = 0; i < atacantes.Length && i < defensores.Length; i++)
        {
            int danoBase = Math.Max(1, atacantes[i].Ataque - defensores[i].Defesa);

            // BUG 2 CORRIGIDO: A lógica de comparação agora é idêntica à do SIMD
            if (vetorEhCritico[i] < atacantes[i].ChanceCritico)
            {
                danoBase = (danoBase * atacantes[i].MultCritico) / 100;
            }
            danoTotal += danoBase;
        }

        return danoTotal;
    }

    public static Personagem[] GerarExercito(int tamanho, string tipo)
    {
        Personagem[] exercito = new Personagem[tamanho];
        for (int i = 0; i < tamanho; i++)
        {
            if (tipo == "atacante")
            {
                exercito[i] = new Personagem
                {
                    Ataque = gerador.Next(80, 120),
                    Defesa = gerador.Next(20, 40),
                    ChanceCritico = gerador.Next(15, 25),
                    MultCritico = gerador.Next(180, 220),
                    Vida = gerador.Next(100, 150),
                };
            }
            else // defensor
            {
                exercito[i] = new Personagem
                {
                    Ataque = gerador.Next(60, 80),
                    Defesa = gerador.Next(40, 70),
                    ChanceCritico = gerador.Next(10, 20),
                    MultCritico = gerador.Next(150, 200),
                    Vida = gerador.Next(120, 180),
                };
            }
        }
        return exercito;
    }
}