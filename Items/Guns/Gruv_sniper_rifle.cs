﻿using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;

namespace RPGworldMod
{

    public class Gruv_sniper_rifle : GunBehaviour
    {
        public static int ID;
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("GRUV Sniper Rifle", "gruv_sniper_rifle");
            Game.Items.Rename("outdated_gun_mods:gruv_sniper_rifle", "rpgworld:gruv_sniper_rifle");
            gun.gameObject.AddComponent<Gruv_sniper_rifle>();
            GunExt.SetShortDescription(gun, "Shoots in bursts");
            GunExt.SetLongDescription(gun, "The once active «GRUV» corporation produced affordable and high-quality variations of weapons. This sniper rifle belongs to this product line, with improvements such as a larger magazine and shooting versatility. But after the death of the planet Gruv, this antique item is good for nothing.");
            GunExt.SetName(gun, "«GRUV» Sniper Rifle");
            GunExt.SetName(gun, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Снайперская винтовка «GRUV»");
            GunExt.SetShortDescription(gun, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Стреляет очередями");
            GunExt.SetLongDescription(gun, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Некогда активная корпорация «GRUV» выпускала доступные и качественные вариации оружия. Эта снайперская винтовка относится к этой линии продукции, имея такие улучшения как увеличенный магазин и вариативность стрельбы. Но после смерти планеты Грув, этот антикварынй предмет ни на что не годится.");
            GunExt.SetupSprite(gun, null, "gruv_sniper_rifle_idle_001", 8);
            GunExt.SetAnimationFPS(gun, gun.shootAnimation, 10);
            GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 11);
            gun.ForcedPositionInAmmonomicon = PickupObjectDatabase.GetById(88).ForcedPositionInAmmonomicon;
            GunExt.AddProjectileModuleFrom(gun, PickupObjectDatabase.GetById(49) as Gun, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.angleVariance = 1f;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 2f;
            gun.DefaultModule.cooldownTime = 1f;
            gun.DefaultModule.numberOfShotsInClip = 40;
            gun.barrelOffset.transform.localPosition = new Vector3(34f / 16f, 6f / 16f, 0f);
            gun.DefaultModule.burstShotCount = 5;
            gun.DefaultModule.burstCooldownTime = 0.15f;
            gun.InfiniteAmmo = true;
            gun.quality = PickupObject.ItemQuality.SPECIAL;
            gun.ShouldBeExcludedFromShops = true;
            gun.CanBeDropped = false;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 9f;
            gun.gunClass = GunClass.SHITTY;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            Gruv_sniper_rifle.ID = gun.PickupObjectId;
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            AkSoundEngine.PostEvent("Play_WPN_sniperrifle_shot_01", gameObject);
        }
        public override void Update()
        {
            if (this.PlayerOwner != null && !this.PlayerOwner.PlayerHasActiveSynergy("RPGW King Of The Third Chamber") && this.PlayerOwner.HasPickupID(Gruv_sniper_rifle.ID))
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