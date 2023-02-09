using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodBar : MonoBehaviour
{
    public Slider slider;


    public void SetMaxBloodLevel(int level)
    {
        slider.maxValue = level;
    }

    public void SetBloodLevel(int level)
    {
        slider.value = level;
    }

}
