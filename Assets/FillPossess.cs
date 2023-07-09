using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillPossess : ManagerBehaviour
{
    public Image ImageFill;
    public float GuageAmount = 5;
    
    void Update()
    {
        ImageFill.fillAmount = (float)GameManager.PossessionGauge / 5;

    }
}
