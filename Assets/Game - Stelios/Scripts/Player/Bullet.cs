using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private string bulletTag;

    #region EVENTS
    [Header("EVENTS")]
    [SerializeField] private AudioEventsSO audioEvents;
    #endregion

    private void OnCollisionEnter2D(Collision2D other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Obstacle"))
        {
            ReturnBullet();

            if (this.gameObject.CompareTag("EnemyBullet"))
                return;

            audioEvents.RaiseHitWall();
        }

        if (damageable != null)
        {
            damageable.TakeDamage();
            ReturnBullet();
        }
    }

    public void ReturnBullet()
    {
        ObjectPoolManager.Instance.ReturnObject(bulletTag, gameObject);
    }
}
