using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    [SerializeField] private Transform raycastStart, pickupHoldPoint;
    [SerializeField] private float pickupRange;
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private Weapon[] arsenal = new Weapon[5];
    private int weaponID = -1;
    private PickupAble currentPickup;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentPickup == null)
            {

                if (Physics.Raycast(raycastStart.position, raycastStart.TransformDirection(Vector3.forward), out RaycastHit hit, pickupRange, pickupLayer))
                {
                    currentPickup = hit.transform.GetComponent<PickupAble>();
                    if (currentPickup != null)
                    {
                        currentPickup.Grab(pickupHoldPoint);
                    }
                }
            }
            else if (currentPickup.GetComponent<Weapon>() != null)
            {
                currentPickup.GetComponent<Weapon>().Unequip();
                currentPickup = null;
            }
            else
            {

                currentPickup.Drop();
                currentPickup = null;
            }
            
        }
 
        if (WeaponReady())
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (weaponID <0)
                {
                    weaponID = 0;
                }
                else
                {
                    weaponID++;
                    if (weaponID > arsenal.Length -1)
                    {
                        weaponID = 0;
                    }
                }
                Weapon currentWeapon;
                if (currentPickup != null)
                {
                    currentWeapon = currentPickup.GetComponent<Weapon>();
                    if (currentWeapon == null)
                    {
                        currentPickup.Drop();
                    }
                    else
                    {
                        currentWeapon.Unequip();
                    }
                }


                while (arsenal[weaponID] == null)
                {
                    weaponID++;
                    if (weaponID > arsenal.Length - 1)
                    {
                        weaponID = 0;
                    }
                }
                currentPickup = arsenal[weaponID];
                currentWeapon = currentPickup.GetComponent<Weapon>();
                currentWeapon.Equip();
            }
        }
    }

    public void NewWeaponHandle(Weapon newWeapon)
    {
        for (int i = 0; i < arsenal.Length; i++) {
            if (arsenal[i] == newWeapon) {
                return;
            }
        }
        for (int i = 0; i < arsenal.Length;i++)
        {
            if (arsenal[i] == null)
            {
                arsenal[i] = newWeapon;
                arsenal[i].Equip();
                return;
            }
        }
    }

    private bool WeaponReady()
    {
        for (int i = 0; i < arsenal.Length; i++)
        {
            if (arsenal[i] != null)
            {

                return true;
            }
        }
        return false;
    }
}
