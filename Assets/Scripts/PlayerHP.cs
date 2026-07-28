using UnityEngine;
using System.Collections;

public class PlayerHP : MonoBehaviour, IDmgable
{

    [SerializeField] protected float maxHP;
    protected float hp;
    public float Player_HP => this.hp;
    public float Player_MaxHP => this.maxHP;


    private void Start()
    {
        hp = maxHP;

    }

    public void TakeDmg(float amount)
    {

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

    private void Die() {
        Debug.Log("death");
        GetHealed(maxHP);
    
    }
}
