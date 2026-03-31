using UnityEngine;

public class ElectricalBox : MonoBehaviour, IInteractable
{
    public enum TipoCaixa { Origem, Destino }

    [Header("Configurações")]
    public TipoCaixa tipo;
    public float metrosIniciais = 10f;
    private bool jaUsada = false;

    public void Active()
    {
        if (jaUsada) return;

        // Tenta encontrar o gerenciador no Player
        WireManager playerWire = FindObjectOfType<WireManager>();

        if (tipo == TipoCaixa.Origem && !playerWire.carregandoFio)
        {
            playerWire.IniciarConexao(metrosIniciais);
            jaUsada = true;
            Debug.Log("Fio coletado! Leve até o destino.");
        }
        else if (tipo == TipoCaixa.Destino && playerWire.carregandoFio)
        {
            if (playerWire.fioDisponivel > 0)
            {
                playerWire.FinalizarConexao();
                jaUsada = true;
                Debug.Log("Conexão elétrica estabelecida!");
            }
            else
            {
                Debug.Log("O fio acabou antes de chegar aqui!");
            }
        }
    }
}