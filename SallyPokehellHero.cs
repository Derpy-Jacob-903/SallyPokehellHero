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
using Il2CppNinjaKiwi.Common.ResourceUtils;
using Il2CppAssets.Scripts.Unity.Towers.Filters;

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
}
public class Sally : ModHero
{
    public override string BaseTower => TowerType.Druid;

    public override int Cost => 600;

    public override string DisplayName => "Sally";
    public override string Title => "Eevee";
    public override string Level1Description => "Quickly throws pins at the Bloons. All Tack Shooters in radius get more range.";
    public override bool Use2DModel => true;
    public override string Description =>
        "Sally quickly throws pins at the Bloons, while supporting other Monkeys, particularly Tack Shooters.";


    public override string NameStyle => TowerType.Etienne; // Yellow colored
    public override string BackgroundStyle => TowerType.Etienne; // Yellow colored
    public override string GlowStyle => TowerType.Etienne; // Yellow colored


    public override int MaxLevel => 20;
    public override float XpRatio => 1.2f;

    [System.Obsolete]
    public override int Abilities => 2;

    public override void ModifyBaseTowerModel(TowerModel towerModel)
    {
        towerModel.mods = Game.instance.model.GetTowerFromId("ObynGreenfoot").mods;
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
    public override string Description => "Increased popping power.";
    public override int Level => 2;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetWeapon().projectile.pierce++;
        //towerModel.GetBehavior<CollectCashZoneModel>().attractRange += 15;
    }
}
public class SallyLevel3 : ModHeroLevel<Sally>
{
    public override string Description => "Sizzly Shots: Throws superhot pins that do +1 damage, have +1 pierce, and can pop any Bloon type. Ability also allows nearby Tack Shooters to hit all Bloon types except Camo.";
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
            CreateEffectOnAbilityModel.effectModel.scale *= 0.75f;
        }
        ability.addedViaUpgrade = Id;
        towerModel.AddBehavior(ability);
    }
}
public class SallyLevel4 : ModHeroLevel<Sally>
{
    public override string Description => "Increased attack range.";
    public override int Level => 4;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.IncreaseRange(10);
    }
}
public class SallyLevel5 : ModHeroLevel<Sally>
{
    public override string Description => "Sally can detect Camo Bloons. Also allows all Tack Shooters and Eevee in radius to attack Camo Bloons.";
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
    public override string Description => "Sally's pins can pop Frozen Bloons. Tack Shooters in radius can pop frozen bloons.";
    public override int Level => 6;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        foreach (var damageModel in towerModel.GetDescendants<DamageModel>().ToArray())
        {
            damageModel.immuneBloonProperties &= ~BloonProperties.Frozen;
        }
        var camoBuff = Game.instance.model.GetTowerFromId("MonkeyVillage-030").GetDescendant<DamageTypeSupportModel>().Duplicate();
        //if () camoBuff.filters.Clear();
        var dummyBuff = camoBuff.Duplicate();
        camoBuff.name = "DamageTypeSupportModel_Sally4Tack";
        camoBuff.immuneBloonProperties = (BloonProperties)(1 + 2 + 4 + 8); ;
        //camoBuff.filters.AddTo(new FilterInTowerTiersModel("Sally_FilterInTowerTiersModel", 0,2,0,5,0,5));
        camoBuff.filters.AddTo(new FilterInBaseTowerIdModel("Sally_FilterInBaseTowerIdModel", new Il2CppStringArray(["TackShooter"])));
        camoBuff.ApplyBuffIcon<SallyLevel6BuffIcon>();
        camoBuff.onlyShowBuffIfMutated = true;
        towerModel.AddBehavior(camoBuff);

        //var dummyBuff = Game.instance.model.GetTowerFromId("MonkeyVillage-020").GetDescendant<VisibilitySupportModel>().Duplicate();
    }
}

public class SallyLevel6BuffIcon : ModBuffIcon
{
    protected override int Order => 1;
    public override SpriteReference IconReference => new SpriteReference(VanillaSprites.HardTacksIcon);
    public override int MaxStackSize => 1;
}

public class SallyLevel6DummyBuffIcon : ModBuffIcon
{
    protected override int Order => 1;
    public override string Icon => "4XX-DummyMIB";
    public override int MaxStackSize => 1;
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
    public override string Description => "Throws 5 pins at a time. Eevee in radius can pop frozen bloons.";
    public override int Level => 8;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetDescendant<RandomEmissionModel>().count += 2;

        var camoBuff = Game.instance.model.GetTowerFromId("MonkeyVillage-030").GetDescendant<DamageTypeSupportModel>().Duplicate();
        camoBuff.immuneBloonProperties = (BloonProperties)(1 + 2 + 4 + 8);
        camoBuff.name = "DamageTypeSupportModel_Sally4Eevee";
        //camoBuff.filters.AddTo(new FilterInTowerTiersModel("Sally_FilterInTowerTiersModel", 0, 2, 0, 2, 0, 2));
        camoBuff.filters.AddTo(new FilterInBaseTowerIdModel("Sally_FilterInBaseTowerIdModel", new Il2CppStringArray(["Eevee-Eevee"])));
        camoBuff.ApplyBuffIcon<SallyLevel6BuffIcon>();
        camoBuff.onlyShowBuffIfMutated = true;
        //camoBuff.filters.Clear();
        towerModel.AddBehavior(camoBuff);
    }
}
public class SallyLevel9 : ModHeroLevel<Sally>
{
    public override string Description => "Increased attack speed.";
    public override int Level => 9;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetWeapons().ForEach(model => model.rate -= 0.1f);
    }
}
public class SallyLevel10 : ModHeroLevel<Sally>
{
    public override string Description => "Freezy Frost: Fire an icicle, capable of freezing MOAB-class Bloons";
    public override int Level => 10;
    //public override string AbilityName => "Freezy Frost";
    //public override string AbilityDescription => "Fires a high pierce icicle";
    public override void ApplyUpgrade(TowerModel towerModel)
    {

        var ability = new AbilityModel("Sally_Ability_Minty", "Minty Icicle", "Fires a high pierce icicle.", 0, 0, new Il2CppNinjaKiwi.Common.ResourceUtils.SpriteReference(VanillaSprites.IcicleImpaleUpgradeIcon), 30, null, false, false, "SallyLevel10", 0.0f, 0, -1, false, false);
        ability.AddBehavior(new ActivateAttackModel("ActivateAttackModel_Sally", 10, true, new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<AttackModel>(1),true,false,false,false,true));
        ability.GetDescendant<ActivateAttackModel>().attacks[0] = Game.instance.model.GetTowerFromId("IceMonkey-105").Duplicate().GetAttackModel();
        ability.GetDescendant<ActivateAttackModel>().attacks[0].weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("DarkPhoenixV1").GetDescendant<DamageModifierForTagModel>());

        ability.GetDescendant<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetDescendant<FreezeModel>().layers = 10;
        ability.GetDescendant<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetDescendant<FreezeModel>().Lifespan = 10;
        ability.GetDescendant<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetDescendant<FreezeModel>().lifespan = 10;
        ability.GetDescendant<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetDescendant<FreezeModel>().damageModel.damage = 10;

        ability.addedViaUpgrade = Id;
        towerModel.AddBehavior(ability);
    }
}

public class SallyLevel11 : ModHeroLevel<Sally>
{
    public override string Description => "Increased attack range.";
    public override int Level => 11;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.IncreaseRange(10);
    }
}

public class SallyLevel12 : ModHeroLevel<Sally>
{
    public override string Description => "Bloon Bleed: Every 10th shot causes a slow damage over time effect.";
    public override int Level => 12;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        //towerModel.GetWeapon().projectile.AddBehavior(Game.instance.model.GetTowerFromId("DartlingGunner-200").Duplicate().GetAttackModel().weapons[0].projectile.GetDescendants<AddBehaviorToBloonModel>());
        TowerModel dartling = Game.instance.model.GetTowerFromId(TowerType.Sauda + " 9");

        var alternateProjectileModel = new AlternateProjectileModel("", towerModel.GetAttackModel().weapons[0].projectile.Duplicate(), towerModel.GetAttackModel().weapons[0].emission.Duplicate(), 10);

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
    public override string Description => "Throws superhot pins permanently. Freezy Frost affects Lead Bloons. Sizzly Shots now gives double attack speed.";
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
    public override string Description => "Pins do +4 damage to MOAB-Class Bloons.";
    public override int Level => 14;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetWeapon().projectile.AddBehavior<DamageModifierForTagModel>(new DamageModifierForTagModel("DamageModifierForTagModel_Projectile", "Moabs", 1.0f, 4.0f, false, false));
        towerModel.GetAttackModel().weapons[0].GetBehavior<AlternateProjectileModel>().projectile.AddBehavior<DamageModifierForTagModel>(new DamageModifierForTagModel("DamageModifierForTagModel_Projectile", "Moabs", 1.0f, 4.0f, false, false));

    }
}
public class SallyLevel15 : ModHeroLevel<Sally>
{
    public override string Description => "Ability cooldowns reduced by 15%. Tack Shooters in radius can pop Lead Bloons.";
    public override int Level => 15;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetAbilities().ForEach(model => model.cooldown *= 0.85f);

        foreach (var buff in towerModel.GetDescendants<DamageTypeSupportModel>().ToArray())
        {
            if (buff.name.EndsWith("4Eevee"))
            {
                towerModel.RemoveBehavior(buff);
            }
        }

        var camoBuff = Game.instance.model.GetTowerFromId("MonkeyVillage-030").GetDescendant<DamageTypeSupportModel>().Duplicate();
        
        camoBuff.onlyShowBuffIfMutated = true;

        camoBuff.name = "DamageTypeSupportModel_Sally4Tack";
        camoBuff.immuneBloonProperties = (BloonProperties)(2 + 4 + 8);
        //camoBuff.filters.AddTo(new FilterInTowerTiersModel("Sally_FilterInTowerTiersModel", 0, 2, 0, 5, 0, 5));
        //camoBuff.filters.Clear();
        camoBuff.filters.AddTo(new FilterInBaseTowerIdModel("Sally_FilterInBaseTowerIdModel", new Il2CppStringArray(["TackShooter"])));
    }
}
public class SallyLevel16 : ModHeroLevel<Sally>
{
    public override string Description => "Bloon Bleed+: Every 5th attack causes a slow damage over time effect.";
    public override int Level => 16;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetAttackModel().weapons[0].GetBehavior<AlternateProjectileModel>().interval = 5;
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
    public override string Description => "Pins pop twice as many layers of Bloon per hit.";
    public override int Level => 18;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetWeapon().projectile.GetDamageModel().damage *= 2;
        towerModel.GetAttackModel().weapons[0].GetBehavior<AlternateProjectileModel>().projectile.GetDamageModel().damage *= 2;
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
    public override string Description => "Throws 8 pins at a time. Tack Shooters in radius can pop any Bloon Type.";
    public override int Level => 19;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetDescendant<RandomEmissionModel>().count += 3;

        foreach (var buff in towerModel.GetDescendants<DamageTypeSupportModel>().ToArray())
        {
            if (buff.name.EndsWith("4Eevee"))
            {
                towerModel.RemoveBehavior(buff);
            }
        }
        var camoBuff = Game.instance.model.GetTowerFromId("MonkeyVillage-030").GetDescendant<DamageTypeSupportModel>().Duplicate();
        camoBuff.name = "DamageTypeSupportModel_Sally4Tack";
        //camoBuff.filters.Clear();
        camoBuff.filters.AddTo(new FilterInBaseTowerIdModel("Sally_FilterInBaseTowerIdModel", new Il2CppStringArray(["TackShooter"])));
        camoBuff.onlyShowBuffIfMutated = true;
        towerModel.AddBehavior(camoBuff);
    }
}

public class SallyLevel20 : ModHeroLevel<Sally>
{
    public override string Description => "Bloon Bleed++: Every 4th attack causes a slow damage over time effect and pop twice as many layers of Bloon. Ability cooldowns reduced by 15%. All pins do +5 more damage to MOAB-Class Bloons.";
    public override int Level => 20;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetAttackModel().weapons[0].GetBehavior<AlternateProjectileModel>().projectile.GetDamageModel().damage *= 2;
        towerModel.GetAttackModel().weapons[0].GetBehavior<AlternateProjectileModel>().interval = 4;

        towerModel.GetAbilities().ForEach(model => model.cooldown *= 0.85f);

        towerModel.GetWeapon().projectile.GetBehavior<DamageModifierForTagModel>().damageAddative += 5;
        towerModel.GetAttackModel().weapons[0].GetBehavior<AlternateProjectileModel>().projectile.GetBehavior<DamageModifierForTagModel>().damageAddative += 14;
    }
}
