using UnityEngine;

public class Points_Maneger : MonoBehaviour
{
    public static Points_Maneger Instance { get; private set; }

    [Header("Configurações de Pontos")]
    public float pontosAtuais = 50f;

    [Header("Penalidades")]
    public float penalidadeMorte = 20f;

    // Valores de recompensa baseados na dificuldade
    public float pontosFacil = 5f;
    public float pontosMedia = 15f;
    public float pontosDificil = 30f;
    public float penalidadeFalha = 22f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AdicionarPontos(DialogueData.Dificuldade dificuldade)
    {
        float pontosParaAdicionar = 0;

        switch (dificuldade)
        {
            case DialogueData.Dificuldade.Facil: pontosParaAdicionar = pontosFacil; break;
            case DialogueData.Dificuldade.Media: pontosParaAdicionar = pontosMedia; break;
            case DialogueData.Dificuldade.Dificil: pontosParaAdicionar = pontosDificil; break;
        }

        pontosAtuais += pontosParaAdicionar;
        AtualizarInterface();
    }

    public void RemoverPontos()
    {
        pontosAtuais -= penalidadeFalha;
        AtualizarInterface();
    }

    public void PlayerMorreu()
    {
        pontosAtuais -= penalidadeMorte;
        AtualizarInterface();

        Debug.Log("Player morreu! Penalidade aplicada.");
    }

    private void AtualizarInterface()
    {
        // Procura o AcceptanceManager na cena atual e atualiza a barra
        // Isso é necessário porque o AcceptanceManager da SimpleScene 
        // é diferente do AcceptanceManager do MiniGame
        AcceptanceManager am = FindFirstObjectByType<AcceptanceManager>();
        if (am != null)
        {
            am.UpdateAcceptance(pontosAtuais);
        }

        Debug.Log("Pontuação Total: " + pontosAtuais);
    }
}
