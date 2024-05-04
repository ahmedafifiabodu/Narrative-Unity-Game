using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private PlayerMovement playerMovement;

    private void Start() => playerMovement = ServiceLocator.Instance.GetService<PlayerMovement>();

    private void Update()
    {
        if (!ServiceLocator.Instance.GetService<InputManager>().gameObject.activeSelf)
            animator.SetFloat(GameConstant.Speed, 0);
        else
        {
            animator.SetFloat(GameConstant.Speed, playerMovement.GetPlayerVelocityMagnitude() / playerMovement.GetMaxSpeed);

            if (playerMovement.ForceDirection.y > 0)
                animator.SetBool(GameConstant.JumpingUp, true);
            else if (!playerMovement.IsPlayerGrounded())
                animator.SetBool(GameConstant.JumpingDown, true);
            else if (playerMovement.IsPlayerGrounded())
            {
                animator.SetBool(GameConstant.JumpingUp, false);
                animator.SetBool(GameConstant.JumpingDown, false);
            }
        }
    }
}