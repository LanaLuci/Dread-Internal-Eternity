using UnityEngine;
using System.Collections;

public class PlayerHP : MonoBehaviour, IDmgable
{

    [SerializeField] protected float maxHP, electrifyRadius;
    private float hp, electrifyTimer;
    public float Player_HP => this.hp;
    public float Player_MaxHP => this.maxHP;
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
        Debug.Log(amount);
        if (hp <= 0)
            Die();
    }
    public void GetHealed(float amount)
    {
        hp += amount;
        if (hp > maxHP)
            hp = maxHP;
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
