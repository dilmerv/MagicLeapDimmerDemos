using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.MagicLeap;

public class DimmerController : MonoBehaviour
{
    private bool segmentedDimmerActivated;

    private MagicLeapInputs magicLeapInputs;

    private MagicLeapInputs.ControllerActions controllerActions;

    private void Awake()
    {
        magicLeapInputs = new MagicLeapInputs();
        magicLeapInputs.Enable();
        controllerActions = new MagicLeapInputs.ControllerActions(magicLeapInputs);
        
        // Touchpad position will be used for global dimmer changes
        controllerActions.TouchpadPosition.performed += TouchpadPositionPerformed;   
        
        // Bumper will toggle segmented dimmer state on and off
        controllerActions.Bumper.performed += BumperPerformed;
    }

    private void TouchpadPositionPerformed(InputAction.CallbackContext obj)
    {
        var touchValue = obj.ReadValue<Vector2>();
        var touchPosition = touchValue.x;
        float normalizedValue = (touchPosition + 1.0f) / 2.0f;
        
        Logger.Instance.LogInfo($"Pressed Touchpad {touchValue}");
        Logger.Instance.LogInfo($"{normalizedValue}");
        
        // This is what applies the global dimmer value from 0 to 1
        MLGlobalDimmer.SetValue(normalizedValue);
    }
    
    private void BumperPerformed(InputAction.CallbackContext obj)
    {
        bool bumperDown = obj.ReadValueAsButton();

        if (bumperDown)
        {
            segmentedDimmerActivated = !segmentedDimmerActivated;
            if (segmentedDimmerActivated)
            {
                MLSegmentedDimmer.Activate();
                Logger.Instance.LogInfo("Segmented dimmer activated...");
            }
            else
            {
                MLSegmentedDimmer.Deactivate();
                Logger.Instance.LogInfo("Segmented dimmer deactivated...");
            }
        }

    }
}
