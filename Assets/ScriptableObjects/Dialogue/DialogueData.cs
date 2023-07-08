using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "ScriptableObjects/Dialogue")]
public class DialogueData : ScriptableObject
{
    public float TextSpeed;
    public string[] Lines;
}
