using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

[System.Serializable]
public struct WireColorData
{
    public string nomeCor; // Ex: "Vermelho", "Azul"
    public Color cor;      // Cor visual no Inspector
}

public class WireTaskManager : MonoBehaviour
{
    [Header("Referência ao TextMeshPro")]
    public TextMeshProUGUI textoOrdem;

    [Header("Nós/Terminais de Fio")]
    public List<WireNode> nosIniciais; // Ex: Os 3 quadrados de cima/esquerda
    public List<WireNode> nosFinais;   // Ex: Os 3 quadrados de baixo/direita

    [Header("6 Cores Possíveis")]
    public List<WireColorData> coresDisponiveis = new List<WireColorData>();

    private int fiosConectados = 0;

    void Start()
    {
        GerarNovaTarefa();
    }

    public void GerarNovaTarefa()
    {
        fiosConectados = 0;

        // 1. Garante que temos pelo menos a quantidade de cores necessária no minigame
        int quantidadePares = Mathf.Min(nosIniciais.Count, nosFinais.Count);

        // 2. Embaralha as 6 cores e escolhe apenas a quantidade de pares necessária (ex: 3)
        List<WireColorData> coresSorteadas = coresDisponiveis
            .OrderBy(x => Random.value)
            .Take(quantidadePares)
            .ToList();

        // 3. Embaralha a ordem para os nós iniciais e finais independentemente
        List<WireColorData> ordemIniciais = coresSorteadas.OrderBy(x => Random.value).ToList();
        List<WireColorData> ordemFinais = coresSorteadas.OrderBy(x => Random.value).ToList();

        // 4. Aplica as cores aos nós
        for (int i = 0; i < quantidadePares; i++)
        {
            nosIniciais[i].ConfigurarNo(ordemIniciais[i], this, true);
            nosFinais[i].ConfigurarNo(ordemFinais[i], this, false);
        }

        // 5. Atualiza o TextMeshPro com a ordem correta sugerida (ordem dos nós iniciais)
        AtualizarTextoInstrucao(ordemIniciais);
    }

    private void AtualizarTextoInstrucao(List<WireColorData> ordem)
    {
        if (textoOrdem == null) return;

        string texto = "<b>ORDEM DOS FIOS:</b>\n";
        for (int i = 0; i < ordem.Count; i++)
        {
            // Adiciona o nome da cor na lista
            texto += $"{(i + 1)}. {ordem[i].nomeCor}\n";
        }

        textoOrdem.text = texto;
    }

    public void RegistrarConexao()
    {
        fiosConectados++;
        if (fiosConectados >= nosIniciais.Count)
        {
            Debug.Log("Painel de ignição concluído com sucesso!");
            if (textoOrdem != null)
            {
                textoOrdem.text = "<color=green>SISTEMA ATIVADO!</color>";
            }
            // Aqui você pode chamar seu evento de vitória ou ativar a ignição do jogo
        }
    }
}