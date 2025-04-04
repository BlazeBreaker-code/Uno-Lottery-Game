using TMPro;

public class GameBoardTrackerMonoBehaviour : ScratchReactive
{
    #region Unity Methods

    /// <summary>
    /// Called when the scratch is complete. Triggers the reveal animation and informs the GameManager.
    /// </summary>
    protected override void OnScratchComplete()
    {
        string gameBoardText = GetComponentInParent<TextMeshProUGUI>().text;

        // Construct the card name using a formatted string
        string cardName = FormatCardName(gameBoardText);

        // Trigger reveal animation and notify GameManager
        TriggerRevealAnimation(cardName);
        ServiceLocator.Get<GameManager>().CellCompleted();
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Formats the card name by concatenating the text of the game board.
    /// </summary>
    /// <param name="gameBoardText">The text of the game board to use for formatting the card name.</param>
    /// <returns>The formatted card name.</returns>
    private string FormatCardName(string gameBoardText)
    {
        return gameBoardText.Substring(1) + "_" + gameBoardText[0];
    }

    /// <summary>
    /// Triggers the reveal animation for the given card name.
    /// </summary>
    /// <param name="cardName">The name of the card to reveal.</param>
    private void TriggerRevealAnimation(string cardName)
    {
        ServiceLocator.Get<WinnerManagerMonoBehaviour>().NotifyCardScratched(cardName);
    }

    #endregion
}
