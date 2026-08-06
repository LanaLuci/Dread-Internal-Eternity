using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Slider hpDisplay;
    [SerializeField] private PlayerHP hpPlayer;


    void Start()
    {
        hpPlayer.OnHPChanged += UpdatePlayerHP;
    }

    public void UpdatePlayerHP(float current, float max)
    {
        if (hpDisplay != null && hpPlayer != null)
        {
            hpDisplay.value = Mathf.Lerp(0f, hpDisplay.maxValue, current / max);
        }
    }

}
