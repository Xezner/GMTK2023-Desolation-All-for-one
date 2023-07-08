using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataHolder : MonoBehaviour
{
    [SerializeField] private CharacterData _characterData;
    public CharacterData CharacterData { get { return _characterData; } }
    private void Update()
    {
        //if (Input.GetMouseButtonDown(1))
        //{
        //    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        //    if (hit.collider != null)
        //    {
        //        CharacterDataHolder characterData = hit.collider.GetComponent<CharacterDataHolder>();
        //        if (characterData != null)
        //        {
        //            // Access the script's data or perform any actions
        //            Debug.Log("Clicked on: " + characterData.name);
        //        }
        //    }
        //}
    }

}
