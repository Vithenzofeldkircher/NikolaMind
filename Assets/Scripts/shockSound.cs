using UnityEngine;

public class shockSound : MonoBehaviour
{
    public Transform jogador;
    public AudioSource somChoque;

    public float distancia = 5f;

    private void Update()
    {
        float distanciaAtual = Vector2.Distance(
            transform.position,
            jogador.position
        );

        if (distanciaAtual <= distancia)
        {
            if (!somChoque.isPlaying)
            {
                somChoque.Play();
            }
        }
        else
        {
            if (somChoque.isPlaying)
            {
                somChoque.Stop();
            }
        }
    }
}