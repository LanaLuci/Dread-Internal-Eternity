using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    private Transform player;
    [SerializeField] private GameObject[] doorParts;
    [SerializeField] private Transform[] doorPartOpenState;
    [SerializeField] private Transform[] doorPartCloseState;
    [SerializeField] private float spd;
    private bool locked = false;
    private bool isOpen;
    

    void Start()
    {
        player = GameObject.Find("Player").transform;
        isOpen = false;

    }

    void FixedUpdate()
    {
        if (locked)
            return;
        float dist = Vector3.Distance(this.transform.position, player.transform.position);
        if (dist < 6f)
        {
            isOpen = true;
        }
        else
        {
            isOpen = false;

        }
        for (int i = 0; i < doorParts.Length; i ++)
        {
            Transform target = isOpen ? doorPartOpenState[i] : doorPartCloseState[i];

            doorParts[i].transform.position = Vector3.MoveTowards(doorParts[i].transform.position,target.position,spd * Time.deltaTime);
        }

    }

    public void DoorLock()
    {
        if (!locked)
        {
            locked = true;
            for (int i = 0; i < doorParts.Length; i++)
            {
                Transform target = doorPartCloseState[i];

                doorParts[i].transform.position = target.position;
            }
        }

    }

    public void DoorUnlock()
    {
        if (locked)
            locked = false;
    }
}
