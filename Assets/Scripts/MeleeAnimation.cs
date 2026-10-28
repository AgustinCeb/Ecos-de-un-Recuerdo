using UnityEngine;

public class MeleeAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void AnimAttack()
    {
        animator.SetTrigger("Attack");
    }
    public void SetState(int state)
    {
        animator.SetInteger("State", state);
    }
    public void AnimDeath()
    {
        animator.SetTrigger("Death");
    }
}
