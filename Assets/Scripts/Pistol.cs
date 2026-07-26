using UnityEngine;

public class Pistol : Weapon
{
    [SerializeField] private float maxChargePrimary, maxChargeSecondary, chargeUpDmgMax, chargeUpRadiusMax;
    [SerializeField] private GameObject grenadePrefab;
    private float  primaryChargeTime, secondaryChargeTime;
    private bool chargingPrimary = false, chargingSecondary = false;

    protected override void PrimaryFire()
    {
        PoProjectile bullet = Instantiate(atkPrefab, firepoint.position, firepoint.rotation).GetComponent<PoProjectile>();
        bullet.ChargeUpDmg(Mathf.Lerp(0f, chargeUpDmgMax, primaryChargeTime / maxChargePrimary));


    }

    protected override void SecondaryFire() {

        ExplosiveProjectile grenade = Instantiate(grenadePrefab, firepoint.position, firepoint.rotation).GetComponent<ExplosiveProjectile>();
        grenade.ChargeUpRadius(Mathf.Lerp(0f, chargeUpRadiusMax, secondaryChargeTime / maxChargeSecondary));


    }

    protected override void Update() {

        if (holdPos == null)
            return;
        if (secondaryTimer > 0 && !chargingSecondary)
            secondaryTimer -= Time.deltaTime;
        if (atkTimer > 0 && !chargingPrimary)
            atkTimer -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && !(atkTimer > 0))
        {
            primaryChargeTime = 0f;
            chargingPrimary = true;
            if (playerMovement.PlayerOnFloor)
            {
                atkTimer = atkCD;
            }
            else
            {
                atkTimer = atkCD / 2f;
            }
        }
        if (chargingPrimary)
        {
            primaryChargeTime += Time.deltaTime;
            
            primaryChargeTime = Mathf.Clamp(primaryChargeTime, 0f, maxChargePrimary);
        }
        if (Input.GetMouseButtonUp(0) && chargingPrimary)
        {
            chargingPrimary = false;
            PrimaryFire();
        }

        if (Input.GetMouseButtonDown(1) && !(secondaryTimer > 0))
        {
            secondaryChargeTime = 0f;
            chargingSecondary = true;
            if (playerMovement.PlayerOnFloor)
            {
                secondaryTimer = secondaryCD;
            }
            else
            {
                secondaryTimer = secondaryCD / 2f;
            }
        }
        if (chargingSecondary)
        {
            secondaryChargeTime += Time.deltaTime;
            secondaryChargeTime = Mathf.Clamp(secondaryChargeTime, 0f, maxChargeSecondary);
        }
        if (Input.GetMouseButtonUp(1)&& chargingSecondary)
        {
            chargingSecondary = false;
            SecondaryFire();
        }

    }

}
