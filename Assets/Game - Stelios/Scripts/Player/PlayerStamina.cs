using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    private float staminaEffectDuration;
    private float maxStaminaEffectDuration;
    private float staminaBoost;
    private const float staminaBoostMultiplier = 0.3f;
    private float currentSpeed;

    private bool staminaOnEffect = false;

    private const string STAMINA_TAG = "Stamina";

    #region GAME DATA
    [Header("GAME DATA")]
    [SerializeField] private PlayerData playerData;
    #endregion

    public float CurrentSpeed => currentSpeed;

    private void Start()
    {
        currentSpeed = playerData.MoveSpeed;
        staminaBoost = currentSpeed * staminaBoostMultiplier;
        maxStaminaEffectDuration = 5;
    }

    private void Update()
    {
        if (staminaOnEffect)
        {
            staminaEffectDuration += Time.deltaTime;

            if (staminaEffectDuration >= maxStaminaEffectDuration)
            {
                staminaEffectDuration = 0;
                staminaOnEffect = false;
                SpeedBackToNormal();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(STAMINA_TAG))
        {
            other.gameObject.SetActive(false);
            staminaOnEffect = true;
            staminaEffectDuration = 0;
            IncreaseSpeed();
        }
    }

    public void IncreaseSpeed()
    {
        currentSpeed += staminaBoost;
    }

    public void SpeedBackToNormal()
    {
        currentSpeed = playerData.MoveSpeed;
    }
}
