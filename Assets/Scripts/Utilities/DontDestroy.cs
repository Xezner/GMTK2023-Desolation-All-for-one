using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        var dontDestroy = FindObjectsOfType<DontDestroy>();
        for(int i = 1; i < dontDestroy.Length; i++)
        {
            Destroy(dontDestroy[i].gameObject);
        }
    }
}
