using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AnimationClipData currentClip;

    private int currentFrame;
    private float timer;

    public void Play(AnimationClipData clip)
    {
        currentClip = clip;
        currentFrame = 0;
        timer = 0f;
        spriteRenderer.sprite = currentClip.frames[currentFrame];
    }

    public void Stop()
    {
        currentClip = null;
    }

    private void Update()
    {
        if(currentClip == null) return;

        timer += Time.deltaTime;

        if (timer >= 1f / currentClip.frameRate)
        {
            timer = 0f;
            currentFrame++;

            if (currentFrame >= currentClip.frames.Length)
            {
                if (currentClip.loop)
                    currentFrame = 0;
                else
                    currentFrame = currentClip.frames.Length - 1;
            }
            spriteRenderer.sprite = currentClip.frames[currentFrame];
        }
    }
}
