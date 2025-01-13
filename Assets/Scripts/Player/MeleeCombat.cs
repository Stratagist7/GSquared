using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeCombat : MonoBehaviour
{
    private static readonly int indexKey = Animator.StringToHash("i_attack_index");
    private static readonly int attackKey = Animator.StringToHash("t_attack");
    public const int MELEE_DAMAGE = 15;
    
    [SerializeField] private Animator animator;
    [SerializeField] private InputActionReference actionRef;
    [SerializeField] private GameObject hitCollider;
    
    private bool isAttacking = false;

    private void Update()
    {
        if (isAttacking == false && actionRef.action.phase == InputActionPhase.Performed)
        {
            Attack();
        }
    }
    
    private void OnEnable()
    {
        actionRef.action.Enable();
    }

    private void OnDisable()
    {
        actionRef.action.Disable();
        animator.SetInteger(indexKey, 0);
    }

    private void Attack()
    {
        int index = Random.Range(1, 5);
        animator.SetInteger(indexKey, index);
        animator.SetTrigger(attackKey);
    }

    public void SetIsAttacking(bool argValue)
    {
        isAttacking = argValue;
        if (argValue == false)
        {
            animator.SetInteger(indexKey, -1);
        }
    }

    public void SetHitCollider(bool argValue)
    {
        hitCollider.SetActive(argValue);
    }
}
