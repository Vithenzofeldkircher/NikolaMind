using UnityEngine;

public class Mission_Pass : MonoBehaviour
{
    [Header("Configuração de UI")]
    [SerializeField] private GameObject painelVitoria;

    private bool jaGanhou = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(painelVitoria != null)
            painelVitoria.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!jaGanhou && WireManager.Instance != null && WireManager.Instance != this)
            return;
        mostar_vitoria();
    }

    void mostar_vitoria()
    {
        jaGanhou = true;
        painelVitoria.SetActive(true);


        Time.timeScale = 0f;
    }
}
