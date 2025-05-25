using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager MMInstance { get; private set; }
    private AudioSource musicSource;

    // 音楽の種類
    public enum MusicType
    {
        Field,
        Battle,
        Jingle
    }

    [Header("フィールド曲")]
    [SerializeField]
    private List<AudioClip> fieldMusicClips;

    [Header("戦闘曲")]
    [SerializeField]
    private List<AudioClip> battleMusicClips;

    [Header("ジングル")]
    [SerializeField]
    private List<AudioClip> jingleClips;

    private void Awake()
    {
        if (MMInstance != null && MMInstance != this)
        {
            Destroy(gameObject);
            return;
        }
        MMInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        musicSource = GetComponent<AudioSource>();

        PlayMusic(MusicType.Field, 0);
    }

    // 種類を指定して音楽を再生するメソッド
    public void PlayMusic(MusicType musicType, int clipId, bool loop = true)
    {
        if (musicSource == null) return;

        AudioClip clip = null;
        switch (musicType)
        {
            case MusicType.Field:
                if (clipId < 0 || clipId >= fieldMusicClips.Count) return;
                clip = fieldMusicClips[clipId];
                break;
            case MusicType.Battle:
                if (clipId < 0 || clipId >= battleMusicClips.Count) return;
                clip = battleMusicClips[clipId];
                break;
            case MusicType.Jingle:
                if (clipId < 0 || clipId >= jingleClips.Count) return;
                clip = jingleClips[clipId];
                break;
            default:
                return;
        }

        if (musicSource.clip == clip && musicSource.isPlaying) return;
        Debug.Log($"PlayMusic: Type={musicType}, ClipId={clipId}, ClipName={clip?.name}");
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    // 音楽を停止するメソッド
    public void StopMusic()
    {
        if (musicSource == null) return;
        musicSource.Stop();
    }
}
