using UnityEngine;

public class ClickToAnimate : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnMouseDown()
    {
        if (animator != null)
        {
            animator.SetTrigger("StartAnimation");
        }
    }
}
