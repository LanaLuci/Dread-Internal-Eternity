using UnityEngine;
using System;
using System.Collections;

public class PoEnemy : MonoBehaviour, IDmgable
{
    [Header("Generic")]
    [SerializeField] protected LayerMask blockLayer;
    [SerializeField] protected float maxHP, electrifyRadius;
    [SerializeField] protected Animator animator;

    protected float hp;
    protected Rigidbody rb;
    public event Action<PoEnemy> OnDeath;


    protected float electrifyTimer;
    public bool electrified { get; protected set; }

    protected virtual void Start()
    {
        hp = maxHP;
        rb = GetComponent<Rigidbody>();
    }

    public virtual void TakeDmg(float amount)
    {
        if (amount > hp * 0.4f)
            Debug.Log("TS GONNA HURT");
        Debug.Log("take " +  amount + " dmg");
        if (electrified)
        {
            Electrify(amount, electrifyTimer);
        }

            hp -= amount;



        if (hp <= 0)
        {
            Die();
        }
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

    public virtual void GetElectrified(float duration)
    {
        StartCoroutine(ElectrifyCoroutine(duration));

    }

    protected virtual IEnumerator ElectrifyCoroutine(float duration)
    {
        electrified = true;
        electrifyTimer = duration;
        while (electrifyTimer > 0)
        {
            electrifyTimer -= Time.deltaTime;
            yield return null;
        }
        
        electrified = false;
    }
    protected virtual IEnumerator BurnCoroutine(float dmg, int stack)
    {
        while (stack > 0)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("LIGHT UP THE ROOM");
            TakeDmg(dmg);

            stack--;


        }
    }

    protected virtual void Electrify(float amount, float duration)
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, electrifyRadius);
        foreach (var hit in hits)
        {
            IDmgable dmgable = hit.transform.GetComponent<IDmgable>();
            if (dmgable != null && dmgable != this) {
                Debug.Log("ZAP");
                dmgable.TakeDmg(amount);
                if (!dmgable.electrified)
                {

                    dmgable.GetElectrified(duration * 0.75f);
                }
            }
        } 
    }

    protected virtual void Die()
    {
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }
}
