using TMPro;
using UnityEngine;

[System.Serializable]
public class TitleSceneUIRefs
{
    public TextMeshProUGUI insertCoinText;
    public TextMeshProUGUI pressStartText;
    public TextMeshProUGUI creditCoinText;
}

public class TitleSceneUIBridge : MonoBehaviour
{
    [SerializeField] private TitleSceneUIRefs titleSceneUIRefs;

    private void Start()
    {
        UIManager.Instance.AssignTitleSceneUIRefsAtRuntime(titleSceneUIRefs);
        ResetMachineValues();
    }

    public void ResetMachineValues()
    {
        titleSceneUIRefs.insertCoinText.enabled = true;
        titleSceneUIRefs.creditCoinText.text = "00";
        UIManager.Instance.CreditCounter = 0;
    }
}
