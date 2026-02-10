using UnityEngine;

public partial class Balance : MonoBehaviour
{
    public float amplitude = 10f; // O quão longe ele vai
    public float velocidade = 2f; // O quão rápido ele balança

    void Update()
    {
        // Calcula a rotação baseada no tempo
        float angulo = Mathf.Sin(Time.time * velocidade) * amplitude;
        
        // Aplica no eixo Z (perfeito para sprites 2D ou objetos 3D de frente)
        transform.rotation = Quaternion.Euler(0, 0, angulo);
    }
}
