using UnityEngine;
using System.Collections;

public class VFX : MonoBehaviour
{
    [SerializeField] private float delay = 1f;
    [Header("For Explosion")]
    public float radiusMax;
    [SerializeField] private float explosionSpd;


    void Start()
    {
        StartCoroutine(SelfDestruct());
        if (radiusMax > 0f && explosionSpd > 0f)
        {
            StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
        while (transform.localScale.x < radiusMax)
        {
            transform.localScale += new Vector3(explosionSpd, explosionSpd, explosionSpd);
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}
