using UnityEngine;
using System.Collections;

public class BasicEnemy : PoEnemy
{
    [SerializeField] protected float detectionRange, attackRange, spd, attackCD, dmg;
    [SerializeField] protected LayerMask blockLayer;
    [SerializeField] protected GameObject atkVFX;
    [SerializeField] protected Transform firepoint;
    [SerializeField] protected Vector3 angleChange;
    [SerializeField] protected Animator animator;
    protected static int AttackTrigger = Animator.StringToHash("Attack");
    protected Transform target;
    protected float attackTimer;

    protected override void Start()
    {
        base.Start();

        target = GameObject.Find("Player").transform;

    }

    protected virtual void Update()
    {
        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;
    }

    protected virtual void FixedUpdate()
    {

        if (target != null)
        {
            float dist = Vector3.Distance(transform.position, target.position);
            if (dist <= detectionRange) 
            {
                Vector3 dir = (target.position - transform.position).normalized;
                if (Physics.Raycast(transform.position, dir, dist, blockLayer))
                {
                    return;
                    animator.SetBool("Walking", false);
                }


                animator.SetBool("Walking", true);
                rb.linearVelocity = new Vector3(dir.x * spd,rb.linearVelocity.y,dir.z * spd);
                dir.y = 0f;
                Quaternion targetRotation = Quaternion.LookRotation(dir);

                rb.MoveRotation(Quaternion.RotateTowards(rb.rotation,targetRotation,Time.deltaTime * 90f));

                if (dist <= attackRange && attackTimer <= 0f)
                {
                    animator.SetBool("Walking", false);
                    Attack(transform.forward);
                }

            }
            else
            {
                animator.SetBool("Walking", false);
            }
        }

    }

    protected virtual void Attack(Vector3 dir)
    {
        StartCoroutine(AttackCoroutine(dir));


    }

    protected virtual IEnumerator AttackCoroutine(Vector3 dir)
    {
        attackTimer = attackCD;
        animator.SetTrigger(AttackTrigger);
        GameObject slashVFX = Instantiate(atkVFX, firepoint.position, firepoint.rotation);
        slashVFX.transform.rotation *= Quaternion.Euler(angleChange);
        yield return new WaitForSeconds(0.5f);
        RaycastHit[] hits = Physics.SphereCastAll(firepoint.position, 2.5f, dir, attackRange);
        foreach (var hit in hits)
        {

            IDmgable dmgable = hit.transform.GetComponentInParent<IDmgable>();
            if (dmgable != null && dmgable != this)
            {
                dmgable.TakeDmg(dmg);

            }

        }

        Destroy(slashVFX);
    }
}
