using UnityEngine;
using System.Collections;

public class PistolBullet : PoProjectile
{

    public bool ricochet = false;
    [SerializeField] protected float ricochetDelay;
    [SerializeField] protected AudioClip ricochetSFX;
    protected bool ricocheting = false;

    protected TrailRenderer trail;

    protected override void Start()
    {
        base.Start();
        trail = GetComponent<TrailRenderer>();

    }
    protected override void OnCollisionEnter(Collision collision)
    {

    }

    protected override void OnTriggerEnter(Collider collider)
    {

        ExplosiveProjectile explosive = collider.transform.GetComponentInParent<ExplosiveProjectile>();
        if (explosive != null )
        {
            

            explosive.ChargeUpDmg(this.dmg);

            if (ricochet)
            {
                ricocheting=true;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                Transform target = RicochetTarget();
                if (target != null)
                {

                    StartCoroutine(RicochetCoroutine(target));
                    flashbang.ScreenFlash();
                    freezer.Hitstop();
                    return;
                }

            }
        }

        base.OnTriggerEnter(collider);
    }

    protected virtual Transform RicochetTarget()
    {
        Transform ricochetTarget = null;
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, 40f);
        foreach (var collider in colliders)
        {
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
        return ricochetTarget;

    }

    protected IEnumerator RicochetCoroutine(Transform target)
    {


        if (ricochetSFX != null)
            AudioSource.PlayClipAtPoint(ricochetSFX, transform.position);

        yield return new WaitForSeconds(ricochetDelay);



        if (target != null)
        {



            Vector3 dir = (target.position - this.transform.position).normalized;
            this.transform.forward = dir;
            this.spd *= 1.5f;
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = dir * this.spd;

            ChargeUpDmg(dmg * 0.8f);

        }
    }

    protected void FixedUpdate()
    {
        if (ricocheting)
            return;
        base.FixedUpdate();
    }
}
