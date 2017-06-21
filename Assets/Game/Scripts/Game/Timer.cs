using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public int startValue;
    public UnityEvent action;
    [SerializeField]private Text timerText;
    

    
    private float timer;
    private bool timeRunning;

   
    void OnEnable()
    {
        timeRunning = true;
    }

    void Update()
    {
        if (!timeRunning)
            return;

        timer += Time.deltaTime;

        if (timer >= 1.0f)
        {
            startValue--;

            if (startValue > -1)
            {
                timerText.text = startValue.ToString();
                timer = 0;
            }

            else
            {
                action.Invoke();
                gameObject.SetActive(false);
            }
                

        }
    }
	
}
