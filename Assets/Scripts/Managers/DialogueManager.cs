using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header ("Dialogue")]
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private DialogueData _dialogueData;
    [SerializeField] private DialogueData _textSpeed;
    private string[] lines;
    private float textSpeed;

    private int _index;

    void Start()
    {
        textComponent.text = string.Empty;
        lines = _dialogueData.Lines;
        textSpeed = _dialogueData.TextSpeed;

        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[_index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[_index];
            }
        }
    }

    void StartDialogue()
    {
        _index = 0;
        StartCoroutine(ShowLine());
    }
    IEnumerator ShowLine() //Types the letter 1 by 1
    {
        foreach (char c in lines[_index].ToCharArray()) // takes string and breakdowns letter into arrays
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (_index < lines.Length -1)
        {
            _index++;
            textComponent.text = string.Empty;
            StartCoroutine(ShowLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
