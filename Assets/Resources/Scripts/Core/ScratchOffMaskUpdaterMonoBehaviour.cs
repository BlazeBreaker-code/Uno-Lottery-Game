using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Allows sprites using certain custom shaders to properly be scratched.
/// </summary>
public class ScratchOffMaskUpdaterMonoBehaviour : MonoBehaviour
{
    #region Serialized Fields

    public GameObject playerCards; // Player's cards for scratch interaction.

    [Header("Mask Settings")]
    [Tooltip("Fixed brush size in world units. This size remains consistent across sprites.")]
    [Range(0.05f, 1f)]
    public float brushSizeWorld = 0.2f; // Brush size in world units.

    #endregion

    #region Private Variables

    private int textureWidth = 0; // Width of the mask texture.
    private int textureHeight = 0; // Height of the mask texture.

    private InputAction clickAction; // Input action for detecting left-click.
    private Texture2D maskTexture; // The texture applied to masks during scratching.
    private bool isClicked = false; // Tracks whether the mouse is clicked.

    // Dictionary of GameObjects and their associated mask textures.
    private Dictionary<GameObject, Texture2D> maskDict = new Dictionary<GameObject, Texture2D>();

    #endregion

    #region Events

    // Event that notifies subscribers that the scratch mask has been updated.
    public static event Action<Texture2D, Bounds> OnScratchUpdated;

    #endregion

    #region Unity Methods

    /// <summary>
    /// Sets up input actions for clicking and releases the event.
    /// </summary>
    private void Awake()
    {
        clickAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
        clickAction.started += OnClickPress;
        clickAction.canceled += OnClickRelease;
    }

    private void OnEnable() => clickAction?.Enable();
    private void OnDisable() => clickAction?.Disable();

    #endregion

    #region Input Handling

    /// <summary>
    /// Starts the scratch-off effect when the left-click is pressed.
    /// </summary>
    private void OnClickPress(InputAction.CallbackContext context)
    {
        isClicked = true;
        StartCoroutine(ScratchOff());
    }

    /// <summary>
    /// Stops the scratch-off effect when the left-click is released.
    /// </summary>
    private void OnClickRelease(InputAction.CallbackContext context)
    {
        isClicked = false;
    }

    #endregion

    #region Scratch Logic

    /// <summary>
    /// Tracks user input and updates the scratch mask. 
    /// Applies a circle on the texture as the user clicks.
    /// </summary>
    private IEnumerator ScratchOff()
    {
        int scratchLayerMask = LayerMask.GetMask("Scratch");

        while (isClicked)
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Collider2D hitCollider = Physics2D.OverlapPoint(clickPosition, scratchLayerMask);

            if (hitCollider != null)
            {
                SpriteRenderer sr = hitCollider.GetComponent<SpriteRenderer>();
                if (sr == null) break;

                GameObject go = sr.gameObject;

                textureWidth = (int)sr.sprite.rect.width;
                textureHeight = (int)sr.sprite.rect.height;

                if (!maskDict.ContainsKey(go))
                {
                    maskTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
                    Color32[] colors = new Color32[textureWidth * textureHeight];
                    Array.Fill(colors, Color.black);
                    maskTexture.SetPixels32(colors);
                    maskTexture.Apply();
                    sr.material.SetTexture("_MaskTex", maskTexture);
                    maskDict[go] = maskTexture;
                }
                else
                {
                    maskTexture = maskDict[go];
                }

                Vector2 relativePos = clickPosition - (Vector2)hitCollider.bounds.min;
                Vector2 uv = new Vector2(relativePos.x / hitCollider.bounds.size.x, relativePos.y / hitCollider.bounds.size.y);
                int pixelX = Mathf.RoundToInt(uv.x * textureWidth);
                int pixelY = Mathf.RoundToInt(uv.y * textureHeight);

                float pixelsPerUnit = textureWidth / hitCollider.bounds.size.x;
                int effectiveBrushSize = Mathf.RoundToInt(brushSizeWorld * pixelsPerUnit);

                DrawCircle(maskTexture, pixelX, pixelY, effectiveBrushSize, new Color(1f, 1f, 1f, 0f));
                maskTexture.Apply();

                Bounds maskBounds = sr.bounds;

                // Notify subscribers that the scratch mask has been updated
                OnScratchUpdated?.Invoke(maskTexture, maskBounds);
            }
            yield return null;
        }
    }

    #endregion

    #region Brush and Reset Functions

    /// <summary>
    /// Resets all scratched objects to their original un-scratched state.
    /// </summary>
    public void ResetScratchedObjects()
    {
        foreach (var entry in maskDict)
        {
            GameObject obj = entry.Key;
            Texture2D maskTexture = entry.Value;

            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                // Reset mask texture to fully black (no scratch).
                Texture2D originalMaskTexture = new Texture2D(maskTexture.width, maskTexture.height, TextureFormat.RGBA32, false);
                Color32[] blackPixels = new Color32[originalMaskTexture.width * originalMaskTexture.height];
                Array.Fill(blackPixels, Color.black);
                originalMaskTexture.SetPixels32(blackPixels);
                originalMaskTexture.Apply();

                // Apply reset texture to material.
                sr.material.SetTexture("_MaskTex", originalMaskTexture);
                sr.material.SetFloat("_ScratchAmount", 0f); // Reset scratch effect.
                sr.material.color = Color.white; // Reset to original color.
            }
        }

        maskDict.Clear();
    }

    #endregion

    #region Brush Drawing

    /// <summary>
    /// Draws a filled circle on the texture at the specified position and radius.
    /// </summary>
    private void DrawCircle(Texture2D tex, int cx, int cy, int radius, Color col)
    {
        int x, y, px, nx, py, ny, d;
        for (x = 0; x <= radius; x++)
        {
            d = (int)Mathf.Ceil(Mathf.Sqrt(radius * radius - x * x));
            for (y = 0; y <= d; y++)
            {
                px = cx + x;
                nx = cx - x;
                py = cy + y;
                ny = cy - y;

                if (px >= 0 && px < tex.width && py >= 0 && py < tex.height)
                    tex.SetPixel(px, py, col);
                if (nx >= 0 && nx < tex.width && py >= 0 && py < tex.height)
                    tex.SetPixel(nx, py, col);
                if (px >= 0 && px < tex.width && ny >= 0 && ny < tex.height)
                    tex.SetPixel(px, ny, col);
                if (nx >= 0 && nx < tex.width && ny >= 0 && ny < tex.height)
                    tex.SetPixel(nx, ny, col);
            }
        }
    }

    #endregion
}
