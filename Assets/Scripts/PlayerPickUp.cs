using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    [SerializeField] private Transform raycastStart, pickupHoldPoint;
    [SerializeField] private float pickupRange;
    [SerializeField] private LayerMask pickupLayer;
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
            else
            {
                currentPickup.Drop();
                currentPickup = null;
            }
            
        }
    }
}
