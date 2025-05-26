using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Countdown_Script : MonoBehaviour
{
    [SerializeField] private Image Countdown;

    private void Awake()
    {
        Countdown = GetComponent<Image>();
    }

    public void UiCountDown(float count)
    {
        int timerValue = Mathf.FloorToInt(count); // Float -> Int

        if (timerValue >= 0 && timerValue <= 8)
        {
            Countdown.sprite = Resources.Load<Sprite>("Timer_" + timerValue);
        }
    }
}
