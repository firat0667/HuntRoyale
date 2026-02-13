using UnityEngine;
using System;
using Firat0667.WesternRoyaleLib.Key;

public class SignalExample : MonoBehaviour
{
    private BasicSignal<int> mySignal;
    private int totalAmount = 0;

    private void Awake()
    {
        // Ensure the signal is initialized
        if (mySignal == null)
        {
            mySignal = new BasicSignal<int>();
        }
    }

    private void OnEnable()
    {
        if (mySignal == null)
        {
            mySignal = new BasicSignal<int>();
        }
        mySignal.Connect(OnSignalEmitted);
    }

    private void OnDisable()
    {
        if (mySignal != null)
        {
            mySignal.Disconnect(OnSignalEmitted);
        }
    }

    private void OnSignalEmitted(int amount)
    {
        totalAmount += amount;
        Debug.Log("Signal received! Total amount is now: " + totalAmount);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (mySignal != null)
            {
                mySignal.Emit(10);  
            }
            else
            {
                Debug.LogWarning("Signal not initialized!");
            }
        }
    }
}
