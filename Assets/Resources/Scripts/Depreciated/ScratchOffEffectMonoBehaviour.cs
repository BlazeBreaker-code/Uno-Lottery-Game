using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Obsolete("ScratchOffEffectMonoBehaviour is obsolete. We are now using a Shader instead.", false)]
public class ScratchOffEffectMonoBehaviour : MonoBehaviour
{
    #region Serialized Fields

    /// <summary>
    /// Prefab for the mask that will be instantiated during the scratch effect.
    /// </summary>
    public GameObject maskPrefab;

    #endregion

    #region Private Fields

    private InputAction clickAction;  // Input action to detect mouse click.
    private bool isClicked = false;   // Tracks if the mouse button is being held down.

    #endregion

    #region Unity Methods

    /// <summary>
    /// Sets up the input action for detecting left-click.
    /// </summary>
    private void Awake()
    {
        clickAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
        clickAction.started += OnClickPress;
        clickAction.canceled += OnClickRelease;
    }

    private void OnEnable()
    {
        clickAction.Enable();
    }

    private void OnDisable()
    {
        clickAction.Disable();
    }

    #endregion

    #region Input Handling

    /// <summary>
    /// Starts the scratch effect when the mouse button is pressed.
    /// </summary>
    private void OnClickPress(InputAction.CallbackContext context)
    {
        isClicked = true;
        StartCoroutine(ScratchOff());
    }

    /// <summary>
    /// Stops the scratch effect when the mouse button is released.
    /// </summary>
    private void OnClickRelease(InputAction.CallbackContext context)
    {
        isClicked = false;
    }

    #endregion

    #region Scratch Logic

    /// <summary>
    /// Instantiates the maskPrefab at the position of the mouse while the left mouse button is pressed.
    /// </summary>
    private IEnumerator ScratchOff()
    {
        while (isClicked)
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Instantiate(maskPrefab, clickPosition, Quaternion.identity);  // Instantiate the mask at the click position.
            yield return null;  // Wait until the next frame.
        }
    }

    #endregion
}
