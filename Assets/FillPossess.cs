using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillPossess : MonoBehaviour
{
    public Image ImageFill;
    public float GuageAmount = 5;
    
    void Update()
    {
        ImageFill.fillAmount = GuageAmount / 5;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            PossessPoint(1);
            //ano lng to uhh to show yung fill
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GuageAmount = 0;
            //then probably a code to reset when possessed to go back 0

        }
    }

    public void PossessPoint(float FillGuage)
    {
        GuageAmount += FillGuage;
    }

    
}
