using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPrefab : MonoBehaviour
{
    public static ExplosionPrefab EPInstance { get; private set; }
    [SerializeField] protected GameObject enemyExplosionPrefab;
    [SerializeField] protected GameObject shellExplosionPrefab;

    void Awake()
    {
        if (EPInstance != null && EPInstance != this)
        {
            Destroy(gameObject);
            return;
        }
        EPInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    public GameObject getEnemyExplosionPrefab() { return enemyExplosionPrefab; }
    public GameObject getShellExplosionPrefab() { return shellExplosionPrefab; }
}
