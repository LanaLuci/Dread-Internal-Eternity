using UnityEngine;

public class FlashbangEffect : MonoBehaviour
{
    private Animator animator;

    private static int FlashTrigger = Animator.StringToHash("Blind Flash");

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ScreenFlash()
    {
        animator.SetTrigger(FlashTrigger);
    }
}
