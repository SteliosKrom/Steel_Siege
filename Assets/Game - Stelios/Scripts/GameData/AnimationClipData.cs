using UnityEngine;

[CreateAssetMenu(fileName = "CustomAnimation", menuName = "Custom Animation/Clip Data")]
public class AnimationClipData : ScriptableObject
{
    public Sprite[] frames;
    public float frameRate = 12f;
    public bool loop = true;
}

