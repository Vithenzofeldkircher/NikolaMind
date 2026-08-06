using UnityEngine;
using System.Collections;

public class CreditScroll : MonoBehaviour
{
    public RectTransform texto;
    public float velocidade = 50f;
    public float atraso = 1f;

    private Vector2 posicaoInicial;
    private bool mover = false;
    public RectTransform viewport;
    public GameObject painelCreditos;

    void Start()
    {
        posicaoInicial = texto.anchoredPosition;
    }

    void Update()
{
    if (!mover)
        return;

    texto.anchoredPosition += Vector2.up * velocidade * Time.deltaTime;

    // Verifica se o texto saiu completamente da tela
    if (texto.anchoredPosition.y >
        viewport.rect.height + texto.rect.height)
    {
        PararCreditos();
        painelCreditos.SetActive(false);
    }
}

    public void IniciarCreditos()
    {
        StopAllCoroutines();

        texto.anchoredPosition = posicaoInicial;
        mover = false;

        StartCoroutine(Iniciar());
    }

    IEnumerator Iniciar()
    {
        yield return new WaitForSeconds(atraso);
        mover = true;
    }

    public void PararCreditos()
    {
        StopAllCoroutines();
        mover = false;
    }
}