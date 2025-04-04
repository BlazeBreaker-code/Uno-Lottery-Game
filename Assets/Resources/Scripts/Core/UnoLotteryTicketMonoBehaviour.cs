using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// Creates an Uno Lottery ticket with all visuals setup.
/// </summary>
public class UnoLotteryTicketMonoBehaviour : MonoBehaviour
{
    #region Public Variables

    public GameObject prizes; // UI element for prize display.
    public GameObject gameBoardCards; // Game board displaying cards.
    public GameObject playerCards; // Player's cards to interact with.

    #endregion

    #region Private Variables

    private GameBoardDeck gameBoardDeck; // Deck of cards for the game board.
    private PlayerDeck playerDeck; // Deck of cards for the player.

    #endregion

    #region Unity Methods

    /// <summary>
    /// Called on start to initialize the game.
    /// </summary>
    public void Start()
    {
        PlayNewGame();
    }

    #endregion

    #region Set Up Functions

    /// <summary>
    /// Sets up the game board with cards.
    /// </summary>
    private void SetUpGameBoard()
    {
        gameBoardCards.SetActive(false);

        foreach (SpriteRenderer renderer in gameBoardCards.GetComponentsInChildren<SpriteRenderer>())
        {
            Card card = gameBoardDeck.DrawCard();
            if (card == null)
            {
                Debug.LogWarning("No more cards in the deck!");
                break;
            }

            // Load and assign the sprite for the card
            renderer.sprite = Resources.Load<Sprite>("Sprites/Uno_Cards/" + card.ToString());
            if (renderer.sprite == null)
            {
                Debug.Log(card.ToString());
            }
        }

        gameBoardCards.SetActive(true);
    }

    /// <summary>
    /// Sets up the player's cards for interaction.
    /// </summary>
    private void SetUpPlayerCards()
    {
        playerCards.SetActive(false);

        List<TextMeshProUGUI> newPlayerCards = playerCards.GetComponentsInChildren<TextMeshProUGUI>().ToList();

        for (int i = 0; i < 66; i += 2)
        {
            Card card = playerDeck.DrawCard();
            if (card == null)
            {
                Debug.LogWarning("No more cards in the deck!");
                break;
            }

            string cardName = card.Number.ToString() + card.Color.ToString();
            newPlayerCards[i].text = card.Number.ToString();
            newPlayerCards[i + 1].text = cardName;
        }

        playerCards.SetActive(true);
    }

    /// <summary>
    /// Sets up the prize texts for the lottery ticket.
    /// </summary>
    private void SetUpPrizes()
    {
        List<TextMeshProUGUI> prizes_Texts = prizes.GetComponentsInChildren<TextMeshProUGUI>().ToList();

        foreach (TextMeshProUGUI prize_Text in prizes_Texts)
        {
            int randomWinning = GenerateWeightedRandom(1, 150000, exponent: 20f);
            string output = GetPrizeAmount(randomWinning);

            prize_Text.text = output;
        }
    }

    /// <summary>
    /// Starts a new game by initializing the decks and setting up the board, player cards, and prizes.
    /// </summary>
    public void PlayNewGame()
    {
        gameBoardDeck = new GameBoardDeck();
        playerDeck = new PlayerDeck();

        SetUpPlayerCards();
        SetUpGameBoard();
        SetUpPrizes();
    }

    #endregion

    #region Helper Functions

    /// <summary>
    /// Generates a weighted random number based on the given exponent.
    /// </summary>
    private int GenerateWeightedRandom(int min, int max, float exponent)
    {
        float r = UnityEngine.Random.value;
        int result = min + (int)(Mathf.Pow(r, exponent) * (max - min));
        return result;
    }

    /// <summary>
    /// Formats the prize amount based on the generated number.
    /// </summary>
    private string GetPrizeAmount(int randomWinning)
    {
        string output;
        switch (randomWinning)
        {
            case 1:
                output = "$1";
                break;
            case 2:
                output = "$2";
                break;
            case 3:
            case 4:
            case 5:
                output = "$5";
                break;
            default:
                output = randomWinning < 1000
                    ? "$" + (int)Math.Round(randomWinning / 10.0, MidpointRounding.AwayFromZero) * 10
                    : $"${(int)Math.Round(randomWinning / 1000.0, MidpointRounding.AwayFromZero)}k";
                break;
        }
        return output;
    }

    #endregion
}
