using BehaviourAPI.UnityToolkit.GUIDesigner.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerMaskStat", menuName = "ScriptableObjects/CombatMaskStats")]

public class PowerMaskStats : ScriptableObject
{
    public float speedModifier;
    public float detectRadius;
    public int lifeModifier;

    public RuntimeAnimatorController controller;
    public BehaviourSystem IASystem;
    public MaskTypes type;
    public int Price;
    public float cooldown;
    public Sprite sprite;
    public float attackDistance;
    [SerializeField][SerializeReference] public List<ABaseEffect> effects;
    public HittableCheckTypes hittablecheck;
    public float initCooldown = 0.2f;
    public string descriptionText;

    public void addDamageEffect(int damage)
    {
        effects.Add(new DamageEffect(damage));
    }
    public void addFreezeEffect(float duration, float slowMultiplier)
    {
        effects.Add(new FreezeEffect(duration, slowMultiplier));
    }

}