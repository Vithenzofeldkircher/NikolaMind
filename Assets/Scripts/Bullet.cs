using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float life_Time = 3f;
    private Vector2 Move_Direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Setup (Vector2 direction)
    {
        Move_Direction = direction;
        Destroy(gameObject, life_Time);
    }

    // Update is called once per frame
    void Update()
    {
        //move a bala na direção definida no setup, sem mudar a trajetoria
        transform.Translate(Move_Direction * speed * Time.deltaTime);
    }
}
