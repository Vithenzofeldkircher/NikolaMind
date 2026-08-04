using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Dialogue_System : MonoBehaviour
{
    [Header("Referências UI")]
    public TMP_Text dialogueText;
    public TMP_Text nomeText;
    public GameObject painelDialogo;

    [Header("Configurações")]
    public float typingSpeed = 0.03f;
    public string botaoAvancar = "Submit";

    [Header("Referências de Quest")]
    public AcceptanceManager acceptanceManager; // Arraste seu script da barra aqui
    public GameObject botoesEscolha; // O objeto pai dos botões Sim/Não

    private DialogueData currentData;
    private int currentLine = 0;
    private bool isTyping = false;
    private bool dialogoAtivo = false;

    [Header("Dicas Visuais")]
    public GameObject avisoContinuar;

    void Start() => painelDialogo.SetActive(false);

    void Update()
    {
        if (!dialogoAtivo) return;

        if (Input.GetButtonDown(botaoAvancar))
        {
            if (isTyping) CompletarTexto();
            else AvancarFala();
        }
    }

    public void IniciarDialogo(DialogueData data)
    {
        currentData = data;
        currentLine = 0;
        dialogoAtivo = true;
        painelDialogo.SetActive(true);
        MostrarFala();
        Debug.Log("O diálogo começou com: " + data.name);
    }

    public void FecharDialogoManualmente()
    {
        if (dialogoAtivo)
        {
            Finalizar();
        }
    }

    private void MostrarFala()
    {
        StopAllCoroutines();
        nomeText.text = currentData.falas[currentLine].nomePersonagem;
        StartCoroutine(TypeLine(currentData.falas[currentLine].texto));
    }

    private IEnumerator TypeLine(string line)
    {
        avisoContinuar.SetActive(false);
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
        avisoContinuar.SetActive(true);
    }

    private void CompletarTexto()
    {
        StopAllCoroutines();
        dialogueText.text = currentData.falas[currentLine].texto;
        isTyping = false;
        avisoContinuar.SetActive(true);
    }

    private void AvancarFala()
    {
        currentLine++;
        if (currentLine < currentData.falas.Count)
        {
            MostrarFala();
        }
        else
        {
            // Se for fim de quest, mostra os botões em vez de fechar direto
            if (currentData.ehFimDeQuest)
            {
                MostrarBotoesEscolha();
            }
            else
            {
                Finalizar();
            }
        }
    }

    private void MostrarBotoesEscolha()
    {
        isTyping = false;
        dialogoAtivo = false; // Trava o avanço pelo teclado
        botoesEscolha.SetActive(true);
    }

    public void ResponderSucesso()
    {
        // Chama o ScoreManager global passando a dificuldade dos dados atuais
        if (Points_Maneger.Instance != null)
        {
            Points_Maneger.Instance.AdicionarPontos(currentData.dificuldade);
        }

        LimparEFinalizar();
    }

    public void ResponderFalha()
    {
        // Chama o ScoreManager global para aplicar a penalidade
        if (Points_Maneger.Instance != null)
        {
            Points_Maneger.Instance.RemoverPontos();
        }

        LimparEFinalizar();
    }

    private void LimparEFinalizar()
    {
        botoesEscolha.SetActive(false);
        Finalizar();
    }

    private void Finalizar()
    {
        dialogoAtivo = false;
        painelDialogo.SetActive(false);
        currentLine = 0; // Reseta para o próximo diálogo
        avisoContinuar.SetActive(false);
    }

    public bool EstaEmDialogo() => dialogoAtivo;
}