/// <summary>
/// Represents the possible colors for a card.
/// </summary>
public enum CardColor
{
    Red,    // Red card color
    Green,  // Green card color
    Blue,   // Blue card color
    Yellow  // Yellow card color
}

/// <summary>
/// Represents a card in the game, consisting of a color and a number.
/// </summary>
public class Card
{
    #region Properties

    /// <summary>
    /// The color of the card.
    /// </summary>
    public CardColor Color { get; private set; }

    /// <summary>
    /// The number on the card.
    /// </summary>
    public int Number { get; }

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the Card class with the specified color and number.
    /// </summary>
    /// <param name="color">The color of the card.</param>
    /// <param name="number">The number on the card.</param>
    public Card(CardColor color, int number)
    {
        Color = color;
        Number = number;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Returns a string representation of the card in the format "Color_Number".
    /// </summary>
    public override string ToString() => $"{Color}_{Number}";

    #endregion
}
