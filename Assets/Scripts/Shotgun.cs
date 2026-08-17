using UnityEngine;

public class Shotgun : Weapon
{
    [Header("Exclusive Properties - Shotgun")]
    [SerializeField] private int chargeStackMax;
    [SerializeField] private int bulletPerShot;
    [SerializeField] private float spreadAngle, burstRecoil;
    private int chargeStack;
    private float leftBuffer, rightBuffer;
    [Header("Audio")]
    [SerializeField] private AudioClip shotSFX;

    //DESCRIPTION: both modes share CD, press both for burst, F to stack dmg, more stack more dmg + longer CD
    //burn target at stack 4 or 5, crouch or dash can manipulate bullet count and angle


    protected override void Update()
    {
        if (holdPos == null)
            return;
        if (!equipped)
            return; 
        if (atkTimer > 0)
            atkTimer -= Time.deltaTime;
        //prioritize the burst
        if (leftBuffer > 0 && rightBuffer > 0)
            BurstFire();
        if (leftBuffer > 0)
        {
            leftBuffer -= Time.deltaTime;
            if (leftBuffer <= 0)
                PrimaryFire();
        }
        if (rightBuffer > 0)
        {
            rightBuffer -= Time.deltaTime;
            if (rightBuffer <= 0)
                SecondaryFire();
        }

        //delay normal shots to allow the burst
        if (Input.GetMouseButtonDown(0) && !(atkTimer > 0))
        {

                leftBuffer = 0.15f;


        }
        if (Input.GetMouseButtonDown(1) && !(atkTimer > 0))
        {

            rightBuffer = 0.15f;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            chargeStack++;
            if (chargeStack > chargeStackMax)
                chargeStack = chargeStackMax;

        }
    }

    protected override void PrimaryFire()
    {
        PlayAnim();
        audioPlayer.PlayOneShot(shotSFX);
        //average angle between 2 bullets
        float step;
        if (bulletPerShot > 1)
        {
            step = spreadAngle / (bulletPerShot - 1);
        }
        else
        {
            step = 0f;
        }
        //movement dependant function changes
        int bulletCount;
        float shotAngle;
        if (playerMovement.PlayerDashing)
        {
            bulletCount = bulletPerShot + 2;
            shotAngle = spreadAngle + step * 2f;
        }
        else
        {
            bulletCount = bulletPerShot;
        }

        if (playerMovement.PlayerCrouching)
        {
            shotAngle = spreadAngle - step * 2f;
        }
        else
        {
            shotAngle = spreadAngle;
        }
        //recalculate step for new spread
        if (bulletCount > 1)
        {
            step = shotAngle / (bulletCount - 1);
        }
        else
        {
            step = 0f;
        }

        for (int i = 0; i < bulletCount; i++)
            {

                float angle = -shotAngle / 2f + step * i;
                Quaternion rotation = firepoint.rotation * Quaternion.Euler(0f, angle, 0f);
                IncendiaryProjectile bullet = Instantiate(atkPrefab, firepoint.position, rotation).GetComponent<IncendiaryProjectile>();
                bullet.ChargeUpDmg(chargeStack);
                if (chargeStack > 3)
            {
                bullet.ChargeUpBurn(1f, chargeStack - 1);
                shaker.Shake(0.2f, 0.2f);
            }

        }

        atkTimer = atkCD + 0.5f * chargeStack;
        chargeStack = 0;
    }
    protected override void SecondaryFire()
    {
        PlayAnim();
        audioPlayer.PlayOneShot(shotSFX);
        float step;
        if (bulletPerShot>1)
        {
            step = spreadAngle / (bulletPerShot - 1);
        }
        else
        {
            step = 0f;
        }
        int bulletCount;
        float shotAngle;
        if (playerMovement.PlayerDashing)
        {
            bulletCount = bulletPerShot + 2;
            shotAngle = spreadAngle + step * 2f;
        }
        else
        {
            bulletCount = bulletPerShot;
        }

        if (playerMovement.PlayerCrouching)
        {
            shotAngle = spreadAngle - step * 2f;
        }
        else
        {
            shotAngle = spreadAngle;
        }
        if (bulletCount > 1)
        {
            step = shotAngle / (bulletCount - 1);
        }
        else
        {
            step = 0f;
        }

        for (int i = 0; i < bulletCount; i++)
            {
                float angle = -shotAngle / 2f + step * i;
                Quaternion rotation = firepoint.rotation * Quaternion.Euler(angle, 0f, 0f);
            IncendiaryProjectile bullet = Instantiate(atkPrefab, firepoint.position, rotation).GetComponent<IncendiaryProjectile>();
            bullet.ChargeUpDmg(chargeStack);
            if (chargeStack > 3)
            {
                bullet.ChargeUpBurn(1f, chargeStack - 1);
                shaker.Shake(0.2f, 0.2f);
            }
        }
        atkTimer = atkCD + 0.5f * chargeStack;
        chargeStack = 0;
    }

    protected virtual void BurstFire()
    {
        PlayAnim();
        audioPlayer.PlayOneShot(shotSFX);
        shaker.Shake(0.1f, 0.25f);
        leftBuffer = 0f;
        rightBuffer = 0f;
        for (int i = 0; i < bulletPerShot * 4; i++)
        {
            Vector2 angle = Random.insideUnitCircle * spreadAngle / 2f;
            Quaternion rotation = firepoint.rotation * Quaternion.Euler(angle.x, angle.y, 0f);
            IncendiaryProjectile bullet = Instantiate(atkPrefab, firepoint.position, rotation).GetComponent<IncendiaryProjectile>();
            bullet.ChargeUpDmg(chargeStack);
            if (chargeStack > 3)
            {
                bullet.ChargeUpBurn(1f, chargeStack - 1);
                shaker.Shake(0.2f, 0.2f);
            }
        }

        playerMovement.Knockback(burstRecoil,0);

        atkTimer = atkCD *2f + 0.5f * chargeStack;
        chargeStack = 0;
    }
}
