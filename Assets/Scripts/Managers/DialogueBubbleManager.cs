using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBubbleManager : MonoBehaviour
{
    [Header ("DialogueBubble")]
    [SerializeField]private TextMeshProUGUI bubbleTextComponent;
    [SerializeField]private string[] bubbleLines;
    [SerializeField]private float bubbleTextSpeed;

    private int _bubbleIndex;

    void Start()
    {
        bubbleTextComponent.text = string.Empty;
       

        StartBubbleDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (bubbleTextComponent.text == bubbleLines[_bubbleIndex])
            {
                NextBubbleLine();
            }
            else
            {
                StopAllCoroutines();
                bubbleTextComponent.text = bubbleLines[_bubbleIndex];
            }
        }
    }

    void StartBubbleDialogue()
    {
        _bubbleIndex = 0;
        StartCoroutine(ShowBubbleLine());
    }
    IEnumerator ShowBubbleLine()
    {
        foreach (char c in bubbleLines[_bubbleIndex].ToCharArray()) 
        {
            bubbleTextComponent.text += c;
            yield return new WaitForSeconds(bubbleTextSpeed);
        }
    }

    void NextBubbleLine()
    {
        if (_bubbleIndex < bubbleLines.Length -1)
        {
            _bubbleIndex++;
            bubbleTextComponent.text = string.Empty;
            StartCoroutine(ShowBubbleLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
