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

    public override int Cost => 800;

    public override string DisplayName => "Sally";
    public override string Title => "Big goofball that wants to become a rap star!";
    public override string Level1Description => "Quickly throws pins at the Bloons.";
    public override bool Use2DModel => true;
    public override string Description =>
        "Sally quickly throws pins at the Bloons.";


    public override string NameStyle => TowerType.Etienne; // Yellow colored
    public override string BackgroundStyle => TowerType.Etienne; // Yellow colored
    public override string GlowStyle => TowerType.Etienne; // Yellow colored


    public override int MaxLevel => 10;
    public override float XpRatio => 1.2f;

    [System.Obsolete]
    public override int Abilities => 0;

    public override void ModifyBaseTowerModel(TowerModel towerModel)
    {
        towerModel.mods = Game.instance.model.GetTowerFromId("ObynGreenfoot").mods;
        towerModel.GetDescendant<RandomEmissionModel>().count = 3;
        towerModel.GetWeapon().rate *= 0.6f;
        towerModel.GetWeapon().Rate *= 0.6f;
        towerModel.GetWeapon().projectile.display = Game.instance.model.GetTowerFromId(TowerType.TackShooter).Duplicate().GetWeapon().projectile.display;
    }
}
public class SallyLevel2 : ModHeroLevel<Sally>
{
    public override string Description => "Plus 1 pierce per pin.";
    public override int Level => 2;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetWeapon().projectile.pierce++;
        //towerModel.GetBehavior<CollectCashZoneModel>().attractRange += 15;
    }
}
public class SallyLevel3 : ModHeroLevel<Sally>
{
    public override string Description => "Spicy Pins: Throws superhot pins that do +1 damage, have +1 pierce, and can pop any Bloon type.";
    public override int Level => 3;
    //public override string AbilityName => "Spicy Pins";
    //public override string AbilityDescription => "Throws superhot pins that do +1 damage, have +1 pierce, and can pop any Bloon type.";
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        var abilityChurchill = Game.instance.model.GetTowerFromId("CaptainChurchill 3").Duplicate().GetBehavior<AbilityModel>();

        var ability = new AbilityModel("Sally_Ability_Spicy", "Spicy Shots", "Fires heated pins", 0,0, Game.instance.model.GetTowerFromId("TackShooter").GetUpgrade(TOP, 3).icon, 30, null, false, false, "SallyLevel3", 0.0f, 0, -1, false, false);
        ability.AddBehavior(new LongArmOfLightModel("LongArmOfLightModel_Sally",10,1,new AssetPathModel("AssetPathModel_Spicy", Game.instance.model.GetTowerFromId("TackShooter-300").Duplicate().GetWeapon().projectile.display),1,0,1, "Sally_Ability_Spicy"));
        //ability.AddBehavior(abilityChurchill.GetDescendant<ChangeDamageTypeModel>());
        //ability.GetDescendant<ChangeDamageTypeModel>().lifespanFrames = 900;
        //ability.AddBehavior(Game.instance.model.GetTowerFromId("Gwendolin 6").GetDescendant<HeatItUpDamageBuffModel>().Duplicate());
        //ability.GetDescendant<HeatItUpDamageBuffModel>().lifespan = 10;
        //ability.GetDescendant<HeatItUpDamageBuffModel>().lifespanFrames = 900;
        //ability.GetDescendant<HeatItUpDamageBuffModel>().is;
        //ability.AddBehavior(Game.instance.model.GetTowerFromId("TackShooter-300").GetDescendant<PierceUpTowersModel>());
        //ability.AddBehavior(new MutateProjectileOnAbilityModel("MutateProjectileOnAbilityModel_SpicyAbility", 1800, "", 1, ));
        //ability.AddBehavior(new ChangeProjectileDisplayModel("ChangeProjectileDisplayModel_SpicyAbility", 30, Game.instance.model.GetTowerFromId("TackShooter-300").Duplicate().GetWeapon().projectile.display.Cast<AssetPathModel>(), towerModel.GetWeapon().projectile, "ChangeProjectileDisplayModel_SpicyAbility"));
        //var balls = abilityChurchill.GetBehavior<MutateProjectileOnAbilityModel>().projectileModel;
        ability.AddBehavior(Game.instance.model.GetTowerFromId("Adora 3").GetDescendant<CreateEffectOnAbilityModel>().Duplicate());
        ability.AddBehavior(Game.instance.model.GetTowerFromId("CaptainChurchill 3").GetDescendant<CreateSoundOnAbilityModel>().Duplicate());
        //abilityChurchill.RemoveBehavior<MutateCreateProjectileOnExhaustPierceOnAbilityModel>();
        //abilityChurchill.RemoveBehavior<DamageModifierForTagModel>();
        //balls = GetTowerModel<Sally>().GetWeapon().projectile.Duplicate();
        //balls.pierce++;
        //balls.GetDescendant<DamageModel>().damage++;
        //balls.display = Game.instance.model.GetTowerFromId("TackShooter-300").Duplicate().GetWeapon().projectile.display;
        towerModel.AddBehavior(ability);
    }
}
public class SallyLevel4 : ModHeroLevel<Sally>
{
    public override string Description => "Plus 10 Range.";
    public override int Level => 4;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.IncreaseRange(10);
    }
}
public class SallyLevel5 : ModHeroLevel<Sally>
{
    public override string Description => "Pins can pop Frozen Bloons.";
    public override int Level => 5;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        foreach (var damageModel in towerModel.GetDescendants<DamageModel>().ToArray())
        {
            damageModel.immuneBloonProperties &= ~BloonProperties.Frozen;
        }
    }
}
public class SallyLevel6 : ModHeroLevel<Sally>
{
    public override string Description => "Sally can detect Camo Bloons.";
    public override int Level => 6;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
    }
}
public class SallyLevel7 : ModHeroLevel<Sally>
{
    public override string Description => "Plus 2 pierce per pin. All towers in range get +1 pierce";
    public override int Level => 7;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetWeapon().projectile.pierce += 2;
        towerModel.AddBehavior(new PierceSupportModel("PierceSupportModel_Sally", true, 1, "Sally:Pierce", null, false, "BuffIconSharpeningStone", "BuffIconSharpeningStone"));
        towerModel.GetBehavior<PierceSupportModel>().ApplyBuffIcon<SallyLevel7BuffIcon>();
        //towerModel.AddBehavior(new PierceSupportModel("PierceSupportModel_Sally", true, 1, "Sally:Pierce", new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<TowerFilterModel>(1),);
        //towerModel.GetBehavior<PierceSupportModel>().filters.AddTo<TowerFilterModel>(new fi
        //towerModel.GetBehavior<CollectCashZoneModel>().attractRange += 15;
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
    public override string Description => "Throws 4 pins at a time.";
    public override int Level => 8;
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetDescendant<RandomEmissionModel>().count++;
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
    public override string Description => "Minty Icicle: Fires a high pierce icicle";
    public override int Level => 10;
    //public override string AbilityName => "Minty Icicle";
    //public override string AbilityDescription => "Fires a high pierce icicle";
    public override void ApplyUpgrade(TowerModel towerModel)
    {

        var ability = new AbilityModel("Sally_Ability_Minty", "Minty Icicle", "Fires heated pins", 0, 0, Game.instance.model.GetTowerFromId(TowerType.IceMonkey).GetUpgrade(BOTTOM, 5).icon, 30, null, false, false, "SallyLevel10", 0.0f, 0, -1, false, false);
        ability.AddBehavior(new ActivateAttackModel("ActivateAttackModel_Sally", 10, true, new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.AttackModel>(1),true,false,false,false,true));
        ability.GetDescendant<ActivateAttackModel>().attacks[0] = Game.instance.model.GetTowerFromId("IceMonkey-205").GetAttackModel();
        ability.GetDescendant<ActivateAttackModel>().attacks[0].weapons[0].projectile.pierce = 100;

        towerModel.AddBehavior(ability);
    }
}