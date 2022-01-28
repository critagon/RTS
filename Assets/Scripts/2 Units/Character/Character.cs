using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Unit
{
    CharacterData _characterData;
    CharacterManager characterManager;

    public Character(CharacterData characterData, List<ResourceValue> production) : base(characterData, production)
    {
        _characterData = characterData;
        characterManager = _transform.GetComponent<CharacterManager>();

        _currentHealth = characterData.HP;
    }

    public Character(CharacterData characterData) : this(characterData, new List<ResourceValue>()) 
    {
    } //no production

    public int DataIndex
    {
        get
        {
            for (int i = 0; i < Globals.CHARACTER_DATA.Length; i++)
            {
                if (Globals.BUILDING_DATA[i].code == _characterData.code)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
