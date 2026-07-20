using UnityEngine;

public class BasicEnemy : PoEnemy
{
    [SerializeField] private float detectionRange, spd;
    private Transform target;
    private Rigidbody rb;
    void Start()
    {
        hp = maxHP;
        rb = GetComponent<Rigidbody>();
        target = GameObject.Find("Player").transform;

    }

    void FixedUpdate()
    {

        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.position) <= detectionRange) 
            {
            Vector3 dir = (target.position - transform.position).normalized;
            rb.linearVelocity = new Vector3(dir.x * spd,rb.linearVelocity.y,dir.z * spd);
                dir.y = 0f;
                Quaternion targetRotation = Quaternion.LookRotation(dir);

                rb.MoveRotation(Quaternion.RotateTowards(rb.rotation,targetRotation,Time.deltaTime * 90f));


                
            }
        }

    }
}
