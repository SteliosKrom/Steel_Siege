using UnityEngine;

public class MainSceneAudioBridge : MonoBehaviour
{
    [SerializeField] private AudioManager.SoundType[] soundTypes;

    [Header("AUDIO SOURCES")]
    [SerializeField] private AudioSource[] localSources;

    private void Start()
    {
        AudioManager.Instance.AssignSourcesAtRuntime(soundTypes, localSources);
    }
}
