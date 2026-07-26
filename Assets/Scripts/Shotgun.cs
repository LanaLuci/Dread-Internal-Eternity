using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] private int chargeStackMax, bulletPerShot;
    [SerializeField] private float spreadAngle;
    private int chargeStack;

    protected override void Update()
    {
        if (holdPos == null)
            return;
        if (atkTimer > 0)
            atkTimer -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && !(atkTimer > 0))
        {

            PrimaryFire();
        }
        if (Input.GetMouseButtonDown(1) && !(atkTimer > 0))
        {
            SecondaryFire();
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
}
