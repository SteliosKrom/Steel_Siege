using System.Collections;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    private const string SHIELD_TAG = "Shield";
    [SerializeField] private bool hasShield;

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject shieldAura;
    #endregion

    #region SCRIPTABLE OBJECTS
    [Header("SCRIPTABLE OBJECTS")]
    [SerializeField] private PowerUpData shieldData;
    #endregion

    #region PROPERTIES
    public bool HasShield { get => hasShield; set => hasShield = value; }
    #endregion
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(SHIELD_TAG))
        {
            other.gameObject.SetActive(false);
            StartCoroutine(ShieldPowerUpEffectCoroutine());
        }
    }

    public IEnumerator ShieldPowerUpEffectCoroutine()
    {
        shieldAura.SetActive(true);
        HasShield = true;

        AudioManager.Instance.PlaySFX(AudioManager.SoundType.GainPowerUp);

        yield return new WaitForSeconds(shieldData.EffectDuration);

        shieldAura.SetActive(false);
        HasShield = false;
    }
}
