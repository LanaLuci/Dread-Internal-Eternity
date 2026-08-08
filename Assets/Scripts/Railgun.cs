using UnityEngine;
using System.Collections;


public class Railgun : Weapon
{
    [Header("Exclusive Properties - Railgun")]
    [SerializeField] protected LayerMask reflectLayer;
    [SerializeField] protected float primaryDmg, secondaryDmgInit, secondaryDmgHold, maxHoldSecondary, maxRange, holdRecoil;

    protected bool holdingSecondary = false;
    protected int primaryBounce;

    //DESCRIPTION: both modes have long CD but can reload even while not equipped, F to toggle bounce mode for primary fire, bounce mode gives longer CD
    //secondary fire can hold for sustaining dmg, the longer the hold the longer the CD, interrupt the sustaining beam with primary fire to electrify targets


    protected float holdSecondaryTime;

    protected override void Start()
    {
        base.Start();

    }

    protected override void Update()
    {
        if (holdPos == null)
            return;
        if (atkTimer > 0)
            atkTimer -= Time.deltaTime;
        if (secondaryTimer > 0)
            secondaryTimer -= Time.deltaTime;

        if (!equipped)
            return;
        if (Input.GetMouseButtonDown(0) && !(atkTimer>0))
        {
            //although using same input, interrupt fire counts as part of the secondary fire
            if (!holdingSecondary)
            {

                PrimaryFire();
            }
            else
            {
                InterruptFire();
            }
        }
        if (Input.GetMouseButtonDown(1) && !(secondaryTimer > 0) && !holdingSecondary)
        {
            holdSecondaryTime = 0f;
            holdingSecondary = true;
            SecondaryFire();
            
        }
        if (holdingSecondary)
        {
            holdSecondaryTime += Time.deltaTime;
            
            if (holdSecondaryTime > maxHoldSecondary)
            {
                //overheated
                holdingSecondary = false;
                secondaryTimer = secondaryCD + 6f;
                atkTimer = secondaryCD + 6f;
                
                
            }
        }
        if (Input.GetMouseButtonUp(1) && holdingSecondary)
        {
            holdingSecondary = false;
            secondaryTimer = secondaryCD + Mathf.Lerp(0f, 4f, holdSecondaryTime / maxHoldSecondary);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (primaryBounce == 0)
            {
                primaryBounce = 2;
            }
            else
            {
                primaryBounce = 0;
            }
        }
    }

    protected override void PrimaryFire()
    {
        PlayAnim();
        Vector3 start, dir;
        start = firepoint.position;
        dir = firepoint.forward;
        int remainingBounce = primaryBounce;
        if (primaryBounce > 0)
        {
            shaker.Shake(0.2f, 0.2f);
            atkTimer = atkCD * 2f;
        }
        else 
        {
            shaker.Shake(0.15f, 0.2f);
            atkTimer = atkCD;
        }


        
        while (remainingBounce >= 0)
        {

            if (Physics.Raycast(start, dir, out RaycastHit wallHit, maxRange, reflectLayer))
            {


                StartCoroutine(BeamRender(start, wallHit.point, 0.4f));



                RaycastHit[] hits = Physics.SphereCastAll(start, 0.2f, dir, wallHit.distance);
                foreach (var hit in hits)
                {
                    IDmgable dmgable = hit.transform.GetComponent<IDmgable>();
                    if (dmgable != null)
                        dmgable.TakeDmg(primaryDmg);
                }

                start = wallHit.point;
                dir = Vector3.Reflect(dir, wallHit.normal);
                remainingBounce--;

            }
            else
            {
                break;
            }

        }
    }

    protected override void SecondaryFire() 
    {
        shaker.Shake(0.2f, 0.1f);
        if (Physics.Raycast(firepoint.position, firepoint.forward, out RaycastHit wallHit, maxRange, reflectLayer))
        {





            RaycastHit[] hits = Physics.SphereCastAll(firepoint.position, 1f, firepoint.forward, wallHit.distance);
            foreach (var hit in hits)
            {
                IDmgable dmgable = hit.transform.GetComponent<IDmgable>();
                if (dmgable != null)
                    dmgable.TakeDmg(secondaryDmgInit);
            }

            StartCoroutine(SecondaryHold());

        }
    }

    protected virtual void InterruptFire()
    {
        //not resetting the CD for primary, allow a primary shot to follow to exploit the electrify effect on targets
        //affected by bounce mode toggle
        holdingSecondary = false;
        secondaryTimer = secondaryCD + Mathf.Lerp(0f, 4f, holdSecondaryTime / maxHoldSecondary);
        Vector3 start, dir;
        start = firepoint.position;
        dir = firepoint.forward;
        int remainingBounce = primaryBounce;
        while (remainingBounce >= 0)
        {
            if (Physics.Raycast(start, dir, out RaycastHit wallHit, Mathf.Infinity, reflectLayer))
            {

                StartCoroutine(BeamRender(start, wallHit.point, 0.4f));



                RaycastHit[] hits = Physics.SphereCastAll(start, 0.2f, dir, wallHit.distance);
                foreach (var hit in hits)
                {
                    IDmgable dmgable = hit.transform.GetComponent<IDmgable>();
                    if (dmgable != null)
                    {
                        dmgable.TakeDmg(primaryDmg * 0.4f);
                        if (!dmgable.electrified)
                            dmgable.GetElectrified(2.5f);
                    }

                }

                start = wallHit.point;
                dir = Vector3.Reflect(dir, wallHit.normal);
                remainingBounce--;


            }
            else
            {
                break;
            }
        }

    }

    protected virtual IEnumerator SecondaryHold() {
    while (holdingSecondary)
        {

            if (Physics.Raycast(firepoint.position, firepoint.forward, out RaycastHit wallHit, maxRange, reflectLayer))
            {

                StartCoroutine(BeamRender(firepoint.position, wallHit.point, 2f));
                shaker.Shake(0.1f, 0.15f);


                RaycastHit[] hits = Physics.SphereCastAll(firepoint.position, 1f, firepoint.forward, wallHit.distance);
                foreach (var hit in hits)
                {
                    IDmgable dmgable = hit.transform.GetComponent<IDmgable>();
                    if (dmgable != null)
                        dmgable.TakeDmg(secondaryDmgHold);
                }
                playerMovement.Knockback(holdRecoil, 0);
            }
            yield return new WaitForSeconds(0.1f);

        }
    }

    protected virtual IEnumerator BeamRender(Vector3 start, Vector3 end, float diameter)
    {
        GameObject beam = Instantiate(atkPrefab);
        beam.SetActive(true);
        beam.transform.position = (start + end) / 2f;
        beam.transform.LookAt(end);

        float length = Vector3.Distance(start, end);
        beam.transform.localScale = new Vector3(diameter, length / 2f, diameter);
        beam.transform.rotation *= Quaternion.Euler(90, 0, 0);

        yield return new WaitForSeconds(0.15f);
        Destroy(beam.gameObject);
    }



}
