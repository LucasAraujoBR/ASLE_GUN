using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float stopTime = 3f; // Tempo que a animação ficará pausada
    public float timeToReduce = 3f; // Tempo que a animação ficará pausada
    public float timeToIncrease = 3f; // Tempo que a animação ficará pausada
    public String action = "";


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 para o botão esquerdo
        {
            action = "increase";
            Debug.Log("Ação: " + action);
        }
        else if (Input.GetMouseButtonDown(1)) // 1 para o botão direito
        {
            action = "reduce";
            Debug.Log("Ação: " + action);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"OnCollision `{collision.gameObject.name} tag {collision.gameObject.tag}");

        if (collision.gameObject.CompareTag("TimeObstacle")) // A armadilha tem essa tag
        {
            Debug.Log("Projetil colidiu com armadilha!");

            // Pega o script da armadilha e chama o método de parar animação
            TrapController trap = collision.gameObject.GetComponent<TrapController>();
            if (trap != null)
            {
                Animator animator =  trap.GetComponent<Animator>();
                if (animator != null) {
                    bool isReduceAction = action == "reduce";

                    float speedLimit = isReduceAction ? trap.minAnimationSpeed : trap.maxAnimationSpeed;
                    float newAnimatioSpeed = isReduceAction ?  Math.Max(speedLimit, animator.speed - timeToReduce) : Math.Min(speedLimit, animator.speed + timeToIncrease);
                    animator.speed = newAnimatioSpeed;
                }
            }

            // Opcional: Destruir o projetil depois da colisão
            Destroy(gameObject);
        }
    }
}