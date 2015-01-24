public class MaceAbility : BaseAbility
{
    public override AbilityType Id
    {
        get { return AbilityType.Mace; }
    }

    public override float Dmg
    {
        get
        {
            foreach (var item in Player.EquippedItems)
            {
                var itemData = ItemsProvider.GetItem(item);
                if (itemData.AbilityId == Id)
                {
                    return itemData.Dmg * DmgK;
                }
            }

            return 0f;
        }
    }

    public override float Def
    {
        get { return 0f; }
    }

    public override float AttackSpeed
    {
        get { return 0f; }
    }

    private const float DmgK = 2.3f;

    private bool _isDone;

    public override void UpdateOnAttack()
    {
        if (!_isDone)
        {
            _isDone = true;
            return;
        }

        Player.Abilities.Remove(this);
    }
}
