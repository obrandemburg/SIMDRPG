
namespace SIMDRPG
{
    public class PersonagemVetorizado
    {
        public int[] Ataque;
        public int[] Defesa;
        public int[] ChanceCritico; // 0-100
        public int[] MultCritico;   // multiplicador de crítico (ex: 200 = 2x)
        public int[] Vida;
        //public int[] Vivo;

        public PersonagemVetorizado(Personagem[] exercito)
        {
            Ataque = new int[exercito.Length];
            Defesa = new int[exercito.Length];
            ChanceCritico = new int[exercito.Length];
            MultCritico = new int[exercito.Length];
            Vida = new int[exercito.Length];
            //Vivo = new int[exercito.Length];

            //Conversão objeto para vetor
            for (int i = 0; i < exercito.Length; i++)
            {

                Ataque[i] = exercito[i].Ataque;
                Defesa[i] = exercito[i].Defesa;
                ChanceCritico[i] = exercito[i].ChanceCritico;
                MultCritico[i] = exercito[i].MultCritico;
                Vida[i] = exercito[i].Vida;
                //Vivo[i] = exercito[i].Vivo ? -1 : 0;

            }


        }

    }
}
