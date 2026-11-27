using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private Camera overviewCamera;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private PlayerController player;
    [SerializeField] private Transform spawnPoint;

    public bool IsPlayerCameraActive { get; private set; }

    private void Start()
    {
        SetPlayerCamera(false);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame)
            SwitchCamera();
    }

    public void SwitchCamera()
    {
        SetPlayerCamera(!IsPlayerCameraActive);
    }

    private void SetPlayerCamera(bool usePlayerCamera)
    {
        IsPlayerCameraActive = usePlayerCamera;

        if (player != null)
        {
            if (usePlayerCamera) SpawnPlayer();
            player.gameObject.SetActive(usePlayerCamera);
        }

        if (playerCamera != null)
        {
            playerCamera.gameObject.SetActive(usePlayerCamera);
            playerCamera.tag = usePlayerCamera ? "MainCamera" : "Untagged";
        }

        if (overviewCamera != null)
        {
            overviewCamera.gameObject.SetActive(!usePlayerCamera);
            overviewCamera.tag = usePlayerCamera ? "Untagged" : "MainCamera";
        }
    }

    private void SpawnPlayer()
    {
        Vector3 position = spawnPoint != null ? spawnPoint.position : player.transform.position;
        Quaternion rotation = spawnPoint != null
            ? Quaternion.Euler(0f, spawnPoint.eulerAngles.y, 0f)
            : player.transform.rotation;

        CharacterController controller = player.GetComponent<CharacterController>();
        float scaleY = Mathf.Abs(player.transform.lossyScale.y);
        float halfHeight = controller != null
            ? (controller.height * 0.5f - controller.center.y) * scaleY + controller.skinWidth + 0.05f
            : 0.5f * scaleY;

        if (Physics.Raycast(position + Vector3.up * 200f, Vector3.down, out RaycastHit hit, 500f,
                Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            position = hit.point + Vector3.up * halfHeight;
        }

        if (controller != null) controller.enabled = false;
        player.transform.SetPositionAndRotation(position, rotation);
        if (controller != null) controller.enabled = true;
    }
}
