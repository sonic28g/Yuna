using UnityEngine;

public class FallRespawn : MonoBehaviour
{
    public float heightLimit = 10f; // altura máxima permitida antes de fazer respawn
    public LayerMask groundLayer;

    private float heightStartFalling;
    private bool isFalling = false;
    private CharacterController controller;
    private Vector3 lastPosition;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        lastPosition = transform.position;
    }

    void Update()
    {
        float verticalSpeed = (transform.position.y - lastPosition.y) / Time.deltaTime;

        // Se estiver a descer e ainda não tinha começado a cair
        if (verticalSpeed < -0.1f && !isFalling)
        {
            isFalling = true;
            heightStartFalling = transform.position.y;
        }

        // Se estiver no chão após queda
        if (isFalling && IsGrounded())
        {
            float finalHeight = transform.position.y;
            float fallDistance = heightStartFalling - finalHeight;

            if (fallDistance >= heightLimit)
            {
                CheckpointManager.Instance.RespawnPlayer();
            }

            isFalling = false;
        }

        lastPosition = transform.position;
    }

    bool IsGrounded()
    {
        // Usa o built-in do CharacterController, ou raycast se necessário
        return controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, 0.2f, groundLayer);
    }
}