using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class WireTaskManager : MonoBehaviour
{
    [Header("Referência ao TextMeshPro")]
    public TextMeshProUGUI textoOrdem;

    [Header("Nós/Terminais de Fio")]
    public List<WireNode> nosIniciais; // Fusíveis de Entrada (grupo superior)
    public List<WireNode> nosFinais;   // Terminais de Saída (grupo inferior)

    [Header("6 Cores Possíveis")]
    public List<WireColorData> coresDisponiveis = new List<WireColorData>();

    private int fiosConectados = 0;
    private WireNode noInicialSelecionado = null;

    void OnEnable()
    {
        GerarNovaTarefa();
    }

    public void GerarNovaTarefa()
    {
        fiosConectados = 0;
        noInicialSelecionado = null;

        int quantidadePares = Mathf.Min(nosIniciais.Count, nosFinais.Count);

        // Sorteia cores sem repetição
        List<WireColorData> coresSorteadas = coresDisponiveis
            .OrderBy(x => Random.value)
            .Take(quantidadePares)
            .ToList();

        List<WireColorData> ordemIniciais = coresSorteadas.OrderBy(x => Random.value).ToList();
        List<WireColorData> ordemFinais = coresSorteadas.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < quantidadePares; i++)
        {
            nosIniciais[i].ConfigurarNo(ordemIniciais[i], this, true);
            nosFinais[i].ConfigurarNo(ordemFinais[i], this, false);
        }

        AtualizarTextoInstrucao(ordemIniciais);
    }

    private void AtualizarTextoInstrucao(List<WireColorData> ordem)
    {
        if (textoOrdem == null) return;

        string texto = "<b>ORDEM DOS FIOS:</b>\n";
        for (int i = 0; i < ordem.Count; i++)
        {
            texto += $"{(i + 1)}. {ordem[i].nomeCor}\n";
        }

        textoOrdem.text = texto;
    }

    public void SelecionarNoInicial(WireNode no)
    {
        // Se já havia outro fusível erguido que não foi conectado, abaixa ele antes de erguer o novo
        if (noInicialSelecionado != null && noInicialSelecionado != no && !noInicialSelecionado.conectado)
        {
            noInicialSelecionado.ResetarRotacao();
        }

        noInicialSelecionado = no;
    }

    public void TentarConectar(WireNode noDestino)
    {
        if (noInicialSelecionado == null) return;

        // Verifica se as cores correspondem
        if (noDestino.corAtual.nomeCor == noInicialSelecionado.corAtual.nomeCor)
        {
            // --- ACERTOU! ---
            // O fusível se mantém erguido e a conexão é confirmada
            noInicialSelecionado.ConfirmarConexao();
            noDestino.ConfirmarConexao();
            noInicialSelecionado = null;
            RegistrarConexao();
        }
        else
        {
            // --- ERROU! ---
            // O fusível erguido volta a ficar deitado (reseta rotação)
            noInicialSelecionado.ResetarRotacao();
            noInicialSelecionado = null;
        }
    }

    public void RegistrarConexao()
    {
        fiosConectados++;
        if (fiosConectados >= nosIniciais.Count)
        {
            Debug.Log("Minigame concluído!");
            if (textoOrdem != null)
            {
                textoOrdem.text = "<color=green>SISTEMA ATIVADO!</color>";
            }
        }
    }
}