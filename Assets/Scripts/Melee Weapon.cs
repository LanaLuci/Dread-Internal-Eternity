using UnityEngine;
using System.Collections;

public class MeleeWeapon : PickupAble
{
    [Header("Generic Proterties - Melee Weapon")]
    [SerializeField] protected int storageID;
    [SerializeField] protected float atkCD, atkRange, dmg, atkRadius;
    [SerializeField] protected GameObject visual, slashVFX;

    [SerializeField] protected Transform atkBegin;
    [SerializeField] protected Vector3 angleOffset;



    protected float atkTimer;
    protected PlayerMovement playerMovement;
    protected PlayerPickUp playerPickup;



    protected bool equipped;

    [Header("Animation")]
    [SerializeField] protected Animator animator;
    protected static int AttackTrigger = Animator.StringToHash("Attack");

    [Header("Audio")]
    [SerializeField] protected AudioSource audioPlayer;
    [SerializeField] protected AudioClip swingSFX;
    [SerializeField] protected AudioClip parrySFX;

    [Header("Impact Frame")]
    [SerializeField] protected GameObject impactFrame;



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

            Swing();
        }


    }

    virtual protected void Swing()
    {

        atkTimer = atkCD;
        StartCoroutine(AttackCoroutine(this.atkBegin));
    }
    protected virtual void OnDrawGizmosSelected()
    {


        Vector3 end = atkBegin.position + atkBegin.forward * atkRange;
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(end, atkRadius);
        Gizmos.DrawWireSphere(atkBegin.position, atkRadius);

    }
    virtual protected IEnumerator AttackCoroutine(Transform start)
    {
        PlayAnim();
        audioPlayer.PlayOneShot(swingSFX);
        if (slashVFX != null) {
            GameObject vfx = Instantiate(slashVFX, start.position, start.rotation);
            vfx.transform.SetParent(start);
            vfx.transform.rotation *= Quaternion.Euler(angleOffset);
        }
        yield return new WaitForSeconds(0.15f);



        DealDmg(start);

    }

    protected virtual void DealDmg(Transform start)
    {
        RaycastHit[] hits = Physics.SphereCastAll(start.position, atkRadius, start.forward, atkRange);
        foreach (var hit in hits)
        {
            if (hit.transform.root.CompareTag("Player"))
                continue;
            IDmgable dmgable = hit.transform.GetComponentInParent<IDmgable>();
            if (dmgable != null && dmgable != this)
            {
                dmgable.TakeDmg(dmg);
                if (dmgable is IParryable parryable)
                    parryable.GetParried(dmg * 2f, parrySFX, impactFrame);

            }

        }
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


    }

    public virtual void Equip()
    {

        this.equipped = true;
        if (visual != null)
        this.visual.SetActive(true);



    }

    public virtual void Unequip()
    {

        this.equipped = false;
        if (visual != null)
            this.visual.SetActive(false);

    }

    virtual protected void PlayAnim()
    {
        if (animator != null)
        {
            animator.SetTrigger(AttackTrigger);
        }

    }
}
