using MelonLoader;
using BTD_Mod_Helper;
using SallyPokehellHero;
using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Unity;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Simulation.Towers;
using System;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2Cpp;
using Il2CppSystem.IO;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using UnityEngine.AddressableAssets;
using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppAssets.Scripts.Simulation.Towers.Filters;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;
using Random = System.Random;
using static Il2CppAssets.Scripts.Utils.ObjectCache;
using BTD_Mod_Helper.Api.Legends;
using Il2CppAssets.Scripts.Models.Artifacts.Behaviors;
using Il2CppAssets.Scripts.Models.Artifacts;
using Il2CppAssets.Scripts.Models.ServerEvents;
using BTD_Mod_Helper.Api.ModOptions;
using Il2CppAssets.Scripts.Models.TowerSets;
using System.Collections.Generic;

[assembly: MelonInfo(typeof(SallyPokehellHero.SallyPokehellHero), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace SallyPokehellHero;

public class SallyPokehellHero : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<SallyPokehellHero>("SallyPokehellHero loaded!");
    }
    public override void OnTowerSelected(Tower tower)
    {
        if (tower.model.name.Contains("Sally"))
        {
            ModContent.GetAudioClip<SallyPokehellHero>("vo_selected_" + new Random().Next(1, 7)).Play();
        }
    }
    public override void OnTowerUpgraded(Tower tower, string upgradeName, TowerModel newBaseTowerModel)
    {
        if (tower.model.name.Contains("Sally"))
        {
            ModContent.GetAudioClip<SallyPokehellHero>("vo_selected_" + new Random().Next(1, 7)).Play();
        }
    }
    public override void OnTowerCreated(Tower tower, Entity target, Model modelToUse)
    {
        if (tower.model.name.Contains("Sally"))
        {
            ModContent.GetAudioClip<SallyPokehellHero>("vo_selected_" + new Random().Next(1, 2)).Play();
        }
    }
    //public override void OnBloonCreated(Bloon bloon)
    //{
    //    if (Sally is placed)
    //    {
    //      ModContent.GetAudioClip<SallyPokehellHero>("vo_selected_" + new Random().Next(1, 2)).Play();
    //    }
    //}

    ModSettingBool SwellingSpikesBuff = false;


}
public class Sally : ModHero
{
    public override string BaseTower => TowerType.Druid;

    public override int Cost => 600;

    public override string DisplayName => "Sally";
    public override string Title => "Tack Technician";
    public override string Level1Description => "Quickly throws pins at the Bloons. Increases range of all [TackShooters] in her radius by 10%.";
    public override bool Use2DModel => true;
    public override string Description =>
        "Sally throws pins at the Bloons, while supporting other Monkeys, particularly [TackShooters].";


    public override string NameStyle => TowerType.Etienne; // Yellow colored
    public override string BackgroundStyle => TowerType.Etienne; // Yellow colored
    public override string GlowStyle => TowerType.Etienne; // Yellow colored


    public override int MaxLevel => 20;
    public override float XpRatio => 1.2f;

    [System.Obsolete]
    public override int Abilities => 2;

    public override string RogueStarterArtifact => "SallyPokehellHero-TackTechnician1";
    public override IEnumerable<(string, int[])> RogueStarterInstas =>
    [
        (TowerType.TackShooter, new int[] { 0, 0, 0 }),
        (TowerType.SpikeFactory, new int[] { 1, 0, 2 })
    ];

    public override bool IncludeInRogueLegends => true;

    public override void ModifyBaseTowerModel(TowerModel towerModel)
    {
        //towerModel.mods = Game.instance.model.GetTowerFromId("ObynGreenfoot").mods;
        towerModel.GetDescendant<RandomEmissionModel>().count = 3;
        towerModel.GetWeapon().rate *= 0.9f;
        towerModel.GetWeapon().Rate *= 0.9f;
        towerModel.GetWeapon().projectile.display = Game.instance.model.GetTowerFromId(TowerType.TackShooter).Duplicate().GetWeapon().projectile.display;

        var tackBuff = Game.instance.model.GetTowerFromId("MonkeyVillage").GetDescendant<RangeSupportModel>().Duplicate();
        tackBuff.mutatorId = "Sally:Range";
        tackBuff.filters = new Il2CppReferenceArray<TowerFilterModel>(0);
        var fillterModels = tackBuff.filters.ToList();
        fillterModels.Clear();
        fillterModels.Add(new FilterInBaseTowerIdModel("Sally_FilterInBaseTowerIdModel", new Il2CppStringArray(["TackShooter"])));
        tackBuff.filters = fillterModels.ToIl2CppReferenceArray();
        towerModel.AddBehavior(tackBuff);
    }
}
public class SallyLevel2 : ModHeroLevel<Sally>
{
    public override string Description => "Increased popping power. All [TackShooters] in radius get 5% more range and pierce.";
    public override int Level => 2;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetWeapon().projectile.pierce++;

        towerModel.GetDescendant<RangeSupportModel>().multiplier += 0.05f;

        var tackBuff2 = Game.instance.model.GetTowerFromId("Mermonkey-300").GetDescendant<PiercePercentageSupportModel>().Duplicate();
        tackBuff2.percentIncrease = 1.05f;
        tackBuff2.mutatorId = "Sally:CoralSharpening";
        tackBuff2.filters = new Il2CppReferenceArray<TowerFilterModel>(0);
        var fillterModels = tackBuff2.filters.ToList();
        fillterModels.Clear();
        fillterModels.Add(new FilterInBaseTowerIdModel("Sally_FilterInBaseTowerIdModel", new Il2CppStringArray(["TackShooter"])));
        tackBuff2.filters = fillterModels.ToIl2CppReferenceArray();
        towerModel.AddBehavior(tackBuff2);
    }
}
public class SallyLevel3 : ModHeroLevel<Sally>
{
    public override string Description => "[Sizzly Shots]: Throws superhot pins that do +1 damage, have +1 pierce, and can pop any Bloon type. Ability also allows nearby [TackShooters] to hit all Bloon types except Camo.";
    public override int Level => 3;
    //public override string AbilityName => "Spicy Pins";
    //public override string AbilityDescription => "Throws superhot pins that do +1 damage, have +1 pierce, and can pop any Bloon type.";
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        var abilityChurchill = Game.instance.model.GetTowerFromId("CaptainChurchill 3").Duplicate().GetBehavior<AbilityModel>();

        var ability = new AbilityModel("Sally_Ability_Spicy", "Spicy Shots", "Fires heated pins", 0,0, new Il2CppNinjaKiwi.Common.ResourceUtils.SpriteReference(VanillaSprites.HotShotsUpgradeIcon), 45, null, false, false, "SallyLevel3", 0.0f, 0, -1, false, false);
        ability.AddBehavior(new LongArmOfLightModel("LongArmOfLightModel_Sally", 12, 1, new AssetPathModel("AssetPathModel_Spicy",
            new Il2CppNinjaKiwi.Common.ResourceUtils.PrefabReference(Game.instance.model.GetTowerFromId("TackShooter-300").GetWeapon().projectile.Duplicate().display.AssetGUID)), 1, 0, 1, "Sally_Ability_Spicy"));

        ability.AddBehavior(Game.instance.model.GetTowerFromId("Adora 3").GetDescendant<CreateEffectOnAbilityModel>().Duplicate());
        ability.AddBehavior(Game.instance.model.GetTowerFromId("CaptainChurchill 3").GetDescendant<CreateSoundOnAbilityModel>().Duplicate());

        var abilityBrickell = Game.instance.model.GetTowerFromId("AdmiralBrickell 5").GetDescendant<ActivateTowerDamageSupportZoneModel>().Duplicate();
        abilityBrickell.damageIncrease = 1;
        abilityBrickell.lifespan = 12;
        abilityBrickell.lifespanFrames = abilityBrickell.lifespan * 60;
        var fillterModels = abilityBrickell.filters.ToList();
        fillterModels.Clear();
        fillterModels.Add(new FilterInBaseTowerIdModel("Sally_FilterInBaseTowerIdModel", new Il2CppStringArray(["TackShooter"])));
        abilityBrickell.filters = fillterModels.ToIl2CppReferenceArray();
        ability.AddBehavior(abilityBrickell);
        ability.AddBehavior(Game.instance.model.GetTowerFromId("CaptainChurchill 3").GetDescendant<CreateSoundOnAbilityModel>().Duplicate());

        ability.AddBehavior(Game.instance.model.GetTowerFromId("AdmiralBrickell 5").GetDescendant<CreateEffectOnAbilityModel>().Duplicate());

        foreach (var CreateEffectOnAbilityModel in ability.GetDescendants<CreateEffectOnAbilityModel>().ToArray())
        {
            CreateEffectOnAbilityModel.effectModel.lifespan = 12;
        }
        ability.addedViaUpgrade = Id;
        towerModel.AddBehavior(ability);
    }
}
public class SallyLevel4 : ModHeroLevel<Sally>
{
    public override string Description => "Increased attack range. All [TackShooters] in radius get upgrades up to Tier 4 discounted by 10%.";
    public override int Level => 4;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.IncreaseRange(10);

        var tackBuff = Game.instance.model.GetTowerFromId("MonkeyVillage-001").GetDescendant<DiscountZoneModel>().Duplicate();
        tackBuff.discountMultiplier = 0.10f;
        tackBuff.stackName = "DiscountZoneT4";
        tackBuff.groupName = "Sally";
        tackBuff.tierCap = 4;
        tackBuff.towerBaseIds = "TackShooter";
        tackBuff.towerBaseIdList = new Il2CppStringArray(["TackShooter"]);
        towerModel.AddBehavior(tackBuff);
        /*tackBuff.filters = new Il2CppReferenceArray<TowerFilterModel>(0);
        var fillterModels = tackBuff.filters.ToList();
        fillterModels.Clear();
        fillterModels.Add(new FilterInBaseTowerIdModel("Sally_FilterInBaseTowerIdModel", new Il2CppStringArray(["TackShooter"])));
        fillterModels.Add(new FilterInTowerTiersModel("Sally_FilterInTowerTiersModel", 0, 3, 0, 3, 0, 3));
        tackBuff.filters = fillterModels.ToIl2CppReferenceArray();
        towerModel.AddBehavior(tackBuff);*/
    }
}
public class SallyLevel5 : ModHeroLevel<Sally>
{
    public override string Description => ModHelper.HasMod("Eevee") ?
    "[SallyPokehellHero-Sally] can detect Camo Bloons. Also allows all [TackShooters] and [Eevee-Eevee] in radius to attack [ft_camobloonstitle]." :
    "[SallyPokehellHero-Sally] can detect Camo Bloons. Also allows all [TackShooters] in radius to attack [ft_camobloonstitle].";
    public override int Level => 5;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

        var camoBuff = Game.instance.model.GetTowerFromId("MonkeyVillage-020").GetDescendant<VisibilitySupportModel>().Duplicate();
        var fillterModels = camoBuff.filters.ToList();
        fillterModels.Clear();
        fillterModels.Add(new FilterInBaseTowerIdModel("Sally_FilterInBaseTowerIdModel", new Il2CppStringArray(["TackShooter", "Eevee-Eevee"])));
        camoBuff.filters = fillterModels.ToIl2CppReferenceArray();
        towerModel.AddBehavior(camoBuff);
    }
}
public class SallyLevel6 : ModHeroLevel<Sally>
{
    public override string Description => ModHelper.HasMod("Eevee") ?
    "[SallyPokehellHero-Sally]'s pins can pop [ft_frozenbloonstitle]. Also allows all [TackShooters], Druid and [Eevee-Eevee] in radius to pop [ft_frozenbloonstitle]." :
    "[SallyPokehellHero-Sally]'s pins can pop [ft_frozenbloonstitle]. Also allows all [TackShooters] in radius to pop [ft_frozenbloonstitle].";
    public override int Level => 6;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        foreach (var damageModel in towerModel.GetDescendants<DamageModel>().ToArray())
        {
            damageModel.immuneBloonProperties &= ~BloonProperties.Frozen;
        }
    }
}
public class SallyLevel7 : ModHeroLevel<Sally>
{
    public override string Description => "Increased popping power. All towers in range get +1 pierce.";
    public override int Level => 7;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetWeapon().projectile.pierce += 2;
        towerModel.AddBehavior(new PierceSupportModel("PierceSupportModel_Sally", true, 1, "Sally:Pierce", null, false, "BuffIconSharpeningStone", "BuffIconSharpeningStone"));
        towerModel.GetBehavior<PierceSupportModel>().ApplyBuffIcon<SallyLevel7BuffIcon>();
    }
}
public class SallyLevel7BuffIcon : ModBuffIcon
{
    protected override int Order => 1;
    public override string Icon => "Sally-Sharp-Buff";
    public override int MaxStackSize => 1;
}

public class SallyLevel8 : ModHeroLevel<Sally>
{
    public override string Description => "Throws 5 pins at a time. All [TackShooters] in radius get an additional 15% discount on upgrades up to Tier 3";
    public override int Level => 8;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetDescendant<RandomEmissionModel>().count += 2; 
        var tackBuff = Game.instance.model.GetTowerFromId("MonkeyVillage-001").GetDescendant<DiscountZoneModel>().Duplicate();
        tackBuff.discountMultiplier = 0.15f;
        tackBuff.stackName = "DiscountZone3";
        tackBuff.groupName = "Sally";
        tackBuff.tierCap = 3;
        tackBuff.towerBaseIds = "TackShooter";
        tackBuff.towerBaseIdList = new Il2CppStringArray(["TackShooter"]);
        towerModel.AddBehavior(tackBuff);
    }
}
public class SallyLevel9 : ModHeroLevel<Sally>
{
    public override string Description => "Increased attack speed. All [TackShooters] in radius get 15% more range.";
    public override int Level => 9;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetWeapons().ForEach(model => model.rate -= 0.1f);

        var tackBuff = towerModel.GetDescendant<RangeSupportModel>();
        tackBuff.multiplier += 0.15f;
    }
}
public class SallyLevel10 : ModHeroLevel<Sally>
{
    public override string Description => "[Freezy Frost]: Fire an icicle, capable of freezing [ft_moabclassbloonstitle]";
    public override int Level => 10;
    //public override string AbilityName => "Freezy Frost";
    //public override string AbilityDescription => "Fires a high pierce icicle";
    public override void ApplyUpgrade(TowerModel towerModel)
    {

        var ability = new AbilityModel("Sally_Ability_Minty", "Minty Icicle", "Fires a high pierce icicle.", 0, 0, new Il2CppNinjaKiwi.Common.ResourceUtils.SpriteReference(VanillaSprites.IcicleImpaleUpgradeIcon), 30, null, false, false, "SallyLevel10", 0.0f, 0, -1, false, false);
        ability.AddBehavior(new ActivateAttackModel("ActivateAttackModel_Sally", 10, true, new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<AttackModel>(1),true,false,false,false,true,true));
        ability.GetDescendant<ActivateAttackModel>().attacks[0] = Game.instance.model.GetTowerFromId("IceMonkey-105").Duplicate().GetAttackModel();
        ability.GetDescendant<ActivateAttackModel>().attacks[0].weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("DarkPhoenixV1").GetDescendant<DamageModifierForTagModel>());

        ability.GetDescendant<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetDescendant<FreezeModel>().layers = 10;
        ability.GetDescendant<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetDescendant<FreezeModel>().Lifespan = 10;
        ability.GetDescendant<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetDescendant<FreezeModel>().lifespan = 10;
        ability.GetDescendant<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetDescendant<FreezeModel>().damageModel.damage--;

        ability.addedViaUpgrade = Id;
        towerModel.AddBehavior(ability);
    }
}

public class SallyLevel11 : ModHeroLevel<Sally>
{
    public override string Description => "Increased attack range. All towers in radius get 25% more pierce.";
    public override int Level => 11;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.IncreaseRange(10); 
        var tackBuff2 = Game.instance.model.GetTowerFromId("Mermonkey-300").GetDescendant<PiercePercentageSupportModel>().Duplicate();
        tackBuff2.percentIncrease = 1.25f;
        tackBuff2.mutatorId = "Sally:CoralSharpening2";
        tackBuff2.filters = new Il2CppReferenceArray<TowerFilterModel>(0);
        //var fillterModels = tackBuff2.filters.ToList();
        //fillterModels.Clear();
        //fillterModels.Add(new FilterInBaseTowerIdModel("Sally_FilterInBaseTowerIdModel", new Il2CppStringArray(["TackShooter"])));
        //tackBuff2.filters = fillterModels.ToIl2CppReferenceArray();
        towerModel.AddBehavior(tackBuff2);
    }
}

public class SallyLevel12 : ModHeroLevel<Sally>
{
    public override string Description => "[Bloon Bleed]: Every 10th attack causes a slow damage over time effect.";
    public override int Level => 12;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        //towerModel.GetWeapon().projectile.AddBehavior(Game.instance.model.GetTowerFromId("DartlingGunner-200").Duplicate().GetAttackModel().weapons[0].projectile.GetDescendants<AddBehaviorToBloonModel>());
        TowerModel dartling = Game.instance.model.GetTowerFromId(TowerType.Sauda + " 9");

        var alternateProjectileModel = new AlternateProjectileModel("AlternateProjectileModel_Sally", towerModel.GetAttackModel().weapons[0].projectile.Duplicate(), towerModel.GetAttackModel().weapons[0].emission.Duplicate(), 10);

        foreach (var addBehaviorToBloonModel in dartling.GetDescendants<AddBehaviorToBloonModel>().ToArray())
        {
            if (addBehaviorToBloonModel.name.Contains("Bleed") && addBehaviorToBloonModel.name.Contains("Moab")) //Non-MOAB bleed is called "BleedNonMoab"
            {
                //foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                //{
                alternateProjectileModel.projectile.AddBehavior(addBehaviorToBloonModel);
                alternateProjectileModel.projectile.collisionPasses = new[] { 0, 1 };

            }
        }
        towerModel.GetAttackModel().weapons[0].AddBehavior(alternateProjectileModel);
    }
}
public class SallyLevel13 : ModHeroLevel<Sally>
{
    public override string Description => "Throws pins that pop [ft_leadbloonstitle] permanently. [Freezy Frost] affects Lead Bloons. [Sizzly Shots] now gives double attack speed.";
    public override int Level => 13;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetWeapon().projectile.pierce++;
        towerModel.GetWeapon().projectile.GetDamageModel().damage++;
        towerModel.GetWeapon().projectile.display = Game.instance.model.GetTowerFromId("TackShooter-300").GetWeapon().projectile.Duplicate().display;
        towerModel.GetWeapon().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None; //
        //towerModel.GetAbilities().Remove(towerModel.GetAbility());

        var alternateProjectile = towerModel.GetAttackModel().weapons[0].GetBehavior<AlternateProjectileModel>().projectile;
        alternateProjectile.pierce++;
        alternateProjectile.GetDamageModel().damage++;
        alternateProjectile.display = Game.instance.model.GetTowerFromId("TackShooter-300").GetWeapon().projectile.Duplicate().display;
        alternateProjectile.GetDamageModel().immuneBloonProperties = BloonProperties.None; //

        var ability2 = towerModel.GetAbilities()[1];
        ability2.GetDescendant<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetDescendant<FreezeModel>().damageModel.immuneBloonProperties = BloonProperties.White;

        var ability = towerModel.GetAbilities()[0];
        //ability.GetDescendant<ActivateTowerDamageSupportZoneModel>().immuneBloonProperties = BloonProperties.Purple;
        var abilityBrickell = Game.instance.model.GetTowerFromId("AdmiralBrickell 5").GetDescendant<ActivateRateSupportZoneModel>().Duplicate();
        abilityBrickell.lifespan = 12;
        abilityBrickell.lifespanFrames = abilityBrickell.lifespan * 60;
        ability.AddBehavior(abilityBrickell);
    }
}
public class SallyLevel14 : ModHeroLevel<Sally>
{
    public override string Description => "Pins do +4 damage to MOAB-Class Bloons. All [TackShooters] and Magic Monkeys in radius have ALL upgrade costs reduced by 10%.";
    public override int Level => 14;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetWeapon().projectile.AddBehavior<DamageModifierForTagModel>(new DamageModifierForTagModel("DamageModifierForTagModel_Projectile", "Moabs", 1.0f, 4.0f, false, false));
        towerModel.GetAttackModel().weapons[0].GetBehavior<AlternateProjectileModel>().projectile.AddBehavior<DamageModifierForTagModel>(new DamageModifierForTagModel("DamageModifierForTagModel_Projectile", "Moabs", 1.0f, 4.0f, false, false));
        var tackBuff = Game.instance.model.GetTowerFromId("MonkeyVillage-001").GetDescendant<DiscountZoneModel>().Duplicate();
        tackBuff.discountMultiplier = 0.15f;
        tackBuff.stackName = "DiscountZone3";
        tackBuff.groupName = "Sally";
        tackBuff.tierCap = 3;
        tackBuff.towerBaseIds = "TackShooter";
        tackBuff.towerBaseIdList = new Il2CppStringArray(["TackShooter"]);
        foreach (var t in Game.instance.model.towers) // this is dumb
        { 
            if (t.towerSet == Il2CppAssets.Scripts.Models.TowerSets.TowerSet.Magic && tackBuff.towerBaseIdList.Contains(t.baseId)) { tackBuff.towerBaseIdList.AddTo(t.baseId); }
        }
        tackBuff.towerBaseIds = "TackShooter";
        for (var i = 1; i < tackBuff.towerBaseIdList.Length;) // this is dumber
        {
            tackBuff.towerBaseIds += ", " + tackBuff.towerBaseIdList[i];
        }
        towerModel.AddBehavior(tackBuff);
    }
}
public class SallyLevel15 : ModHeroLevel<Sally>
{
    public override string Description => "Ability cooldowns reduced by 15%.";
    public override int Level => 15;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetAbilities().ForEach(model => model.cooldown *= 0.85f);
    }
}
public class SallyLevel16 : ModHeroLevel<Sally>
{
    public override string Description => "[Bloon Bleed] Pro: Every 5th attack causes a slow damage over time effect and pops twice as many layers of Bloon.";
    public override int Level => 16;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetAttackModel().weapons[0].GetBehavior<AlternateProjectileModel>().interval = 5;
        towerModel.GetAttackModel().weapons[0].GetBehavior<AlternateProjectileModel>().projectile.GetDamageModel().damage *= 2;
    }
}

public class SallyLevel17 : ModHeroLevel<Sally>
{
    public override string Description => "Increased popping power. All towers in range get +1 damage";
    public override int Level => 17;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetWeapon().projectile.pierce += 3;

        towerModel.AddBehavior(new DamageSupportModel("DamageSupportModel_Sally", true, 1, "Sally:Damage", null, false, false, 50));
        towerModel.GetBehavior<PierceSupportModel>().ApplyBuffIcon<SallyLevel16BuffIcon>();
        //towerModel.GetBehavior<CollectCashZoneModel>().attractRange += 15;
    }
}
public class SallyLevel18 : ModHeroLevel<Sally>
{
    public override string Description => "Pins pop twice as many layers of Bloon per hit. All [TackShooters] in radius get Tier 5 upgrades discounted by 30% more.";
    public override int Level => 18;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetWeapon().projectile.GetDamageModel().damage *= 2;
        towerModel.GetAttackModel().weapons[0].GetBehavior<AlternateProjectileModel>().projectile.GetDamageModel().damage *= 2; 
        
        var tackBuff = Game.instance.model.GetTowerFromId("MonkeyVillage-001").GetDescendant<DiscountZoneModel>().Duplicate();
        tackBuff.discountMultiplier = 0.30f;
        tackBuff.stackName = "DiscountZoneT5";
        tackBuff.groupName = "Sally";
        tackBuff.upgradeId = "Inferno Ring";
        var tackBuff2 = tackBuff.Duplicate();
        tackBuff2.upgradeId = "Super Maelstrom";
        var tackBuff3 = tackBuff.Duplicate();
        tackBuff3.upgradeId = "The Tack Zone";
        var tackBuff4 = tackBuff.Duplicate();
        tackBuff4.upgradeId = "TeslaCoil-TeslaCoil"; //Im guessing this one 
        var tackBuff5 = tackBuff.Duplicate();
        tackBuff5.upgradeId = "AlternatePaths-ExplosionKing";
        towerModel.AddBehavior(tackBuff);
        towerModel.AddBehavior(tackBuff2);
        towerModel.AddBehavior(tackBuff3);
        towerModel.AddBehavior(tackBuff4);
        towerModel.AddBehavior(tackBuff5);
    }
}
public class SallyLevel16BuffIcon : ModBuffIcon
{
    protected override int Order => 1;
    public override string Icon => "Sally-Sharp-Buff-Plus";
    public override int MaxStackSize => 1;
}
public class SallyLevel19 : ModHeroLevel<Sally>
{
    public override string Description => "Throws 8 pins at a time. Pins do +5 more damage to [ft_moabclassbloonstitle].";
    public override int Level => 19;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetDescendant<RandomEmissionModel>().count += 3;
        towerModel.GetWeapon().projectile.GetBehavior<DamageModifierForTagModel>().damageAddative += 5;
        towerModel.GetAttackModel().weapons[0].GetBehavior<AlternateProjectileModel>().projectile.GetBehavior<DamageModifierForTagModel>().damageAddative += 19;
    }
}

public class SallyLevel20 : ModHeroLevel<Sally>
{
    public override string Description => "All [TackShooters] specific buffs affect the [TackShooter Paragon]. Ability cooldowns reduced by 15%.";
    public override int Level => 20;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetAttackModel().weapons[0].GetBehavior<AlternateProjectileModel>().interval = 4;
        towerModel.GetAbilities().ForEach(model => model.cooldown *= 0.85f);
        var r = towerModel.GetBehavior<RangeSupportModel>().Duplicate();
        r.onlyAffectParagon = true;
        var p = towerModel.GetBehavior<PierceSupportModel>().Duplicate();
        p.onlyAffectParagon = true;
    }
}

public class TackTechnician : ModItemArtifact
{
    protected override float RegistrationPriority => 2.9f; //Need to register before 'ModTower's
    public override string Icon => VanillaSprites.TackShooterIcon;

    public override int MinTier => Common;
    public override int MaxTier => Legendary;

    public override string DescriptionCommon => "Upgrades for Tack Shooters are 20% less expensive, but Primary Monkeys have -20% pierce. Adds a 1-1-0 Tack Shooter to your Party.";
    public override string DescriptionRare => "Upgrades for Tack Shooters and Spike Factories are 25% less expensive, but Primary and Support Monkeys have -15% pierce. Adds a 2-1-0 Tack Shooter to your Party.";
    public override string DescriptionLegendary => "Upgrades for Tack Shooters, Engineer Monkeys, and Spike Factories are 50% less expensive, but ALL Monkeys have -10% pierce. Adds a 2-0-0 Tack Shooter to your Party.";


    /*
    public override string DescriptionCommon => "Upgrading Tack Shooters is 20% less costly. Tack Shooters and Military Monkeys cost 20% more to place. Adds a 1-1-0 Tack Shooter to your Party.";
    public override string DescriptionRare => "Upgrading Tack Shooters and Spike Factories is 30% less costly. Tack Shooters, Spike Factories, and Military Monkeys cost 18% more to place. Adds a 2-1-0 Tack Shooter to your Party.";
    public override string DescriptionLegendary => "Upgrading Tack Shooters, Engineer Monkeys, and Spike Factories is 40% less costly. Tack Shooters, Engineer Monkeys, Spike Factories, and Military Monkeys cost 16% more to place. Adds a 2-0-0 Tack Shooter to your Party.";
    */
    public static float Buff(int tier) => tier switch
    {
        Common => .20f,
        Rare => .25f,
        Legendary => .50f,
        _ => .2f
    };

    public static float Debuff(int tier) => tier switch
    {
        Common => 1 - .20f,
        Rare => 1 - .15f,
        Legendary => 1 - .1f,
        _ => -.2f
    };
    public static Il2CppStringArray BuffTowers(int tier) => tier switch
    {
        Common => [TowerType.TackShooter],
        Rare => [TowerType.TackShooter, TowerType.SpikeFactory],
        Legendary => [TowerType.TackShooter, TowerType.SpikeFactory, TowerType.EngineerMonkey],
        _ => new[] { TowerType.TackShooter }
    };
    public static Il2CppStructArray<TowerSet> DebuffTowerSet(int tier)
    {
        Il2CppStructArray<TowerSet> result;
        switch (tier)
        {
            case Common:
                result = new Il2CppStructArray<TowerSet>(1);
                result[0] = TowerSet.Primary;
                break;
            case Rare:
                result = new Il2CppStructArray<TowerSet>(2);
                result[0] = TowerSet.Primary;
                result[1] = TowerSet.Support;
                break;
            case Legendary:
                result = new Il2CppStructArray<TowerSet>(0);  // explicitly empty
                break;
            default:
                result = new Il2CppStructArray<TowerSet>(1);
                result[0] = TowerSet.Primary;
                break;
        }
        return result;
    }

    public static Il2CppStructArray<int> InstaTiers(int tier)
    {
        Il2CppStructArray<int> result;
        switch (tier)
        {
            case Common:
                result = new Il2CppStructArray<int>(3);
                result[0] = 1;
                result[1] = 1;
                result[2] = 0;
                break;
            case Rare:
                result = new Il2CppStructArray<int>(3);
                result[0] = 2;
                result[1] = 1;
                result[2] = 0;
                break;
            case Legendary:
                result = new Il2CppStructArray<int>(3);
                result[0] = 2;
                result[1] = 0;
                result[2] = 0;
                break;
            default:
                result = new Il2CppStructArray<int>(3);
                result[0] = 0;
                result[1] = 0;
                result[2] = 2;
                break;
        }
        return result;
    }


    public override void ModifyArtifactModel(ItemArtifactModel artifactModel)
    {
        artifactModel.AddBoostBehavior(new DiscountBoostBehaviorModel("", Buff(artifactModel.tier)), boost =>
        {
            boost.towerTypes = BuffTowers(artifactModel.tier);
        });

        artifactModel.AddBoostBehavior(new PierceBoostBehaviorModel("", Debuff(artifactModel.tier)), boost =>
        {
            boost.towerSets = DebuffTowerSet(artifactModel.tier);
        });

        artifactModel.instaTowerToGive = new Il2CppAssets.Scripts.Data.Legends.RogueInstaMonkey
        {
            baseId = "TackShooter",
            tiers = InstaTiers(artifactModel.tier),
            currentCooldown = 0,
            lootType = Il2CppAssets.Scripts.Data.Legends.RogueLootType.permanent,
            uniqueId = 0,
            isFree = false
        };
    }
}
