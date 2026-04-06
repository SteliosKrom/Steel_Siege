using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private float enterCreditDelay = 2f;

    #region UI
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI insertCoinText;
    [SerializeField] private TextMeshProUGUI pressStartText;
    [SerializeField] private TextMeshProUGUI creditText;
    #endregion

    public int CreditCounter { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        insertCoinText.enabled = true;
        pressStartText.enabled = false;
        creditText.text = CreditCounter.ToString("00");
    }

    public void InsertCoin()
    {
        StartCoroutine(EnterCreditDelay());
    }

    public IEnumerator EnterCreditDelay()
    {
        insertCoinText.enabled = false;
        CreditCounter++;
        creditText.text = CreditCounter.ToString("00");

        yield return new WaitForSeconds(enterCreditDelay);

        pressStartText.enabled = true;
    }
}
