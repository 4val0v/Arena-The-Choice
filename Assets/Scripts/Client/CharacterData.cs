using System.Collections.Generic;

public class CharacterData
{
    public CharacterClass Class { get; set; }

    public float BaseHp { get; set; }
    public float BaseDmg { get; set; }
    public float BaseDef { get; set; }
    public float BaseAttackSpeed { get; set; }
    public float BaseAccuracy { get; set; }
}

public static class CharacterDataProviders
{
    private readonly static Dictionary<CharacterClass, CharacterData> _characters;

    static CharacterDataProviders()
    {
        _characters = new Dictionary<CharacterClass, CharacterData>();

        _characters.Add(CharacterClass.Man, new CharacterData
        {
            Class = CharacterClass.Man,
            BaseHp = 100,
            BaseDmg = 20,
            BaseAttackSpeed = 10,
            BaseDef = 10,
            BaseAccuracy = 0.6f,
        });

        _characters.Add(CharacterClass.Woman, new CharacterData
        {
            Class = CharacterClass.Woman,
            BaseHp = 100,
            BaseDmg = 10,
            BaseAttackSpeed = 20,
            BaseDef = 10,
            BaseAccuracy = 0.6f,
        });

        _characters.Add(CharacterClass.Orc, new CharacterData
        {
            Class = CharacterClass.Orc,
            BaseHp = 100,
            BaseDmg = 15,
            BaseAttackSpeed = 10,
            BaseDef = 15,
            BaseAccuracy = 0.6f,
        });
    }

    public static CharacterData GetBaseData(CharacterClass classId)
    {
        return _characters[classId];
    }
}