using UnityEngine;

public class PlayAnimationOnClick : MonoBehaviour
{
    public Animator animator; // Компонент Animator
    public string triggerName = "PlayAnimation"; // Назва тригера

    private void OnMouseDown()
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
    }
}
