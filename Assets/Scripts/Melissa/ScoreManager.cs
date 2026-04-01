using UnityEngine;
public class ScoreManager : MonoBehaviour
{
   public static int pontos = 0;
   public static void AdicionarPontos(int valor)
   {
       pontos += valor;
   }
   public static void ResetarPontos()
   {
       pontos = 0;
   }
}