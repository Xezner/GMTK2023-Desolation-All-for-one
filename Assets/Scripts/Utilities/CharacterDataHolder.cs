using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataHolder : MonoBehaviour
{
    [SerializeField] private CharacterData _characterData;
    public CharacterData CharacterData { get { return _characterData; } }

}
