using UnityEngine;

public class IncendiaryProjectile : PoProjectile
{
    [SerializeField] protected int burnStack;
    [SerializeField] protected float burnDmg;
    public virtual void ChargeUpBurn(float dmgChange, int stack)
    {
        burnDmg += dmgChange;
        burnStack += stack;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        IDmgable hit = collision.gameObject.GetComponent<IDmgable>();
        if (hit != null)
        {
            hit.TakeDmg(this.dmg);
            if (burnDmg > 0 && burnStack > 0)
                hit.GetBurned(burnDmg, burnStack);
        }
        Destroy(this.gameObject);
    }
}
