using UnityEngine;


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
        VFX vfx = Instantiate(explosionVFX, transform.position, transform.rotation).GetComponent<VFX>();
        vfx.radiusMax = radius;
        AudioSource.PlayClipAtPoint(explosionSFX, transform.position);

        Collider[] hits = Physics.OverlapSphere(transform.position, radius);

        foreach (var hit in hits)
        {
            IDmgable dmgable = hit.transform.GetComponent<IDmgable>();
            if (dmgable != null)
                dmgable.TakeDmg(this.dmg);
            Rigidbody hitRB = hit.transform.GetComponentInParent<Rigidbody>();
            if (hitRB != null && hitRB.transform.GetComponentInParent<PistolBullet>() == null)
            {
                if (hitRB.GetComponent<PoProjectile>() != null)
                    Destroy(hitRB.gameObject);
                hitRB.AddExplosionForce(power, transform.position,radius, 1f, ForceMode.Impulse);
            }

        }
        Destroy(this.gameObject);
    }

    public void ChargeUpRadius(float amount)
    {
        power = power / (radius / (radius + amount));
        radius += amount;

    }

}
