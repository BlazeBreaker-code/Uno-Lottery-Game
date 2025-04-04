using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Public Variables

    public GameObject playAgainScreen; // Play again screen UI element.
    public TextMeshProUGUI currentWinnings; // Displays current winnings in the UI.
    public int requiredCells = 33; // Total number of cells to complete in the game.

    #endregion

    #region Private Variables

    private int completedCells = 0; // Number of cells completed by the player.
    private bool gameFinished = false; // Tracks if the game has finished.
    private int prizeToCollect = 0; // Tracks the number of prizes to collect.

    #endregion

    #region Unity Methods

    /// <summary>
    /// Called when the script is first initialized.
    /// Registers the GameManager with the ServiceLocator.
    /// </summary>
    public void Awake()
    {
        ServiceLocator.Register<GameManager>(this);
    }

    #endregion

    #region Game Logic Methods

    /// <summary>
    /// Increments completed cells and checks if the game has finished.
    /// </summary>
    public void CellCompleted()
    {
        completedCells++;

        // Check if the game is finished after each cell completion
        if (completedCells >= requiredCells && !gameFinished)
        {
            gameFinished = true;

            // Start the end game process if there are no prizes to collect
            if (prizeToCollect == 0)
            {
                StartCoroutine(EndGame());
            }
        }
    }

    /// <summary>
    /// Increments the prize counter when waiting to redeem a prize.
    /// </summary>
    public void WaitingToRedeem()
    {
        prizeToCollect++;
    }

    /// <summary>
    /// Decrements the prize counter when a prize is collected.
    /// Starts the end game process if the game is finished and no more prizes are left to collect.
    /// </summary>
    public void PrizeComplete()
    {
        prizeToCollect--;

        // Check if the game is finished and no prizes are left to collect
        if (gameFinished && prizeToCollect == 0)
        {
            StartCoroutine(EndGame());
        }
    }

    /// <summary>
    /// Ends the game after a delay and resets game variables.
    /// </summary>
    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2f);

        // Show the play again screen if there are no prizes to collect
        if (prizeToCollect == 0)
        {
            playAgainScreen.SetActive(true);
            completedCells = 0;
            gameFinished = false;
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Resets the scratch trackers for all cells in the game.
    /// </summary>
    public void ResetScratchTrackers()
    {
        foreach (var cell in ServiceLocator.GetAll<ScratchReactive>())
        {
            cell.ResetCell();
        }
    }

    /// <summary>
    /// Adds the specified amount to the current winnings and updates the UI.
    /// </summary>
    /// <param name="newWinnings">Amount to add to the current winnings.</param>
    public void AddToCurrentWinnings(int newWinnings)
    {
        // Get the current winnings text, removing the "$" symbol and any commas
        string winnings = currentWinnings.text.Replace("$", "").Replace(",", "");

        // Try parsing the current winnings as an integer
        if (int.TryParse(winnings, out int currentValue))
        {
            // Add the value to the current winnings
            int updatedValue = currentValue + newWinnings;

            // Format the updated value with commas
            string formattedWinnings = updatedValue.ToString("#,0");

            // Update the text with the formatted value, adding the "$" symbol
            currentWinnings.text = "$" + formattedWinnings;
        }
        else
        {
            // Handle the case where parsing the current winnings failed
            Debug.LogError("Failed to parse current winnings text. Text was: " + currentWinnings.text);
        }
    }

    #endregion
}
