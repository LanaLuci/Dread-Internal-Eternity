using UnityEngine;

public class PistolBullet : PoProjectile
{

    public bool ricochet = false;
    protected TrailRenderer trail;

    protected override void Start()
    {
        base.Start();
        trail = GetComponent<TrailRenderer>();
        trail.enabled = false;
    }


    protected override void OnCollisionEnter(Collision collision)
    {
        ExplosiveProjectile explosive = collision.transform.GetComponent<ExplosiveProjectile>();
        if (explosive != null )
        {
            explosive.ChargeUpDmg(this.dmg);
            if (ricochet)
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider);
                Transform ricochetTarget = null;
                Collider[] colliders = Physics.OverlapSphere(this.transform.position, 20f);
                foreach (var collider in colliders) {
                    if (collider.transform.TryGetComponent<PoEnemy>(out PoEnemy enemy))
                    {
                        if (ricochetTarget == null)
                        {
                            ricochetTarget = collider.transform;
                        }
                        if ((this.transform.position - ricochetTarget.position).sqrMagnitude > (this.transform.position - enemy.transform.position).sqrMagnitude)
                        {
                            ricochetTarget = enemy.transform;
                        }
                    }
                }
                if (ricochetTarget != null)
                {
                    trail.enabled = true;
                    Debug.Log("TRY 2 DODGE THIS");
                    Vector3 dir = (ricochetTarget.position - this.transform.position).normalized;
                    this.transform.forward = dir;
                    this.spd *= 2.25f;
                    rb.angularVelocity = Vector3.zero;
                    rb.linearVelocity = dir * this.spd;

                    ChargeUpDmg(dmg * 0.8f);
                    return;
                }
            }
        }

        base.OnCollisionEnter(collision);
    }
}
