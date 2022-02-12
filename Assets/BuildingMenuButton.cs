using UnityEngine;
using UnityEngine.UI;

public class BuildingMenuButton : MonoBehaviour
{
    public Button buildingButton;
    public Transform buildingSelectionPanel;

    bool isEnabled;

    public void OnButtonPress()
    {

        if (buildingSelectionPanel.gameObject.activeSelf == false)
        {
            isEnabled = true;
        
        }

        if (buildingSelectionPanel.gameObject.activeSelf == true)
        {
            isEnabled = false;
        }

    }

    private void Update()
    {
        if (isEnabled == true)
        {
            buildingSelectionPanel.gameObject.SetActive(true);
        }

        if (isEnabled == false)
        {
            buildingSelectionPanel.gameObject.SetActive(false);
        }
    }
}
