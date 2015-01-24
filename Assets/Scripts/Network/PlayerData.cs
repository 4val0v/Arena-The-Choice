using System.Collections.Generic;

/// <summary>
/// Only Data! No logic
/// </summary>
public class PlayerData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CharacterClass Class { get; set; }
    public List<int> EquippedItems { get; set; }
}
