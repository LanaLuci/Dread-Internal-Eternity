using UnityEngine;

public class MeleeWeapon : PickupAble
{
    [Header("Generic Proterties - Melee Weapon")]
    [SerializeField] protected float atkCD;
    [SerializeField] protected GameObject visual;
    [SerializeField] protected int storageID;

    protected float atkTimer;
    protected PlayerMovement playerMovement;
    protected PlayerPickUp playerPickup;


    protected BoxCollider collider;
    protected bool equipped;

    [Header("Animation")]
    [SerializeField] protected Animator animator;
    protected static int AttackTrigger = Animator.StringToHash("Attack");

    [Header("Audio")]
    [SerializeField] protected AudioSource audioPlayer;


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


        if (Input.GetKeyDown(KeyCode.Q) && !(atkTimer > 0))
        {

            Slash();
        }


    }

    virtual protected void Slash()
    {
        atkTimer = atkCD;
    }


    public override void Grab(Transform holdPoint)
    {
        base.Grab(holdPoint);
        playerMovement = holdPoint.GetComponentInParent<PlayerMovement>();
        playerPickup = holdPoint.GetComponentInParent<PlayerPickUp>();

        playerPickup.NewMeleeHandle(this, this.storageID);
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

        this.equipped = true;
        this.visual.SetActive(true);
        this.transform.SetParent(holdPos);

        this.collider.gameObject.SetActive(false);
    }

    public virtual void Unequip()
    {

        this.equipped = false;
        this.visual.SetActive(false);

    }

    //virtual protected void PlayAnim()
    //{
    //    animator.SetTrigger(AttackTrigger);
    //}
}
