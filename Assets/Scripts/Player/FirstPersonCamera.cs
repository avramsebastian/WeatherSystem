using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float eyeHeight = 0.4f;
    [SerializeField] private bool hidePlayerMesh = true;
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;

    private float yaw;
    private float pitch;
    private bool cursorLocked;
    private Renderer[] playerRenderers;

    private void OnEnable()
    {
        if (target != null)
        {
            yaw = target.eulerAngles.y;
            pitch = 0f;

            playerRenderers = target.GetComponentsInChildren<Renderer>();
            SetPlayerMeshVisible(!hidePlayerMesh);
        }
        SetCursorLocked(true);
    }

    private void OnDisable()
    {
        SetPlayerMeshVisible(true);
        SetCursorLocked(false);
    }

    private void LateUpdate()
    {
        if (target == null) return;

        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
            SetCursorLocked(!cursorLocked);

        if (cursorLocked && Mouse.current != null)
        {
            Vector2 delta = Mouse.current.delta.ReadValue();
            yaw += delta.x * mouseSensitivity;
            pitch -= delta.y * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        transform.SetPositionAndRotation(
            target.position + Vector3.up * eyeHeight,
            Quaternion.Euler(pitch, yaw, 0f));
    }

    private void SetPlayerMeshVisible(bool visible)
    {
        if (playerRenderers == null) return;
        foreach (Renderer playerRenderer in playerRenderers)
            if (playerRenderer != null) playerRenderer.enabled = visible;
    }

    private void SetCursorLocked(bool locked)
    {
        cursorLocked = locked;
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
