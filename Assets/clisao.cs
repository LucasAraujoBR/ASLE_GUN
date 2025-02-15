using UnityEngine;
using System.Collections;

public class TrapController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>(); // Pega o Animator da armadilha
    }

    public void StopAnimationForSeconds(float delay)
    {
        if (animator != null)
        {
            Debug.Log("Parando animação da armadilha!");
            animator.speed = 0.5f; // Para a animação

            StartCoroutine(ResumeAnimation(delay));
        }
    }

    private IEnumerator ResumeAnimation(float delay)
    {
        Debug.Log($"Aguardando {delay} segundos para reativar...");
        yield return new WaitForSeconds(delay);

        Debug.Log("Reativando animação da armadilha!");
        animator.speed = 5; // Retoma a animação
    }
}