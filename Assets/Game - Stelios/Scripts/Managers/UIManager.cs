using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    #region UI
    [Header("UI")]
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
        creditText.text = CreditCounter.ToString("00");
    }

    public void InsertCoin()
    {
        CreditCounter++;
        creditText.text = CreditCounter.ToString("00");
    }
}
