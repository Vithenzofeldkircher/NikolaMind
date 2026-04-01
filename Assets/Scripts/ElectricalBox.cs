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

        // Acessando via Singleton
        WireManager playerWire = WireManager.Instance;

        if (playerWire == null) return;

        if (tipo == TipoCaixa.Origem && !playerWire.carregandoFio)
        {
            // Passa a quantidade de fio e a posição desta caixa para o LineRenderer começar daqui
            playerWire.IniciarConexao(metrosIniciais, transform.position);
            jaUsada = true;
            Debug.Log("Fio conectado na origem!");
        }
        else if (tipo == TipoCaixa.Destino && playerWire.carregandoFio)
        {
            // usa 'fioAtual' que é o cálculo real com as curvas/quinas
            if (playerWire.fioAtual > 0)
            {
                playerWire.FinalizarConexao();
                jaUsada = true;
                Debug.Log("Conexão estabelecida com sucesso!");
            }
            else
            {
                
                Debug.Log("O fio não alcança!");
            }
        }
    }
}