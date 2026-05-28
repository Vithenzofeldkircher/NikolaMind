using UnityEngine;

public class Wire_Extender : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float bonusMetros = 5f;
    private bool jaAtivado = false;

    //fazer um a mecanica onde o objeto aumenta o numero do fio porém que não seja necessario enrolar o fio no proprio objeto e sim apenas interagir e conectar ele.

    // Referência ao script de coleta original (opcional, para desativar a coleta manual)
    private ItemColetavel itemColetavel;

    void Awake()
    {
        itemColetavel = GetComponent<ItemColetavel>();
    }

    public void AtivarExtensao()
    {
        if (jaAtivado) return;

        jaAtivado = true;

        // Aumenta o fio no Manager
        if (WireManager.Instance != null)
        {
            WireManager.Instance.fioMaximo += bonusMetros;
            Debug.Log($"Fio aumentado em {bonusMetros}m!");
        }

        // Bloqueia a interação de pegar o item (ItemColetavel)
        if (itemColetavel != null)
        {
            itemColetavel.enabled = false;
        }
    }
}