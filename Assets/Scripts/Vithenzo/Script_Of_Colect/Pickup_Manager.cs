using UnityEngine;

public class Pickup_Manager : MonoBehaviour
{
    public static Pickup_Manager Instance;

    public bool Esta_Carregando_Item = true;

    public Transform Ponto_De_Segurar;

    private GameObject itemAtual;

    private void Awake() => Instance = this;
   

    void Update()
    {

        // If que serve para ele largar o item com o G também
        if (Esta_Carregando_Item && Input.GetButtonDown("G"))
        {
            Largar_Item();
        }
    }

    public void Segurar_Item()
    {
        // faz o item seguir o player
        itemAtual.transform.SetParent(Ponto_De_Segurar);

        itemAtual.transform.localPosition = Vector3.zero;

        // Desativa o colisor para não dar erro de física enquanto carrega
        if (itemAtual.TryGetComponent(out Collider2D col)) col.enabled = false;

        if(itemAtual.TryGetComponent(out MostrarE visual)) visual.Hide();
    }


    public void Largar_Item()
    {
        if(itemAtual == null) return;

        itemAtual.transform.SetParent(null);

        if(itemAtual.TryGetComponent(out Collider2D col)) col.enabled = true;

        itemAtual = null;
        print("Item largado");
    }

}
