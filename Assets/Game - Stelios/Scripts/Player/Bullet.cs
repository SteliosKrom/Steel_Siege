using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private string BULLET_TAG;

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
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = 0f;
        ObjectPoolManager.Instance.ReturnObject(BULLET_TAG, gameObject);
    }
}
