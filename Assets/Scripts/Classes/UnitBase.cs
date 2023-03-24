using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Classes
{
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

        public float HealthPoints { get; set; }

        public float Damage { get; set; }

        public UnitBase(float hp, float damage, Action<float>  onHpChange, Action<UnitBase> onDeath)
        {
            CurrentHP = hp;
            Damage = damage;
            OnHealthPointsChanged = onHpChange;
            OnDeath = onDeath;
        }

        public void AddHP(float hp) { 
            CurrentHP += hp; 
            OnHealthPointsChanged(CurrentHP);
        }

        public void AddDamage(float damage) => Damage += damage;

        public void Hit(float damage)
        {
            CurrentHP -= damage;
            OnHealthPointsChanged(CurrentHP);
            if (HealthPoints <= 0)
                OnDeath(this);
        }
    }
}