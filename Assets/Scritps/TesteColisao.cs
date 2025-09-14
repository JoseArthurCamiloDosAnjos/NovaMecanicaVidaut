using UnityEngine;

public class TesteColisao : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colidiu com: " + collision.collider.name);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger com: " + other.name);
    }
}
