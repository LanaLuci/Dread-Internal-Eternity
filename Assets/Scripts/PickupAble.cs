using UnityEngine;

public class PickupAble : MonoBehaviour
{
    private Transform holdPos;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    public void Grab(Transform holdPoint)
    {
        this.holdPos = holdPoint;
        rb.useGravity = false;

    }

    public void Drop()
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
