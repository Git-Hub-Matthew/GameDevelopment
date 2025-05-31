using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Countdown_Script : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Countdown;

    public void UiCountDown(float count)
    {
        //int cooldown = (int)count;
        Countdown.text = string.Format("{0:0}", count);
    }
}
