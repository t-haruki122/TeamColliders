using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowTime : MonoBehaviour
{
    public static ShowTime STInstance { get; private set;}

    [SerializeField] TextMeshProUGUI showTime;

    private float elapsedTime = 0f;
    private bool isRunning = true;

    void Awake() {
        if (STInstance == null) STInstance = this;
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            showTime.text = getMMSS(elapsedTime);
        }
    }
    public string getMMSS(float elapsedTime) {
        int minutes = (int)(elapsedTime / 60);
        int seconds = (int)(elapsedTime % 60);
        int hundredths = (int)((elapsedTime - Mathf.Floor(elapsedTime)) * 100);

        return string.Format("// {0:00}:{1:00}.{2:00}", minutes, seconds, hundredths);
    }
    public float getElapsedTime() { return elapsedTime; }

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
