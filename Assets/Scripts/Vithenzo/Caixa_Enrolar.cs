using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Caixa_Enrolar : MonoBehaviour, IInteragivelFio
{
    [Header("Configurações de Sprite")]
    [SerializeField] private Sprite spriteNormal;
    [SerializeField] private Sprite spriteMudado;

    private SpriteRenderer spriteRenderer;

    // Trocamos o bool por um contador de pontos de contato
    private int pontosAncorados = 0;

    // A propriedade continua existindo para o Mission_Pass validar, mas agora checa se o contador é maior que 0
    public bool EstaComFio => pontosAncorados > 0;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteNormal != null && spriteRenderer != null)
            spriteRenderer.sprite = spriteNormal;
    }

    public void AoTocarFio()
    {
        // Aumenta o número de dobras do fio presas nesta caixa
        pontosAncorados++;

        // Só troca o sprite se for o PRIMEIRO ponto a encostar na caixa
        if (pontosAncorados == 1)
        {
            if (spriteMudado != null && spriteRenderer != null)
                spriteRenderer.sprite = spriteMudado;

            Debug.Log($"[Caixa] Fio começou a enrolar em: {gameObject.name}");
        }
    }

    public void AoSoltarFio()
    {
        // Prevenção para não deixar o contador ficar negativo por acidente
        if (pontosAncorados > 0)
        {
            pontosAncorados--;
        }

        // Só reverte o sprite se o ÚLTIMO ponto do fio se soltar da caixa
        if (pontosAncorados == 0)
        {
            if (spriteNormal != null && spriteRenderer != null)
                spriteRenderer.sprite = spriteNormal;

            Debug.Log($"[Caixa] Fio se soltou COMPLETAMENTE de: {gameObject.name}");
        }
    }
}