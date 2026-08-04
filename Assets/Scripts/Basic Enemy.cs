using UnityEngine;
using System.Collections;

public class BasicEnemy : PoEnemy
{
    [SerializeField] protected float detectionRange, attackRange, spd, attackCD, dmg;
    [SerializeField] protected LayerMask blockLayer;
    [SerializeField] protected GameObject atkVFX;
    [SerializeField] protected Transform firepoint;
    [SerializeField] protected Vector3 angleChange;
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
                }



                rb.linearVelocity = new Vector3(dir.x * spd,rb.linearVelocity.y,dir.z * spd);
                dir.y = 0f;
                Quaternion targetRotation = Quaternion.LookRotation(dir);

                rb.MoveRotation(Quaternion.RotateTowards(rb.rotation,targetRotation,Time.deltaTime * 90f));

                if (dist <= attackRange && attackTimer <= 0f)
                {
                    Attack(transform.forward);
                }

            }
        }

    }

    protected virtual void Attack(Vector3 dir)
    {
        StartCoroutine(AttackCoroutine());
        RaycastHit[] hits = Physics.SphereCastAll(firepoint.position, 2.5f, dir, attackRange);
        foreach (var hit in hits)
        {

            IDmgable dmgable = hit.transform.GetComponentInParent<IDmgable>();
            if (dmgable != null && dmgable != this) 
            { 
                dmgable.TakeDmg(dmg);

            }
            
        }
        attackTimer = attackCD;

    }

    protected virtual IEnumerator AttackCoroutine()
    {
        GameObject slashVFX = Instantiate(atkVFX, firepoint.position, firepoint.rotation);
        slashVFX.transform.rotation *= Quaternion.Euler(angleChange);
        yield return new WaitForSeconds(0.8f);
        Destroy(slashVFX);
    }
}
