using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class WireTaskManager : MonoBehaviour
{
    [Header("Referência ao TextMeshPro")]
    public TextMeshProUGUI textoOrdem;

    [Header("Nós/Terminais de Fio")]
    public List<WireNode> nosIniciais; // Fusíveis do topo
    public List<WireNode> nosFinais;   // Botões de baixo

    [Header("Receitas de Mistura de Cores")]
    [Tooltip("Cadastre aqui as combinações (Ex: Azul + Amarelo = Verde)")]
    public List<ColorCombination> receitasDisponiveis = new List<ColorCombination>();

    private int fiosConectados = 0;
    private WireNode noInicialSelecionado = null;

    private List<ColorCombination> sequenciaExigida = new List<ColorCombination>();
    private int indiceEtapaAtual = 0;

    private void Start()
    {
        GerarNovaTarefa();
    }

    private void OnEnable()
    {
        GerarNovaTarefa();
    }

    public void GerarNovaTarefa()
    {
        fiosConectados = 0;
        indiceEtapaAtual = 0;
        noInicialSelecionado = null;

        if (nosIniciais == null || nosFinais == null || nosIniciais.Count == 0 || nosFinais.Count == 0)
        {
            Debug.LogError("[WireTaskManager] As listas de Nós não estão preenchidas!");
            return;
        }

        if (receitasDisponiveis == null || receitasDisponiveis.Count == 0)
        {
            Debug.LogError("[WireTaskManager] Adicione receitas de cores no Inspector!");
            return;
        }

        int quantidadePares = Mathf.Min(nosIniciais.Count, nosFinais.Count);

        // 1. Sorteia quais receitas serão usadas nesta rodada
        List<ColorCombination> receitasSorteadas = receitasDisponiveis
            .OrderBy(x => Random.value)
            .Take(quantidadePares)
            .ToList();

        // 2. Extrai as cores superiores e inferiores das receitas sorteadas
        List<WireColorData> coresTopo = receitasSorteadas.Select(r => r.corSuperior).OrderBy(x => Random.value).ToList();
        List<WireColorData> coresFundo = receitasSorteadas.Select(r => r.corInferior).OrderBy(x => Random.value).ToList();

        // 3. Aplica as cores embaralhadas nos botões
        for (int i = 0; i < quantidadePares; i++)
        {
            if (nosIniciais[i] != null)
                nosIniciais[i].ConfigurarNo(coresTopo[i], this, true);

            if (nosFinais[i] != null)
                nosFinais[i].ConfigurarNo(coresFundo[i], this, false);
        }

        // 4. Sorteia a ordem em que as cores RESULTADO serão pedidas na lista
        sequenciaExigida = receitasSorteadas.OrderBy(x => Random.value).ToList();

        AtualizarTextoInstrucao();
    }

    private void AtualizarTextoInstrucao()
    {
        if (textoOrdem == null) return;

        string texto = "<b>ORDEM DOS FIOS (MISTURA):</b>\n";
        for (int i = 0; i < sequenciaExigida.Count; i++)
        {
            if (i < indiceEtapaAtual)
            {
                // Etapa concluída
                texto += $"<s>{(i + 1)}. {sequenciaExigida[i].nomeResultado}</s> <color=green>?</color>\n";
            }
            else if (i == indiceEtapaAtual)
            {
                // Etapa atual exigida
                texto += $"<color=yellow>? {(i + 1)}. {sequenciaExigida[i].nomeResultado}</color>\n";
            }
            else
            {
                // Próximas etapas
                texto += $"{(i + 1)}. {sequenciaExigida[i].nomeResultado}\n";
            }
        }

        textoOrdem.text = texto;
    }

    public void SelecionarNoInicial(WireNode no)
    {
        if (noInicialSelecionado != null && noInicialSelecionado != no && !noInicialSelecionado.conectado)
        {
            noInicialSelecionado.ResetarRotacao();
        }

        noInicialSelecionado = no;
    }

    public void TentarConectar(WireNode noDestino)
    {
        if (noInicialSelecionado == null) return;

        // Receita que o jogador precisa formar agora
        ColorCombination receitaEsperada = sequenciaExigida[indiceEtapaAtual];

        // Verifica se a combinação do fusível erguido + botão inferior gera a cor exigida
        bool corTopoBate = (noInicialSelecionado.corAtual.nomeCor == receitaEsperada.corSuperior.nomeCor);
        bool corFundoBate = (noDestino.corAtual.nomeCor == receitaEsperada.corInferior.nomeCor);

        if (corTopoBate && corFundoBate)
        {
            // --- ACERTOU A MISTURA DE CORES! ---
            Debug.Log($"<color=green>ACERTOU!</color> {noInicialSelecionado.corAtual.nomeCor} + {noDestino.corAtual.nomeCor} = {receitaEsperada.nomeResultado}");

            noInicialSelecionado.ConfirmarConexao();
            noDestino.ConfirmarConexao();
            noInicialSelecionado = null;

            indiceEtapaAtual++;
            fiosConectados++;

            AtualizarTextoInstrucao();

            if (fiosConectados >= nosIniciais.Count)
            {
                if (textoOrdem != null)
                {
                    textoOrdem.text = "<color=green>SISTEMA ATIVADO!</color>";
                }
            }
        }
        else
        {
            // --- ERROU A MISTURA! ---
            Debug.Log($"<color=red>MISTURA INCORRETA!</color> A etapa exige a cor '{receitaEsperada.nomeResultado}'.");

            // O fusível erguido se abaixa
            noInicialSelecionado.ResetarRotacao();
            noInicialSelecionado = null;
        }
    }
}