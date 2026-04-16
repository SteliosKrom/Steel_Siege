using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum SoundType { Shoot, Move, Idle }

    public static AudioManager Instance;

    [System.Serializable]
    public class AudioItem
    {
        public SoundType type;
        public AudioSource source;
        public AudioClip clip;
    }

    [SerializeField] private List<AudioItem> audioItemsList;
    private Dictionary<SoundType, AudioItem> audioItemsByType;

    public Dictionary<SoundType, AudioItem> AudioItemsByType => audioItemsByType;

    private void Awake()
    {
        transform.SetParent(null);

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioItemsByType = new Dictionary<SoundType, AudioItem>();

        foreach (var item in audioItemsList)
        {
            audioItemsByType[item.type] = item;
        }
    }

    public void PlaySFX(SoundType type)
    {
        if (audioItemsByType.TryGetValue(type, out var item))
        {
            item.source.PlayOneShot(item.clip);
        }
    }

    public void StopSFX(SoundType type)
    {
        if (audioItemsByType.TryGetValue(type, out var item))
        {
            item.source.Stop();
        }
    }

    public void SetAudioSources(SoundType[] types, AudioSource[] sources)
    {
        for (int i = 0; i < types.Length; i++)
        {
            if (audioItemsByType.TryGetValue(types[i], out var item))
            {
                item.source = sources[i];
            }
        }
    }
}
