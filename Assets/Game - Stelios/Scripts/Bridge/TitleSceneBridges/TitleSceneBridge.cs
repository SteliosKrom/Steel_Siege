using TMPro;
using UnityEngine;

[System.Serializable]
public class TitleSceneRefs
{
    public TextMeshProUGUI insertCoinText;
    public TextMeshProUGUI pressStartText;
    public TextMeshProUGUI creditCoinText;
}

public class TitleSceneBridge : MonoBehaviour
{
    [SerializeField] private TitleSceneRefs titleSceneRefs;

    private void Start()
    {
        UIManager.Instance.AssignTitleSceneRefsAtRuntime(titleSceneRefs);
        ResetMachineValues();
    }

    public void ResetMachineValues()
    {
        titleSceneRefs.insertCoinText.enabled = true;
        titleSceneRefs.creditCoinText.text = "00";
        UIManager.Instance.CreditCounter = 0;
    }
}
