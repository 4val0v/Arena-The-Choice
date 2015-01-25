public class EnemyBigShieldAbility : BigShieldAbility
{
    public override AbilityType Id
    {
        get { return AbilityType.EnemyBigShield; }
    }

    protected override void RemoveFromAbilities()
    {
        PunNetClient.Instance.EnemyData.Abilities.Remove(this);
    }
}
