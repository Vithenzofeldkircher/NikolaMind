using UnityEngine;

public class Flutuar : MonoBehaviour
{
    public float velocidade = 2f;   // velocidade do movimento
    public float altura = 0.5f;     // o quanto sobe/desce

    private Vector3 posicaoInicial;

    void Start()
    {
        posicaoInicial = transform.position;
    }

    void Update()
    {
        float novaAltura = Mathf.Sin(Time.time * velocidade) * altura;
        transform.position = posicaoInicial + new Vector3(0, novaAltura, 0);
    }
}