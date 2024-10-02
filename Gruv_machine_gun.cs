using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using UnityEngine.Networking;
using Dungeonator;

namespace RPGworldMod
{

    public class Gruv_machine_gun : GunBehaviour
    {
        public static int ID;
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("GRUV Machine gun", "gruv_machine_gun");
            Game.Items.Rename("outdated_gun_mods:gruv_machine_gun", "rpgworld:gruv_machine_gun");
            gun.gameObject.AddComponent<Gruv_machine_gun>();
            GunExt.SetShortDescription(gun, "Infinite");
            GunExt.SetLongDescription(gun, "Accelerates over time if there are enemies in the room. Resets after dodgerolling.\n\nThe once active GRUV corporation produced affordable and high-quality variations of weapons. This machine gun belongs to this product line, having infinite amounts of ammunition. But after the death of the planet Gruv, this untique item is good for nothing.");
            GunExt.SetupSprite(gun, null, "gruv_machine_gun_idle_001", 8);
            GunExt.SetAnimationFPS(gun, gun.shootAnimation, 10);
            gun.SetAnimationFPS(gun.reloadAnimation, 0);
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(84) as Gun, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.angleVariance = 6f;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = -1f;
            gun.DefaultModule.cooldownTime = 0.4f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.barrelOffset.transform.localPosition = new Vector3(33f / 16f, 6f / 16f, 0f);
            gun.InfiniteAmmo = true;
            gun.quality = PickupObject.ItemQuality.SPECIAL;
            gun.ShouldBeExcludedFromShops = true;
            gun.CanBeDropped = false;
            Projectile projectile = ProjectileUtility.SetupProjectile(86);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 5f;
            projectile.baseData.speed *= 1f;
            projectile.baseData.range = 18f;
            projectile.AdditionalScaleMultiplier = 0.8f;
            projectile.ignoreDamageCaps = true;
            projectile.gameObject.SetActive(false);
            gun.gunClass = GunClass.SHITTY;
            gun.usesContinuousFireAnimation = true;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).loopStart = 1;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            Gruv_machine_gun.ID = gun.PickupObjectId;
        }


        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController playerController = this.gun.CurrentOwner as PlayerController;
            bool flag = playerController == null;
            if (flag)
            {
                this.gun.ammo = this.gun.GetBaseMaxAmmo();
            }
            gun.ClipShotsRemaining = 2;
            gun.GainAmmo(2);
        }
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            AkSoundEngine.PostEvent("Play_WPN_minigun_shot_01", gameObject);
            gun.ClearReloadData();
        }
        public override void Update()
        {
            PlayerController playerController = this.gun.CurrentOwner as PlayerController;
            if (this.gun.IsFiring && !playerController.IsDodgeRolling && (playerController.CurrentRoom == null || playerController.CurrentRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.All) != 0))
            {
                if (gun.DefaultModule.cooldownTime > 0.015f) gun.DefaultModule.cooldownTime -= 0.002f;
                if (gun.DefaultModule.cooldownTime == 0.4f) GunExt.SetAnimationFPS(gun, gun.shootAnimation, 10);
                if (gun.DefaultModule.cooldownTime <= 0.365f) GunExt.SetAnimationFPS(gun, gun.shootAnimation, 11);
                if (gun.DefaultModule.cooldownTime <= 0.33f) GunExt.SetAnimationFPS(gun, gun.shootAnimation, 12);
                if (gun.DefaultModule.cooldownTime <= 0.295f) GunExt.SetAnimationFPS(gun, gun.shootAnimation, 13);
                if (gun.DefaultModule.cooldownTime <= 0.26f) GunExt.SetAnimationFPS(gun, gun.shootAnimation, 14);
                if (gun.DefaultModule.cooldownTime <= 0.225f) GunExt.SetAnimationFPS(gun, gun.shootAnimation, 15);
                if (gun.DefaultModule.cooldownTime <= 0.19f) GunExt.SetAnimationFPS(gun, gun.shootAnimation, 16);
                if (gun.DefaultModule.cooldownTime <= 0.155f) GunExt.SetAnimationFPS(gun, gun.shootAnimation, 17);
                if (gun.DefaultModule.cooldownTime <= 0.12f) GunExt.SetAnimationFPS(gun, gun.shootAnimation, 18);
                if (gun.DefaultModule.cooldownTime <= 0.085f) GunExt.SetAnimationFPS(gun, gun.shootAnimation, 19);
                if (gun.DefaultModule.cooldownTime <= 0.05f) GunExt.SetAnimationFPS(gun, gun.shootAnimation, 20);
                if (gun.DefaultModule.cooldownTime <= 0.015f) GunExt.SetAnimationFPS(gun, gun.shootAnimation, 21);
            }
            else gun.DefaultModule.cooldownTime = 0.4f;
            if (gun.DefaultModule.cooldownTime == 0.4f) GunExt.SetAnimationFPS(gun, gun.shootAnimation, 10);
            if (this.PlayerOwner != null && !this.PlayerOwner.PlayerHasActiveSynergy("King of the fourth floor") && this.PlayerOwner.HasPickupID(Gruv_machine_gun.ID))
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
            }
        }
    }
}