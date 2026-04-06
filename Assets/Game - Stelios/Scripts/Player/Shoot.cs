using System.Collections;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private float shootDelay = 1f;

    private bool canShoot = true;

    [SerializeField] private Transform shootingPoint;
    [SerializeField] private PlayerData playerData;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canShoot)
        {
            ShootBullet();
            StartCoroutine(ShootDelay());
        }
    }

    public void ShootBullet()
    {
        GameObject bullet = ObjectPoolManager.Instance.GetObject("Bullet");
        bullet.transform.position = shootingPoint.position;
        bullet.transform.rotation = shootingPoint.rotation;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = shootingPoint.up * playerData.BulletSpeed;
    }

    public IEnumerator ShootDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }
}
