public class EnemyShieldAbility : ShieldAbility
{
    public override AbilityType Id
    {
        get { return AbilityType.EnemyShield; }
    }

    protected override void RemoveFromAbilities()
    {
        PunNetClient.Instance.EnemyData.Abilities.Remove(this);
    }
}
