using TMPro;
using UnityEngine;
using System.Collections;

public class Dialogue_System : MonoBehaviour
{
    [Header("Referências UI")]
    public TMP_Text dialogueText;
    public TMP_Text nomeText;
    public GameObject painelDialogo;

    [Header("Configurações")]
    public float typingSpeed = 0.03f;
    public string botaoAvancar = "Submit";

    private DialogueData currentData;
    private int currentLine = 0;
    private bool isTyping = false;
    private bool dialogoAtivo = false;

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
    }

    private void MostrarFala()
    {
        StopAllCoroutines();
        nomeText.text = currentData.falas[currentLine].nomePersonagem;
        StartCoroutine(TypeLine(currentData.falas[currentLine].texto));
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    private void CompletarTexto()
    {
        StopAllCoroutines();
        dialogueText.text = currentData.falas[currentLine].texto;
        isTyping = false;
    }

    private void AvancarFala()
    {
        currentLine++;
        if (currentLine < currentData.falas.Count) MostrarFala();
        else Finalizar();
    }

    private void Finalizar()
    {
        dialogoAtivo = false;
        painelDialogo.SetActive(false);
    }

    public bool EstaEmDialogo() => dialogoAtivo;
}