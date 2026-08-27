using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para trocar de cena

public class Dialogue_System : MonoBehaviour
{
    [Header("Referências UI")]
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text nomeText;
    [SerializeField] private GameObject painelDialogo;
    [SerializeField] private GameObject avisoContinuar;

    [Header("Áudio")]
    [SerializeField] private AudioSource somDigitacao;
    [SerializeField] private AudioClip[] clipsDigitacao;

    [Header("Configurações de Diálogo")]
    [SerializeField] private float typingSpeed = 0.03f;
    [SerializeField] private float tempoAutoAvanco = 2f; // Tempo para a fala passar sozinha

    [Header("Configurações de Cena")]
    [Tooltip("Escreva o nome exato da cena que será carregada no final")]
    [SerializeField] private string nomeProximaCena;

    private DialogueData currentData;
    private int currentLine = 0;
    private bool isTyping = false;
    private bool dialogoAtivo = false;

    // Controle de Corrotinas (Boas práticas do SOLID: Controle isolado de responsabilidades)
    private Coroutine typingCoroutine;
    private Coroutine autoAdvanceCoroutine;

    void Start()
    {
        painelDialogo.SetActive(false);
    }

    void Update()
    {
        if (!dialogoAtivo) return;

        // Permite pular o diálogo usando a tecla de Espaço
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                CompletarTexto();
            }
            else
            {
                PularParaProximaFala();
            }
        }
    }

    public void IniciarDialogo(DialogueData data)
    {
        currentData = data;
        currentLine = 0;
        dialogoAtivo = true;
        painelDialogo.SetActive(true);
        MostrarFala();
    }

    private void MostrarFala()
    {
        // Para qualquer corrotina que esteja rodando antes de começar uma nova
        PararCorrotinasAtivas();

        nomeText.text = currentData.falas[currentLine].nomePersonagem;
        typingCoroutine = StartCoroutine(TypeLine(currentData.falas[currentLine].texto));
    }

    private IEnumerator TypeLine(string line)
    {
        avisoContinuar.SetActive(false);
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            TocarSomDigitacao(c);
            yield return new WaitForSeconds(typingSpeed);
        }

        FinalizarDigitacao();
    }

    private void TocarSomDigitacao(char c)
    {
        if (somDigitacao != null && clipsDigitacao != null && clipsDigitacao.Length > 0 && c != ' ')
        {
            int indexAleatorio = UnityEngine.Random.Range(0, clipsDigitacao.Length);
            if (clipsDigitacao[indexAleatorio] != null)
            {
                somDigitacao.PlayOneShot(clipsDigitacao[indexAleatorio]);
            }
        }
    }

    private void CompletarTexto()
    {
        PararCorrotinasAtivas();
        dialogueText.text = currentData.falas[currentLine].texto;
        FinalizarDigitacao();
    }

    private void FinalizarDigitacao()
    {
        isTyping = false;
        avisoContinuar.SetActive(true);
        // Inicia a contagem para passar o texto automaticamente
        autoAdvanceCoroutine = StartCoroutine(RotinaAutoAvanco());
    }

    private IEnumerator RotinaAutoAvanco()
    {
        yield return new WaitForSeconds(tempoAutoAvanco);
        AvancarFala();
    }

    private void PularParaProximaFala()
    {
        PararCorrotinasAtivas();
        AvancarFala();
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
            Finalizar();
        }
    }

    private void PararCorrotinasAtivas()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        if (autoAdvanceCoroutine != null) StopCoroutine(autoAdvanceCoroutine);
    }

    private void Finalizar()
    {
        dialogoAtivo = false;
        painelDialogo.SetActive(false);
        currentLine = 0;
        avisoContinuar.SetActive(false);

        // Carrega a próxima cena baseada no texto do Inspector
        if (!string.IsNullOrEmpty(nomeProximaCena))
        {
            SceneManager.LoadScene(nomeProximaCena);
        }
        else
        {
            Debug.LogWarning("O diálogo terminou, mas o nome da próxima cena está vazio no Inspector!");
        }
    }

    public bool EstaEmDialogo() => dialogoAtivo;
}