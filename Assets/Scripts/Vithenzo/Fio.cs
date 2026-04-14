using UnityEngine;
using UnityEngine.UI;

public class Fio : MonoBehaviour
{
    public int idDaCor; // 0: Vermelho, 1: Verde, etc.
    public Gerenciador_Puzzle gerenciador;

    public void AoClicar()
    {
        // Avisa ao gerente que este fio foi pressionado
        gerenciador.RegistrarClique(idDaCor);
    }
}