
using Gungeon;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using Alexandria.TranslationAPI;
using static RPGworldMod.Plugin;
using System.Runtime.CompilerServices;

namespace RPGworldMod
{

    public class Alt_ankh_shield : GunBehaviour
    {
        public static int ID;
        public static Gun gun_fr;
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Alt Ankh shield", "alt_ankh_shield");
            Game.Items.Rename("outdated_gun_mods:alt_ankh_shield", "rpgworld:alt_ankh_shield");
            gun.gameObject.AddComponent<Alt_ankh_shield>();
            GunExt.SetName(gun, "Ankh Shield");
            GunExt.SetShortDescription(gun, "Behind The Wall");
            GunExt.SetLongDescription(gun, "Durable defensive tool. Stops bullets and grants immunity to contact damage, fire, poison and electricity while in use. More effective if you move away from the danger or don't move at all. Stopping bullets wastes durability. Has more durability the more master rounds wielder has.\n\n" +
                "Super strong ancient artifact. And even though it looks like an ordinary obsidian board, it is one of the most durable objects in the Gungeon. The shield was endowed with divine strength and protective properties by the Ankh amulet, located in its very center. There is probably nothing more reliable than holding in hands this piece of equipment under the protection of the ancient gods.");
            GunExt.SetName(gun, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Щит Анх");
            GunExt.SetShortDescription(gun, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "За стеной");
            GunExt.SetLongDescription(gun, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Прочный защитный инструмент. Останавливает пули и дает иммунитет к контактному урону, огню, яду и электричеству во время использования. Наиболее эффективен если отступать или стоять на месте. Блокирование пуль тратит прочность. Имеет тем больше прочности, чем больше у патронов мастера у владельца.\n\nСверхпрочный древний артефакт. И пусть с виду он выглядит как обычная обсидиановая доска, - он один из самых прочных предметов Оружелья. Божественной прочностью и свойствами оберега щит наделил амулет Анх, расположенный в самом его центре. Наверное, нет ничего более надёжного, чем держать этот элемент снаряжения под защитой древних богов.");
            GunExt.SetupSprite(gun, null, "alt_ankh_shield_idle_001", 8);
            GunExt.SetAnimationFPS(gun, gun.idleAnimation, 10);
            GunExt.SetAnimationFPS(gun, gun.shootAnimation, 240);
            GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 11);
            GunExt.SetAnimationFPS(gun, gun.outOfAmmoAnimation, 10);
            GunExt.SetAnimationFPS(gun, gun.emptyAnimation, 10);
            GunExt.SetAnimationFPS(gun, gun.emptyReloadAnimation, 11);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(417) as Gun).gunSwitchGroup;
            gun.doesScreenShake = false;
            GunExt.AddProjectileModuleFrom(gun, PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.ammoCost = 0;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.reloadTime = 2f;
            gun.InfiniteAmmo = true;
            //gun.barrelOffset.transform.localPosition = new Vector3(0f / 16f, 4f / 16f, 0f);
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 13;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.gunClass = GunClass.SILLY;
            gun.gunHandedness = GunHandedness.TwoHanded;
            gun.outOfAmmoAnimation = gun.UpdateAnimation("empty");
            gun.emptyAnimation = gun.UpdateAnimation("empty");
            gun.reloadAnimation = gun.UpdateAnimation("reload");
            gun.emptyReloadAnimation = gun.UpdateAnimation("empty_reload");
            gun.TrimGunSprites();
            gun.ForcedPositionInAmmonomicon = PickupObjectDatabase.GetById(380).ForcedPositionInAmmonomicon;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Alt Ankh Durability",
                "RPGworldMod/Resources/CustomGunAmmoTypes/alt_ankh_durability_clipfull",
                "RPGworldMod/Resources/CustomGunAmmoTypes/ankh_durability_clipempty");
            Projectile projectile = ProjectileUtility.SetupProjectile(86);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.sprite.renderer.enabled = false;
            projectile.baseData.damage = 0f;
            projectile.pierceMinorBreakables = true;
            projectile.damagesWalls = false;
            Shield block = projectile.gameObject.AddComponent<Shield>();
            block.DestroyBaseAfterFirstSlash = true;
            block.DestroyBaseAfterFirstSlash = true;
            block.soundToPlay = null;
            block.InteractMode = SlashDoer.ProjInteractMode.DESTROY;
            block.SlashVFX.type = VFXPoolType.None;
            block.SlashRange = 1f;
            block.SlashDimensions = 180f;
            block.playerKnockback = 0f;
            block.slashKnockback = 0f;
            block.SlashDamageUsesBaseProjectileDamage = true;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            Alt_ankh_shield.ID = gun.PickupObjectId;
            gun.usesContinuousFireAnimation = true;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).loopStart = 1;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.idleAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.idleAnimation).loopStart = 6;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.outOfAmmoAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.outOfAmmoAnimation).loopStart = 2;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.emptyAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.emptyAnimation).loopStart = 2;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).loopStart = 13;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.emptyReloadAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.emptyReloadAnimation).loopStart = 13;
            gun_fr = gun;
        }


        bool startedFiring = false;
        DamageTypeModifier poisonImmunity = new DamageTypeModifier
        {
            damageType = CoreDamageTypes.Poison,
            damageMultiplier = 0f,
        };
        DamageTypeModifier fireImmunity = new DamageTypeModifier
        {
            damageType = CoreDamageTypes.Fire,
            damageMultiplier = 0f,
        };
        DamageTypeModifier electricImmunity = new DamageTypeModifier
        {
            damageType = CoreDamageTypes.Electric,
            damageMultiplier = 0f,
        };
        public static int Damage = 0;
        public static PlayerController Wielder;
        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController playerController = gun.CurrentOwner as PlayerController;
            bool flag = playerController == null;
            if (flag)
            {
                gun.ammo = gun.GetBaseMaxAmmo();
            }
            gun.ClipShotsRemaining = gun.ClipCapacity - Damage;
        }

        public override void OwnedUpdate(GameActor owner, GunInventory inventory)
        {
            //Plugin.Log("Owned!");
            if (Wielder != null && !gun.IsFiring && startedFiring)
            {
                startedFiring = false;
                Wielder.healthHaver.damageTypeModifiers.Remove(poisonImmunity);
                Wielder.healthHaver.damageTypeModifiers.Remove(fireImmunity);
                Wielder.healthHaver.damageTypeModifiers.Remove(electricImmunity);
                PassiveItem.DecrementFlag(Wielder, typeof(LiveAmmoItem));
                PassiveItem.DecrementFlag(Wielder, typeof(Alt_ankh_shield));
            }
            base.OwnedUpdate(owner, inventory);
        }

        public override void OnDropped()
        {
            if (startedFiring)
            {
                startedFiring = false;
                Wielder.healthHaver.damageTypeModifiers.Remove(poisonImmunity);
                Wielder.healthHaver.damageTypeModifiers.Remove(fireImmunity);
                Wielder.healthHaver.damageTypeModifiers.Remove(electricImmunity);
                PassiveItem.DecrementFlag(Wielder, typeof(LiveAmmoItem));
                PassiveItem.DecrementFlag(Wielder, typeof(Alt_ankh_shield));
            }
                base.OnDropped();
        }
        public static int AmountOfMasterRounds(PlayerController player)
        {
            int result = 0;
            for (int j = 0; j < player.passiveItems.Count; j++)
            {
                if (player.passiveItems[j].PickupObjectId >= 467 && player.passiveItems[j].PickupObjectId <= 471)
                {
                    result++;
                }
            }
            return 13 + 12* result;
        }
        public override void OnReloadPressed(PlayerController player, Gun gun, bool manual)
        {
            bool flag = gun.IsReloading && this.HasReloaded;
            if (flag)
            {
                this.HasReloaded = false;
                AkSoundEngine.PostEvent("Play_ENM_bombshee_scream_01", base.gameObject);
            }
            base.OnReloadPressed(player, gun, manual);
        }



        public override void OnReloadEndedPlayer(PlayerController owner, Gun gun)
        {
            Damage = 0;
            playEmptyAnim = false;
            base.OnReloadEndedPlayer(owner, gun);
        }

        private bool HasReloaded;
        public static bool playEmptyAnim = false;
        public override void Update()
        {
            gun_fr = gun;
            if (Wielder != null && !gun.IsReloading && playEmptyAnim && !Wielder.PlayerHasActiveSynergy("RPGW Autumn Mirage"))
            {
                gun.Play(gun.emptyAnimation);
            }
            if (Wielder != null && Wielder.PlayerHasActiveSynergy("RPGW Autumn Mirage")) Damage = 0;
                if (gun.CurrentOwner is PlayerController player)
                {
                    if (Wielder != player) Wielder = player;
                gun.DefaultModule.numberOfShotsInClip = AmountOfMasterRounds(player);
                if (Damage <= gun.ClipCapacity)
                    {
                        if (!gun.IsReloading)
                        {
                            gun.ClipShotsRemaining = gun.ClipCapacity - Damage;
                        }
                    }
                    else if (!gun.IsReloading) gun.ClipShotsRemaining = 0;
                }
            PlayerController playerController = gun.CurrentOwner as PlayerController;
            if (gun.IsFiring)
            {
                if (!startedFiring)
                {
                    startedFiring = true;
                    AkSoundEngine.PostEvent("Play_ITM_Crisis_Stone_Shield_01", gun.gameObject);
                    //gun.gunHandedness = GunHandedness.HiddenOneHanded;
                    if (playerController != null)
                    {
                        playerController.healthHaver.damageTypeModifiers.Add(poisonImmunity);
                        playerController.healthHaver.damageTypeModifiers.Add(fireImmunity);
                        playerController.healthHaver.damageTypeModifiers.Add(electricImmunity);
                        playerController.knockbackDoer.TriggerTemporaryKnockbackInvulnerability(1f);
                        PassiveItem.IncrementFlag(playerController, typeof(LiveAmmoItem));
                        PassiveItem.IncrementFlag(playerController, typeof(Alt_ankh_shield));
                    }
                }
            }
            else if (startedFiring)
            {
                startedFiring = false;
                //gun.gunHandedness = GunHandedness.OneHanded;
                if (playerController != null)
                {
                    playerController.healthHaver.damageTypeModifiers.Remove(poisonImmunity);
                    playerController.healthHaver.damageTypeModifiers.Remove(fireImmunity);
                    playerController.healthHaver.damageTypeModifiers.Remove(electricImmunity);
                    PassiveItem.DecrementFlag(playerController, typeof(LiveAmmoItem));
                    PassiveItem.DecrementFlag(playerController, typeof(Alt_ankh_shield));
                }
                //AkSoundEngine.PostEvent("Play_ITM_Crisis_Stone_Shield_01", gun.gameObject);
            }
            bool flag = gun.CurrentOwner;
            if (flag)
            {
                bool flag2 = !gun.PreventNormalFireAudio;
                if (flag2)
                {
                    gun.PreventNormalFireAudio = true;
                }
                bool flag3 = !gun.IsReloading && !this.HasReloaded;
                if (flag3)
                {
                    this.HasReloaded = true;
                }
            }
        }

        public class Shield : ProjectileSlashingBehaviour
        {
            //public override void SlashHitBullet(Projectile target)
            //{
            //    if (!Wielder.PlayerHasActiveSynergy("RPGW Autumn Mirage"))
            //    {
            //        if (!target.HasDiedInAir) Damage++;
            //        if ((gun_fr.ClipCapacity - Damage) <= 1 && !playEmptyAnim)
            //        {
            //            Damage = gun_fr.ClipCapacity;
            //            DoMacroBlank(Wielder.specRigidbody.UnitCenter);
            //            playEmptyAnim = true;
            //        }
            //    }
            //    target.DieInAir(false, false, false, true);
            //    AkSoundEngine.PostEvent("Play_ITM_Crisis_Stone_Impact_01", target.gameObject);
            //    base.SlashHitBullet(target);
            //}
        }


        public static void DoMacroBlank(Vector2 center)
        {
            GameObject silencerVFX = (GameObject)ResourceCache.Acquire("Global VFX/BlankVFX_Ghost");
            AkSoundEngine.PostEvent("Play_OBJ_silenceblank_small_01", Wielder.gameObject);
            GameObject gameObject = new GameObject("silencer");
            SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
            float additionalTimeAtMaxRadius = 0.25f;
            silencerInstance.TriggerSilencer(center, 25f, 5f, silencerVFX, 0f, 0f, 0f, 0f, 0f, 0f, additionalTimeAtMaxRadius, Wielder, false, false);
        }

    }
}