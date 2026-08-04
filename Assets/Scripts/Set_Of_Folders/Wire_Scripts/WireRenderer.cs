using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class WireRenderer : MonoBehaviour
{
    private LineRenderer line;

    void Awake()
    {
        // Pegamos a referência do LineRenderer assim que o objeto "acorda".
        // Usamos Awake em vez de Start para garantir que a referência esteja pronta 
        // antes que a física tente desenhar o fio no primeiro frame.
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;
    }

    // O WirePhysics vai chamar esta função todo frame passando os pontos atualizados.
    public void AtualizarDesenho(List<Vector3> pontosDoFio)
    {
        // Se a lista estiver vazia, não há o que desenhar. Retornamos para evitar erros.
        if (pontosDoFio == null || pontosDoFio.Count == 0) return;

        // O número de vértices da linha visual deve ser igual à quantidade de pontos na lista.
        line.positionCount = pontosDoFio.Count;

        // SetPositions pega um Array e desenha a linha interligando cada ponto.
        // Convertemos a nossa List para Array com .ToArray().
        line.SetPositions(pontosDoFio.ToArray());
    }
}