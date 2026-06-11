using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class WireRenderer : MonoBehaviour
{
    //fazer de uma maneira em que os pontos fiquem fixos e "imutaveis" quando o fio atingir a caixa, mechendo apenas no ultimo ponto, mas ainda sendo capaz de retirar esses pontos fixos 
    public LayerMask layerColisao; // No Inspetor, selecione a Layer "obstaculo"
    private LineRenderer line;
    public List<Vector3> pontosDoFio = new List<Vector3>();

    // Variável para guardar a referência direta da última caixa que o fio colidiu
    private Caixa_Enrolar Ultima_Caixa_Ativada;

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

        Vector3 origemRaio = ultimoPontoFixo + direcao.normalized * 0.02f;
        float distanciaAjustada = distancia - 0.02f;

        if (distanciaAjustada <= 0) return;

        RaycastHit2D hit = Physics2D.Raycast(origemRaio, direcao.normalized, distanciaAjustada, layerColisao);

        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out Wire_Extender extensor))
            {
                extensor.AtivarExtensao();
            }

            // CORREÇÃO: Salva a referência da caixa ao colidir, assim salva qual caixa foi ativada
            if (hit.collider.TryGetComponent(out Caixa_Enrolar caixa))
            {
                caixa.Enrolar_Fio();
                Ultima_Caixa_Ativada = caixa;
            }

            Vector3 pontoQuina;

            if (hit.collider is BoxCollider2D boxCollider)
            {
                pontoQuina = CalcularQuinaExata(boxCollider, hit.point);
            }
            else
            {
                pontoQuina = (Vector3)hit.point + ((Vector3)hit.normal * distanciaDaQuina);
            }

            if (Vector3.Distance(pontoQuina, ultimoPontoFixo) > 0.05f)
            {
                pontosDoFio.Insert(pontosDoFio.Count - 1, pontoQuina);
            }
        }
    }

    Vector3 CalcularQuinaExata(BoxCollider2D box, Vector2 pontoHit)
    {
        Vector3 localHit = box.transform.InverseTransformPoint(pontoHit);

        float extentsX = box.size.x / 2f;
        float extentsY = box.size.y / 2f;
        Vector2 offset = box.offset;

        float quinaX = localHit.x > offset.x ? offset.x + extentsX : offset.x - extentsX;
        float quinaY = localHit.y > offset.y ? offset.y + extentsY : offset.y - extentsY;

        Vector3 quinaLocal = new Vector3(quinaX, quinaY, 0);
        Vector3 quinaMundo = box.transform.TransformPoint(quinaLocal);

        Vector3 direcaoParaFora = (quinaMundo - box.bounds.center).normalized;

        return quinaMundo + direcaoParaFora * distanciaDaQuina;
    }

    void VerificarRetorno()
    {
        if (pontosDoFio.Count > 2)
        {
            Vector3 pontoPenultimo = pontosDoFio[pontosDoFio.Count - 3];
            Vector3 direcao = pontoPenultimo - transform.position;
            float distancia = direcao.magnitude;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direcao.normalized, distancia - 0.05f, layerColisao);

            // Se não houver nada bloqueando a visão do ponto anterior, o fio desenrola da quina
            if (hit.collider == null)
            {
                // CORREÇÃO: Removemos o OverlapCircle problemático.
                // Se o script guardava uma caixa ativa, manda ela voltar ao sprite normal direto!
                if (Ultima_Caixa_Ativada != null)
                {
                    Ultima_Caixa_Ativada.DesenrolarFio();
                    Ultima_Caixa_Ativada = null; // Limpa a variável para a próxima colisão
                }

                // Remove o ponto da lista com segurança
                pontosDoFio.RemoveAt(pontosDoFio.Count - 2);
            }
        }
    }

    public void InicializarFio(Vector3 pos)
    {
        pontosDoFio.Clear();
        pontosDoFio.Add(pos);
        pontosDoFio.Add(transform.position);
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