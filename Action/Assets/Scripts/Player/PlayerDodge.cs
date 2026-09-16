using System.Collections;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInput playerInput;

    [Header("Dodge")]
    [SerializeField] private float dodgeSpeed = 10f;
    [SerializeField] private float dodgeDuration = 0.25f;
    [SerializeField] private float dodgeCooldown = 0.5f;

    [Header("Colliders")]
    [SerializeField] private Collider dodgeCollider;
    [SerializeField] private Collider reflectCollider;

    private CharacterController controller;

    private float lastDodgeTime;

    public bool IsDodging { get; private set; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        SetDodgeColliders(false);
    }

    private void Update()
    {
        if (!playerInput.DodgePressed)
            return;

        TryDodge();
    }

    private void TryDodge()
    {
        if (IsDodging)
            return;

        if (Time.time <
            lastDodgeTime + dodgeCooldown)
            return;

        lastDodgeTime = Time.time;

        playerInput.ConsumeDodge();

        Vector2 input =
            playerInput.MoveInput;

        Vector3 direction;

        if (input.sqrMagnitude > 0.01f)
        {
            direction =
                transform.forward * input.y +
                transform.right * input.x;

            direction.Normalize();
        }
        else
        {
            direction =
                transform.forward;
        }

        StartCoroutine(
            DodgeRoutine(direction)
        );
    }

    private IEnumerator DodgeRoutine(
        Vector3 direction)
    {
        IsDodging = true;

        SetDodgeColliders(true);

        float timer = 0f;

        while (timer < dodgeDuration)
        {
            controller.Move(
                direction *
                dodgeSpeed *
                Time.deltaTime
            );

            timer += Time.deltaTime;

            yield return null;
        }

        SetDodgeColliders(false);

        IsDodging = false;
    }

    private void SetDodgeColliders(bool active)
    {
        if (dodgeCollider != null)
        {
            dodgeCollider.enabled = active;
        }

        if (reflectCollider != null)
        {
            reflectCollider.enabled = active;
        }
    }
}