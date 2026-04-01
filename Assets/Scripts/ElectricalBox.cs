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

        // Acessando via Singleton em vez de Find
        WireManager playerWire = WireManager.Instance;

        // Verificação de segurança caso o Player não exista na cena
        if (playerWire == null) return;

        if (tipo == TipoCaixa.Origem && !playerWire.carregandoFio)
        {
            playerWire.IniciarConexao(metrosIniciais, transform.position);
            jaUsada = true;
            Debug.Log("Fio coletado!");
        }
        else if (tipo == TipoCaixa.Destino && playerWire.carregandoFio)
        {
            // O cálculo de fioDisponivel agora será preciso com as quinas
            if (playerWire.fioDisponivel > 0)
            {
                playerWire.FinalizarConexao();
                jaUsada = true;
                Debug.Log("Conexão estabelecida!");
            }
            else
            {
                Debug.Log("Fio curto demais!");
            }
        }
    }
}