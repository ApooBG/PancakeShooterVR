using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerVibration : MonoBehaviour
{
    // Define controller types
    public OVRInput.Controller leftController = OVRInput.Controller.LTouch;
    public OVRInput.Controller rightController = OVRInput.Controller.RTouch;

    // Haptic feedback parameters
    public float vibrationFrequency = 1.0f; // Frequency of the vibration
    public float vibrationAmplitude = 1.0f; // Amplitude (intensity) of the vibration
    public float vibrationDuration = 0.5f; // Duration of the vibration in seconds

    // Method to stop haptic feedback
    void StopHapticFeedback()
    {
        // Stop vibration on the left controller
        OVRInput.SetControllerVibration(0, 0, leftController);
        // Stop vibration on the right controller
        OVRInput.SetControllerVibration(0, 0, rightController);
        this.enabled = false;
    }

    public void RightControllerVibration()
    {
        OVRInput.SetControllerVibration(vibrationFrequency, vibrationAmplitude, rightController);
        Invoke("StopHapticFeedback", vibrationDuration);
    }

    public void LeftControllerVibration()
    {
        OVRInput.SetControllerVibration(vibrationFrequency, vibrationAmplitude, leftController);
        Invoke("StopHapticFeedback", vibrationDuration);
    }
}
