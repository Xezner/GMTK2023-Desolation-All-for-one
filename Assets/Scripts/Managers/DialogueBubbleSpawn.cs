using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBubbleSpawn : MonoBehaviour
{
    public GameObject DialogueBubblePrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(DialogueBubblePrefab, transform.position, Quaternion.identity);
        }
    }
}
