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
        Vector3 ultimoPontoFixo = pontosDoFio[pontosDoFio.Count - 2];
        Vector3 direcao = transform.position - ultimoPontoFixo;
        float distancia = Vector3.Distance(transform.position, ultimoPontoFixo);

        // Avança a origem do raio levemente para frente para não colidir na própria quina atual
        Vector3 origemRaio = ultimoPontoFixo + direcao.normalized * 0.02f;
        float distanciaAjustada = distancia - 0.02f;

        if (distanciaAjustada <= 0) return;

        // Passando a origem e direção corretas e normalizadas
        RaycastHit2D hit = Physics2D.Raycast(origemRaio, direcao.normalized, distanciaAjustada, layerColisao);

        if (hit.collider != null)
        {
            // Verifica se o que o fio tocou é um extensor
            if (hit.collider.TryGetComponent(out Wire_Extender extensor))
            {
                extensor.AtivarExtensao();
            }

            Vector3 pontoQuina;

            // SE FOR UM BOX COLLIDER: Calcula matematicamente o canto do retângulo
            if (hit.collider is BoxCollider2D boxCollider)
            {
                pontoQuina = CalcularQuinaExata(boxCollider, hit.point);
            }
            else
            {
                // Fallback padrão usando a normal caso use outro tipo de colisor
                pontoQuina = (Vector3)hit.point + ((Vector3)hit.normal * distanciaDaQuina);
            }

            // Evita criar quinas redundantes coladas uma na outra
            if (Vector3.Distance(pontoQuina, ultimoPontoFixo) > 0.05f)
            {
                pontosDoFio.Insert(pontosDoFio.Count - 1, pontoQuina);
            }
        }
    }

    // Método que encontra o vértice mais próximo do boxcollider e joga o ponto para fora
    Vector3 CalcularQuinaExata(BoxCollider2D box, Vector2 pontoHit)
    {
        // Converte o ponto de impacto global para o espaço local do objeto
        Vector3 localHit = box.transform.InverseTransformPoint(pontoHit);

        float extentsX = box.size.x / 2f;
        float extentsY = box.size.y / 2f;
        Vector2 offset = box.offset;

        // Identifica qual das 4 quinas do retângulo está mais próxima do local do impacto
        float quinaX = localHit.x > offset.x ? offset.x + extentsX : offset.x - extentsX;
        float quinaY = localHit.y > offset.y ? offset.y + extentsY : offset.y - extentsY;

        Vector3 quinaLocal = new Vector3(quinaX, quinaY, 0);
        Vector3 quinaMundo = box.transform.TransformPoint(quinaLocal);

        // CORREÇÃO 2: Usa box.bounds.center em vez de box.transform.position 
        // para garantir o centro real do colisor mesmo com offsets ou pivots alterados
        Vector3 direcaoParaFora = (quinaMundo - box.bounds.center).normalized;

        // Retorna a posição final com a folga de segurança aplicada
        return quinaMundo + direcaoParaFora * distanciaDaQuina;
    }

    void VerificarRetorno()
    {
        // Se houver mais de 2 pontos, verificamos se o player voltou e "desenrolou" da quina
        if (pontosDoFio.Count > 2)
        {
            Vector3 pontoPenultimo = pontosDoFio[pontosDoFio.Count - 3];
            Vector3 direcao = pontoPenultimo - transform.position;
            float distancia = direcao.magnitude;

            // CORREÇÃO 3: Direção normalizada e uma pequena redução na distância máxima (-0.05f)
            // para evitar que o raio raspe na quina atual e impeça o fio de desenrolar
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direcao.normalized, distancia - 0.05f, layerColisao);

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