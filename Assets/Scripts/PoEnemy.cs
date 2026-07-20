using UnityEngine;

public class PoEnemy : MonoBehaviour, IDmgable
{
    [SerializeField] protected float maxHP;
    protected float hp;

    public virtual void TakeDmg(float amount)
    {
        hp -= amount;
        if (hp < 0)
            Destroy(gameObject);
    }
    public virtual void GetHealed(float amount)
    {
        hp += amount;
        if (hp > maxHP)
            hp = maxHP;
    }

}
