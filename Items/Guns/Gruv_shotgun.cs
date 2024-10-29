using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;

namespace RPGworldMod
{

    public class Gruv_shotgun : GunBehaviour
    {
        public static int ID;
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("GRUV Shotgun", "gruv_shotgun");
            Game.Items.Rename("outdated_gun_mods:gruv_shotgun", "rpgworld:gruv_shotgun");
            gun.gameObject.AddComponent<Gruv_shotgun>();
            GunExt.SetShortDescription(gun, "Single shot");
            GunExt.SetLongDescription(gun, "The once active «GRUV» corporation produced affordable and high-quality variations of weapons. This shotgun belongs to this product line, having a distinctive lethality. But after the death of the planet Gruv, this antique item is good for nothing.");
            GunExt.SetName(gun, "«GRUV» Shotgun");
            GunExt.SetName(gun, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Дробовик «GRUV»");
            GunExt.SetShortDescription(gun, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Однозарядный");
            GunExt.SetLongDescription(gun, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Некогда активная корпорация «GRUV» выпускала доступные и качественные вариации оружия. Этот дробовик относится к этой линии продукции, обладая отличительной убойностью. Но после смерти планеты Грув, этот антикварынй предмет ни на что не годится.");
            GunExt.SetupSprite(gun, null, "gruv_shotgun_idle_001", 8);
            GunExt.SetAnimationFPS(gun, gun.shootAnimation, 10);
            GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 5);
            gun.ForcedPositionInAmmonomicon = PickupObjectDatabase.GetById(88).ForcedPositionInAmmonomicon;
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(51) as Gun).gunSwitchGroup;
            for (int i = 0; i < 12; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(51) as Gun, true, false);
            }

            int iterator = 0;
            foreach (ProjectileModule mod in gun.Volley.projectiles)
            {

                mod.ammoCost = 1;
                mod.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
                mod.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                mod.cooldownTime = 1.2f;
                mod.angleVariance = 14f;
                mod.numberOfShotsInClip = 1;
                Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(mod.projectiles[0]);
                mod.projectiles[0] = projectile;
                projectile.gameObject.SetActive(false);
                FakePrefab.MarkAsFakePrefab(projectile.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(projectile);
                projectile.baseData.range *= 0.5f;
                projectile.baseData.damage = 4f;
                iterator++;
            }

            gun.reloadTime = 1.6f;
            gun.InfiniteAmmo = true;
            gun.quality = PickupObject.ItemQuality.SPECIAL;
            gun.ShouldBeExcludedFromShops = true;
            gun.gunClass = GunClass.SHITTY;
            gun.Volley.UsesShotgunStyleVelocityRandomizer = true;
            gun.CanBeDropped = false;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            Gruv_shotgun.ID = gun.PickupObjectId;
        }

        public override void Update()
        {
            if (this.PlayerOwner != null && !this.PlayerOwner.PlayerHasActiveSynergy("RPGW King Of The First Chamber") && this.PlayerOwner.HasPickupID(Gruv_shotgun.ID))
            {
                gun.m_owner = null;
                this.PlayerOwner.RemoveItemFromInventory(gun);
            }
            base.Update();
        }
    }
}