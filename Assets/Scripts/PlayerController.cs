using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float runSpeed = 8f;
    public float jumpHeight = 2.0f;
    public float gravity = -20f;
    public float mouseSensitivity = 2f;
    public Transform cameraPivot;
    public Transform leftLeg, rightLeg, leftArm, rightArm;

    private CharacterController controller;
    private float pitch, verticalVelocity, walkCycle;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (CoinCollectorGame.GameEnded)
        {
            verticalVelocity = 0f;
            return;
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            float mx = Input.GetAxis("Mouse X") * mouseSensitivity;
            float my = Input.GetAxis("Mouse Y") * mouseSensitivity;
            transform.Rotate(0, mx, 0);
            pitch = Mathf.Clamp(pitch - my, -70f, 70f);
            if (cameraPivot) cameraPivot.localRotation = Quaternion.Euler(pitch, 0, 0);
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 move = (transform.right * h + transform.forward * v).normalized;
        bool moving = move.sqrMagnitude > 0.01f;
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : speed;

        if (controller.isGrounded && verticalVelocity < 0f) verticalVelocity = -2f;
        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        verticalVelocity += gravity * Time.deltaTime;
        Vector3 velocity = move * currentSpeed;
        velocity.y = verticalVelocity;
        controller.Move(velocity * Time.deltaTime);

        // Simple procedural walk cycle for the low-poly human character.
        if (moving && controller.isGrounded)
        {
            walkCycle += Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 13f : 9f);
            float swing = Mathf.Sin(walkCycle) * 28f;
            if (leftLeg) leftLeg.localRotation = Quaternion.Euler(swing, 0, 0);
            if (rightLeg) rightLeg.localRotation = Quaternion.Euler(-swing, 0, 0);
            if (leftArm) leftArm.localRotation = Quaternion.Euler(-swing * 0.75f, 0, 0);
            if (rightArm) rightArm.localRotation = Quaternion.Euler(swing * 0.75f, 0, 0);
        }
        else
        {
            if (leftLeg) leftLeg.localRotation = Quaternion.Slerp(leftLeg.localRotation, Quaternion.identity, Time.deltaTime * 8f);
            if (rightLeg) rightLeg.localRotation = Quaternion.Slerp(rightLeg.localRotation, Quaternion.identity, Time.deltaTime * 8f);
            if (leftArm) leftArm.localRotation = Quaternion.Slerp(leftArm.localRotation, Quaternion.identity, Time.deltaTime * 8f);
            if (rightArm) rightArm.localRotation = Quaternion.Slerp(rightArm.localRotation, Quaternion.identity, Time.deltaTime * 8f);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
        if (Input.GetMouseButtonDown(0) && Cursor.lockState != CursorLockMode.Locked)
        { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
    }
}
