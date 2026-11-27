using TMPro;
using UnityEngine;

public class ControlsHint : MonoBehaviour
{
    [SerializeField] private CameraSwitcher cameraSwitcher;
    [SerializeField] private TMP_Text hintText;

    [SerializeField, TextArea] private string overviewHint = "C - Switch to player";
    [SerializeField, TextArea] private string playerHint = "WASD - Move\nMouse - Look\nTab - Show cursor\nC - Switch to overview";

    private bool? lastState;

    private void Update()
    {
        if (cameraSwitcher == null || hintText == null) return;

        bool isPlayer = cameraSwitcher.IsPlayerCameraActive;
        if (lastState == isPlayer) return;

        lastState = isPlayer;
        hintText.text = isPlayer ? playerHint : overviewHint;
    }
}