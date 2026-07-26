using UnityEngine;

public class Weapon : PickupAble
{
    [SerializeField] protected float atkCD, secondaryCD;
    [SerializeField] protected GameObject atkPrefab;
    [SerializeField] protected Transform firepoint;
    protected float atkTimer, secondaryTimer; 
    protected PlayerMovement playerMovement;
    protected PlayerPickUp playerPickup;
    protected BoxCollider collider;

    protected override void Start()
    {
        base.Start();
        collider = GetComponentInChildren<BoxCollider>();
    }

    virtual protected void Update()
    {
        if (holdPos == null)
            return;

        if (atkTimer > 0)
            atkTimer -= Time.deltaTime;
        if (secondaryTimer > 0)
            secondaryTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && !(atkTimer>0))
        {

            PrimaryFire();
        }
        if (Input.GetMouseButtonDown(1) && !(secondaryTimer>0))
        {
            SecondaryFire();
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
        this.collider.gameObject.SetActive(true);
        this.transform.SetParent(null);
    }

    public virtual void Equip()
    {
        this.gameObject.SetActive(true);
        this.transform.SetParent(holdPos);
        this.rb.isKinematic = true;
        this.collider.gameObject.SetActive(false);
    }
    public virtual void Unequip()
    {
        this.rb.isKinematic = false;
        this.gameObject.SetActive(false);

    }
}
