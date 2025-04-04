using System.Collections.Generic;
using System;

public class UnoDeck
{
    #region Properties

    /// <summary>
    /// The list of cards in the deck.
    /// </summary>
    protected List<Card> Cards { get; private set; } = new List<Card>();

    /// <summary>
    /// Gets the number of cards remaining in the deck.
    /// </summary>
    public int RemainingCards => Cards.Count;

    #endregion

    #region Private Variables

    private Random rand = new Random();

    #endregion

    #region Public Methods

    /// <summary>
    /// Shuffles the deck using a simple in-place algorithm.
    /// </summary>
    public void Shuffle()
    {
        int n = Cards.Count;

        // Loop through all cards in the deck and swap each with a random card.
        for (int i = 0; i < n; i++)
        {
            int j = rand.Next(i + 1);
            SwapCards(i, j);
        }
    }

    /// <summary>
    /// Draws the top card from the deck and removes it.
    /// </summary>
    /// <returns>The drawn <see cref="Card"/> if available; otherwise, null if the deck is empty.</returns>
    public Card DrawCard()
    {
        if (Cards.Count == 0)
        {
            Console.WriteLine("No cards left in the deck!");
            return null;
        }

        var drawnCard = Cards[Cards.Count - 1];
        Cards.RemoveAt(Cards.Count - 1);
        return drawnCard;
    }

    /// <summary>
    /// Set the list of cards in the deck.
    /// </summary>
    /// <param name="cards">The list of cards to set.</param>
    protected void SetCards(List<Card> cards)
    {
        Cards = cards;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Swaps two cards in the deck at specified indices.
    /// </summary>
    /// <param name="i">The index of the first card to swap.</param>
    /// <param name="j">The index of the second card to swap.</param>
    private void SwapCards(int i, int j)
    {
        var temp = Cards[i];
        Cards[i] = Cards[j];
        Cards[j] = temp;
    }

    #endregion
}
