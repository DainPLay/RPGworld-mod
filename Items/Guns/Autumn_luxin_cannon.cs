using Alexandria.ItemAPI;
using Dungeonator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static tk2dSpriteCollectionDefinition;

namespace RPGworldMod
{
    class Autumn_luxin_cannon : GunBehaviour
    {


        public bool synergyActive = false;

        public static string old_idle_animation;
        public static string old_shoot_animation;
        public static string old_empty_animation;
        public static string old_final_shoot_animation;
        public static VFXPool old_empty_reload_effects;
        public static VFXPool old_final_muzzle_flash_effects;
        public static VFXPool old_muzzle_flash_effects;
        public static void Add() //SpecialAPI is cool
        {
            Gun gun = PickupObjectDatabase.GetById(199) as Gun;
            gun.gameObject.AddComponent<Autumn_luxin_cannon>();
            old_idle_animation = gun.idleAnimation;
            old_shoot_animation = gun.shootAnimation;
            old_empty_animation = gun.emptyAnimation;
            old_final_shoot_animation = gun.finalShootAnimation;
            old_empty_reload_effects = gun.emptyReloadEffects;
            old_final_muzzle_flash_effects = gun.finalMuzzleFlashEffects;
            old_muzzle_flash_effects = gun.muzzleFlashEffects;
        }
        public static string lastAnim;

        public override void Update()
        {
            if (!synergyActive && PlayerOwner != null && PlayerOwner.PlayerHasActiveSynergy("RPGW Autumn Mirage"))
            {
                gun.idleAnimation = gun.UpdateAnimation("autumn_idle");
                gun.shootAnimation = gun.UpdateAnimation("autumn_shoot");
                gun.finalShootAnimation = gun.UpdateAnimation("autumn_final_shoot");
                gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.finalShootAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
                gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.finalShootAnimation).loopStart = 3;
                gun.emptyReloadEffects = old_empty_reload_effects;
                gun.finalMuzzleFlashEffects = old_final_muzzle_flash_effects;
                gun.muzzleFlashEffects = old_muzzle_flash_effects;

                synergyActive = true;
            }

            else if (synergyActive && (PlayerOwner == null || !PlayerOwner.PlayerHasActiveSynergy("RPGW Autumn Mirage")))
            {
                gun.idleAnimation = old_idle_animation;
                gun.shootAnimation = old_shoot_animation;
                gun.finalShootAnimation = old_final_shoot_animation;
                gun.emptyReloadEffects = old_empty_reload_effects;
                gun.finalMuzzleFlashEffects = old_final_muzzle_flash_effects;
                gun.muzzleFlashEffects = old_muzzle_flash_effects;

                synergyActive = false;
            }
        }
    }
}
