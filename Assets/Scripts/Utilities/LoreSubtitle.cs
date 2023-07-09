using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoreSubtitle : MonoBehaviour
{
    public GameObject T1, T2, T3;

    // Update is called once per frame
    void Start()
    {
        T1.SetActive(true);
        T2.SetActive(true);
        T3.SetActive(true);
    }

    void Update()
    {
        StartCoroutine(TheSequence());

        if (Input.GetKeyDown(KeyCode.Return))
        {
            //dito sa line na to yung change scene
            Debug.Log("change");
        }
    }

    IEnumerator TheSequence()
    {

        yield return new WaitForSeconds(27);

        T1.SetActive(false);
        yield return new WaitForSeconds(10);

        T2.SetActive(false);
        yield return new WaitForSeconds(10);

        T3.SetActive(false);
        yield return new WaitForSeconds(2);

        //dito sa line na to yung change scene
        Debug.Log("change");
    }
}
