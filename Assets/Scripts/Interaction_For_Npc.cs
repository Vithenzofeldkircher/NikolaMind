using UnityEngine;

public class Interaction_For_Npc : MonoBehaviour
{
    public float raioInteracao = 1.5f;
    public string botaoInteracao = "Interact"; // Certifique-se que no Inspector esteja escrito Interact
    public LayerMask layerInteragivel;

    void Update()
    {
        // Debug para ver se no console a tecla está sendo apertada
        if (Input.GetButtonDown(botaoInteracao))
        {
            Debug.Log("Botão de interação pressionado!");
            CheckInteracao();
        }
    }

    void CheckInteracao()
    {
        // um círculo invisível na posição do player para detectar o NPC
        Collider2D hit = Physics2D.OverlapCircle(transform.position, raioInteracao, layerInteragivel);

        if (hit != null)
        {
            Debug.Log("Encontrei algo na Layer Interactable: " + hit.name);
            IInteractable interagivel = hit.GetComponent<IInteractable>();

            if (interagivel != null)
            {
                interagivel.Active();
            }
            else
            {
                Debug.LogWarning("O objeto tem a Layer correta, mas falta o script NPC_Interactable!");
            }
        }
        else
        {
            Debug.Log("Nenhum objeto interagível por perto.");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioInteracao);
    }
}