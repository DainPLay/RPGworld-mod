using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;

namespace RPGworldMod
{

    public class Gruv_rocket_launcher : GunBehaviour
    {
        public static int ID;
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("GRUV Rocket Launcher", "gruv_rocket_launcher");
            Game.Items.Rename("outdated_gun_mods:gruv_rocket_launcher", "rpgworld:gruv_rocket_launcher");
            gun.gameObject.AddComponent<Gruv_rocket_launcher>();
            GunExt.SetShortDescription(gun, "Single Shot");
            GunExt.SetLongDescription(gun, "The once active «GRUV» corporation produced affordable and high-quality variations of weapons. This rocket launcher belongs to this line of products, having such a simple but worthwhile improvement as double the damage. But after the death of the planet Gruv, this antique item is good for nothing.");
            GunExt.SetName(gun, "«GRUV» Rocket Launcher");
            GunExt.SetName(gun, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Ракетница «GRUV»");
            GunExt.SetShortDescription(gun, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Однозарядная");
            GunExt.SetLongDescription(gun, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Некогда активная корпорация «GRUV» выпускала доступные и качественные вариации оружия. Эта ракетница относится к этой линии продукции, имея такое простое, но путное улучшение как вдвое увеличенный урон. Но после смерти планеты Грув, этот антикварынй предмет ни на что не годится.");
            GunExt.SetupSprite(gun, null, "gruv_rocket_launcher_idle_001", 8);
            GunExt.SetAnimationFPS(gun, gun.shootAnimation, 10);
            GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 11);
            GunExt.AddProjectileModuleFrom(gun, PickupObjectDatabase.GetById(39) as Gun, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.ForcedPositionInAmmonomicon = PickupObjectDatabase.GetById(88).ForcedPositionInAmmonomicon;
            gun.reloadTime = 1.2f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.InfiniteAmmo = true;
            gun.quality = PickupObject.ItemQuality.SPECIAL;
            gun.ShouldBeExcludedFromShops = true;
            gun.barrelOffset.transform.localPosition = new Vector3(33f / 16f, 7f / 16f, 0f);
            gun.CanBeDropped = false;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 40f;
            projectile.baseData.speed = 50f;
            projectile.ignoreDamageCaps = true;
            projectile.pierceMinorBreakables = true;
            projectile.transform.parent = gun.barrelOffset;
            gun.gunClass = GunClass.SHITTY;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            Gruv_rocket_launcher.ID = gun.PickupObjectId;
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            AkSoundEngine.PostEvent("Play_WPN_RPG_shot_01", gameObject);
        }
        private bool HasReloaded;
        public override void Update()
        {
            if (this.PlayerOwner != null && !this.PlayerOwner.PlayerHasActiveSynergy("RPGW King Of The Fifth Chamber") && this.PlayerOwner.HasPickupID(Gruv_rocket_launcher.ID))
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
                AkSoundEngine.PostEvent("Play_WPN_rpg_reload_01", base.gameObject);
            }
        }
    }
}