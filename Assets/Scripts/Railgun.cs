using UnityEngine;
using System.Collections;


public class Railgun : Weapon
{
    [SerializeField] protected float primaryDmg, secondaryDmgInit, secondaryDmgHold, maxHoldSecondary;
    [SerializeField] protected LayerMask reflectLayer;
    protected bool holdingSecondary = false;
    protected int primaryBounce;




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
        if (primaryBounce < 0)
            primaryBounce = 0;
        if (!equipped)
            return;
        if (Input.GetMouseButtonDown(0) && !(atkTimer>0))
        {

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
            
            if (holdSecondaryTime >= maxHoldSecondary)
            {
                holdingSecondary = false;
                secondaryTimer = secondaryCD + 6f;
                atkTimer = secondaryCD + 6f;
                
                
            }
        }
        if (Input.GetMouseButtonUp(1) && holdingSecondary)
        {
            holdingSecondary = false;
            secondaryTimer = secondaryCD + Mathf.Lerp(-2f, 4f, holdSecondaryTime / maxHoldSecondary);
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

        Vector3 start, dir;
        start = firepoint.position;
        dir = firepoint.forward;
        if (primaryBounce > 0)
        {
            atkTimer = atkCD * 2f;
        }
        else 
        {

            atkTimer = atkCD;
        }

        
        while (primaryBounce >= 0)
        {

            if (Physics.Raycast(start, dir, out RaycastHit wallHit, Mathf.Infinity, reflectLayer))
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
                primaryBounce--;

            }

        }
    }

    protected override void SecondaryFire() 
    {
        if (Physics.Raycast(firepoint.position, firepoint.forward, out RaycastHit wallHit, Mathf.Infinity, reflectLayer))
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
        holdingSecondary = false;
        secondaryTimer = secondaryCD + Mathf.Lerp(-2f, 4f, holdSecondaryTime / maxHoldSecondary);
        if (Physics.Raycast(firepoint.position, firepoint.forward, out RaycastHit wallHit, Mathf.Infinity, reflectLayer))
        {

            StartCoroutine(BeamRender(firepoint.position, wallHit.point, 0.4f));



            RaycastHit[] hits = Physics.SphereCastAll(firepoint.position, 0.2f, firepoint.forward, wallHit.distance);
            foreach (var hit in hits)
            {
                IDmgable dmgable = hit.transform.GetComponent<IDmgable>();
                if (dmgable != null)
                {
                    dmgable.TakeDmg(primaryDmg);
                    if (!dmgable.electrified)
                        dmgable.GetElectrified(2.5f);
                }

            }




        }

    }

    protected virtual IEnumerator SecondaryHold() {
    while (holdingSecondary)
        {

            if (Physics.Raycast(firepoint.position, firepoint.forward, out RaycastHit wallHit, Mathf.Infinity, reflectLayer))
            {

                StartCoroutine(BeamRender(firepoint.position, wallHit.point, 2f));



                RaycastHit[] hits = Physics.SphereCastAll(firepoint.position, 1f, firepoint.forward, wallHit.distance);
                foreach (var hit in hits)
                {
                    IDmgable dmgable = hit.transform.GetComponent<IDmgable>();
                    if (dmgable != null)
                        dmgable.TakeDmg(secondaryDmgHold);
                }

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
