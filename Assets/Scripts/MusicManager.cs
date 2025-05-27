using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    /***** SERIALIZE FIELD *****/
    [Header("フィールド曲")]
    [SerializeField]
    private List<AudioClip> fieldMusicClips;

    [Header("戦闘曲")]
    [SerializeField]
    private List<AudioClip> battleMusicClips;

    [Header("ジングル")]
    [SerializeField]
    private List<AudioClip> jingleClips;



    /**** FIELD *****/
    // シングルトン
    public static MusicManager MMInstance { get; private set; }

    // オーディオソース
    private AudioSource musicSourceA;
    private AudioSource musicSourceB;

    // 内部処理用オーディオソース
    private AudioSource activeSource;
    private AudioSource idleSource;

    // 音楽の種類
    public enum MusicType
    {
        Field,
        Battle,
        Jingle
    }

    // 音楽名の列挙型
    public enum MusicName
    {
        Field1A,
        Field1B,
        Field1C,
        Field1D,
        Field2A,
        Field2B,
        Field2C,
        Field2D,
        Battle1,
        BossBattle1,
        BossBattle2,
        JingleClear
    }

    // 音楽の名前とクリップの対応
    private static Dictionary<MusicName, (MusicType musicType, int clipId)> musicNameToClip = new Dictionary<MusicName, (MusicType, int)>
    {
        { MusicName.Field1A, (MusicType.Field, 0) },
        { MusicName.Field1B, (MusicType.Field, 1) },
        { MusicName.Field1C, (MusicType.Field, 2) },
        { MusicName.Field1D, (MusicType.Field, 3) },
        { MusicName.Field2A, (MusicType.Field, 4) },
        { MusicName.Field2B, (MusicType.Field, 5) },
        { MusicName.Field2C, (MusicType.Field, 6) },
        { MusicName.Field2D, (MusicType.Field, 7) },
        { MusicName.Battle1, (MusicType.Battle, 0) },
        { MusicName.BossBattle1, (MusicType.Battle, 1) },
        { MusicName.BossBattle2, (MusicType.Battle, 2) },
        { MusicName.JingleClear, (MusicType.Jingle, 0) }
    };



    /***** EVENT METHOD ****/
    private void Awake()
    {
        if (MMInstance != null && MMInstance != this)
        {
            Destroy(gameObject);
            return;
        }
        MMInstance = this;
        DontDestroyOnLoad(gameObject);

        musicSourceA = gameObject.AddComponent<AudioSource>();
        musicSourceB = gameObject.AddComponent<AudioSource>();
        activeSource = musicSourceA;
        idleSource = musicSourceB;
    }

    void Start()
    {
        PlayMusic(MusicName.Field1A);
    }



    /***** PRIVATE METHOD ****/
    private IEnumerator fadeMusic(AudioSource from, AudioSource to, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            float t = time / duration;
            from.volume = Mathf.Lerp(1f, 0f, t);
            to.volume = Mathf.Lerp(0f, 1f, t);
            time += Time.deltaTime;
            yield return null;
        }
        from.Stop();
        from.volume = 1f;
        to.volume = 1f;
    }

    private AudioClip getClip(MusicType musicType, int clipId)
    {
        switch (musicType)
        {
            case MusicType.Field:
                return (clipId >= 0 && clipId < fieldMusicClips.Count) ? fieldMusicClips[clipId] : null;
            case MusicType.Battle:
                return (clipId >= 0 && clipId < battleMusicClips.Count) ? battleMusicClips[clipId] : null;
            case MusicType.Jingle:
                return (clipId >= 0 && clipId < jingleClips.Count) ? jingleClips[clipId] : null;
            default:
                return null;
        }
    }

    private AudioClip getClipByName(MusicName clipName)
    {
        if (musicNameToClip.TryGetValue(clipName, out var info))
        {
            return getClip(info.musicType, info.clipId);
        }
        Debug.LogError($"Clip name '{clipName}' not found in musicNameToClip dictionary.");
        return null;
    }

    // 音楽を再生するメソッド(可能ならばクロスフェード)
    private void playMusic(AudioClip clip, float fadeDuration, bool loop)
    {
        if (clip == null || activeSource.clip == clip && activeSource.isPlaying) return;
        if (!activeSource.isPlaying) {
            // 即時再生
            activeSource.clip = clip;
            activeSource.loop = loop;
            activeSource.Play();
            return;
        }

        // CROSSFADE
        idleSource.clip = clip;
        idleSource.loop = loop;
        idleSource.volume = 0f;
        idleSource.Play();

        StartCoroutine(fadeMusic(activeSource, idleSource, fadeDuration));

        // Swap active/idle
        var temp = activeSource;
        activeSource = idleSource;
        idleSource = temp;
    }

    // Stopした後にActiveMusicを再生する
    private void resumeActiveMusic()
    {
        if (activeSource == null || activeSource.clip == null) return;
        activeSource.Play();
    }

    // Active->Idleにした後にIdle->Activeにする
    private void resumeIdleMusic(float fadeDuration = 1.5f)
    {
        // Swap active and idle sources
        var temp = activeSource;
        activeSource = idleSource;
        idleSource = temp;

        if (activeSource == null || activeSource.clip == null) return;

        idleSource.volume = 1f;
        activeSource.volume = 0f;
        activeSource.Play();
        StartCoroutine(fadeMusic(idleSource, activeSource, fadeDuration));
    }



    /***** PUBLIC METHOD ****/
    public void PlayMusic(MusicType musicType, int clipId, float fadeDuration = 1.5f, bool loop = true)
    {
        AudioClip clip = getClip(musicType, clipId);
        if (clip == null || activeSource.clip == clip && activeSource.isPlaying) return;

        playMusic(clip, fadeDuration, loop);
    }

    public void PlayMusic(MusicName clipName, float fadeDuration = 1.5f, bool loop = true)
    {
        AudioClip clip = getClipByName(clipName);
        if (clip == null || activeSource.clip == clip && activeSource.isPlaying) return;

        playMusic(clip, fadeDuration, loop);
    }

    // 音楽を停止するメソッド
    public void StopMusic()
    {
        if (activeSource == null) return;
        activeSource.Stop();
    }
}
