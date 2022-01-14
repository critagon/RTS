using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Universal Variables
    BuildingPlacer buildingPlacer;

    public Transform buildingMenu;
    public GameObject buildingButtonPrefab;

    Dictionary<string, Button> buildingButtons;

    public Transform resourcesUIParent;
    public GameObject gameResourceDisplayPrefab;

    Dictionary<string, Text> resourceText;

    Transform healthbarCanvas;
    #endregion
    #region Setup
    private void OnEnable()
    {
        EventManager.AddListener("UpdateResourceTexts", OnUpdateResourceText);
        EventManager.AddListener("CheckBuildingButtons", OnCheckBuildingButtons);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener("UpdateResourceTexts", OnUpdateResourceText);
        EventManager.RemoveListener("CheckBuildingButtons", OnCheckBuildingButtons);
    }

    void Awake()
    {
        healthbarCanvas = GameObject.Find("Healthbars").transform;

        #region Building Buttons      
        buildingPlacer = GetComponent < BuildingPlacer>();
        buildingButtons = new Dictionary<string, Button>();

        //create buttons for each building type
        for (int i = 0; i < Globals.BUILDING_DATA.Length; i++)
        {
            BuildingData buildingData = Globals.BUILDING_DATA[i];
            GameObject buildingButton = Instantiate(buildingButtonPrefab);
            buildingButton.name = buildingData.unitName;
            buildingButton.transform.Find("Text").GetComponent<Text>().text = buildingData.unitName;
            Button b = buildingButton.GetComponent<Button>();
            AddBuildingButtonListener(b, i);
            buildingButton.transform.SetParent(buildingMenu);

            buildingButtons[buildingData.code] = b;
            if (!Globals.BUILDING_DATA[i].CanBuy())
            {
                b.interactable = false;
            }
        }
        #endregion

        #region Resource Icons and Text
        resourceText = new Dictionary<string, Text>();

        foreach (KeyValuePair<string, GameResource> pair in Globals.GAME_RESOURCES)
        {
            GameObject display = Instantiate(gameResourceDisplayPrefab);
            
            display.name = pair.Key;
            resourceText[pair.Key] = display.transform.Find("AmountText").GetComponent<Text>();
            SetResourceText(pair.Key, pair.Value.Amount);
            display.transform.SetParent(resourcesUIParent);
            display.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Assets/Images/UI/{pair.Key}");
        }
    }
    #endregion
    #endregion

    #region Display Update
    void AddBuildingButtonListener(Button b, int i)
    {
        b.onClick.AddListener(() => buildingPlacer.PreparePlacedBuilding(i)); //still triggers with spacebar?
    }

    void SetResourceText(string resource, int value)
    {
        resourceText[resource].text = value.ToString();
    }

    public void OnUpdateResourceText()
    {
        foreach (KeyValuePair<string, GameResource> pair in Globals.GAME_RESOURCES)
        {
            SetResourceText(pair.Key, pair.Value.Amount);
        }
    }

    public void OnCheckBuildingButtons()
    {
        foreach (BuildingData data in Globals.BUILDING_DATA)
        {
            buildingButtons[data.code].interactable = data.CanBuy();
        }
    }
    #endregion

    void Update()
    {
        if (Globals.SomethingSelected() == true)
        {
            healthbarCanvas.gameObject.SetActive(true);
        }
        else healthbarCanvas.gameObject.SetActive(false);
    }
}




