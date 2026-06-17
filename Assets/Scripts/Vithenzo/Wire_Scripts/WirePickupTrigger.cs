using UnityEngine;

public class WirePickupTrigger : MonoBehaviour
{
    [Header("UI do Prompt")]
    [SerializeField] private GameObject canvasPromptInteracao; // Arraste aqui a imagem/texto do "E"

    private bool jogadorNaArea = false;

    void OnDisable()
    {
        // Garante que se o trigger for desativado via código, o prompt visual suma imediatamente
        if (canvasPromptInteracao != null)
            canvasPromptInteracao.SetActive(false);

        jogadorNaArea = false;
    }

    void Update()
    {
        // Se o jogador estiver perto do fio no chão e apertar o botão de interação
        if (jogadorNaArea && Input.GetButtonDown("Interact")) 
        {
            // Orquestra a coleta de volta através do Singleton unificado
            WireManager.Instance.RetomarFio();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se quem entrou no raio de coleta foi o Player
        if (collision.CompareTag("Player"))
        {
            jogadorNaArea = true;
            if (canvasPromptInteracao != null)
                canvasPromptInteracao.SetActive(true); // Exibe o "E" na tela
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jogadorNaArea = false;
            if (canvasPromptInteracao != null)
                canvasPromptInteracao.SetActive(false); // Esconde o "E" na tela
        }
    }
}