using UnityEngine;

public class ElectricalBox : MonoBehaviour, IInteractable
{
    public enum TipoCaixa { Origem, Destino }

    [Header("Configurações")]
    public TipoCaixa tipo;
    public float metrosIniciais = 10f;
    public bool jaUsada = false;

    public void Active()
    {
        WireManager playerWire = WireManager.Instance;
        if (playerWire == null) return;

        // Se a missão já foi concluída, ninguém mais interage com nenhuma caixa
        if (playerWire.missaoConcluida)
        {
            Debug.Log("O sistema já está energizado.");
            return;
        }

        if (tipo == TipoCaixa.Origem)
        {
            if (!playerWire.carregandoFio)
            {
                playerWire.IniciarConexao(metrosIniciais, transform.position);
                Debug.Log("Fio retirado.");
            }
        }
        else if (tipo == TipoCaixa.Destino && playerWire.carregandoFio)
        {
            if (playerWire.fioAtual > 0)
            {
                playerWire.FinalizarConexao(transform.position);
                jaUsada = true; // Trava esta caixa específica também
            }
            else
            {
                Debug.Log("Fio curto demais!");
            }
        }
    }
}