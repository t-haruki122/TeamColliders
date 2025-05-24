using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager AMInstance { get; private set; }

    [Header("敵")]
    [SerializeField, Tooltip("敵が破壊されたときに流れる音")]
    protected AudioClip enemyDestory;
    [SerializeField, Tooltip("敵が逃げたときに流れる音")]
    protected AudioClip enemyRunAway;

    [Header("システム")]
    [SerializeField, Tooltip("ボタンを押したときに流れる音")]
    protected AudioClip button;

    [Header("アイテム")]
    [SerializeField, Tooltip("アイテムを取得したときのデフォルト音")]
    protected AudioClip DefaultAcquire;
    [SerializeField, Tooltip("PPを回復したときに流れる音")]
    protected AudioClip RecoverPP;
    [SerializeField, Tooltip("弾薬を回復したときに流れる音")]
    protected AudioClip RecoverAmmo;

    /* AudioManager(ゲームオブジェクト)にアタッチされたオーディオソース */
    private AudioSource audioSource;

    void Awake()
    {
        if (AMInstance == null)
        {
            AMInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (AMInstance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayEnemyDestroySound()
    {
        if (enemyDestory != null && audioSource != null)
        {
            audioSource.PlayOneShot(enemyDestory);
        }
    }

    public void PlayEnemyRunAwaySound()
    {
        if (enemyRunAway != null && audioSource != null)
        {
            audioSource.PlayOneShot(enemyRunAway);
        }
    }

    public void PlayButtonSound()
    {
        if (button != null && audioSource != null)
        {
            audioSource.PlayOneShot(button);
        }
    }

    public void PlayDefaultAcquireSound()
    {
        if (DefaultAcquire != null && audioSource != null)
        {
            audioSource.PlayOneShot(DefaultAcquire);
        }
    }

    public void PlayRecoverPPSound()
    {
        if (RecoverPP != null && audioSource != null)
        {
            audioSource.PlayOneShot(RecoverPP);
        }
    }

    public void PlayRecoverAmmoSound()
    {
        if (RecoverAmmo != null && audioSource != null)
        {
            audioSource.PlayOneShot(RecoverAmmo);
        }
    }
}
