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
            BaseHp = 3000,
            BaseDmg = 0,
            BaseAttackSpeed = 0,
            BaseDef = 0,
            BaseAccuracy = 0,
        });

        _characters.Add(CharacterClass.Woman, new CharacterData
        {
            Class = CharacterClass.Woman,
			BaseHp = 3000,
			BaseDmg = 0,
			BaseAttackSpeed = 0,
			BaseDef = 0,
			BaseAccuracy = 0,
        });

        _characters.Add(CharacterClass.Orc, new CharacterData
        {
            Class = CharacterClass.Orc,
			BaseHp = 3000,
			BaseDmg = 0,
			BaseAttackSpeed = 0,
			BaseDef = 0,
			BaseAccuracy = 0,
        });
    }

    public static CharacterData GetBaseData(CharacterClass classId)
    {
        return _characters[classId];
    }
}