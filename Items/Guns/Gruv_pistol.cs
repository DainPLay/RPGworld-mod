using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;

namespace RPGworldMod
{

    public class Gruv_pistol : GunBehaviour
    {
        public static int ID;
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("GRUV Pistol", "gruv_pistol");
            Game.Items.Rename("outdated_gun_mods:gruv_pistol", "rpgworld:gruv_pistol");
            gun.gameObject.AddComponent<Gruv_pistol>();
            GunExt.SetShortDescription(gun, "Shoots In Bursts");
            GunExt.SetLongDescription(gun, "The once active «GRUV» corporation produced affordable and high-quality variations of weapons. This pistol belongs to this product line, with improvements such as a larger magazine and shooting versatility. But after the death of the planet Gruv, this antique item is good for nothing.");
            GunExt.SetName(gun, "«GRUV» Pistol");
            GunExt.SetName(gun, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Пистолет «GRUV»");
            GunExt.SetShortDescription(gun, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Стреляет очередями");
            GunExt.SetLongDescription(gun, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Некогда активная корпорация «GRUV» выпускала доступные и качественные вариации оружия. Этот пистолет относится к этой линии продукции, имея такие улучшения как увеличенный магазин и вариативность стрельбы. Но после смерти планеты Грув, этот антикварынй предмет ни на что не годится.");
            GunExt.SetupSprite(gun, null, "gruv_pistol_idle_001", 8);
            GunExt.SetAnimationFPS(gun, gun.shootAnimation, 10);
            GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 11);
            GunExt.AddProjectileModuleFrom(gun, PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.angleVariance = 3f;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.4f;
            gun.ForcedPositionInAmmonomicon = PickupObjectDatabase.GetById(88).ForcedPositionInAmmonomicon;
            gun.DefaultModule.cooldownTime = 0.35f;
            gun.DefaultModule.numberOfShotsInClip = 12;
            gun.DefaultModule.burstShotCount = 3;
            gun.DefaultModule.burstCooldownTime = 0.15f;
            gun.InfiniteAmmo = true;
            gun.quality = PickupObject.ItemQuality.SPECIAL;
            gun.ShouldBeExcludedFromShops = true;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 5f;
            projectile.baseData.speed *= 1f;
            projectile.baseData.range = 14f;
            gun.gunClass = GunClass.SHITTY;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            Gruv_pistol.ID = gun.PickupObjectId;
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", gameObject);
        }
        private bool HasReloaded;
        public override void Update()
        {
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
                AkSoundEngine.PostEvent("Play_WPN_SAA_reload_01", base.gameObject);
            }
        }
    }
}