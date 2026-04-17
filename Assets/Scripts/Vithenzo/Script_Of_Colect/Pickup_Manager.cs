using UnityEngine;

public class Pickup_Manager : MonoBehaviour
{


    public static Pickup_Manager Instance;


    public bool estaCarregandoItem => itemAtual != null;


    public Transform pontoDeSegurar;


    private GameObject itemAtual;

    private void Awake() => Instance = this;



    void Update()
    {
        // Largar item com G
        if (estaCarregandoItem && Input.GetButtonDown("Largar"))
        {
            LargarItem();
        }
    }



    public void SegurarItem(GameObject item)
    {
        itemAtual = item;
        itemAtual.transform.SetParent(pontoDeSegurar);
        itemAtual.transform.localPosition = Vector3.zero;

        // Em vez de collider.enabled = false, apenas garantimos a Layer
        itemAtual.layer = LayerMask.NameToLayer("Item");

        // Se o item tiver um Rigidbody2D, precisamos deixá-lo cinemático
        // para ele não cair da mão por causa da gravidade
        if (itemAtual.TryGetComponent(out Rigidbody2D rb))
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero; // Para ele parar de se mexer
        }

        if (itemAtual.TryGetComponent(out MostrarE visual)) visual.Hide();
    }



    public void LargarItem()
    {
        if (itemAtual == null) return;

        itemAtual.transform.SetParent(null);

        if (itemAtual.TryGetComponent(out Rigidbody2D rb))
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // Volta a cair com a gravidade
        }

        itemAtual = null;
    }


}