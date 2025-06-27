using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SIMDRPG
{
    public class PersonagemVetorizado
    {
        public int[] Ataque;
        public int[] Defesa;
        public int[] ehCritico;
        public int[] MultCritico;   // multiplicador de crítico (ex: 200 = 2x)
        public int[] Vida;
        public int[] Vivo;
        public int tamanho;


        public PersonagemVetorizado(Personagem[] exercito)
        {
            tamanho = exercito.Length;
            Ataque = new int[exercito.Length];
            Defesa = new int[exercito.Length];
            ehCritico = new int[exercito.Length];
            MultCritico = new int[exercito.Length];
            Vida = new int[exercito.Length];
            Vivo = new int[exercito.Length];

            for (int i = 0; i < exercito.Length; i++)
            {
                Ataque[i] = exercito[i].Ataque;
                Defesa[i] = exercito[i].Defesa;
                ehCritico[i] = exercito[i].ehCritico;
                MultCritico[i] = exercito[i].MultCritico;
                Vida[i] = exercito[i].Vida;

                Vivo[i] = exercito[i].Vivo ? -1 : 0;
            }

        }

    }
}
