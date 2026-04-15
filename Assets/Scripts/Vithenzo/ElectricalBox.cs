using UnityEngine;

public class ElectricalBox : MonoBehaviour, IInteractable
{
    public enum TipoCaixa { Origem, Destino }

    [Header("Configurações")]
    public TipoCaixa tipo;
    public float metrosIniciais = 10f;
    public bool jaUsada = false;

    public void Active() // Este método é chamado pelo seu sistema de Interação
    {
        WireManager playerWire = WireManager.Instance;
        if (playerWire == null) return;

        // Se a missão já foi concluída, ninguém mais interage
        if (playerWire.missaoConcluida)
        {
            Debug.Log("O sistema já está energizado.");
            return;
        }

        if (tipo == TipoCaixa.Origem)
        {
            if (!playerWire.carregandoFio)
            {
                // Inicia a conexão passando a metragem e a posição desta caixa
                playerWire.IniciarConexao(metrosIniciais, transform.position);
                Debug.Log("Fio retirado da origem.");
            }
        }
        else if (tipo == TipoCaixa.Destino && playerWire.carregandoFio)
        {
            // ACESSO AO RENDERER: Precisamos calcular quanto fio sobra
            WireRenderer renderer = playerWire.GetComponent<WireRenderer>();
            float distanciaGasta = renderer.CalcularDistanciaTotal();
            float fioRestante = playerWire.fioMaximo - distanciaGasta;

            // Se o fio restante for maior que zero (ou uma margem pequena de erro)
            if (fioRestante >= 0)
            {
                playerWire.FinalizarConexao(transform.position);
                jaUsada = true;
                Debug.Log("Conexão finalizada com sucesso!");
            }
            else
            {
                Debug.Log("Fio curto demais para alcançar esta caixa!");
            }
        }
    }
}