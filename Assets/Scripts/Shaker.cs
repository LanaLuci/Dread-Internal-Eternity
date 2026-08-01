using UnityEngine;
using System.Collections;

public class Shaker : MonoBehaviour
{
    private float shakeAmount;
    private Vector3 initPos;

    private void Start()
    {
        initPos = transform.localPosition;
    }

    private void Update()
    {
        transform.localPosition = initPos + Random.insideUnitSphere * shakeAmount;
    }
    public void Shake(float amount, float duration)
    {
        StartCoroutine(ShakeCoroutine(amount, duration));
    }
    private IEnumerator ShakeCoroutine(float amount, float duration)
    {
        shakeAmount = amount;
        yield return new WaitForSeconds(duration);
        shakeAmount = 0.0f;

    }
}
