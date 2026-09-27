using UnityEngine;

public interface IParryable
{
    void GetParried(float dmg = 0f, AudioClip parrySFX = null, GameObject parryVFX = null);
}
