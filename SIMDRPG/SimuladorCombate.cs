using System.Numerics;

namespace SIMDRPG;

public class SimuladorCombate
{
    private static Random gerador = new Random(42);

    public static int CalcularDano(Personagem atacante, Personagem defensor)
    {
        /*if (!atacante.Vivo || !defensor.Vivo)
            return 0;
        */

        int danoBase = Math.Max(1, atacante.Ataque - defensor.Defesa);

        if (atacante.ehCritico == 1)
        {
            danoBase += (danoBase * atacante.MultCritico) / 100;
        }
        return danoBase;
    }

    public static int CalcularDanoSIMD(
    Vector<int> ataque,
    Vector<int> MultCritico,
    Vector<int> Defesa,
    Vector<int> ehCritico)
    {
        Vector<int> danoBase = Vector.Max(Vector<int>.One, ataque - Defesa);

        // Elimina o uso de Select/Equals: usa multiplicação booleana
        Vector<int> bonusCritico = Vector.Divide(danoBase * MultCritico * ehCritico, 100);
        Vector<int> resultado = danoBase + bonusCritico;

        int soma = 0;
        for (int i = 0; i < Vector<int>.Count; i++)
        {
            soma += resultado[i];
        }

        return soma;
    }


    public static int SimularRodadaCombateSIMD(PersonagemVetorizado atacantes, PersonagemVetorizado defensores)
    {
        int danoTotal = 0;
        int bloco = Vector<int>.Count;
        int inicio = 0;

        for (; inicio <= atacantes.tamanho - bloco; inicio += bloco)
        {
            var vetorAtaque = new Vector<int>(atacantes.Ataque, inicio);
            var vetorDefesa = new Vector<int>(atacantes.Defesa, inicio);
            var vetorCritico = new Vector<int>(atacantes.ehCritico, inicio);
            var vetorMultCritico = new Vector<int>(atacantes.MultCritico, inicio);

            danoTotal += CalcularDanoSIMD(vetorAtaque, vetorMultCritico, vetorDefesa, vetorCritico);
        }

        // Fallback escalar para o "resto"
        for (; inicio < atacantes.tamanho; inicio++)
        {
            int danoBase = Math.Max(1, atacantes.Ataque[inicio] - atacantes.Defesa[inicio]);
            int dano = atacantes.ehCritico[inicio] == 1
                ? (danoBase * atacantes.MultCritico[inicio]) / 100
                : danoBase;

            danoTotal += dano;
        }

        return danoTotal;
    }


    public static int SimularRodadaCombate(Personagem[] atacantes, Personagem[] defensores)
    {
        int danoTotal = 0;

        for (int i = 0; i < atacantes.Length && i < defensores.Length; i++)
        {
            danoTotal += CalcularDano(atacantes[i], defensores[i]);
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
                int ChanceCritico = gerador.Next(15, 25);
                exercito[i] = new Personagem
                {
                    Ataque = gerador.Next(80, 120),
                    Defesa = gerador.Next(20, 40),
                    MultCritico = gerador.Next(180, 220),
                    ehCritico = gerador.Next(0, 100) < ChanceCritico ? 1 : 0,
                    Vida = gerador.Next(100, 150),
                    Vivo = true
                };
            }
            else
            {
                int ChanceCritico = gerador.Next(10, 20);
                exercito[i] = new Personagem
                {
                    Ataque = gerador.Next(60, 80),
                    Defesa = gerador.Next(40, 70),
                    ehCritico = gerador.Next(0, 100) < ChanceCritico ? 1 : 0,
                    MultCritico = gerador.Next(150, 200),
                    Vida = gerador.Next(120, 180),
                    Vivo = true
                };
            }
        }

        return exercito;
    }


}
