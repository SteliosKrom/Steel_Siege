using UnityEngine;

public class UIEvents : MonoBehaviour
{
    public void OnPVPModeStay()
    {
        UIManager.Instance.PVPSelectionArrow.color = Color.red;
    }

    public void OnPVPModeExit()
    {
        UIManager.Instance.PVPSelectionArrow.color = Color.white;
    }

    public void OnPVEModeStay()
    {
        UIManager.Instance.PVESelectionArrow.color = Color.red;
    }

    public void OnPVEModeExit()
    {
        UIManager.Instance.PVESelectionArrow.color = Color.white;
    }
}
