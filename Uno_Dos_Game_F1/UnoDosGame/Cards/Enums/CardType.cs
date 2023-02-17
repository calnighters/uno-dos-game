using System.ComponentModel;

namespace UnoDos.Cards.Enums
{
    public enum CardType
    {
        [Description("Zero")]
        Zero,
        [Description("One")]
        One,
        [Description("Two")]
        Two,
        [Description("Three")]
        Three,
        [Description("Four")]
        Four,
        [Description("Five")]
        Five,
        [Description("Six")]
        Six,
        [Description("Seven")]
        Seven,
        [Description("Eight")]
        Eight,
        [Description("Nine")]
        Nine,
        [Description("Reset")]
        Reset,
        [Description("Lose Two")]
        LoseTwo,
        [Description("Swap Deck")]
        SwapDeck,
        [Description("See Through")]
        SeeThrough
    }
}