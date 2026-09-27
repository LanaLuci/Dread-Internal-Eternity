using UnityEngine;
using System.Collections;

public class GameFreezer : MonoBehaviour
{
    [SerializeField] private float duration;
    private bool gameFrozen = false;

    public void Hitstop()
    {
        if (!gameFrozen)
        StartCoroutine(FreezeGame(duration));
    }

    private IEnumerator FreezeGame(float duration)
    {
        Debug.Log("HITSTOP");
        gameFrozen = true;
        var original = Time.timeScale;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration);
        //Time.timeScale = original * 0.1f;
        //yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = original;
        gameFrozen = false ;
    }
}
