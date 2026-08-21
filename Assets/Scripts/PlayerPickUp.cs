using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    [SerializeField] private Transform raycastStart, pickupRightHand, pickupLeftHand;
    [SerializeField] private float pickupRange;
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private Weapon[] arsenal = new Weapon[5];
    [SerializeField] private MeleeWeapon[] storage = new MeleeWeapon[3];
    private int weaponID = -1;
    private int storageID = -1;
    private PickupAble currentRightHandPickup, currentLeftHandPickup;
    public PickupAble PlayerLeftHand => currentLeftHandPickup;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentRightHandPickup == null) //prioritize picking up with right hand
            {

                if (Physics.Raycast(raycastStart.position, raycastStart.TransformDirection(Vector3.forward), out RaycastHit hit, pickupRange, pickupLayer))
                {
                    currentRightHandPickup = hit.transform.GetComponent<PickupAble>();

                    if (currentRightHandPickup != null)
                    {
                        if (currentRightHandPickup.transform.GetComponent<MeleeWeapon>() == null)
                        {
                            currentRightHandPickup.Grab(pickupRightHand);
                        }
                        else
                        {
                            currentLeftHandPickup = currentRightHandPickup; //melee will get auto-switched to left hand
                            currentLeftHandPickup.Grab(pickupLeftHand);
                            currentRightHandPickup = null;
                        }

                    }
                }
                else
                {
                    if (currentLeftHandPickup != null)
                    {
                        if (currentLeftHandPickup.TryGetComponent<MeleeWeapon>(out MeleeWeapon lefthand))
                        {
                            lefthand.Unequip();
                            currentLeftHandPickup = null;
                            storageID--;
                        }
                        else
                        {
                            currentLeftHandPickup.Drop();
                            currentLeftHandPickup = null;
                        }

                    }

                }
            }
            else if (currentLeftHandPickup == null) //prioritize picking up over equip/unequip/drop
            {
                if (Physics.Raycast(raycastStart.position, raycastStart.TransformDirection(Vector3.forward), out RaycastHit hit, pickupRange, pickupLayer))
                {
                    currentLeftHandPickup = hit.transform.GetComponent<PickupAble>();
                    if (currentLeftHandPickup != null && currentLeftHandPickup.transform.GetComponent<Weapon>() == null)
                    {
                        currentLeftHandPickup.Grab(pickupLeftHand);
                    }
                    else
                    {
                        currentLeftHandPickup = null;
                    }
                }
                else
                {
                    if (currentRightHandPickup != null)
                    {
                        if (currentRightHandPickup.TryGetComponent<Weapon>(out Weapon righthand))
                        {
                            righthand.Unequip();
                            currentRightHandPickup = null;
                            weaponID--;
                        }
                        else
                        {
                            currentRightHandPickup.Drop();
                            currentRightHandPickup = null;
                        }

                    }
                }
            }
            else if (currentRightHandPickup.GetComponent<Weapon>() == null) //prioritize drop item in right hand for weapon
            {
                currentRightHandPickup.Drop();
                currentRightHandPickup = null;
            }
            else if (currentLeftHandPickup.GetComponent<MeleeWeapon>() == null) //prioritize drop item in left hand
            {
                currentLeftHandPickup.Drop();
                currentLeftHandPickup = null;

            }
            else if (currentLeftHandPickup.TryGetComponent<MeleeWeapon>(out MeleeWeapon melee) != null) //prioritize unequip melee
            {
                melee.Unequip();
                currentLeftHandPickup = null;
                storageID--;
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

        if (MeleeReady())
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                CycleNextMelee();
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
                EquipMeleeSlot(0);
            else if (Input.GetKeyDown(KeyCode.Alpha7))
                EquipMeleeSlot(1);
            else if (Input.GetKeyDown(KeyCode.Alpha8))
                EquipMeleeSlot(2);
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
            //drop the weapon for its alternative that shares same slot (for later weapon expansion)
                arsenal[id].Drop();
            currentRightHandPickup = null;

            }
        arsenal[id] = newWeapon;
        arsenal[id].Equip();
        weaponID = id;
        currentRightHandPickup = arsenal[weaponID];
        return;

    }

    public void NewMeleeHandle(MeleeWeapon newMelee, int id = 0)
    {

        if (newMelee == null)
            return;
        for (int i = 0; i < storage.Length; i++) {
            if (storage[i] == newMelee) {
                return;
            }
        }

            if (storage[id] != null)
            {

                storage[id].Drop();
            currentLeftHandPickup = null;

            }
        storage[id] = newMelee;
        storage[id].Equip();
        storageID = id;
        currentLeftHandPickup = storage[storageID];
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

    private bool MeleeReady()
    {
        for (int i = 0; i < storage.Length; i++)
        {
            if (storage[i] != null)
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

    private void CycleNextMelee()
    {
        if (storageID < 0)
        {
            storageID = 0;
        }
        else
        {
            storageID++;
            if (storageID > storage.Length - 1)
            {
                storageID = 0;
            }
        }
        while (storage[storageID] == null)
        {
            storageID++;
            if (storageID > storage.Length - 1)
            {
                storageID = 0;
            }
        }
        EquipMeleeSlot(storageID);


    }

    private void EquipWeaponSlot(int slot)
    {
        if (slot < 0 || slot >= arsenal.Length)
            return;
        if (arsenal[slot] == null)
            return;
        Weapon currentWeapon;
        if (currentRightHandPickup != null)
        {
            currentWeapon = currentRightHandPickup.GetComponent<Weapon>();
            if (currentWeapon == null)
            {
                currentRightHandPickup.Drop();
            }
            else
            {
                currentWeapon.Unequip();
            }
        }
        currentRightHandPickup = arsenal[slot];
        currentWeapon = currentRightHandPickup.GetComponent<Weapon>();
        currentWeapon.Equip();
        weaponID = slot;
    }

    private void EquipMeleeSlot(int slot)
    {
        if (slot < 0 || slot >= storage.Length)
            return;
        if (storage[slot] == null)
            return;
        MeleeWeapon currentMelee;
        if (currentLeftHandPickup != null)
        {
            currentMelee = currentLeftHandPickup.GetComponent<MeleeWeapon>();
            if (currentMelee == null)
            {
                currentLeftHandPickup.Drop();
            }
            else
            {
                currentMelee.Unequip();
            }
        }
        currentLeftHandPickup = storage[slot];
        currentMelee = currentLeftHandPickup.GetComponent<MeleeWeapon>();
        currentMelee.Equip();
        storageID = slot;
    }
}
