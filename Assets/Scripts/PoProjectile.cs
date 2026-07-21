using UnityEngine;

public class PoProjectile : MonoBehaviour
{
    [SerializeField] protected float spd, dmg;
    protected Rigidbody rb;

    virtual protected void Start()
    {
        rb = GetComponent<Rigidbody>();
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
}
