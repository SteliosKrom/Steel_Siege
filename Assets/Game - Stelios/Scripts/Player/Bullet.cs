using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Obstacle"))
        {
            DeactivateBullet();
        }

        if (other.gameObject.CompareTag("Player_1"))
        {
            DeactivateBullet();
        }
        else if (other.gameObject.CompareTag("Player_2"))
        {
            DeactivateBullet();
        }
    }

    public void DeactivateBullet()
    {
        ObjectPoolManager.Instance.ReturnObject("Bullet", gameObject);
    }
}
