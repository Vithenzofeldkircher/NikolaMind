using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 10f;
    public float life_Time = 3f;
    public int damageAmount = 1;

    public void Setup(Vector2 direction)
    {
        rb = GetComponent<Rigidbody2D>();

        // Definimos a velocidade uma única vez. 
        // O Rigidbody manterá essa velocidade constante.
        rb.linearVelocity = direction * speed;

        Destroy(gameObject, life_Time);
    }

    // O Update fica vazio pois a física (RB) está movendo o objeto
    void Update() { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Life healthScript = collision.GetComponent<Life>();

        if (healthScript != null)
        {
            healthScript.TakeDamage(damageAmount);
            Destroy(gameObject);
        }

        if (collision.CompareTag("Parede"))
        {
            Destroy(gameObject);
        }
    }
}