using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab; // Arraste o prefabs da bala aqui
    public Transform firePoint;     // Onde o tiro sai (cano da arma)
    public Transform player;        // Arraste o player aqui
    public float fireRate = 1.5f;   // Tempo entre os tiros
    private float nextFireTime;

    void Update()
    {
        // Verifica se o player existe e se já deu o tempo de atirar
        if (player != null && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        // Cria a bala na posição do firePoint
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Calcula a direção fixa para o player
        Vector2 direction = (player.position - firePoint.position).normalized;

        // Passa a direção para o script da bala
        bullet.GetComponent<Bullet>().Setup(direction);
    }
}