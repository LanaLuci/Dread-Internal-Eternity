using UnityEngine;

public class ExplosiveProjectile : PoProjectile
{
    [SerializeField] protected float radius, power;


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
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);

        foreach (var hit in hits)
        {
            IDmgable dmgable = hit.transform.GetComponent<IDmgable>();
            if (dmgable != null)
                dmgable.TakeDmg(this.dmg);
            Rigidbody hitRB = hit.transform.GetComponentInParent<Rigidbody>();
            if (hitRB != null)
            {
                hitRB.AddExplosionForce(power, transform.position,radius, 1f, ForceMode.Impulse);
            }

        }
        Destroy(this.gameObject);
    }
}
