using UnityEngine;

public class ItemColetavel : MonoBehaviour, IInteractable
{
    public void Active()
    {
        // 1. O Pickup_Manager é necessario para este script funcionar
        if (Pickup_Manager.Instance == null)
        {
            Debug.LogError("Pickup_Manager não encontrado na cena! O item não pode ser coletado.");
            return;
        }

        // 2. O WireManager é OPCIONAL
        // Verificamos se ele existe E se o player está segurando um fio
        if (WireManager.Instance != null && WireManager.Instance.carregandoFio)
        {
            Debug.Log("Mãos ocupadas com o fio!");
            return;
        }

        // 3. Lógica de coleta normal
        if (!Pickup_Manager.Instance.estaCarregandoItem)
        {
            Pickup_Manager.Instance.SegurarItem(this.gameObject);
        }
    }
}