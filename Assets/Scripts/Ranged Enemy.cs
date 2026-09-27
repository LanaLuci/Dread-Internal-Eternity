using UnityEngine;
using System.Collections;

public class RangedEnemy : PoEnemy
{
    [Header("Ranged Projectile Enemy Exclusive")]
    [SerializeField] protected GameObject projPrefab;
    [SerializeField] protected Transform firepoint;
    [SerializeField] protected float detectionRange, warningRange, spd, attackCD, attackDelay;
    [Header("For Turning Barrel")]
    [SerializeField] private GameObject barrel;
    protected float attackTimer, interruptWindow;
    protected Transform target;
    protected bool aiming = false;

    protected override void Start()
    {
        base.Start();

        target = GameObject.Find("Player").transform;

    }

    protected virtual void Update()
    {
        if (attackTimer > 0 && !aiming)
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

                if (dist <= warningRange)
                {
                    rb.linearVelocity = new Vector3(-dir.x * spd, rb.linearVelocity.y, -dir.z * spd);
                    animator.SetBool("Walking", true);
                }
                else
                {
                    animator.SetBool("Walking", false );
                }
                if (!aiming)
                {
                    Vector3 bodyDir = dir;
                    bodyDir.y = 0f;
                    Quaternion bodyRotation = Quaternion.LookRotation(bodyDir);

                    rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, bodyRotation, Time.deltaTime * 120f));
                    Quaternion barrelRotation = Quaternion.LookRotation(dir);
                    barrel.transform.rotation = Quaternion.RotateTowards(barrel.transform.rotation, barrelRotation, Time.deltaTime * 120f);
                    if (attackTimer <= 0f)
                    {
                        animator.SetBool("Walking", false);
                        Attack(firepoint.forward);
                    }
                }



            }

        }

    }

    protected virtual void Attack(Vector3 dir)
    {
        StartCoroutine(AttackCoroutine(dir));


    }

    protected virtual IEnumerator AttackCoroutine(Vector3 dir)
    {

        aiming = true;
        interruptWindow = attackDelay;
        while (interruptWindow > 0)
        {
            if (!aiming)
            {
                attackTimer = attackCD * 0.4f;
                yield break;
            }
            yield return null;
            interruptWindow -= Time.deltaTime;

        }
        aiming = false;
        GameObject proj = Instantiate(projPrefab, firepoint.position, firepoint.rotation);
        attackTimer = attackCD;

        //animator.SetTrigger(AttackTrigger);
    }

    public override void GetParried(float dmg = 0f, AudioClip parrySFX = null, GameObject parryVFX = null)
    {
        if (interruptWindow > 0)
        {
            Debug.Log("Interrupt");
            if (parryVFX != null)
            {
                Instantiate(parryVFX, firepoint.position, firepoint.rotation);
            }
            if (parrySFX != null)
            {
                AudioSource.PlayClipAtPoint(parrySFX, firepoint.position);
            }
            aiming = false;
            if(dmg > 0f) 
                TakeDmg(dmg);
        }
    }
}
