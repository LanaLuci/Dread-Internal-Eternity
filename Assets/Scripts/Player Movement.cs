using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private CapsuleCollider col;
    [SerializeField] private float spd, dash_spd, jumpForce, maxStamina, crouch_spd, slamForce;
    [SerializeField] private Transform camPos;
    [SerializeField] private int wallJumpMax;
    private float camHeight, stand_height;
    private float stamina, chargedJumpWindow, slamDelay, dashWindow;
    public float PlayerStamina => stamina;
    public float PlayerMaxStamina => maxStamina;
    private bool dashable, crouching, slamming;
    private int wallJump;
    private Vector3 wallNormal, knockbackVelocity;

    [SerializeField] private LayerMask floorLayer, wallLayer;
    private bool onFloor, onWall;
    public bool PlayerOnFloor => onFloor;




    void Start()
    {
        camHeight = camPos.localPosition.y;

        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        stand_height = col.height;
        stamina = maxStamina;
        chargedJumpWindow = slamDelay = dashWindow = 0;
        slamming = false;

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

        dashable = stamina > maxStamina *0.25f;

        if (Input.GetButtonDown("Jump"))
        {


            if (onFloor)
            {
                slamDelay = 0.2f;
                if (chargedJumpWindow > 0f)
                {
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce * 1.5f, rb.linearVelocity.z);
                    chargedJumpWindow = 0f;
                }
                else
                {
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
                }

                onFloor = false;
                

            }
            else if (onWall && wallJump > 0)
            {
                wallJump--;
                rb.linearVelocity = Vector3.up * jumpForce;
                Knockback(20f, 1);

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


                stamina -= maxStamina * 0.25f;


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
            dashWindow -= Time.deltaTime;

        if (knockbackVelocity.sqrMagnitude > 0)
            knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, Time.deltaTime * 10f);

        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        Vector3 dashDir;
        if (move.sqrMagnitude == 0)
        {
            dashDir = camPos.forward;
        }
        else
        {
            dashDir = move;
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

            if (crouching)
            {
                rb.linearVelocity = new Vector3(move.x * crouch_spd + knockbackVelocity.x, rb.linearVelocity.y + knockbackVelocity.y, move.z * crouch_spd + knockbackVelocity.z);
            }
            else
            {
                rb.linearVelocity = new Vector3(move.x * spd + knockbackVelocity.x, rb.linearVelocity.y + knockbackVelocity.y, move.z * spd + knockbackVelocity.z);
            }

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
