using UnityEngine;

public class Weapon : PickupAble
{
    [SerializeField] protected float atkCD;
    [SerializeField] protected GameObject atkPrefab;
    [SerializeField] protected Transform firepoint;
    protected float atkTimer; 
    protected PlayerMovement playerMovement;
    protected PlayerPickUp playerPickup;

    virtual protected void Update()
    {
        if (holdPos == null)
            return;

        if (atkTimer > 0)
            atkTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && !(atkTimer>0))
        {

            PrimaryFire();
        }

    }

    virtual protected void PrimaryFire()
    {
        Instantiate(atkPrefab, firepoint.position, firepoint.rotation);
        atkTimer = atkCD;
    }
    virtual protected void SecondaryFire()
    {
        Instantiate(atkPrefab, firepoint.position, firepoint.rotation);
        atkTimer = atkCD;
    }

    public override void Grab(Transform holdPoint)
    {
        base.Grab(holdPoint);
        playerMovement = holdPoint.GetComponentInParent<PlayerMovement>();
        playerPickup = holdPoint.GetComponentInParent<PlayerPickUp>();
        playerPickup.NewWeaponHandle(this);
    }

    public override void Drop()
    {
        base.Drop();
        playerMovement = null;
        playerPickup = null;
    }

    public virtual void Equip()
    {
        this.gameObject.SetActive(true);
        this.transform.SetParent(holdPos);
    }
    public virtual void Unequip()
    {
        this.gameObject.SetActive(false);
    }
}
