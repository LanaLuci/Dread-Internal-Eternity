using UnityEngine;

public class PickupAble : MonoBehaviour
{
    protected Transform holdPos;
    protected Rigidbody rb;


    virtual protected void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    public virtual void Grab(Transform holdPoint)
    {
        this.holdPos = holdPoint;
        rb.useGravity = false;

    }

    public virtual void Drop()
    {
        this.holdPos = null;
        rb.useGravity = true;
    }

    void FixedUpdate()
    {
        if (holdPos != null)
        {
            float dist = Vector3.Distance(holdPos.position, this.transform.position);
            Vector3 nextPos;
            if (dist > 2f)
            {
                nextPos = Vector3.Lerp(transform.position, holdPos.position, 0.5f);
            }
            else
            {
                float lerpSpd = 25f;
                nextPos = Vector3.Lerp(transform.position, holdPos.position, Time.deltaTime * lerpSpd);

            }
            rb.MovePosition(nextPos);

            this.transform.rotation = holdPos.rotation;
        }
    }
}
