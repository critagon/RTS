using UnityEngine;

public class CharacterManager : UnitManager
{
    public Transform parent;
    Character _character;
    public CharacterData characterData; //do I need to declare CharacterData here? I set it in inspector in the prefabs.

    public override Unit Unit 
    {
        get { return _character; }
        set { _character = value is Character character ? character : null; } //if _character is a character set to building? otherwise set to null? Not sure what this means?
    }

    void Update()
    {
        if (parent == null)
        {
            parent = GameObject.Find("Characters").transform; 
            transform.SetParent(parent);
        }

        UpdateUnitDisplay();
    }

    /*private void Awake()
    {
        if (_character == null)
        {
            Initialize(_character);
        }*
    }

    protected override void Initialize(Character character)
    {
        _character = new Character(characterData);
    }*/

    protected override void UpdateUnitDisplay()
   {
        //HoverCircle
        if (isHoveringOver && IsDrawingMarquee() == false && isPlacingBuilding == false)
        {
            transform.Find("HoverCircle").gameObject.SetActive(true);
            HealthbarCreate();
        }

        if (!isHoveringOver)
        {
            transform.Find("HoverCircle").gameObject.SetActive(false);
            if (!selectedUnits.ContainsKey(id))
            {
                HealthbarDestroy();
            }
        }
    }
}
