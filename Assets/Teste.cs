using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float stopTime = 3f; // Tempo que a animação ficará pausada

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
                trap.StopAnimationForSeconds(stopTime);
            }

            // Opcional: Destruir o projetil depois da colisão
            Destroy(gameObject);
        }
    }
}