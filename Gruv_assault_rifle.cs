using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;

namespace RPGworldMod
{

    public class Gruv_assault_rifle : GunBehaviour
    {
        public static int ID;
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("GRUV Assault Rifle", "gruv_assault_rifle");
            Game.Items.Rename("outdated_gun_mods:gruv_assault_rifle", "rpgworld:gruv_assault_rifle");
            gun.gameObject.AddComponent<Gruv_assault_rifle>();
            GunExt.SetShortDescription(gun, "Quick reload");
            GunExt.SetLongDescription(gun, "Reloading is instant if the magazine is empty.\n\nThe once active GRUV corporation produced affordable and high-quality variations of weapons. This assault rifle belongs to this product line, having a distinctive reload speed. But after the death of the planet Gruv, this untique item is good for nothing.");
            GunExt.SetupSprite(gun, null, "gruv_assault_rifle_idle_001", 8);
            GunExt.SetAnimationFPS(gun, gun.shootAnimation, 12);
            GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 11);
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.angleVariance = 3f;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.4f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 30;
            gun.barrelOffset.transform.localPosition = new Vector3(29f / 16f, 8f / 16f, 0f);
            gun.InfiniteAmmo = true;
            gun.quality = PickupObject.ItemQuality.SPECIAL;
            gun.ShouldBeExcludedFromShops = true;
            gun.CanBeDropped = false;
            Projectile projectile = ProjectileUtility.SetupProjectile(86);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 5f;
            projectile.baseData.speed *= 1.2f;
            projectile.baseData.range = 18f;
            projectile.AdditionalScaleMultiplier = 0.8f;
            projectile.gameObject.SetActive(false);
            gun.gunClass = GunClass.SHITTY;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            Gruv_assault_rifle.ID = gun.PickupObjectId;
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            AkSoundEngine.PostEvent("Play_WPN_thompson_shot_01", gameObject);
        }
        private bool HasReloaded;
        public override void Update()
        {
            if (gun.ClipShotsRemaining <= 0) gun.reloadTime = 0f;
            else gun.reloadTime = 1.4f;
            if (this.PlayerOwner != null && !this.PlayerOwner.PlayerHasActiveSynergy("King of the second floor") && this.PlayerOwner.HasPickupID(Gruv_assault_rifle.ID))
            {
                gun.m_owner = null;
                this.PlayerOwner.RemoveItemFromInventory(gun);
            }
            bool flag = this.gun.CurrentOwner;
            if (flag)
            {
                bool flag2 = !this.gun.PreventNormalFireAudio;
                if (flag2)
                {
                    this.gun.PreventNormalFireAudio = true;
                }
                bool flag3 = !this.gun.IsReloading && !this.HasReloaded;
                if (flag3)
                {
                    this.HasReloaded = true;
                }
            }
        }
        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            bool flag = gun.IsReloading && this.HasReloaded;
            if (flag)
            {
                this.HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("SND_WPN_ak47_reload_01", base.gameObject);
            }
        }
    }
}