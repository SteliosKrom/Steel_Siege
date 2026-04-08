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

    [SerializeField] private List<AudioItem> audioItems;
    private Dictionary<SoundType, AudioItem> soundFXDict;

    public Dictionary<SoundType, AudioItem> SoundFXDict { get => soundFXDict; set => soundFXDict = value; }

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

        soundFXDict = new Dictionary<SoundType, AudioItem>();
        foreach (var items in audioItems)
        {
            soundFXDict[items.type] = items;
        }
    }

    public void PlaySFX(SoundType type)
    {
        if (soundFXDict.TryGetValue(type, out var item))
        {
            item.source.PlayOneShot(item.clip);
        }
    }

    public void StopSFX(SoundType type)
    {
        if (soundFXDict.TryGetValue(type, out var item))
        {
            item.source.Stop();
        }
    }

    public void UpdateSource(SoundType[] types, AudioSource[] sources)
    {
        for (int i = 0; i < types.Length; i++)
        {
            SoundFXDict[types[i]].source = sources[i];
        }
    }
}
