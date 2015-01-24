﻿public abstract class BaseAbility
{
    public abstract AbilityType Id { get; }

    public abstract float Dmg { get; }
    public abstract float Def { get; }
    public abstract float AttackSpeed { get; }

    protected INetClient Client { get { return PunNetClient.Instance; } }

    protected PlayerData Player { get { return Client.PlayerData; } }

    protected AbilityData BaseData { get { return AbilitiesProvider.GetAbility(Id); } }

    public virtual void Update()
    {

    }

    public virtual void UpdateOnAttack()
    {

    }
}