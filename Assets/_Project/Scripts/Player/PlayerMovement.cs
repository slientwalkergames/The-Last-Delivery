using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float mouseSensitivity = 0.1f; // Farenin dönüş hassasiyeti

    [Header("Zıplama & Zemin Ayarları")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float fallMultiplier = 2.5f;

    [Header("Tırmanma (Vault) Ayarları")]
    [SerializeField] private LayerMask vaultLayer;
    [SerializeField] private float vaultForwardDistance = 1.2f;
    [SerializeField] private float vaultHeightDistance = 2f;
    [SerializeField] private float vaultSpeed = 7f;
    [SerializeField] private float vaultHeightOffset = 1f;

    [Header("Fiziksel İtme (Push) Ayarları")]
    [SerializeField] private float pushForce = 5f;

    private PlayerControls inputActions;
    private Vector2 movementInput;
    private Vector2 lookInput; // Fareden gelen bakış verisi
    private Vector3 currentMoveDirection;
    private Rigidbody rb;
    private bool isGrounded;
    private bool jumpPressed;
    private bool isVaulting;
    private float rotationY; // Karakterin güncel Y rotasyonu

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new PlayerControls();

        // Girdi olaylarına abone oluyoruz
        inputActions.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => movementInput = Vector2.zero;

        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => jumpPressed = true;
    }

    private void Start()
    {
        // PC testi için fare imlecini ekrana gömüyoruz ve gizliyoruz
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Başlangıç rotasyonunu oyunun başındaki rotasyona eşitliyoruz
        rotationY = transform.localEulerAngles.y;
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Update()
    {
        if (isVaulting) return;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void FixedUpdate()
    {
        if (isVaulting) return;

        HandleRotation();          // Önce fareye göre karakteri döndürürüz
        MovePlayerLocal();         // Sonra karakterin kendi eksenine göre yürütürüz
        HandleJumpOrVault();
        ApplyBetterGravity();
    }

    // Modern Teknik: Doğrudan Gövde Rotasyonu (Mouse Look)
    private void HandleRotation()
    {
        // Farenin sağ-sol (X) hareketini hassasiyetle çarpıp güncel rotasyona ekliyoruz
        rotationY += lookInput.x * mouseSensitivity;

        // Rigidbody üzerinden karakteri fizik motorunu bozmadan pürüzsüzce döndürüyoruz
        rb.MoveRotation(Quaternion.Euler(0f, rotationY, 0f));
    }

    // Modern Teknik: Karakter Eksenli Yanal Hareket (Strafe & Backpedal)
    private void MovePlayerLocal()
    {
        // Kameraya ihtiyaç duymuyoruz! Karakterin kendi 'ileri' ve 'sağ' yönlerini baz alıyoruz.
        // Bu sayede W ileri, S arkasını dönmeden geri, A-D ise sağa sola yan adımlar attırır.
        Vector3 moveDir = (transform.forward * movementInput.y) + (transform.right * movementInput.x);

        if (moveDir.magnitude > 1f) moveDir.Normalize();

        // Karakteri hareket ettirirken dikey yerçekimi hızını (rb.velocity.y) aynen koruyoruz
        rb.linearVelocity = new Vector3(moveDir.x * moveSpeed, rb.linearVelocity.y, moveDir.z * moveSpeed);

        // İtme mekaniğinde kullanabilmek için yönü kaydediyoruz
        currentMoveDirection = moveDir;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pushable") && currentMoveDirection != Vector3.zero)
        {
            Rigidbody boxRb = collision.collider.attachedRigidbody;
            if (boxRb != null)
            {
                boxRb.AddForce(currentMoveDirection * pushForce, ForceMode.Force);
            }
        }
    }

    private void HandleJumpOrVault()
    {
        if (jumpPressed)
        {
            if (TryVault())
            {
                jumpPressed = false;
                return;
            }

            if (isGrounded)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            }
        }
        jumpPressed = false;
    }

    private bool TryVault()
    {
        // Karakter döndükçe transform.forward da fareye göre döneceğinden tam baktığı duvara ışın atar
        Vector3 rayStart = transform.position + (Vector3.up * 0.5f);

        if (Physics.Raycast(rayStart, transform.forward, out RaycastHit frontHit, vaultForwardDistance, vaultLayer))
        {
            Vector3 topRayStart = frontHit.point + (transform.forward * 0.1f) + (Vector3.up * vaultHeightDistance);

            if (Physics.Raycast(topRayStart, Vector3.down, out RaycastHit topHit, vaultHeightDistance, vaultLayer))
            {
                Vector3 finalTargetPosition = topHit.point + (Vector3.up * vaultHeightOffset);
                StartCoroutine(VaultRoutine(finalTargetPosition));
                return true;
            }
        }
        return false;
    }

    private IEnumerator VaultRoutine(Vector3 targetPosition)
    {
        isVaulting = true;
        rb.isKinematic = true;

        Vector3 startPosition = transform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * vaultSpeed;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;

        rb.isKinematic = false;
        isVaulting = false;
    }

    private void ApplyBetterGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        Gizmos.color = Color.blue;
        Vector3 rayStart = transform.position + (Vector3.up * 0.5f);
        Gizmos.DrawRay(rayStart, transform.forward * vaultForwardDistance);

        Gizmos.color = Color.green;
        Vector3 topRayStart = rayStart + (transform.forward * vaultForwardDistance) + (Vector3.up * vaultHeightDistance);
        Gizmos.DrawRay(topRayStart, Vector3.down * vaultHeightDistance);
    }
}