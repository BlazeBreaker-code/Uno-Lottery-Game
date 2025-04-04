using System.Collections.Generic;
using System;

public class GameBoardDeck : UnoDeck
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="GameBoardDeck"/> class.
    /// Creates a deck for the player and shuffles it.
    /// </summary>
    public GameBoardDeck()
    {
        // Create the player's deck of cards.
        SetCards(CreateDeck());  // Using the method from UnoDeck to set the cards
        // Shuffle the deck to randomize the order of cards.
        Shuffle();
    }

    #endregion

    #region Deck Creation

    /// <summary>
    /// Creates a deck of Uno cards for a player.
    /// Each color gets one zero card and two copies of each card numbered from 1 to 9.
    /// </summary>
    /// <returns>A list of <see cref="Card"/> objects representing the player's deck.</returns>
    private List<Card> CreateDeck()
    {
        var deck = new List<Card>();

        // Iterate through each possible card color.
        foreach (CardColor color in Enum.GetValues(typeof(CardColor)))
        {
            // Add a single zero card for the current color.
            deck.Add(new Card(color, 0));

            // For numbers 1 to 9, add two copies of each card.
            for (int number = 1; number <= 9; number++)
            {
                deck.Add(new Card(color, number));
                deck.Add(new Card(color, number));
            }
        }

        return deck;
    }

    #endregion
}
