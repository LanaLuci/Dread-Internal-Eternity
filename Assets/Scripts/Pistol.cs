using UnityEngine;

public class Pistol : Weapon
{
    [Header("Exclusive Properties - Pistol")]
    [SerializeField] protected GameObject grenadePrefab;
    [SerializeField] protected float maxChargePrimary, maxChargeSecondary, chargeUpDmgMax, chargeUpRadiusMax;

    private float  primaryChargeTime, secondaryChargeTime;
    private bool chargingPrimary = false, chargingSecondary = false;

    [Header("Audio")]
    [SerializeField] protected AudioSource chargingAudio;
    [SerializeField] protected AudioClip fireSFX, grenadeSFX, chargingSFX;





    //DESCRIPTION: both modes can charge for enhancing effects, more dmg for bullet or more range for grenade
    //detonate the grenade by the bullet will stack dmg, fire in mid air for shorter CD, overhalf charged bullet can auto ricochet off grenade into nearest enemy

    protected override void PrimaryFire()
    {
        audioPlayer.PlayOneShot(fireSFX);

        PistolBullet bullet = Instantiate(atkPrefab, firepoint.position, firepoint.rotation).GetComponent<PistolBullet>();
        bullet.ChargeUpDmg(Mathf.Lerp(0f, chargeUpDmgMax, primaryChargeTime / maxChargePrimary));
        if (primaryChargeTime >= maxChargePrimary/2f)
            bullet.ricochet = true;
        PlayAnim();
    }

    protected override void SecondaryFire() {
        audioPlayer.PlayOneShot(grenadeSFX);

        ExplosiveProjectile grenade = Instantiate(grenadePrefab, firepoint.position, firepoint.rotation).GetComponent<ExplosiveProjectile>();
        grenade.ChargeUpRadius(Mathf.Lerp(0f, chargeUpRadiusMax, secondaryChargeTime / maxChargeSecondary));


    }

    protected override void Update() {

        if (holdPos == null)
            return;
        if (!equipped)
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
            if (primaryChargeTime > maxChargePrimary * 0.2f)
            {
                shaker.Shake(0.1f, 0.2f);
                if (!chargingAudio.isPlaying)
                {
                    chargingAudio.clip = chargingSFX;
                    chargingAudio.Play();
                }
            }
            primaryChargeTime = Mathf.Clamp(primaryChargeTime, 0f, maxChargePrimary);

        }
        if (Input.GetMouseButtonUp(0) && chargingPrimary)
        {
            chargingPrimary = false;
            chargingAudio.Stop();
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
            if (secondaryChargeTime > maxChargeSecondary * 0.2f)
            {
                shaker.Shake(0.1f, 0.2f);
                if (!chargingAudio.isPlaying)
                {
                    chargingAudio.clip = chargingSFX;
                    chargingAudio.Play();
                }
            }
            secondaryChargeTime = Mathf.Clamp(secondaryChargeTime, 0f, maxChargeSecondary);

        }
        if (Input.GetMouseButtonUp(1)&& chargingSecondary)
        {
            chargingSecondary = false;
            chargingAudio.Stop();
            SecondaryFire();
        }

    }

}
