using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    #region Public Variables

    public GameObject firstPanel;   // First panel of the tutorial.
    public GameObject secondPanel;  // Second panel of the tutorial.
    public GameObject thirdPanel;   // Third panel of the tutorial.
    public bool HasTutorialStarted { get; private set; } // Property to track if tutorial has started.

    #endregion

    #region Unity Methods

    /// <summary>
    /// Initializes the first tutorial panel on start.
    /// </summary>
    public void Start()
    {
        HasTutorialStarted = false;  // Ensure the tutorial hasn't started yet.
        firstPanel.SetActive(true);  // Show the first tutorial panel at the beginning.
    }

    #endregion

    #region Tutorial Methods

    /// <summary>
    /// Starts the tutorial sequence by showing the second and third panels after the first.
    /// </summary>
    public IEnumerator TriggerTutorial()
    {
        if (!HasTutorialStarted)
        {
            HasTutorialStarted = true;  // Mark the tutorial as started.

            firstPanel.SetActive(false);  // Hide the first panel.

            // Show second and third panels with a delay in between
            yield return SwitchPanel(secondPanel);  // Show the second panel for 10 seconds.
            yield return SwitchPanel(thirdPanel);   // Show the third panel for 10 seconds.
        }
    }

    /// <summary>
    /// Controls the sequence of showing and hiding panels during the tutorial.
    /// </summary>
    private IEnumerator SwitchPanel(GameObject panel)
    {
        panel.SetActive(true);  // Activate the panel.
        yield return new WaitForSeconds(10);  // Wait for 10 seconds before hiding the panel.
        panel.SetActive(false);  // Deactivate the panel.
    }

    #endregion
}
