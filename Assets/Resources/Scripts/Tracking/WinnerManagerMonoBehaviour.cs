using System.Collections.Generic;
using UnityEngine;

public class WinnerManagerMonoBehaviour : MonoBehaviour
{
    #region Private Variables

    private List<RowWinnerTrackerMonobehaviour> rowTrackers = new List<RowWinnerTrackerMonobehaviour>(); // List of row winner trackers.

    #endregion

    #region Unity Methods

    /// <summary>
    /// Registers the WinnerManager in the ServiceLocator on Awake.
    /// </summary>
    public void Awake()
    {
        ServiceLocator.Register<WinnerManagerMonoBehaviour>(this);
    }

    /// <summary>
    /// Retrieves all RowWinnerTrackerMonoBehaviour instances from the ServiceLocator at the start.
    /// </summary>
    public void Start()
    {
        rowTrackers = ServiceLocator.GetAll<RowWinnerTrackerMonobehaviour>();
    }

    #endregion

    #region Game Logic Methods

    /// <summary>
    /// Notifies all row trackers that a card has been scratched.
    /// </summary>
    public void NotifyCardScratched(string cardName)
    {
        foreach (var row in rowTrackers)
        {
            row.ContainsCard(cardName); // Check if the row contains the scratched card
        }
    }

    #endregion
}
