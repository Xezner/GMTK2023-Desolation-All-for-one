using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimePlay : MonoBehaviour
{
    [SerializeField]
    private bool _firstTimePlayer;
    // Start is called before the first frame update
    void Start()
    {
        _firstTimePlayer = PlayerPrefs.GetInt("_firstTimePlayer") != 0;
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("_firstTimePlayer", (_firstTimePlayer ? 1:0));
    }
}
