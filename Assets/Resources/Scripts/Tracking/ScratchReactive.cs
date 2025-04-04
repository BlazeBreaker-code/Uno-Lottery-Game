using UnityEngine;

public abstract class ScratchReactive : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] protected BoxCollider boxCollider; // BoxCollider for the scratchable area.
    [SerializeField] protected float scratchThreshold = 0.5f; // Threshold for determining if the scratch is complete.

    #endregion

    #region Private Variables

    protected bool isScratchedComplete = false; // Tracks if the scratch is complete.

    #endregion

    #region Unity Methods

    /// <summary>
    /// Registers the ScratchReactive to the ServiceLocator on Awake.
    /// </summary>
    private void Awake()
    {
        ServiceLocator.RegisterToCollection<ScratchReactive>(this);
    }

    /// <summary>
    /// Subscribes to the scratch updated event on enable.
    /// </summary>
    private void OnEnable()
    {
        ScratchOffMaskUpdaterMonoBehaviour.OnScratchUpdated += OnScratchUpdated;
    }

    /// <summary>
    /// Unsubscribes from the scratch updated event on disable.
    /// </summary>
    private void OnDisable()
    {
        ScratchOffMaskUpdaterMonoBehaviour.OnScratchUpdated -= OnScratchUpdated;
    }

    #endregion

    #region Scratch Logic

    /// <summary>
    /// Updates the scratch status based on the current mask and bounds.
    /// </summary>
    public void OnScratchUpdated(Texture2D mask, Bounds scratchBounds)
    {
        if (isScratchedComplete || mask == null || boxCollider == null) return;

        Vector2 scratchCenter = new Vector2(scratchBounds.center.x, scratchBounds.center.y);
        Vector2 scratchExtents = new Vector2(scratchBounds.extents.x, scratchBounds.extents.y);
        Vector2 colliderCenter = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.center.y);

        // If scratch bounds do not overlap with the collider bounds, return early
        if (Mathf.Abs(colliderCenter.x - scratchCenter.x) > scratchExtents.x ||
            Mathf.Abs(colliderCenter.y - scratchCenter.y) > scratchExtents.y)
        {
            return;
        }

        Bounds cellBounds = boxCollider.bounds;

        // Calculate normalized bounds for comparison
        float normalizedX = (cellBounds.min.x - scratchBounds.min.x) / scratchBounds.size.x;
        float normalizedY = (cellBounds.min.y - scratchBounds.min.y) / scratchBounds.size.y;
        float normalizedWidth = cellBounds.size.x / scratchBounds.size.x;
        float normalizedHeight = cellBounds.size.y / scratchBounds.size.y;

        // Get pixel data from the mask
        int startX = Mathf.Clamp(Mathf.FloorToInt(normalizedX * mask.width), 0, mask.width - 1);
        int startY = Mathf.Clamp(Mathf.FloorToInt(normalizedY * mask.height), 0, mask.height - 1);
        int cellWidth = Mathf.Clamp(Mathf.FloorToInt(normalizedWidth * mask.width), 1, mask.width - startX);
        int cellHeight = Mathf.Clamp(Mathf.FloorToInt(normalizedHeight * mask.height), 1, mask.height - startY);

        Color[] pixels = mask.GetPixels(startX, startY, cellWidth, cellHeight);
        int scratchedPixels = 0;

        // Count the number of scratched pixels
        foreach (var pixel in pixels)
        {
            if (pixel.a < 0.1f) scratchedPixels++;
        }

        // Check if the scratch is complete based on the threshold
        float ratio = (float)scratchedPixels / pixels.Length;
        if (ratio >= scratchThreshold)
        {
            isScratchedComplete = true;
            OnScratchComplete(); // Trigger the completion logic
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Resets the scratch state, allowing the cell to be scratched again.
    /// </summary>
    public void ResetCell()
    {
        isScratchedComplete = false;
    }

    #endregion

    #region Abstract Methods

    /// <summary>
    /// Called when the scratch is complete. Must be implemented by derived classes.
    /// </summary>
    protected abstract void OnScratchComplete();

    #endregion
}
