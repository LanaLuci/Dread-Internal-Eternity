using UnityEngine;
using System.Collections;

public class PoEnemy : MonoBehaviour, IDmgable
{
    [SerializeField] protected float maxHP;
    protected float hp;
    protected Rigidbody rb;

    protected virtual void Start()
    {
        hp = maxHP;
        rb = GetComponent<Rigidbody>();
    }

    public virtual void TakeDmg(float amount)
    {
        hp -= amount;
        if (hp <= 0)
            Destroy(gameObject);
    }
    public virtual void GetHealed(float amount)
    {
        hp += amount;
        if (hp > maxHP)
            hp = maxHP;
    }
    public virtual void GetBurned(float amount, int stack)
    {

        StartCoroutine(BurnCoroutine(amount, stack));
    }

    protected virtual IEnumerator BurnCoroutine(float dmg, int stack)
    {
        while (stack > 0)
        {
            yield return new WaitForSeconds(1f);

            TakeDmg(dmg);
            Debug.Log("burn");
            stack--;


        }
    }
}
