using UnityEngine;

public class UIEvents : MonoBehaviour
{
    public void OnPVPModeStay()
    {
        UIManager.Instance.TitleUI.PVPSelectionArrow.color = Color.red;
    }

    public void OnPVPModeExit()
    {
        UIManager.Instance.TitleUI.PVPSelectionArrow.color = Color.white;
    }

    public void OnPVEModeStay()
    {
        UIManager.Instance.TitleUI.PVESelectionArrow.color = Color.red;
    }

    public void OnPVEModeExit()
    {
        UIManager.Instance.TitleUI.PVESelectionArrow.color = Color.white;
    }
}
