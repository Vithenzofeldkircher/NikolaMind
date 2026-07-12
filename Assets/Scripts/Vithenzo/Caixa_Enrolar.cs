using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Caixa_Enrolar : MonoBehaviour, IInteragivelFio
{
    [Header("Configurações de Sprite")]
    [SerializeField] private Sprite spriteNormal;
    [SerializeField] private Sprite spriteMudado;

    private SpriteRenderer spriteRenderer;
    private bool jaTemFio = false;

    // Implementação da propriedade da Interface (DIP / OCP)
    public bool EstaComFio => jaTemFio;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteNormal != null && spriteRenderer != null)
            spriteRenderer.sprite = spriteNormal;
    }

    public void AoTocarFio()
    {
        if (jaTemFio) return;
        jaTemFio = true;

        if (spriteMudado != null && spriteRenderer != null)
            spriteRenderer.sprite = spriteMudado;

        Debug.Log($"Fio enrolado na caixa: {gameObject.name}");
    }

    public void AoSoltarFio()
    {
        if (!jaTemFio) return;
        jaTemFio = false;

        if (spriteNormal != null && spriteRenderer != null)
            spriteRenderer.sprite = spriteNormal;

        Debug.Log($"Fio desenrolado da caixa: {gameObject.name}");
    }
}