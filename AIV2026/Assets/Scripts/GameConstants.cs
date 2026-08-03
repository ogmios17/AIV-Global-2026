/// <summary>
/// Centralized string keys used with the Input System and Animators.
/// Keeping them here avoids scattered magic strings and typos across the codebase.
/// </summary>
public static class ActionMaps
{
    public const string CardSelection = "CardSelection";
    public const string Mash = "Mash";
    public const string Sequence = "Sequence";
    public const string Defeat = "Defeat";
}

/// <summary>Single source of truth for the per-character defeat flavor text.</summary>
public static class CharacterFlavor
{
    public static string DefeatMessage(CharacterType type) => type switch
    {
        CharacterType.NotZilla => "Not Zilla was Godzilla all along!",
        CharacterType.CrackKen => "Krack Ken was a squid all along!",
        _ => string.Empty,
    };
}

/// <summary>Cardinal input directions for the sequence minigame.</summary>
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public static class AnimTriggers
{
    // Move triggers — these must match MoveCard.cardName exactly.
    public const string Attack = "Attack";
    public const string Block = "Block";
    public const string Grapple = "Grapple";
    public const string Shove = "Shove";

    // Fight flow triggers.
    public const string Reveal = "Reveal";
    public const string Next = "Next";
    public const string Damage = "Damage";
    public const string Defeat = "Defeat";
    public const string Out = "Out";

    // Minigame feedback triggers.
    public const string Right = "Right";
    public const string Wrong = "Wrong";
}
