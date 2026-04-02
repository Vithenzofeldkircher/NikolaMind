using UnityEngine;

public class Mission_Pass : MonoBehaviour
{
    // Singleton para facilitar o acesso de outros scripts
    public static Mission_Pass Instance;

    [Header("Configuração de UI")]
    [SerializeField] private GameObject painelVitoria;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        // Garante que comece desativado
        if (painelVitoria != null)
            painelVitoria.SetActive(false);
    }

    // Esta é a função que o WireManager vai chamar
    public void AtivarVitoria()
    {
        if (painelVitoria != null)
        {
            painelVitoria.SetActive(true);
            Debug.Log("Vitória! Painel ativado.");

             Time.timeScale = 0f; 
        }
    }
}