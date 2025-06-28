using System;
using BrokerChain.Status;
using StateMachine;
using UnityEngine;


namespace BrokerChain
{
    public class StatsSystem : ComponentBehavior
    {
        [Header("Status")] public int OriginalDamage;
        
        public int CurrentHp;
        public int Damage;
        public int Defense;
        public int ExtraDamage;

        private void Update()
        {
            UpdateValue();
        }

        private void UpdateValue()
        {
            OriginalDamage = Stats.EntityStats.Damage;
            CurrentHp = Stats.EntityStats.HP;
            Damage = Stats.Damage;
            Defense = Stats.Defense;
            ExtraDamage = Stats.ExtraTakenDamage;
        }

        [SerializeField] private Entity entity;
        [SerializeField] private EntityStatsData entityStatsData;
        
      
        public Stats Stats;
        [SerializeField] private HealthUICtrl healthUICtrl;
        [SerializeField] private StatusUICtrl statusUICtrl;
        public override void LoadComponent()
        {
            base.LoadComponent();
            if (entity == null) entity = GetComponent<Entity>();
            if (entityStatsData == null)
            {
                Debug.LogError("Enetity Stats is null");
                return;
            }
            if (healthUICtrl == null) healthUICtrl = transform.Find("UI/HP bar").GetComponent<HealthUICtrl>();
            
            if (statusUICtrl == null) statusUICtrl = transform.Find("UI/Status").GetComponent<StatusUICtrl>();
        }

       
        public void Init()
        {
            EntityStats entityStats = new EntityStats()
            {
                MaxHP = entityStatsData.HP,
                HP = entityStatsData.HP,
                Defense = entityStatsData.Defense,
                Damage = entityStatsData.Damage
            };
            Stats = new Stats(new StatsMediator(), entityStats);
            if (healthUICtrl == null) healthUICtrl = transform.Find("UI/HP bar").GetComponent<HealthUICtrl>();
            healthUICtrl.Init(Stats.EntityStats);
            Stats.EntityStats.OnHPChange?.Invoke(Stats.EntityStats.HP, Stats.EntityStats.MaxHP);
        }
        public void StartTurn() => Stats?.Mediator.Update(ExpireTiming.StartOfThisTurn);
        public void EndTurn() => Stats?.Mediator.Update(ExpireTiming.EndOfThisTurn);

        public void AddModifier(StatusEffectData statusEffectData)
        {
            
            Action onRemoved = null;
            if (statusUICtrl != null)
            {
                statusUICtrl.AddEffect(statusEffectData);
                onRemoved = () => statusUICtrl.RemoveEffect(statusEffectData);
            }
            Stats.Mediator.AddModifier(statusEffectData, onRemoved);
           
        }

        public int TakeDamage(int damage)
        {
            int defense = Stats.Defense;

            damage += Stats.ExtraTakenDamage;
            
            
            if (defense > 0) damage = Math.Max(0, damage - defense);

            if (damage > 0) entity.IsHurting = true;
            
            entity.damagePopupUI.OrNull()?.Show(damage, Color.red);
            //Change base hp
            int curHP = Math.Max(Stats.EntityStats.HP - damage,0);
            Stats.EntityStats.HP = curHP;
            if (curHP <= 0) entity.IsDead = true;
            return damage;
        }

        public void Health(int value)
        {
            if (value == 0) return;
            entity.damagePopupUI.OrNull()?.Show(value, Color.green);
            Stats.EntityStats.HP = Math.Min(Stats.EntityStats.HP + value, Stats.EntityStats.MaxHP);
        }
    }
}