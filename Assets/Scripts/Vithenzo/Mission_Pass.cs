using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Mission_Pass : MonoBehaviour
{
    public static Mission_Pass Instance;

    [Header("Configuração de Transição")]
    [SerializeField] private string nomeDaProximaCena;

    // Lista para monitorar todas as caixas de enrolar do cenário
    private List<IInteragivelFio> caixasDeEnrolarNoCenario = new List<IInteragivelFio>();

    private void Awake()
    {
        if (Instance == null) Instance = this;

        // SRP: Busca e faz o cache de todas as caixas interativas do mapa automaticamente
        BuscarCaixasNoCenario();
    }

    private void BuscarCaixasNoCenario()
    {
        // Encontra todos os MonoBehaviours e filtra os que assinam o contrato de interface
        MonoBehaviour[] todosScripts = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (MonoBehaviour script in todosScripts)
        {
            if (script is IInteragivelFio caixa)
            {
                caixasDeEnrolarNoCenario.Add(caixa);
            }
        }
    }


    public bool ValidarRequisitosDeVitoria()
    {
        if (caixasDeEnrolarNoCenario.Count == 0) return true;

        foreach (IInteragivelFio caixa in caixasDeEnrolarNoCenario)
        {
            // Se pelo menos uma caixa do mapa não estiver com o fio enrolado, impede a vitória
            if (!caixa.EstaComFio)
            {
                Debug.Log($"Missão pendente: {((MonoBehaviour)caixa).name} ainda não foi envolvida pelo fio.");
                return false;
            }
        }

        return true;
    }


    public void AtivarVitoria()
    {
        Debug.Log("Condições aceitas! Carregando próxima fase...");
        
        if (CursorManager.Instance != null)
            CursorManager.Instance.UnlockCursor();

        if (!string.IsNullOrEmpty(nomeDaProximaCena))
        {
            SceneManager.LoadScene(nomeDaProximaCena);
        }
        else
        {
            Debug.LogError("[Mission_Pass] O nome da próxima cena não foi definido no Inspector!");
        }
    }
}