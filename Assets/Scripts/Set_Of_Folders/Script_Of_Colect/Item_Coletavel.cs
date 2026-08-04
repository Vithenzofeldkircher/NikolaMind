using UnityEngine;

public class ItemColetavel : MonoBehaviour, IInteractable
{
    private Wire_Extender extensorFio;

    void Awake()
    {
        // Cache da referência do extensor caso ele resida no mesmo GameObject
        extensorFio = GetComponent<Wire_Extender>();
    }

    public void Active()
    {
        // 1. Prioridade do Sistema de Cabos: Se este item possuir um extensor e o player estiver 
        // operando um fio, a interação de inventário é abortada e repassada para o comportamento do cabo.
        if (WireManager.Instance != null && WireManager.Instance.carregandoFio)
        {
            if (extensorFio != null)
            {
                extensorFio.InteragirComFio();
                return; // Corta a execução para não disparar a coleta física de inventário
            }

            Debug.Log("Mãos ocupadas com o fio! Não é possível coletar itens comuns.");
            return;
        }

        // 2. Se o extensor estiver ativo no cenário, impede que o jogador o pegue do chão com as mãos
        if (extensorFio != null && extensorFio.EstáAtivo())
        {
            Debug.Log("O objeto está conectado à rede de fios e não pode ser movido.");
            return;
        }

        // 3. Verificação de segurança padrão do Pickup Manager
        if (Pickup_Manager.Instance == null)
        {
            Debug.LogError("Pickup_Manager não encontrado na cena! O item não pode ser coletado.");
            return;
        }

        // 4. Lógica de limitação de inventário (Braço Único)
        if (Pickup_Manager.Instance.estaCarregandoItem)
        {
            Debug.Log("Mãos ocupadas com outro item!");
            return;
        }

        // 5. Executa a rotina de coleta física
        Pickup_Manager.Instance.SegurarItem(this.gameObject);
    }
}