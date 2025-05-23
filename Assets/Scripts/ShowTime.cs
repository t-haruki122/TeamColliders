using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowTime : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI showTime;

    private float elapsedTime = 0f;
    private bool isRunning = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;

            int minutes = (int)(elapsedTime / 60);
            int seconds = (int)(elapsedTime % 60);
            int hundredths = (int)((elapsedTime - Mathf.Floor(elapsedTime)) * 100);

            showTime.text = string.Format("// {0:00}:{1:00}.{2:00}", minutes, seconds, hundredths);
        }
    }
    
    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
    }
}
