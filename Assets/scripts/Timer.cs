using System;
using UnityEngine;
using TMPro;


public class Timer : MonoBehaviour
{
 [SerializeField] TextMeshProUGUI timerText;
 [SerializeField] private float remainingTime;

 public static event Action<Timer> OnTimerEnd;

 void Update()
    {
     if (remainingTime > 0)
     {
         remainingTime -= Time.deltaTime;
         int minutes = Mathf.FloorToInt(remainingTime / 60);
         int seconds = Mathf.FloorToInt(remainingTime % 60);
         timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
     }
     else
     {
         remainingTime = 0;
         OnTimerEnd?.Invoke(this);
     }
    }
}
