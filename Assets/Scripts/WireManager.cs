using UnityEngine;
using TMPro;

public class WireManager : MonoBehaviour
{
    [Header("Configurações do Fio")]
    public float fioDisponivel = 0f;
    public bool carregandoFio = false;
    private Vector3 ultimaPosicao;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI textoMetros;

    void Update()
    {
        if (carregandoFio && fioDisponivel > 0)
        {
            // Calcula a distância movida desde o último frame
            float distanciaMover = Vector3.Distance(transform.position, ultimaPosicao);
            fioDisponivel -= distanciaMover;

            // Impede que fique negativo
            if (fioDisponivel < 0) fioDisponivel = 0;

            AtualizarUI();
        }
        ultimaPosicao = transform.position;
    }

    public void IniciarConexao(float quantidade)
    {
        fioDisponivel = quantidade;
        carregandoFio = true;
        ultimaPosicao = transform.position;
        AtualizarUI();
    }

    public void FinalizarConexao()
    {
        carregandoFio = false;
        fioDisponivel = 0;
        AtualizarUI();
        Debug.Log("Missão Cumprida!");
    }

    private void AtualizarUI()
    {
        if (textoMetros != null)
        {
            textoMetros.text = carregandoFio ? $"Fio restante: {fioDisponivel:F1}m" : "";
        }
    }
}