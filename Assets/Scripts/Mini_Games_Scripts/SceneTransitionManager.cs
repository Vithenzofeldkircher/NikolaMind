using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    // Armazena o nome da cena anterior
    public string NomeCenaAnterior { get; private set; }

    private void Awake()
    {
        // Garante que exista apenas uma instância deste gerenciador (Singleton)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Impede que o objeto seja destruído na troca de cena
    }

    // Salva a cena atual e carrega a cena do minigame
    public void IrParaMinigame(string nomeCenaMinigame)
    {
        NomeCenaAnterior = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(nomeCenaMinigame);
    }

    // Volta para a última cena gravada
    public void VoltarParaCenaAnterior()
    {
        if (!string.IsNullOrEmpty(NomeCenaAnterior))
        {
            SceneManager.LoadScene(NomeCenaAnterior);
        }
        else
        {
            Debug.LogWarning("Nenhuma cena anterior foi gravada!");
        }
    }
}