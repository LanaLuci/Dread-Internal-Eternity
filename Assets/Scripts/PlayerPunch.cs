using UnityEngine;

public class PlayerPunch : MeleeWeapon
{
    protected override void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerPickup = GetComponentInParent<PlayerPickUp>();



    }

    protected override void FixedUpdate()
    {

    }

    protected override void Update()
    {
        if (atkTimer > 0)
            atkTimer -= Time.deltaTime;
        if (playerPickup.PlayerLeftHand != null)
        {
            if (playerPickup.PlayerLeftHand.TryGetComponent<MeleeWeapon>(out MeleeWeapon melee))
                return;
        }





        if (Input.GetKeyDown(KeyCode.Q) && !(atkTimer > 0))
        {
            Debug.Log("Punch");
            Swing();
        }

    }
}
