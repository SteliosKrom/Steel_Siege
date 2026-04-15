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
    [SerializeField] private TitleSceneRefs localTitleSceneRefs;

    private void Start()
    {
        UIManager.Instance.AssignTitleSceneUIRefsAtRuntime(localTitleSceneRefs);
        ResetMachineValues();
    }

    public void ResetMachineValues()
    {
        localTitleSceneRefs.insertCoinText.enabled = true;
        localTitleSceneRefs.creditCoinText.text = "00";
        UIManager.Instance.CreditCounter = 0;
    }
}
