using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public string Npc_Name = "NPC";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Active()
    {
        Debug.Log("Voce iniciou o dialogo!"); //ativado quando chamarem o método, aqui podemos criar falas ou missões
    }

}
