using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] private int chargeStackMax, bulletPerShot;
    [SerializeField] private float spreadAngle, burstRecoil;
    private int chargeStack;
    private float leftBuffer, rightBuffer;




    protected override void Update()
    {
        if (holdPos == null)
            return;
        if (!equipped)
            return; 
        if (atkTimer > 0)
            atkTimer -= Time.deltaTime;
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

        float step;
        if (bulletPerShot > 1)
        {
            step = spreadAngle / (bulletPerShot - 1);
        }
        else
        {
            step = 0f;
        }
        for (int i = 0;i< bulletPerShot;i++)
        {

            float angle = -spreadAngle / 2f + step * i;
            Quaternion rotation = firepoint.rotation * Quaternion.Euler(0f,angle,0f);
            IncendiaryProjectile bullet = Instantiate(atkPrefab, firepoint.position, rotation).GetComponent<IncendiaryProjectile>();
            bullet.ChargeUpDmg(chargeStack);
            if (chargeStack > 3)
                bullet.ChargeUpBurn(1f, chargeStack - 1);
                
        }

        atkTimer = atkCD + 0.5f * chargeStack;
        chargeStack = 0;
    }
    protected override void SecondaryFire()
    {
        float step;
        if (bulletPerShot>1)
        {
            step = spreadAngle / (bulletPerShot - 1);
        }
        else
        {
            step = 0f;
        }

            for (int i = 0; i < bulletPerShot; i++)
            {
                float angle = -spreadAngle / 2f + step * i;
                Quaternion rotation = firepoint.rotation * Quaternion.Euler(angle, 0f, 0f);
            IncendiaryProjectile bullet = Instantiate(atkPrefab, firepoint.position, rotation).GetComponent<IncendiaryProjectile>();
            bullet.ChargeUpDmg(chargeStack);
            if (chargeStack > 3)
                bullet.ChargeUpBurn(1f, chargeStack - 1);
        }
        atkTimer = atkCD + 0.5f * chargeStack;
        chargeStack = 0;
    }

    protected virtual void BurstFire()
    {
        leftBuffer = 0f;
        rightBuffer = 0f;
        for (int i = 0; i < bulletPerShot * 4; i++)
        {
            Vector2 angle = Random.insideUnitCircle * spreadAngle / 2f;
            Quaternion rotation = firepoint.rotation * Quaternion.Euler(angle.x, angle.y, 0f);
            IncendiaryProjectile bullet = Instantiate(atkPrefab, firepoint.position, rotation).GetComponent<IncendiaryProjectile>();
            bullet.ChargeUpDmg(chargeStack);
            if (chargeStack > 3)
                bullet.ChargeUpBurn(1f, chargeStack - 1);
        }

        playerMovement.Knockback(burstRecoil,0);
        atkTimer = atkCD *2f + 0.5f * chargeStack;
        chargeStack = 0;
    }
}
