using TMPro;
using UnityEngine;

public class PrizeTracker : ScratchReactive
{
    #region Public Variables

    public GameObject winningBorder; // UI element for indicating a winning row.
    public TextMeshProUGUI prizeMoney; // Displays the prize amount.

    #endregion

    #region Private Variables

    private bool isWinningRow = false; // Indicates if the current row is a winning row.
    private bool isScratched = false; // Tracks if the prize has been scratched.

    #endregion

    #region Game Logic Methods

    /// <summary>
    /// Handles the winning state of the row. Activates the winning border and notifies the GameManager.
    /// </summary>
    public void YouWin()
    {
        if (!isScratched)
        {
            winningBorder.SetActive(true);
            ServiceLocator.Get<GameManager>().WaitingToRedeem();
        }
        else
        {
            GivePlayerMoney();
        }

        isWinningRow = true;
    }

    /// <summary>
    /// Called when the scratch is complete. If it's a winning row, deactivates the border and gives money.
    /// </summary>
    protected override void OnScratchComplete()
    {
        isScratched = true;

        if (isWinningRow)
        {
            winningBorder.SetActive(false);
            GivePlayerMoney();
            ServiceLocator.Get<GameManager>().PrizeComplete();
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Parses the prize money text and adds the corresponding amount to the player's current winnings.
    /// </summary>
    private void GivePlayerMoney()
    {
        string prize = prizeMoney.text;
        int value = 0;

        // Check if the prize text contains "k" indicating thousands
        if (prize.Contains("k"))
        {
            // Remove "$" and "k", parse it into an integer, then multiply by 1000
            string numberPart = prize.Replace("$", "").Replace("k", "").Trim();
            value = (int)(float.Parse(numberPart) * 1000);
        }
        else
        {
            // For non-thousand prizes, just parse the number
            string numberPart = prize.Replace("$", "").Trim();
            value = int.Parse(numberPart);
        }

        // Add the parsed prize value to the current winnings
        ServiceLocator.Get<GameManager>().AddToCurrentWinnings(value);
    }

    /// <summary>
    /// Resets the prize tracker for the next round.
    /// </summary>
    public void ResetPrize()
    {
        isWinningRow = false;
        isScratched = false;
    }

    #endregion
}
