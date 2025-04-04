using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowWinnerTrackerMonobehaviour : MonoBehaviour
{
    #region Public Variables

    public PrizeTracker prizeTracker; // Reference to the prize tracker.

    #endregion

    #region Private Variables

    private Dictionary<string, SpriteRenderer> cardSpritePairs = new Dictionary<string, SpriteRenderer>(); // Stores card name and associated SpriteRenderer.

    #endregion

    #region Unity Methods

    /// <summary>
    /// Called when the script is initialized. Registers the RowWinnerTracker in the ServiceLocator.
    /// </summary>
    public void Awake()
    {
        ServiceLocator.RegisterToCollection<RowWinnerTrackerMonobehaviour>(this);
    }

    /// <summary>
    /// Called when the object is enabled. Initializes the cardSpritePairs dictionary.
    /// </summary>
    public void OnEnable()
    {
        foreach (SpriteRenderer s in this.gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            string cardName = s.sprite.name;

            // Add card name and sprite renderer to the dictionary
            if (!cardSpritePairs.ContainsKey(cardName))
            {
                cardSpritePairs.Add(cardName, s);
            }
            else
            {
                cardSpritePairs.Add($"{cardName}_second", s); // Handles case where the same card appears twice
            }
        }
    }

    /// <summary>
    /// Called when the object is disabled. Resets all the scratch effects and clears cardSpritePairs.
    /// </summary>
    public void OnDisable()
    {
        foreach (SpriteRenderer s in this.gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            ResetScratchEffect(s);
        }

        cardSpritePairs.Clear(); // Clear the cardSpritePairs dictionary
        prizeTracker.ResetPrize(); // Reset the prize tracker
    }

    #endregion

    #region Game Logic Methods

    /// <summary>
    /// Checks if the card exists in the dictionary and removes it if found.
    /// </summary>
    public bool ContainsCard(string cardName)
    {
        if (!cardSpritePairs.ContainsKey(cardName))
        {
            return false;
        }
        else
        {
            StartCoroutine(RemoveCard(cardName));

            string secondCardName = $"{cardName}_second";
            if (cardSpritePairs.ContainsKey(secondCardName))
                StartCoroutine(RemoveCard(secondCardName)); // Check and remove second instance of the card if it exists

            return true;
        }
    }

    /// <summary>
    /// Removes the card from the dictionary and checks for a winner.
    /// </summary>
    private IEnumerator RemoveCard(string cardName)
    {
        yield return AnimateDiagonalScratch(cardSpritePairs[cardName]); // Animate the scratch effect
        cardSpritePairs.Remove(cardName); // Remove the card from the dictionary
        CheckWinner(); // Check if the player has won
    }

    /// <summary>
    /// Checks if there are no more cards left, and if so, triggers a win.
    /// </summary>
    private void CheckWinner()
    {
        if (cardSpritePairs.Count == 0)
        {
            prizeTracker.YouWin(); // Call the prize tracker to declare the player a winner
        }
    }

    #endregion

    #region Scratch Animation Methods

    /// <summary>
    /// Animates the diagonal scratch effect on the given SpriteRenderer.
    /// </summary>
    private IEnumerator AnimateDiagonalScratch(SpriteRenderer sr, float duration = 1.0f)
    {
        Material mat = sr.material;
        mat.SetFloat("_ScratchProgress", 0f);
        mat.SetFloat("_Lighten", 0.8f);

        float t = 0f;
        while (t < duration)
        {
            float progress = Mathf.Lerp(0f, 1f, t / duration);
            mat.SetFloat("_ScratchProgress", progress);
            t += Time.deltaTime;
            yield return null;
        }

        mat.SetFloat("_ScratchProgress", 1f);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Resets the scratch effect on the given SpriteRenderer.
    /// </summary>
    private void ResetScratchEffect(SpriteRenderer sr)
    {
        Material mat = sr.material;

        // Reset material properties related to the scratch effect
        mat.SetFloat("_ScratchProgress", 0f);
        mat.SetFloat("_Lighten", 0f); // Reset to default lightening value
        mat.SetColor("_Color", Color.white); // Reset the color to default
    }

    #endregion
}
