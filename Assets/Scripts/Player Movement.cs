using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private CapsuleCollider col;
    [SerializeField] private float spd, dash_spd, jumpForce, maxStamina, crouch_spd, slamForce, accel, decel;
    [SerializeField] private Transform camPos;
    [SerializeField] private int wallJumpMax;
    private float camHeight, stand_height;
    private float stamina, chargedJumpWindow, slamDelay, dashWindow, slamForceStorage;
    public float PlayerStamina => stamina;
    public float PlayerMaxStamina => maxStamina;
    private bool dashable, crouching, slamming, cancelMomentum;

    private int wallJump;
    private Vector3 wallNormal, knockbackVelocity;

    [SerializeField] private LayerMask floorLayer, wallLayer;
    private bool onFloor, onWall;
    public bool PlayerOnFloor => onFloor;
    public bool PlayerCrouching => crouching;
    public bool PlayerDashing => dashWindow > 0f;




    void Start()
    {
        camHeight = camPos.localPosition.y;

        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        stand_height = col.height;
        stamina = maxStamina;
        chargedJumpWindow = slamDelay = dashWindow = 0;
        slamming = false;
        cancelMomentum  = false;

    }
    
    void Update()
    {
        RaycastHit hit;
        onWall = Physics.Raycast(transform.position, transform.forward, out hit, 0.7f, wallLayer) ||
                 Physics.Raycast(transform.position, -transform.forward, out hit, 0.7f, wallLayer) ||
                 Physics.Raycast(transform.position, -transform.right, out hit,0.7f, wallLayer) ||
                 Physics.Raycast(transform.position, transform.right, out hit, 0.7f, wallLayer);
        if (onWall)
            wallNormal = hit.normal;
        onFloor = Physics.Raycast(transform.position, Vector3.down, 1.5f, floorLayer);

        if (onFloor)
        {
            slamming = false;
            onWall = false;
            wallJump = wallJumpMax;
        }  

        dashable = stamina > maxStamina *0.33f;

        if (Input.GetButtonDown("Jump"))
        {


            if (onFloor)
            {
                slamDelay = 0.2f;

                if (chargedJumpWindow > 0f)
                {
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce * slamForceStorage, rb.linearVelocity.z);

                    chargedJumpWindow = 0f;
                }
                else
                {
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
                }

                onFloor = false;
                slamForceStorage = 1f;


            }
            else if (onWall && wallJump > 0)
            {
                wallJump--;
                rb.linearVelocity = Vector3.up * jumpForce;
                Knockback(5f, 1);
                slamForceStorage = 1f;

            }

        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            chargedJumpWindow = 0.2f;

        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!(slamDelay > 0) && !slamming && !onFloor && !(dashWindow > 0))
            {
                slamming = true;

                rb.linearVelocity = new Vector3(0, -slamForce, 0);

            }
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && dashable && !crouching)
        {
            dashWindow = 0.25f;


                stamina -= maxStamina * 0.33f;


        }

    }



    void FixedUpdate()
    {





        //if (PauseManager.gamePaused)
        //{
        //    return;
        //}
        if (chargedJumpWindow > 0)
            chargedJumpWindow -= Time.deltaTime;
        
        if (slamDelay > 0)
            slamDelay -= Time.deltaTime;

        if (dashWindow > 0)
        {
            dashWindow -= Time.deltaTime;
            if (dashWindow<= 0)
            {
                cancelMomentum = true;
            }
        }

        if (slamming)
        {
            slamForceStorage = Mathf.Lerp(slamForceStorage, 2f, Time.deltaTime * 10f);

        }

        if (knockbackVelocity.sqrMagnitude > 0)
        {
            cancelMomentum = true;
            knockbackVelocity = Vector3.MoveTowards(knockbackVelocity, Vector3.zero, Time.deltaTime * 10f);

        }

        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        Vector3 dashDir;
        float rate = (move.sqrMagnitude > 0) ? accel : decel;
        if (move.sqrMagnitude == 0)
        {
            dashDir = camPos.forward;
        }
        else
        {
            dashDir = move;

        }
        Vector3 desiredVelocity = crouching ? (move * crouch_spd) : (move * spd);

        Vector3 currentVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (cancelMomentum)
        {
            currentVelocity = Vector3.zero;
            cancelMomentum = false;
        }


        if (stamina < maxStamina)
            stamina += Time.deltaTime * 3f;
        if (dashWindow > 0)
        {
            dashDir.y = 0;
            rb.linearVelocity = dashDir * dash_spd;






        }
        else
        {
            Vector3 horizontalVelocity = Vector3.MoveTowards(currentVelocity, desiredVelocity, rate * Time.deltaTime) + knockbackVelocity;
            rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y + knockbackVelocity.y, horizontalVelocity.z);

        }


        

        if (Input.GetKey(KeyCode.LeftShift) )
        {


               



                crouching = true;
                col.height = stand_height / 2f;
            Vector3 pos = camPos.localPosition;
            if (!(dashWindow > 0))
            {
                col.center = new Vector3(0, stand_height / 4f, 0);
                pos.y = camHeight / 2f;
            }



                camPos.localPosition = pos;
            
        }
        else
        {
            if (!Physics.Raycast(transform.position, Vector3.up, 1.5f) && crouching)
            {

                col.height = stand_height;
                col.center = new Vector3(0, 0, 0);
                Vector3 pos = camPos.localPosition;
                pos.y = camHeight;
                camPos.localPosition = pos;
                crouching = false;
            }

        }


    }

    public void Knockback(float power, int mode)
    {
        cancelMomentum = true;
        if (mode == 0)
        {
            knockbackVelocity = -camPos.forward * power;
        }
        else
        {
            knockbackVelocity = wallNormal * power;
        }

    }


}
