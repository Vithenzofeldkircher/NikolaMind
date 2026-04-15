using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class WireRenderer : MonoBehaviour
{
    public LayerMask layerColisao; // No Inspetor, selecione a Layer "obstaculo"
    private LineRenderer line;
    public List<Vector3> pontosDoFio = new List<Vector3>();

    [SerializeField] private float distanciaDaQuina = 0.1f; // Folga para o fio não entrar na parede

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;
    }

    void Update()
    {
        if (pontosDoFio.Count == 0) return;

        // Se estiver carregando o fio, o último ponto sempre segue o Player
        if (WireManager.Instance.carregandoFio)
        {
            pontosDoFio[pontosDoFio.Count - 1] = transform.position;

            VerificarColisoes();
            VerificarRetorno();
        }

        Desenhar();
    }

    void VerificarColisoes()
    {
        // O ponto de onde o raio sai (penúltimo ponto da lista)
        Vector3 ultimoPontoFixo = pontosDoFio[pontosDoFio.Count - 2];
        Vector3 direcao = transform.position - ultimoPontoFixo;
        float distancia = Vector3.Distance(transform.position, ultimoPontoFixo);

        // Dispara um raio para ver se há algo entre o último ponto fixo e o player
        RaycastHit2D hit = Physics2D.Raycast(ultimoPontoFixo, direcao, distancia, layerColisao);

        if (hit.collider != null)
        {
            // Se bateu em algo, adicionamos um ponto na quina
            // Usamos a 'normal' do impacto para afastar o fio levemente da parede
            Vector3 pontoQuina = (Vector3)hit.point + ((Vector3)hit.normal * distanciaDaQuina);

            // Insere o novo ponto antes da posição atual do player
            pontosDoFio.Insert(pontosDoFio.Count - 1, pontoQuina);
        }
    }

    void VerificarRetorno()
    {
        // Se houver mais de 2 pontos, verificamos se o player voltou e "desenrolou" da quina
        if (pontosDoFio.Count > 2)
        {
            Vector3 pontoPenultimo = pontosDoFio[pontosDoFio.Count - 3];
            float distancia = Vector3.Distance(transform.position, pontoPenultimo);

            // Raio entre o player e o antepenúltimo ponto
            RaycastHit2D hit = Physics2D.Raycast(transform.position, pontoPenultimo - transform.position, distancia, layerColisao);

            // Se não houver nada bloqueando a visão direta para o ponto anterior, removemos a quina atual
            if (hit.collider == null)
            {
                pontosDoFio.RemoveAt(pontosDoFio.Count - 2);
            }
        }
    }

    public void InicializarFio(Vector3 pos)
    {
        pontosDoFio.Clear();
        pontosDoFio.Add(pos); // Ponto na caixa de origem
        pontosDoFio.Add(transform.position); // Ponto no Player
    }

    public void FixarUltimoPonto(Vector3 pos) => pontosDoFio[pontosDoFio.Count - 1] = pos;

    public float CalcularDistanciaTotal()
    {
        float d = 0;
        for (int i = 0; i < pontosDoFio.Count - 1; i++)
            d += Vector3.Distance(pontosDoFio[i], pontosDoFio[i + 1]);
        return d;
    }

    void Desenhar()
    {
        line.positionCount = pontosDoFio.Count;
        line.SetPositions(pontosDoFio.ToArray());
    }
}