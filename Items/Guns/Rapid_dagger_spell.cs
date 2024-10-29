using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using System.Reflection;
using System.Collections.Generic;
using Alexandria.Assetbundle;
using static Alexandria.ItemAPI.TrailAPI;
using Alexandria.BreakableAPI;
using Dungeonator;
using UnityEngine.Networking;

namespace RPGworldMod
{

    public class Rapid_dagger_spell : AdvancedGunBehavior
    {
        public static int ID;
        //public static string OwnerlessAnimation;
        public static string DefaultAnimation;
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Rapid Dagger Spell", "rapid_dagger_spell");
            Game.Items.Rename("outdated_gun_mods:rapid_dagger_spell", "rpgworld:rapid_dagger_spell");
            gun.gameObject.AddComponent<Rapid_dagger_spell>();
            GunExt.SetShortDescription(gun, "Green Lightning");
            GunExt.SetLongDescription(gun, "Does not reveal secret walls. Creates and accelerates a dagger. If shot in the back, oneshots normal enemies and deals double damage to bosses. Regenerates mana over time.\n\nNow you can insert daggers into backs remotely and without having physical daggers!");
            GunExt.SetName(gun, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Заклинание скоростного кинжала");
            GunExt.SetShortDescription(gun, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Зелёная молния");
            GunExt.SetLongDescription(gun, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Не показывает скрытые стены. Создаёт и ускоряет кинжал. Попадание в спину моментально убивает обычных противников и наносит двойной урон боссам. Восстанавливает ману со временем.\n\nТеперь вставлять кинжалы в спины можно дистанционно, и не имея физических кинжалов!");
            GunExt.SetupSprite(gun, null, "rapid_dagger_spell_scroll_idle_001", 8);
            GunExt.SetAnimationFPS(gun, gun.shootAnimation, 10);
            GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 10);
            GunExt.AddProjectileModuleFrom(gun, PickupObjectDatabase.GetById(89) as Gun, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.DefaultModule.angleVariance = 0f;
            gun.reloadTime = 1.4f;
            gun.DefaultModule.cooldownTime = 1.2f;
            gun.DefaultModule.numberOfShotsInClip = 9;
            gun.SetBaseMaxAmmo(10);
            gun.quality = PickupObject.ItemQuality.C;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.barrelOffset.transform.localPosition = new Vector3(19f / 16f, 16f / 16f, 0f);
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 20f;
            projectile.baseData.speed = 120f;
            projectile.baseData.range = 1000f;
            projectile.baseData.force = 10f;
            projectile.pierceMinorBreakables = true;
            projectile.damagesWalls = false;
            projectile.transform.parent = gun.barrelOffset;
            projectile.hitEffects.overrideMidairDeathVFX = VFXToolbox.CreateVFX("Rapid Dagger Impact", new List<string>
            {
                "RPGworldMod/Resources/VFX/rapid_dagger_impact_001",
                "RPGworldMod/Resources/VFX/rapid_dagger_impact_002",
                "RPGworldMod/Resources/VFX/rapid_dagger_impact_003",
                "RPGworldMod/Resources/VFX/rapid_dagger_impact_004"
            }, 12, new IntVector2(16, 16), tk2dBaseSprite.Anchor.MiddleCenter, false, 0f, -1f, null, tk2dSpriteAnimationClip.WrapMode.Once, false, 0);
            List<string> list = new List<string>
            {
                "RPGworldMod/Resources/VFX/rapid_dagger_trail_001",
                "RPGworldMod/Resources/VFX/rapid_dagger_trail_002",
                "RPGworldMod/Resources/VFX/rapid_dagger_trail_003"
            };
            TrailAPI.AddTrailToProjectile(projectile, "RPGworldMod/Resources/VFX/rapid_dagger_trail_001", new Vector2(6f, 4f), new Vector2(0f, 1f), list, 20, list, 20, -1f, 0.0001f, 0.1f, false, false);

            //projectile.hitEffects.alwaysUseMidair = true;
            //projectile.gameObject.GetOrAddComponent<EmmisiveTrail>();
            VFXPool vfxpool_impact = VFXToolbox.CreateVFXPool("Rapid Dagger Impact", new List<string>
            {
                "RPGworldMod/Resources/VFX/rapid_dagger_impact_001",
                "RPGworldMod/Resources/VFX/rapid_dagger_impact_002",
                "RPGworldMod/Resources/VFX/rapid_dagger_impact_003",
                "RPGworldMod/Resources/VFX/rapid_dagger_impact_004"
            }, 12, new IntVector2(16, 16), tk2dBaseSprite.Anchor.MiddleCenter, false, 0f, false, VFXAlignment.NormalAligned, -1f, null, tk2dSpriteAnimationClip.WrapMode.Once, 0);
            projectile.hitEffects.enemy = vfxpool_impact;
            VFXPool vfxpool_impact_tile = VFXToolbox.CreateVFXPool("Rapid Dagger Impact Tile", new List<string>
            {
                "RPGworldMod/Resources/VFX/rapid_dagger_impact_horizontal_001",
                "RPGworldMod/Resources/VFX/rapid_dagger_impact_horizontal_002",
                "RPGworldMod/Resources/VFX/rapid_dagger_impact_horizontal_003",
                "RPGworldMod/Resources/VFX/rapid_dagger_impact_horizontal_004",
                "RPGworldMod/Resources/VFX/rapid_dagger_impact_horizontal_005",
                "RPGworldMod/Resources/VFX/rapid_dagger_impact_horizontal_006",
                "RPGworldMod/Resources/VFX/rapid_dagger_impact_horizontal_007",
                "RPGworldMod/Resources/VFX/rapid_dagger_impact_horizontal_008",
                "RPGworldMod/Resources/VFX/rapid_dagger_impact_horizontal_009"
            }, 12, new IntVector2(15, 10), tk2dBaseSprite.Anchor.MiddleLeft, false, 0f, false, VFXAlignment.NormalAligned, -1f, null, tk2dSpriteAnimationClip.WrapMode.Once, 0);
            projectile.hitEffects.tileMapHorizontal = vfxpool_impact_tile;
            projectile.hitEffects.tileMapVertical = vfxpool_impact_tile;
            GunTools.SetProjectileSpriteRight(projectile, "rapid_dagger_spell_projectile", 16, 6, true, tk2dBaseSprite.Anchor.MiddleCenter, 16, 6, true, false, null, null, null);



            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Spell Mana",
                "RPGworldMod/Resources/CustomGunAmmoTypes/mana_clipfull",
                "RPGworldMod/Resources/CustomGunAmmoTypes/mana_clipempty");
            gun.gunClass = GunClass.SILLY;
            gun.clipObject = BreakableAPIToolbox.GenerateDebrisObject("RPGworldMod/Resources/Debris/empty_mana_bottle.png", true, 1f, 5f, 60f, 20f, null, 1f, null, null, 1, false, null, 1f).gameObject;
            gun.clipsToLaunchOnReload = 1;
            gun.reloadClipLaunchFrame = 6;
            gun.ForcedPositionInAmmonomicon = PickupObjectDatabase.GetById(748).ForcedPositionInAmmonomicon;
            gun.alternateIdleAnimation = gun.UpdateAnimation("scroll_idle");
            gun.outOfAmmoAnimation = gun.UpdateAnimation("noammo");
            gun.emptyAnimation = "rapid_dagger_spell_empty";
            //OwnerlessAnimation = gun.alternateIdleAnimation;
            DefaultAnimation = gun.idleAnimation;
            //GunExt.SetAnimationFPS(gun, OwnerlessAnimation, 0);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(DefaultAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(DefaultAnimation).loopStart = 11;
            gun.TrimGunSprites();
            gun.PreventNormalFireAudio = true;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            Rapid_dagger_spell.ID = gun.PickupObjectId;
        }

        public float cooldown = 2.5f;
        public GameObject critVFX = VFXToolbox.CreateVFX("Rapid Dagger Critical Impact", new List<string>
            {
                "RPGworldMod/Resources/VFX/rapid_dagger_critical_impact_001",
                "RPGworldMod/Resources/VFX/rapid_dagger_critical_impact_002",
                "RPGworldMod/Resources/VFX/rapid_dagger_critical_impact_003",
                "RPGworldMod/Resources/VFX/rapid_dagger_critical_impact_004"
            }, 12, new IntVector2(24, 24), tk2dBaseSprite.Anchor.MiddleCenter, false, 0f, -1f, null, tk2dSpriteAnimationClip.WrapMode.Once, false, 0);

        public override void PostProcessProjectile(Projectile projectile)
        {
            projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HitEnemy));
            base.PostProcessProjectile(projectile);
        }
        private void HitEnemy(Projectile projectile, SpeculativeRigidbody enemy, bool killed)
        {
            Vector2 v = Vector2.Lerp(projectile.specRigidbody.UnitCenter, enemy.UnitCenter, 0.5f);
            PlayerController playerController = this.gun.CurrentOwner as PlayerController;
            BounceProjModifier orAddComponent = projectile.gameObject.GetOrAddComponent<BounceProjModifier>();
            //Plugin.Log("Proj: " + VectorToDegrees(projectile.Direction) + " Enemy: " + enemy.aiActor.FacingDirection + " Raznitsa: " + (VectorToDegrees(projectile.Direction) - enemy.aiActor.FacingDirection));
            if (Math.Abs(VectorToDegrees(projectile.Direction) - enemy.aiActor.FacingDirection) <= 120 && enemy.aiActor.IsNormalEnemy && enemy.aiActor.healthHaver)
            {
                AkSoundEngine.PostEvent("Play_WPN_Vorpal_Shot_Critical_01", enemy.gameObject);
                AkSoundEngine.PostEvent("Play_WPN_Vorpal_Shot_Critical_01", enemy.gameObject);

                SpawnManager.SpawnVFX(critVFX, v, Quaternion.identity);
                if (!enemy.aiActor.healthHaver.IsBoss)
                enemy.aiActor.healthHaver.ApplyDamage(100000f, Vector2.zero, "Backstab", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
                else
                    enemy.aiActor.healthHaver.ApplyDamage(projectile.ModifiedDamage, Vector2.zero, "Backstab", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
            }
        }

        public static float VectorToDegrees(Vector2 v)
        {
            return Mathf.Atan2(v.y, v.x) * 57.29578f;
        }
        public static IntVector2 PassiveOffset = new IntVector2(0, 0);
        public static IntVector2 ActiveOffset = new IntVector2(-14, -10);

        protected override void Update()
        {
            if (cooldown > 0f)
                cooldown -= BraveTime.DeltaTime;
            if (cooldown <= 0f /*&& gun.ammo > 0*/ && gun.ammo < gun.AdjustedMaxAmmo)
            {
                gun.ammo++;
                if (gun.m_owner is PlayerController player && (player.CurrentRoom == null || player.CurrentRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.All) == 0)) cooldown = 1f;
                else cooldown = 2.5f;
            }

            if ((gun.IsReloading && gun.m_owner != null && (gun.m_owner.FacingDirection < 25f || gun.m_owner.FacingDirection > 155f)) || (gun.spriteAnimator.IsPlaying(gun.idleAnimation) && gun.spriteAnimator != null && gun.spriteAnimator.CurrentFrame > 2 && gun.spriteAnimator.CurrentFrame < 11)) gun.sprite.HeightOffGround = 0.875f;
            else gun.sprite.HeightOffGround = -0.075f;

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

            if (gun == null)
                return;

            var targetAnim = DefaultAnimation;
            if (gun.CurrentOwner == null)
            {
                gun.emptyAnimation = gun.outOfAmmoAnimation;
            }
            else gun.emptyAnimation = "rapid_dagger_spell_empty";

            if (gun.CurrentOwner == null || gun.CurrentOwner.CurrentGun != gun)
            {
                targetAnim = gun.outOfAmmoAnimation;
            }


            if (gun.idleAnimation == targetAnim)
                return;

            gun.idleAnimation = targetAnim;
            if (gun.IsEmpty) gun.Play(gun.idleAnimation);
            else gun.PlayIdleAnimation();// Thanks to SpecialAPI for making it work
        }
        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            bool flag = gun.IsReloading && this.HasReloaded;
            if (flag)
            {
                this.HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_WPN_Bubbler_Drink_01", base.gameObject);
            }
        }
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            AkSoundEngine.PostEvent("Play_WPN_blasphemy_shot_01", gameObject);
        }
        private bool HasReloaded;

    }
}