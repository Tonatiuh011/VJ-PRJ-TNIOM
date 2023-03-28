using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UnitBase
{
    protected Action<float> OnHealthPointsChanged { get; set; }

    protected Action<UnitBase> OnDeath { get; set; }

    public float CurrentHP { get; private set; }

    public float AvgHP {
        get {
            try
            {
                return CurrentHP / HealthPoints;
            } catch {
                return 0;
            }
        }
    }

    public bool IsDead { get; private set; } = false;

    public float HealthPoints { get; set; }

    public float Damage { get; set; }

    public UnitBase(float hp, float damage, Action<float>  onHpChange, Action<UnitBase> onDeath)
    {
        HealthPoints = hp;
        CurrentHP = HealthPoints;
        Damage = damage;
        OnHealthPointsChanged = onHpChange;
        OnDeath = onDeath;
    }

    public void AddHP(float hp) {
        CurrentHP += hp;
        OnHealthPointsChanged(CurrentHP);
        IsDead = false;
    }

    public void AddDamage(float damage) => Damage += damage;

    public void Hit(float damage)
    {
        CurrentHP -= damage;
        OnHealthPointsChanged(CurrentHP);
        if (CurrentHP <= 0)
        {
            OnDeath(this);
            IsDead = true;
        }
    }
}