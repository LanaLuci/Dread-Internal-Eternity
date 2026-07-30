using UnityEngine;

public interface IDmgable
{

    bool electrified { get; }
    void TakeDmg(float amount);
    void GetHealed(float amount);
    void GetBurned(float amount, int stack);
    void GetElectrified(float duration);
}
