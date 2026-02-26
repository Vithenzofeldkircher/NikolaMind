using UnityEngine;

public class Interaction : MonoBehaviour
{
    [Header("NPC's")]
    public float Distance_Of_Interation = 3f;
    
    private IInteractable Interactable_Target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Interactable_Target != null && Input.GetButtonDown("Fire1"))
        {
            Interactable_Target.Interagir();//chama o metedo Active() quando o player presiona "f"
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            Interactable_Target = interactable;
            Debug.Log("Jogador entrou na área de interação."); // aqui vamos utilizar o Interactable para confirmar a entrada do player
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IInteractable>(out IInteractable interactable) && interactable == Interactable_Target)
        {
            Interactable_Target = null;
            Debug.Log("Jogador saiu da área de interação.");
        }
    }

    
}
