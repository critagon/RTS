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

    public Transform selectionDisplayParent;
    public GameObject selectionDisplayPrefab;

    Dictionary<string, Text> resourceText;

    //Transform mainCanvas;
    Transform healthbarCanvas;
    #endregion
    #region Setup
    private void OnEnable()
    {
        EventManager.AddListener("UpdateResourceTexts", OnUpdateResourceText);
        EventManager.AddListener("CheckBuildingButtons", OnCheckBuildingButtons);

        EventManager.AddTypedListener("OnSelectUnit", OnSelectUnit);
        EventManager.AddTypedListener("OnDeselectUnit", OnDeselectUnit);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener("UpdateResourceTexts", OnUpdateResourceText);
        EventManager.RemoveListener("CheckBuildingButtons", OnCheckBuildingButtons);

        EventManager.RemoveTypedListener("OnSelectUnit", OnSelectUnit);
        EventManager.RemoveTypedListener("OnDeselectUnit", OnDeselectUnit);
    }

    void Awake()
    {
        healthbarCanvas = GameObject.Find("Healthbars").transform;

        #region Building Buttons      
        buildingPlacer = GetComponent <BuildingPlacer>();
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
        print("Unit in UIManager is: " + unit); //to debug whether unit is null or not before starting.
        if (unit != null)
        {
            Transform alreadyInstantiatedChild = selectionDisplayParent.Find(unit.Code);
            if (alreadyInstantiatedChild != null)
            {
                Text text = alreadyInstantiatedChild.Find("Count").GetComponent<Text>();
                int count = int.Parse(text.text);
                text.text = (count + 1).ToString();
            }
            else
            {
                GameObject gameObj = Instantiate(selectionDisplayPrefab) as GameObject;
                gameObj.name = unit.Code;
                Transform objTransform = gameObj.transform;
                objTransform.Find("Count").GetComponent<Text>().text = "1";
                objTransform.Find("Name").GetComponent<Text>().text = unit.unitData.unitName;
                objTransform.SetParent(selectionDisplayParent);
            }
        }
        
    }

    public void RemoveSelectedUnitFromSelectionDisplay(string code)
    {
        if (code != null)
        {
            Transform listItem = selectionDisplayParent.Find(code);
            if (listItem == null) return;
            Text text = listItem.Find("Count").GetComponent<Text>();
            int count = int.Parse(text.text);
            count -= 1;
            if (count == 0)
                DestroyImmediate(listItem.gameObject);
            else
                text.text = count.ToString();
        }
    }

    #endregion
}




