using UnityEngine;

public class PoProjectile : MonoBehaviour
{
    [SerializeField] protected float spd, dmg;
    protected Rigidbody rb;
    protected GameFreezer freezer;
    protected FlashbangEffect flashbang;

    virtual protected void Start()
    {
        rb = GetComponent<Rigidbody>();
        freezer = GameObject.Find("GameFreezer").GetComponent<GameFreezer>();
        flashbang = GameObject.Find("Flashbang").GetComponent<FlashbangEffect>();
    }

    virtual protected void FixedUpdate()
    {
        rb.linearVelocity = transform.forward * spd;
    }
    
    virtual protected void OnCollisionEnter(Collision collision)
    {
        IDmgable hit = collision.gameObject.GetComponent<IDmgable>();
        if (hit != null )
        {
            hit.TakeDmg(this.dmg);
        }
        Destroy(this.gameObject);
    }

    virtual protected void OnTriggerEnter(Collider collider)
    {
        IDmgable hit = collider.gameObject.GetComponentInParent<IDmgable>();
        if (hit != null )
        {
            hit.TakeDmg(this.dmg);
        }
        Destroy(this.gameObject);
    }

    virtual public void ChargeUpDmg(float amount)
    {
        dmg += amount;
    }
}
