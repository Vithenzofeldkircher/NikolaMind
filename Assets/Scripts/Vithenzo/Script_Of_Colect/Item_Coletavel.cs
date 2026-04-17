using UnityEngine;

public class ItemColetavel : MonoBehaviour, IInteractable
{
    public void Active()
    {
        // Só pega se o player não estiver com o fio OU outro item
        if (WireManager.Instance.carregandoFio)
        {
            Debug.Log("Mãos ocupadas com o fio!");
            return;
        }

        if (!Pickup_Manager.Instance.estaCarregandoItem)
        {
            Pickup_Manager.Instance.SegurarItem(this.gameObject);
        }
    }
}