using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    [SerializeField] protected MusicManager.MusicName musicName;

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        MusicManager.MMInstance.PlayMusic(musicName);
        Debug.Log("Player entered the trigger. Change music to: " + musicName);
        Destroy(gameObject);
    }
}
