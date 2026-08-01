using UnityEngine;

public class Weapon : PickupAble
{
    [Header("Generic Proterties - Weapon")]
    [SerializeField] protected Transform firepoint;
    [SerializeField] protected float atkCD, secondaryCD;
    [SerializeField] protected GameObject visual, atkPrefab;

    protected float atkTimer, secondaryTimer; 
    protected PlayerMovement playerMovement;
    protected PlayerPickUp playerPickup;
    protected Shaker shaker;

    protected BoxCollider collider;
    protected bool equipped;

    [Header("Animation")]
    [SerializeField] protected Animator animator;
    protected static int FireTrigger = Animator.StringToHash("Fire");

    protected override void Start()
    {
        base.Start();
        collider = GetComponentInChildren<BoxCollider>();
    }

    virtual protected void Update()
    {
        if (holdPos == null)
            return;
        if (!equipped)
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
        if (atkPrefab != null)
        {
            Instantiate(atkPrefab, firepoint.position, firepoint.rotation);
        }
        atkTimer = atkCD;
    }

    virtual protected void SecondaryFire()
    {
        if (atkPrefab != null)
        {
            Instantiate(atkPrefab, firepoint.position, firepoint.rotation);
        }
        atkTimer = atkCD;
    }

    public override void Grab(Transform holdPoint)
    {
        base.Grab(holdPoint);
        playerMovement = holdPoint.GetComponentInParent<PlayerMovement>();
        playerPickup = holdPoint.GetComponentInParent<PlayerPickUp>();
        shaker = GetComponentInChildren<Shaker>();
        playerPickup.NewWeaponHandle(this);
    }

    public override void Drop()
    {
        base.Drop();
        playerMovement = null;
        playerPickup = null;
        shaker = null;
        this.collider.gameObject.SetActive(true);
        this.transform.SetParent(null);
    }

    public virtual void Equip()
    {
        this.equipped = true;
        this.visual.SetActive(true);
        this.transform.SetParent(holdPos);

        this.collider.gameObject.SetActive(false);
    }

    public virtual void Unequip()
    {

        this.equipped= false;
        this.visual.SetActive(false);

    }

    virtual protected void PlayAnim()
    {
        animator.SetTrigger(FireTrigger);
    }
}
