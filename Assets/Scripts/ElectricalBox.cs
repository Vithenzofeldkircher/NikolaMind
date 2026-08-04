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
        if (Pickup_Manager.Instance != null && Pickup_Manager.Instance.estaCarregandoItem)
        {
            Debug.Log("Mãos ocupadas com um item!");
            return;
        }

        WireManager playerWire = WireManager.Instance;
        if (playerWire == null) return;

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
                Debug.Log("Fio retirado da origem.");
            }
        }
        else if (tipo == TipoCaixa.Destino && playerWire.carregandoFio)
        {
            WirePhysics physics = playerWire.GetComponent<WirePhysics>();
            float distanciaGasta = physics.CalcularDistanciaTotal();
            float fioRestante = playerWire.fioMaximo - distanciaGasta;

            if (fioRestante >= 0)
            {
                // Inversão de Dependência & OCP: Pergunta ao Mission_Pass se o cenário está pronto
                if (Mission_Pass.Instance != null && Mission_Pass.Instance.ValidarRequisitosDeVitoria())
                {
                    playerWire.FinalizarConexao(transform.position);
                    jaUsada = true;
                    Debug.Log("Conexão finalizada com sucesso!");

                    // Dispara a mudança de cena
                    Mission_Pass.Instance.AtivarVitoria();
                }
                else
                {
                    Debug.Log("Você alcançou o destino, mas o fio precisa contornar TODAS as caixas obrigatórias antes!");
                }
            }
            else
            {
                Debug.Log("Fio curto demais para alcançar esta caixa!");
            }
        }
    }
}