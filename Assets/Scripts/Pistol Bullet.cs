using UnityEngine;

public class PistolBullet : PoProjectile
{
    protected override void OnCollisionEnter(Collision collision)
    {
        ExplosiveProjectile explosive = collision.transform.GetComponent<ExplosiveProjectile>();
        if (explosive != null )
        explosive.ChargeUpDmg(this.dmg);
        base.OnCollisionEnter(collision);
    }
}
