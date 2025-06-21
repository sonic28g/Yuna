using UnityEngine;

public class Kanzashi : MonoBehaviour
{
    private Rigidbody rb;
    private bool isStuck = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isStuck) return;

        // Verifica se a camada ou tag do objeto é uma parede
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Parar o movimento
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Fixar a posição ao ponto de colisão
            transform.position = collision.contacts[0].point;
            transform.rotation = Quaternion.LookRotation(-collision.contacts[0].normal); // Para "entrar" na parede

            // Torna-se filho da parede para acompanhar qualquer movimento
            transform.SetParent(collision.transform);

            isStuck = true;
        }
    }
}
