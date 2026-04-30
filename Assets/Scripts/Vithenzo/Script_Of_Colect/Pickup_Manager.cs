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

        // MUDA PARA A LAYER QUE NÃO COLIDE COM O PLAYER
        itemAtual.layer = LayerMask.NameToLayer("ItemNaMao");

        if (TryGetComponent(out HandFollow seguidor))
        {
            seguidor.ResetarDirecaoAoPegar();
        }

        if (itemAtual.TryGetComponent(out Rigidbody2D rb))
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.simulated = true;
            rb.linearVelocity = Vector2.zero;
        }

        if (itemAtual.TryGetComponent(out MostrarE visual)) visual.Hide();
    }

    public void LargarItem()
    {
        if (itemAtual == null) return;

        itemAtual.transform.SetParent(null);

        // VOLTA PARA A LAYER QUE COLIDE COM O PLAYER NOVAMENTE
        itemAtual.layer = LayerMask.NameToLayer("ItemNoChao");

        if (itemAtual.TryGetComponent(out Rigidbody2D rb))
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.simulated = true;
        }

        itemAtual = null;
    }
}