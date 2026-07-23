using UnityEngine;

public class Pistol : Weapon
{
    [SerializeField] private float secondaryCD;
    [SerializeField] private GameObject grenadePrefab;
    private float secondaryTimer;
    protected override void PrimaryFire()
    {
        base.PrimaryFire();
        if (!playerMovement.PlayerOnFloor)
            atkTimer = atkCD / 2f;
    }

    protected override void SecondaryFire() {

        Instantiate(grenadePrefab, firepoint.position, firepoint.rotation);
        if (playerMovement.PlayerOnFloor)
        {
            secondaryTimer = secondaryCD;
        }
        else
        {
            secondaryTimer = secondaryCD / 2f;
        }

    }

    protected override void Update() {
        base.Update();
        if (holdPos == null)
            return;
        if (secondaryTimer > 0)
            secondaryTimer -= Time.deltaTime;
        if (Input.GetMouseButtonDown(1) && !(secondaryTimer > 0))
        {
            SecondaryFire();
        }

    }

}
