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
                weaponID--;
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
                CycleNextWeapon();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
                EquipWeaponSlot(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                EquipWeaponSlot(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                EquipWeaponSlot(2);
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                EquipWeaponSlot(3);
            else if (Input.GetKeyDown(KeyCode.Alpha5))
                EquipWeaponSlot(4);
        }
    }

    public void NewWeaponHandle(Weapon newWeapon, int id = 0)
    {
        if (newWeapon == null)
            return;
        for (int i = 0; i < arsenal.Length; i++) {
            if (arsenal[i] == newWeapon) {
                return;
            }
        }

            if (arsenal[id] != null)
            {
                arsenal[id].Drop();
            currentPickup = null;

            }
        arsenal[id] = newWeapon;
        arsenal[id].Equip();
        weaponID = id;
        currentPickup = arsenal[weaponID];
        return;

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

    private void CycleNextWeapon()
    {
        if (weaponID < 0)
        {
            weaponID = 0;
        }
        else
        {
            weaponID++;
            if (weaponID > arsenal.Length - 1)
            {
                weaponID = 0;
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
        EquipWeaponSlot(weaponID);


    }

    private void EquipWeaponSlot(int slot)
    {
        if (slot < 0 || slot > arsenal.Length)
            return;
        if (arsenal[slot] == null)
            return;
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
        currentPickup = arsenal[slot];
        currentWeapon = currentPickup.GetComponent<Weapon>();
        currentWeapon.Equip();
        weaponID = slot;
    }
}
