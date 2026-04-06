using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform player;
    public float fireRate = 1.5f;

    [Header("Configurações de Alcance")]
    public float range = 10f; // Distância máxima para atirar

    private float nextFireTime;

    void Update()
    {
        if (player == null) return;

        // 1. Calcula a distância entre a torreta e o player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 2. Só atira se estiver dentro do range E no tempo certo
        if (distanceToPlayer <= range && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = (player.position - firePoint.position).normalized;
        bullet.GetComponent<Bullet>().Setup(direction);
    }

    // 3. Desenha um círculo no editor para você ver o alcance visualmente
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}