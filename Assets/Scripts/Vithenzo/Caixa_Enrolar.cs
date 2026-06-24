using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Caixa_Enrolar : MonoBehaviour, IInteragivelFio
{
    [Header("Configurações de Sprite")]
    [SerializeField] private Sprite spriteNormal;
    [SerializeField] private Sprite spriteMudado;

    private SpriteRenderer spriteRenderer;
    private bool jaTemFio = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Garante que a caixa comece com o sprite padrão
        if (spriteNormal != null && spriteRenderer != null)
            spriteRenderer.sprite = spriteNormal;
    }

    // SOLID (LSP/DIP): Implementação do contrato da interface quando o fio toca a caixa
    public void AoTocarFio()
    {
        if (jaTemFio) return;

        jaTemFio = true;

        if (spriteMudado != null && spriteRenderer != null)
            spriteRenderer.sprite = spriteMudado;

        // Correção do Bug: Adicionado o '$' para a interpolação de string funcionar
        Debug.Log($"Fio enrolado na caixa: {gameObject.name}");
    }

    // SOLID (LSP/DIP): Implementação do contrato da interface quando o fio se afasta
    public void AoSoltarFio()
    {
        if (!jaTemFio) return;

        jaTemFio = false;

        if (spriteNormal != null && spriteRenderer != null)
            spriteRenderer.sprite = spriteNormal;

        Debug.Log($"Fio desenrolado da caixa: {gameObject.name}");
    }
}