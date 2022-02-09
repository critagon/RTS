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

    //public Transform selectionDisplayParent;
    //public GameObject selectionDisplayPrefab;
    public Transform selectedUnitsListParent;
    public GameObject selectedUnitDisplayPrefab;

    Dictionary<string, Text> resourceText;

    //Transform mainCanvas;
    Transform healthbarCanvas;
    #endregion
    #region Setup
    private void OnEnable()
    {
        EventManager.AddListener("UpdateResourceTexts", OnUpdateResourceText);
        EventManager.AddListener("CheckBuildingButtons", OnCheckBuildingButtons);

        EventManager.AddTypedListener("SelectUnit", OnSelectUnit);
        EventManager.AddTypedListener("DeselectUnit", OnDeselectUnit);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener("UpdateResourceTexts", OnUpdateResourceText);
        EventManager.RemoveListener("CheckBuildingButtons", OnCheckBuildingButtons);

        EventManager.RemoveTypedListener("SelectUnit", OnSelectUnit);
        EventManager.RemoveTypedListener("DeselectUnit", OnDeselectUnit);
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

    #region Resource and Building Display Update
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
        foreach (BuildingData buildingData in Globals.BUILDING_DATA)
        {
            buildingButtons[buildingData.code].interactable = buildingData.CanBuy();
        }
    }
    #endregion

    #region Selection Display

    private void OnSelectUnit(CustomEventData customEventData)
    {
        AddSelectedUnitToSelectionDisplay(customEventData.unit);
    }

    private void OnDeselectUnit(CustomEventData customEventData)   
    {
        RemoveSelectedUnitFromSelectionDisplay(customEventData.unit.Code);
    }

    public void AddSelectedUnitToSelectionDisplay(Unit unit)
    {
        // if there is another unit of the same type already selected,
        // increase the counter
        print(unit);
        Transform alreadyInstantiatedChild = selectedUnitsListParent.Find(unit.Code);
        if (alreadyInstantiatedChild != null)
        {
            Text t = alreadyInstantiatedChild.Find("Count").GetComponent<Text>();
            int count = int.Parse(t.text);
            t.text = (count + 1).ToString();
        }
        // else create a brand new counter initialized with a count of 1
        else
        {
            GameObject g = Instantiate(selectedUnitDisplayPrefab) as GameObject;
            g.name = unit.Code;
            Transform t = g.transform;
            t.Find("Count").GetComponent<Text>().text = "1";
            t.Find("Name").GetComponent<Text>().text = unit.unitData.unitName;
            t.SetParent(selectedUnitsListParent);
        }
    }

    public void RemoveSelectedUnitFromSelectionDisplay(string code)
    {
        Transform listItem = selectedUnitsListParent.Find(code);
        if (listItem == null) return;
        Text t = listItem.Find("Count").GetComponent<Text>();
        int count = int.Parse(t.text);
        count -= 1;
        if (count == 0)
            DestroyImmediate(listItem.gameObject);
        else
            t.text = count.ToString();
    }

    #endregion
}




