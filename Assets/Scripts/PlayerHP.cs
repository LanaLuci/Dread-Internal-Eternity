using UnityEngine;
using System;
using System.Collections;

public class PlayerHP : MonoBehaviour, IDmgable
{

    [SerializeField] protected float maxHP, electrifyRadius;
    private float hp, electrifyTimer;
    public event Action<float, float> OnHPChanged;
    public bool electrified { get; private set; }


    private void Start()
    {
        hp = maxHP;

    }

    public void TakeDmg(float amount)
    {
        if (electrified)
        {
            Electrify(amount, electrifyTimer);
        }
        hp -= amount;
        Debug.Log("you have " + hp + " left");
        OnHPChanged?.Invoke(hp, maxHP);
        if (hp <= 0)
            Die();
    }
    public void GetHealed(float amount)
    {
        hp += amount;
        if (hp > maxHP)
            hp = maxHP;
        OnHPChanged?.Invoke(hp, maxHP);
    }
    public void GetBurned(float amount, int stack)
    {

        StartCoroutine(BurnCoroutine(amount, stack));
    }

    private IEnumerator BurnCoroutine(float dmg, int stack)
    {
        while (stack > 0)
        {
            yield return new WaitForSeconds(1f);

            TakeDmg(dmg);

            stack--;


        }
    }

    public virtual void GetElectrified(float duration)
    {
        StartCoroutine(ElectrifyCoroutine(duration));

    }
    private IEnumerator ElectrifyCoroutine(float duration)
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

    private void Electrify(float amount, float duration)
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, electrifyRadius);
        foreach (var hit in hits)
        {
            IDmgable dmgable = hit.transform.GetComponent<IDmgable>();
            if (dmgable != null && dmgable != this)
            {
                Debug.Log("ZAP");
                dmgable.TakeDmg(amount);
                if (!dmgable.electrified)
                {

                    dmgable.GetElectrified(duration * 0.75f);
                }
            }
        }
    }

    private void Die() {
        Debug.Log("death");
        GetHealed(maxHP);
    
    }
}
