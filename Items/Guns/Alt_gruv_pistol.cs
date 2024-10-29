using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using System.Collections.Generic;
using Alexandria.Assetbundle;

namespace RPGworldMod
{

    public class Alt_gruv_pistol : GunBehaviour
    {
        public static int ID;
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Alt GRUV Pistol", "alt_gruv_pistol");
            Game.Items.Rename("outdated_gun_mods:alt_gruv_pistol", "rpgworld:alt_gruv_pistol");
            gun.gameObject.AddComponent<Alt_gruv_pistol>();
            GunExt.SetShortDescription(gun, "Shoots In Bursts");
            GunExt.SetLongDescription(gun, "The once active «GRUV» corporation produced affordable and high-quality variations of weapons. This pistol belongs to this product line, with improvements such as a larger magazine and shooting versatility. But after the death of the planet Gruv, this antique item is good for nothing.");
            GunExt.SetName(gun, "«GRUV» Pistol");
            GunExt.SetName(gun, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Пистолет «GRUV»");
            GunExt.SetShortDescription(gun, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Стреляет очередями");
            GunExt.SetLongDescription(gun, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Некогда активная корпорация «GRUV» выпускала доступные и качественные вариации оружия. Этот пистолет относится к этой линии продукции, имея такие улучшения как увеличенный магазин и вариативность стрельбы. Но после смерти планеты Грув, этот антикварынй предмет ни на что не годится.");
            GunExt.SetupSprite(gun, null, "alt_gruv_pistol_idle_001", 8);
            GunExt.SetAnimationFPS(gun, gun.idleAnimation, 9);
            GunExt.SetAnimationFPS(gun, gun.emptyAnimation, 9);
            GunExt.SetAnimationFPS(gun, gun.shootAnimation, 10);
            GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 11);
            GunExt.SetAnimationFPS(gun, gun.emptyReloadAnimation, 11);
            GunExt.AddProjectileModuleFrom(gun, PickupObjectDatabase.GetById(89) as Gun, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.angleVariance = 3f;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.4f;
            gun.DefaultModule.cooldownTime = 0.35f;
            gun.DefaultModule.numberOfShotsInClip = 12;
            gun.DefaultModule.burstShotCount = 3;
            gun.DefaultModule.burstCooldownTime = 0.15f;
            gun.InfiniteAmmo = true;
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.ShouldBeExcludedFromShops = true;
            gun.emptyAnimation = gun.UpdateAnimation("empty");
            gun.emptyReloadAnimation = gun.UpdateAnimation("empty_reload");
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.muzzleFlashEffects = VFXToolbox.CreateVFXPool("Green Shadow Muzzleflash", new List<string>
            {
                "RPGworldMod/Resources/VFX/green_shadow_soul_flare_001",
                "RPGworldMod/Resources/VFX/green_shadow_soul_flare_002",
                "RPGworldMod/Resources/VFX/green_shadow_soul_flare_003"
            }, 10, new IntVector2(14, 7), tk2dBaseSprite.Anchor.MiddleLeft, false, 0f, false, VFXAlignment.Fixed, -1f, null, tk2dSpriteAnimationClip.WrapMode.Once, 0);
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Green Shadow Soul",
                "RPGworldMod/Resources/CustomGunAmmoTypes/green_shadow_soul_clipfull",
                "RPGworldMod/Resources/CustomGunAmmoTypes/green_shadow_soul_clipempty");
            //gun.muzzleFlashEffects = 
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.emptyReloadAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.emptyReloadAnimation).loopStart = 8;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 5f;
            projectile.baseData.speed *= 1f;
            projectile.baseData.range = 14f;
            projectile.hitEffects.overrideMidairDeathVFX = VFXToolbox.CreateVFX("Green Shadow Soul Impact", new List<string>
            {
                "RPGworldMod/Resources/VFX/green_shadow_soul_impact_enemy_001",
                "RPGworldMod/Resources/VFX/green_shadow_soul_impact_enemy_002",
                "RPGworldMod/Resources/VFX/green_shadow_soul_impact_enemy_003",
                "RPGworldMod/Resources/VFX/green_shadow_soul_impact_enemy_004"
            }, 12, new IntVector2(11, 11), tk2dBaseSprite.Anchor.MiddleCenter, false, 0f, -1f, null, tk2dSpriteAnimationClip.WrapMode.Once, false, 0);

            VFXPool vfxpool_impact = VFXToolbox.CreateVFXPool("Rapid Dagger Impact", new List<string>
            {
                "RPGworldMod/Resources/VFX/green_shadow_soul_impact_enemy_001",
                "RPGworldMod/Resources/VFX/green_shadow_soul_impact_enemy_002",
                "RPGworldMod/Resources/VFX/green_shadow_soul_impact_enemy_003",
                "RPGworldMod/Resources/VFX/green_shadow_soul_impact_enemy_004"
            }, 12, new IntVector2(16, 16), tk2dBaseSprite.Anchor.MiddleCenter, false, 0f, false, VFXAlignment.NormalAligned, -1f, null, tk2dSpriteAnimationClip.WrapMode.Once, 0);
            projectile.hitEffects.enemy = vfxpool_impact;
            VFXPool vfxpool_impact_tile = VFXToolbox.CreateVFXPool("Rapid Dagger Impact Tile", new List<string>
            {
                "RPGworldMod/Resources/VFX/green_shadow_soul_impact_horizontal_001",
                "RPGworldMod/Resources/VFX/green_shadow_soul_impact_horizontal_002",
                "RPGworldMod/Resources/VFX/green_shadow_soul_impact_horizontal_003",
                "RPGworldMod/Resources/VFX/green_shadow_soul_impact_horizontal_004",
                "RPGworldMod/Resources/VFX/green_shadow_soul_impact_horizontal_005",
                "RPGworldMod/Resources/VFX/green_shadow_soul_impact_horizontal_006",
                "RPGworldMod/Resources/VFX/green_shadow_soul_impact_horizontal_007",
                "RPGworldMod/Resources/VFX/green_shadow_soul_impact_horizontal_008"
            }, 12, new IntVector2(19, 23), tk2dBaseSprite.Anchor.MiddleLeft, false, 0f, false, VFXAlignment.NormalAligned, -1f, null, tk2dSpriteAnimationClip.WrapMode.Once, 0);
            projectile.hitEffects.tileMapHorizontal = vfxpool_impact_tile;
            projectile.hitEffects.tileMapVertical = vfxpool_impact_tile;
            //GunTools.SetProjectileSpriteRight(projectile, "green_shadow_soul_001", 21, 8, true, tk2dBaseSprite.Anchor.LowerLeft, 21, 8, true, false, null, null, null);

            projectile.AnimateProjectile(new List<string>
            {
                "green_shadow_soul_001",
                "green_shadow_soul_002",
                "green_shadow_soul_003",
                "green_shadow_soul_004",
                "green_shadow_soul_005",
                "green_shadow_soul_006"
            }, 10, true, AnimateBullet.ConstructListOfSameValues<IntVector2>(new IntVector2(21, 8), 6), AnimateBullet.ConstructListOfSameValues<bool>(false, 6), AnimateBullet.ConstructListOfSameValues<tk2dBaseSprite.Anchor>(tk2dBaseSprite.Anchor.MiddleCenter, 6), AnimateBullet.ConstructListOfSameValues<bool>(true, 6), AnimateBullet.ConstructListOfSameValues<bool>(false, 6), AnimateBullet.ConstructListOfSameValues<Vector3?>(null, 6), new List<IntVector2?>
            {
                new IntVector2?(new IntVector2(21, 8)),
                new IntVector2?(new IntVector2(21, 8)),
                new IntVector2?(new IntVector2(21, 8)),
                new IntVector2?(new IntVector2(21, 8)),
                new IntVector2?(new IntVector2(21, 8)),
                new IntVector2?(new IntVector2(21, 8))
            }, AnimateBullet.ConstructListOfSameValues<IntVector2?>(null, 6), AnimateBullet.ConstructListOfSameValues<Projectile>(null, 6));
            gun.gunClass = GunClass.SHITTY;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            Alt_gruv_pistol.ID = gun.PickupObjectId;
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            AkSoundEngine.PostEvent("Play_WPN_vertebraek47_shot_01", gameObject); 
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