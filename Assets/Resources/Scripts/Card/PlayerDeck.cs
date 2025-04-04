using System.Collections.Generic;
using System;

public class PlayerDeck : UnoDeck
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerDeck"/> class.
    /// Creates a deck for the player and shuffles it.
    /// </summary>
    public PlayerDeck()
    {
        // Create the player's deck of cards.
        SetCards(CreateDeck());  // Using the method from UnoDeck to set the cards
        // Shuffle the deck to randomize the order of cards.
        Shuffle();
    }

    #endregion

    #region Deck Creation

    /// <summary>
    /// Creates a deck of Uno cards for the player.
    /// Each color gets one zero card and one copy of each card numbered from 1 to 9.
    /// </summary>
    /// <returns>A list of <see cref="Card"/> objects representing the player's deck.</returns>
    private List<Card> CreateDeck()
    {
        var deck = new List<Card>();

        // Iterate through each card color.
        foreach (CardColor color in Enum.GetValues(typeof(CardColor)))
        {
            // Add a single zero card for the current color.
            deck.Add(new Card(color, 0));

            // Add one copy of each numbered card from 1 to 9.
            for (int number = 1; number <= 9; number++)
            {
                deck.Add(new Card(color, number));
            }
        }

        return deck;
    }

    #endregion
}
