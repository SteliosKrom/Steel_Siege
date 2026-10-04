using System.Collections;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    private const string STAMINA_TAG = "Stamina";
    private const float staminaBoostMultiplier = 0.3f;

    private float staminaBoost;
    [SerializeField] private float currentSpeed;

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject staminaAura;
    #endregion

    #region EVENTS
    [Header("EVENTS")]
    [SerializeField] private GameEventsSO gameEvents;
    [SerializeField] private AudioEventsSO audioEvents;
    #endregion

    #region GAME DATA
    [Header("GAME DATA")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private PowerUpData powerUpData;
    #endregion

    public float CurrentSpeed { get => currentSpeed; set => currentSpeed = value; }

    private void Start()
    {
        currentSpeed = playerData.MoveSpeed;
        staminaBoost = currentSpeed * staminaBoostMultiplier;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(STAMINA_TAG))
        {
            other.gameObject.SetActive(false);
            audioEvents.RaiseGainPowerUp();
            StartCoroutine(StaminaPowerUpEffectCoroutine());
        }
    }

    public void SpeedBackToNormal()
    {
        currentSpeed = playerData.MoveSpeed;
    }

    public IEnumerator StaminaPowerUpEffectCoroutine()
    {
        staminaAura.SetActive(true);
        CurrentSpeed += staminaBoost;

        yield return new WaitForSeconds(powerUpData.EffectDuration);

        staminaAura.SetActive(false);
        SpeedBackToNormal();
    }
}
