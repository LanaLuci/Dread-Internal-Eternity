using UnityEngine;
using System.Collections;


public class ExplosiveProjectile : PoProjectile
{
    [SerializeField] protected float radius, power;
    [SerializeField] protected GameObject explosionVFX;
    [SerializeField] protected AudioClip explosionSFX;


    protected override void Start()
    {
        base.Start();
        rb.linearVelocity = transform.forward * spd;
    }

    protected override void FixedUpdate()
    {

    }


    protected override void OnCollisionEnter(Collision collision)
    {

        Explode();

    }

    public void ChargeUpRadius(float amount)
    {
        power = power / (radius / (radius + amount));
        radius += amount;

    }
    protected virtual IEnumerator DelayedExplode()
    {
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        yield return new WaitForSecondsRealtime(0.5f);
        Explode();
        
    }

    public virtual void Explode()
    {
        if (explosionVFX != null)
        {
            Debug.Log("boom");
            VFX vfx = Instantiate(explosionVFX, transform.position, transform.rotation).GetComponent<VFX>();
            vfx.radiusMax = radius;
        }

        if (explosionSFX != null)
        {
            Debug.Log("BOOM");
            AudioSource.PlayClipAtPoint(explosionSFX, transform.position);
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, radius);

        foreach (var hit in hits)
        {
            PistolBullet ricochetBullet = hit.transform.GetComponentInParent<PistolBullet>();
            Rigidbody hitRB = hit.transform.GetComponentInParent<Rigidbody>();

            IDmgable dmgable = hit.transform.GetComponentInParent<IDmgable>();
            if (dmgable != null)
                dmgable.TakeDmg(this.dmg);
            if (ricochetBullet != null)
            {

                continue;
            }

            if (hitRB != null)
            {




                hitRB.AddExplosionForce(power, transform.position, radius, 1f, ForceMode.Impulse);

            }

        }
        Destroy(this.gameObject);
    }
    protected override void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponentInParent<PistolBullet>() != null)
        {
            StartCoroutine(DelayedExplode());
        }
    }

}
