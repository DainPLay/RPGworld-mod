
using BepInEx;
using System;
using UnityEngine;
using SGUI;
using HarmonyLib;
using System.Collections;
using MonoMod.Cil;
using System.Reflection;
using Mono.Cecil.Cil;
using HutongGames.PlayMaker.Actions;
using Reaktion;
using System.ComponentModel;
using System.Collections.Generic;

namespace RPGworldMod
{
    [BepInDependency(Alexandria.Alexandria.GUID)] // this mod depends on the Alexandria API: https://enter-the-gungeon.thunderstore.io/package/Alexandria/Alexandria/
    [BepInDependency(ETGModMainBehaviour.GUID)]
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "dainplay.etg.rpgworld";
        public const string NAME = "RPGworld mod";
        public const string VERSION = "1.1.2";
        public const string TEXT_COLOR = "#00FFAA";

        public void Start()
        {
            ConsoleCommands();
            ETGModMainBehaviour.WaitForGameManagerStart(GMStart);
        }

        public void Awake()
        {
            new Harmony(GUID).PatchAll();
        }

        [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.ClearBlinkShadow))]
        public class PlayerController_ClearBlinkShadow
        {
            [HarmonyPrefix]
            public static bool Prefix(PlayerController __instance)
            {
                if (__instance.PlayerHasActiveSynergy("RPGW Green Scarf")) {
                    for (int j = 0; j < __instance.activeItems.Count; j++)
                    {
                        if (__instance.activeItems[j].PickupObjectId == Gungeon.Game.Items.GetSafe("rpgworld:an_item").PickupObjectId && !__instance.activeItems[j].IsOnCooldown)
                        {
                            __instance.activeItems[j].ApplyCooldown(__instance);
                            __instance.healthHaver.TriggerInvulnerabilityPeriod(1f);
                            __instance.knockbackDoer.TriggerTemporaryKnockbackInvulnerability(1f);
                            __instance.CurrentRoom.ApplyActionToNearbyEnemies(__instance.sprite.WorldCenter, 2f, new Action<AIActor, float>(PlayerController_ClearBlinkShadow.ProcessEnemy));
                        }
                    }
                }
                return true;
            }



            public static void ProcessEnemy(AIActor a, float distance)
            {
                if (a.IsNormalEnemy && a.healthHaver && !a.healthHaver.IsBoss)
                {
                    a.healthHaver.ApplyDamage(100000f, Vector2.zero, "Telefrag", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
                }
            }
        }

        //[HarmonyPatch(typeof(GameUIAmmoController), nameof(GameUIAmmoController.GetOffsetVectorForGun))]
        //public class GameUIAmmoController_GetOffsetVectorForGun
        //{
        //    [HarmonyPrefix]
        //    public static bool Prefix(GameUIAmmoController __instance, ref Vector3 __result, Gun newGun, ref bool isFlippingGun)
        //    {
        //        if (newGun.PickupObjectId == Rapid_dagger_spell.ID)
        //        {
        //            tk2dSpriteDefinition tk2dSpriteDefinition;
        //            if (__instance.m_cachedGunSpriteDefinition != null && !isFlippingGun)
        //            {
        //                tk2dSpriteDefinition = __instance.m_cachedGunSpriteDefinition;
        //            }
        //            else
        //            {
        //                tk2dSpriteDefinition = newGun.GetSprite().Collection.spriteDefinitions[newGun.DefaultSpriteID];
        //            }
        //            Vector3 vector = Vector3.Scale(-tk2dSpriteDefinition.GetBounds().min + -tk2dSpriteDefinition.GetBounds().extents, __instance.gunSprites[0].scale);
        //            if (isFlippingGun)
        //            {
        //                vector += new Vector3(__instance.m_currentGunSpriteXOffset, 0f, __instance.m_currentGunSpriteZOffset);
        //            }
        //            vector += new Vector3(-1f / 16f, 1f / 16f, 0f);
        //            __result = vector;
        //            return false;
        //        }
        //        return true;
        //    }
        //}

        [HarmonyPatch(typeof(ScarfAttachmentDoer), nameof(ScarfAttachmentDoer.LateUpdate))]
        public class ScarfAttachmentDoer_LateUpdate
        {
            [HarmonyPrefix]
            public static bool Prefix(ScarfAttachmentDoer __instance)
            {
                Color Green = new Color(0f, 0.596f, 0.149f);
                Color Original = new Color(0.6029412f, 0.1374351f, 0.1374351f);
                if (__instance.AttachTarget is PlayerController player)
                {
                    if (player.PlayerHasActiveSynergy("RPGW Green Scarf") && __instance.m_mr.material.GetColor("_OverrideColor") != Green)
                    {
                        __instance.m_mr.material.SetColor("_OverrideColor", Green);
                    }
                    else if (!player.PlayerHasActiveSynergy("RPGW Green Scarf") && __instance.m_mr.material.GetColor("_OverrideColor") != Original)
                    {
                        __instance.m_mr.material.SetColor("_OverrideColor", Original);
                    }
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.HandleDeath_CR))]
        public class PlayerController_HandleDeath_CR
        {
            [HarmonyPostfix]
            public static IEnumerator Postfix(IEnumerator orig, PlayerController __instance)
            {
                if (__instance.HasGun(Ankh_shield.ID) || __instance.HasGun(Alt_ankh_shield.ID))
                {
                    bool hasRevivingSynergy = __instance.PlayerHasActiveSynergy("RPGW Destructive Ankh Candy") && __instance.HasPassiveItem(110);
                    bool wasPitFalling = __instance.IsFalling;
                    Pixelator.Instance.DoFinalNonFadedLayer = true;
                    if (__instance.CurrentGun)
                    {
                        __instance.CurrentGun.CeaseAttack(false, null);
                    }
                    __instance.CurrentInputState = PlayerInputState.NoInput;
                    GameManager.Instance.MainCameraController.SetManualControl(true, false);
                    __instance.ToggleGunRenderers(false, "death");
                    __instance.ToggleHandRenderers(false, "death");
                    __instance.ToggleAttachedRenderers(false);
                    Transform cameraTransform = GameManager.Instance.MainCameraController.transform;
                    Vector3 cameraStartPosition = cameraTransform.position;
                    Vector3 cameraEndPosition = __instance.CenterPosition;
                    GameManager.Instance.MainCameraController.OverridePosition = cameraStartPosition;
                    if (__instance.CurrentGun)
                    {
                        __instance.CurrentGun.DespawnVFX();
                    }
                    __instance.HandleDeathPhotography();
                    yield return null;
                    __instance.ToggleHandRenderers(false, "death");
                    if (__instance.CurrentGun)
                    {
                        __instance.CurrentGun.DespawnVFX();
                    }
                    __instance.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unfaded"));
                    GameUIRoot.Instance.ForceClearReload(__instance.PlayerIDX);
                    GameUIRoot.Instance.notificationController.ForceHide();
                    float elapsed = 0f;
                    float duration = 0.8f;
                    tk2dBaseSprite spotlightSprite = ((GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("DeathShadow", ".prefab"), __instance.specRigidbody.UnitCenter, Quaternion.identity)).GetComponent<tk2dBaseSprite>();
                    spotlightSprite.spriteAnimator.ignoreTimeScale = true;
                    spotlightSprite.spriteAnimator.Play();
                    tk2dSpriteAnimator whooshAnimator = spotlightSprite.transform.GetChild(0).GetComponent<tk2dSpriteAnimator>();
                    whooshAnimator.ignoreTimeScale = true;
                    whooshAnimator.Play();
                    Pixelator.Instance.CustomFade(0.6f, 0f, Color.white, Color.black, 0.1f, 0.5f);
                    Pixelator.Instance.LerpToLetterbox(0.35f, 0.8f);
                    BraveInput.AllowPausedRumble = true;
                    __instance.DoVibration(Vibration.Time.Normal, Vibration.Strength.Hard);
                    CompanionItem pigItem = null;
                    tk2dSpriteAnimator pigVFX = null;
                    bool isDoingPigSave = false;
                    string pigMoveAnim = "pig_move_right";
                    string pigSaveAnim = "pig_jump_right";
                    for (int i = 0; i < __instance.passiveItems.Count; i++)
                    {
                        if (__instance.passiveItems[i] is CompanionItem)
                        {
                            CompanionItem companionItem = __instance.passiveItems[i] as CompanionItem;
                            CompanionController companionController = (!companionItem || !companionItem.ExtantCompanion) ? null : companionItem.ExtantCompanion.GetComponent<CompanionController>();
                            if (companionController && companionController.name.StartsWith("Pig"))
                            {
                                pigVFX = (UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/VFX_HeroPig")) as GameObject).GetComponent<tk2dSpriteAnimator>();
                                pigItem = companionItem;
                                isDoingPigSave = true;
                            }
                            else if (companionItem.DisplayName == "Pig")
                            {
                                pigVFX = (UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/VFX_HeroPig")) as GameObject).GetComponent<tk2dSpriteAnimator>();
                                pigItem = companionItem;
                                isDoingPigSave = true;
                            }
                            if (companionItem.ExtantCompanion && companionItem.ExtantCompanion.GetComponent<SackKnightController>())
                            {
                                SackKnightController component = companionItem.ExtantCompanion.GetComponent<SackKnightController>();
                                if (component.CurrentForm == SackKnightController.SackKnightPhase.HOLY_KNIGHT)
                                {
                                    pigItem = companionItem;
                                    pigVFX = (UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/VFX_HeroJunk")) as GameObject).GetComponent<tk2dSpriteAnimator>();
                                    isDoingPigSave = true;
                                    pigMoveAnim = "junk_shspcg_move_right";
                                    pigSaveAnim = "junk_shspcg_sacrifice_right";
                                }
                            }
                        }
                    }
                    if (!isDoingPigSave && __instance.OverrideAnimationLibrary != null)
                    {
                        __instance.OverrideAnimationLibrary = null;
                        __instance.ResetOverrideAnimationLibrary();
                        GameObject effect = __instance.BlankVFXPrefab = (GameObject)BraveResources.Load("Global VFX/VFX_BulletArmor_Death", ".prefab");
                        __instance.PlayEffectOnActor(effect, Vector3.zero, true, false, false);
                    }
                    while (elapsed < duration)
                    {
                        if (GameManager.INVARIANT_DELTA_TIME == 0f)
                        {
                            elapsed += 0.05f;
                        }
                        elapsed += GameManager.INVARIANT_DELTA_TIME;
                        float t = elapsed / duration;
                        GameManager.Instance.MainCameraController.OverridePosition = Vector3.Lerp(cameraStartPosition, cameraEndPosition, t);
                        __instance.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                        spotlightSprite.color = new Color(1f, 1f, 1f, t);
                        Pixelator.Instance.saturation = Mathf.Clamp01(1f - t);
                        yield return null;
                    }
                    spotlightSprite.color = Color.white;
                    yield return __instance.StartCoroutine(__instance.InvariantWait(0.4f));
                    Transform clockhairTransform = ((GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Clockhair", ".prefab"))).transform;
                    ClockhairController clockhair = clockhairTransform.GetComponent<ClockhairController>();
                    elapsed = 0f;
                    duration = clockhair.ClockhairInDuration;
                    Vector3 clockhairTargetPosition = __instance.CenterPosition;
                    Vector3 clockhairStartPosition = clockhairTargetPosition + new Vector3(-20f, 5f, 0f);
                    clockhair.renderer.enabled = false;
                    clockhair.spriteAnimator.Play("clockhair_intro");
                    clockhair.hourAnimator.Play("hour_hand_intro");
                    clockhair.minuteAnimator.Play("minute_hand_intro");
                    clockhair.secondAnimator.Play("second_hand_intro");
                    if (!isDoingPigSave && (GameManager.Instance.CurrentGameType == GameManager.GameType.SINGLE_PLAYER || GameManager.Instance.GetOtherPlayer(__instance).IsGhost) && __instance.OnRealPlayerDeath != null)
                    {
                        __instance.OnRealPlayerDeath(__instance);
                    }
                    bool hasWobbled = false;
                    while (elapsed < duration)
                    {
                        if (GameManager.INVARIANT_DELTA_TIME == 0f)
                        {
                            elapsed += 0.05f;
                        }
                        elapsed += GameManager.INVARIANT_DELTA_TIME;
                        float t2 = elapsed / duration;
                        float smoothT = Mathf.SmoothStep(0f, 1f, t2);
                        Vector3 currentPosition = Vector3.Slerp(clockhairStartPosition, clockhairTargetPosition, smoothT);
                        clockhairTransform.position = currentPosition.WithZ(0f);
                        if (t2 > 0.5f)
                        {
                            clockhair.renderer.enabled = true;
                            clockhair.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                        }
                        if (t2 > 0.75f)
                        {
                            clockhair.hourAnimator.GetComponent<Renderer>().enabled = true;
                            clockhair.minuteAnimator.GetComponent<Renderer>().enabled = true;
                            clockhair.secondAnimator.GetComponent<Renderer>().enabled = true;
                            clockhair.hourAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                            clockhair.minuteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                            clockhair.secondAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                        }
                        if (!hasWobbled && clockhair.spriteAnimator.CurrentFrame == clockhair.spriteAnimator.CurrentClip.frames.Length - 1)
                        {
                            clockhair.spriteAnimator.Play("clockhair_wobble");
                            hasWobbled = true;
                        }
                        clockhair.sprite.UpdateZDepth();
                        __instance.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                        yield return null;
                    }
                    if (!hasWobbled)
                    {
                        clockhair.spriteAnimator.Play("clockhair_wobble");
                    }
                    clockhair.SpinToSessionStart(clockhair.ClockhairSpinDuration);
                    elapsed = 0f;
                    duration = clockhair.ClockhairSpinDuration + clockhair.ClockhairPauseBeforeShot;
                    while (elapsed < duration)
                    {
                        if (GameManager.INVARIANT_DELTA_TIME == 0f)
                        {
                            elapsed += 0.05f;
                        }
                        elapsed += GameManager.INVARIANT_DELTA_TIME;
                        clockhair.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                        yield return null;
                    }
                    if (isDoingPigSave)
                    {
                        elapsed = 0f;
                        duration = 2f;
                        Vector2 targetPosition = clockhairTargetPosition;
                        Vector2 startPosition = targetPosition + new Vector2(-18f, 0f);
                        Vector2 pigOffset = pigVFX.sprite.WorldCenter - pigVFX.transform.position.XY();
                        pigVFX.Play(pigMoveAnim);
                        while (elapsed < duration)
                        {
                            Vector2 lerpPosition = Vector2.Lerp(startPosition, targetPosition, elapsed / duration);
                            pigVFX.transform.position = (lerpPosition - pigOffset).ToVector3ZisY(0f);
                            pigVFX.sprite.UpdateZDepth();
                            if (duration - elapsed < 0.1f && !pigVFX.IsPlaying(pigSaveAnim))
                            {
                                pigVFX.Play(pigSaveAnim);
                            }
                            pigVFX.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                            if (GameManager.INVARIANT_DELTA_TIME == 0f)
                            {
                                elapsed += 0.05f;
                            }
                            elapsed += GameManager.INVARIANT_DELTA_TIME;
                            yield return null;
                        }
                    }
                    else
                    if (__instance.HasGun(Alt_ankh_shield.ID)) {
                        elapsed = 0f;
                        duration = 0.1f;
                        Gun gun_shield = null;
                        for (int i = 0; i < __instance.inventory.AllGuns.Count; i++)
                        {
                            if (__instance.inventory.AllGuns[i].PickupObjectId == Alt_ankh_shield.ID)
                            {
                                gun_shield = __instance.inventory.AllGuns[i];
                            }
                        }

                        Alt_ankh_shield.Damage = 0;
                        Alt_ankh_shield.playEmptyAnim = false;
                        Vector2 targetPosition = clockhairTargetPosition;
                        __instance.ToggleGunRenderers(true, "");
                        gun_shield.sprite.HeightOffGround = 0.875f;
                        __instance.ForceIdleFacePoint(Vector2.left, true);
                        __instance.spriteAnimator.Play((!__instance.UseArmorlessAnim) ? "item_get" : "item_get_armorless");
                        clockhair.spriteAnimator.Play("clockhair_fire");
                        AkSoundEngine.PostEvent("Play_ITM_Crisis_Stone_Impact_01", __instance.gameObject);
                        while (elapsed < duration)
                        {
                            __instance.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                            clockhair.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                            if (GameManager.INVARIANT_DELTA_TIME == 0f)
                            {
                                elapsed += 0.05f;
                            }
                            elapsed += GameManager.INVARIANT_DELTA_TIME;
                            yield return null;
                        }
                        if (!hasRevivingSynergy)
                        {
                            elapsed = 0f;
                            duration = 2f;
                            clockhair.spriteAnimator.Play("clockhair_wobble");
                            while (elapsed < duration)
                            {
                                __instance.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                                clockhair.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                                if (GameManager.INVARIANT_DELTA_TIME == 0f)
                                {
                                    elapsed += 0.05f;
                                }
                                elapsed += GameManager.INVARIANT_DELTA_TIME;
                                yield return null;
                            }

                            elapsed = 0f;
                            duration = 0.1f;
                            AkSoundEngine.PostEvent("Play_OBJ_silenceblank_small_01", __instance.gameObject);
                            __instance.ToggleGunRenderers(false, "death");
                            __instance.spriteAnimator.Play((!__instance.UseArmorlessAnim) ? "death" : "death_armorless");
                            clockhair.spriteAnimator.Play("clockhair_fire");
                            AkSoundEngine.PostEvent("Play_ITM_Crisis_Stone_Impact_01", __instance.gameObject);
                            while (elapsed < duration)
                            {
                                __instance.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                                clockhair.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                                if (GameManager.INVARIANT_DELTA_TIME == 0f)
                                {
                                    elapsed += 0.05f;
                                }
                                elapsed += GameManager.INVARIANT_DELTA_TIME;
                                yield return null;
                            }

                            elapsed = 0f;
                            duration = 2f;
                            clockhair.spriteAnimator.Play("clockhair_wobble");
                            while (elapsed < duration)
                            {
                                __instance.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                                clockhair.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                                if (GameManager.INVARIANT_DELTA_TIME == 0f)
                                {
                                    elapsed += 0.05f;
                                }
                                elapsed += GameManager.INVARIANT_DELTA_TIME;
                                yield return null;
                            }
                        }
                    }
                    else
                    if (__instance.HasGun(Ankh_shield.ID))
                    {
                        elapsed = 0f;
                        duration = 0.1f;
                        Gun gun_shield = null;
                        for (int i = 0; i < __instance.inventory.AllGuns.Count; i++)
                        {
                            if (__instance.inventory.AllGuns[i].PickupObjectId == Ankh_shield.ID)
                            {
                                gun_shield = __instance.inventory.AllGuns[i];
                            }
                        }

                        Ankh_shield.Damage = 0;
                        Ankh_shield.playEmptyAnim = false;
                        Vector2 targetPosition = clockhairTargetPosition;
                        __instance.ToggleGunRenderers(true, "");
                        gun_shield.sprite.HeightOffGround = 0.875f;
                        __instance.ForceIdleFacePoint(Vector2.left, true);
                        __instance.spriteAnimator.Play((!__instance.UseArmorlessAnim) ? "item_get" : "item_get_armorless");
                        clockhair.spriteAnimator.Play("clockhair_fire");
                        AkSoundEngine.PostEvent("Play_ITM_Crisis_Stone_Impact_01", __instance.gameObject);
                        while (elapsed < duration)
                        {
                            __instance.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                            clockhair.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                            if (GameManager.INVARIANT_DELTA_TIME == 0f)
                            {
                                elapsed += 0.05f;
                            }
                            elapsed += GameManager.INVARIANT_DELTA_TIME;
                            yield return null;
                        }

                        if (!hasRevivingSynergy)
                        {
                            elapsed = 0f;
                            duration = 2f;
                            clockhair.spriteAnimator.Play("clockhair_wobble");
                            while (elapsed < duration)
                            {
                                __instance.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                                clockhair.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                                if (GameManager.INVARIANT_DELTA_TIME == 0f)
                                {
                                    elapsed += 0.05f;
                                }
                                elapsed += GameManager.INVARIANT_DELTA_TIME;
                                yield return null;
                            }

                            elapsed = 0f;
                            duration = 0.1f;
                            AkSoundEngine.PostEvent("Play_OBJ_silenceblank_small_01", __instance.gameObject);
                            __instance.ToggleGunRenderers(false, "death");
                            __instance.spriteAnimator.Play((!__instance.UseArmorlessAnim) ? "death" : "death_armorless");
                            clockhair.spriteAnimator.Play("clockhair_fire");
                            AkSoundEngine.PostEvent("Play_ITM_Crisis_Stone_Impact_01", __instance.gameObject);
                            while (elapsed < duration)
                            {
                                __instance.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                                clockhair.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                                if (GameManager.INVARIANT_DELTA_TIME == 0f)
                                {
                                    elapsed += 0.05f;
                                }
                                elapsed += GameManager.INVARIANT_DELTA_TIME;
                                yield return null;
                            }

                            elapsed = 0f;
                            duration = 2f;
                            clockhair.spriteAnimator.Play("clockhair_wobble");
                            while (elapsed < duration)
                            {
                                __instance.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                                clockhair.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                                if (GameManager.INVARIANT_DELTA_TIME == 0f)
                                {
                                    elapsed += 0.05f;
                                }
                                elapsed += GameManager.INVARIANT_DELTA_TIME;
                                yield return null;
                            }
                        }
                    }
                    elapsed = 0f;
                    duration = 0.1f;
                    clockhairStartPosition = clockhairTransform.position;
                    clockhairTargetPosition = clockhairStartPosition + new Vector3(0f, 12f, 0f);
                    clockhair.spriteAnimator.Play("clockhair_fire");
                    clockhair.hourAnimator.GetComponent<Renderer>().enabled = false;
                    clockhair.minuteAnimator.GetComponent<Renderer>().enabled = false;
                    clockhair.secondAnimator.GetComponent<Renderer>().enabled = false;
                    __instance.DoVibration(Vibration.Time.Normal, Vibration.Strength.Hard);
                    if (!isDoingPigSave && !hasRevivingSynergy)
                    {
                        __instance.spriteAnimator.Play((!__instance.UseArmorlessAnim) ? "death_shot" : "death_shot_armorless");
                    }
                    while (elapsed < duration)
                    {
                        if (GameManager.INVARIANT_DELTA_TIME == 0f)
                        {
                            elapsed += 0.05f;
                        }
                        elapsed += GameManager.INVARIANT_DELTA_TIME;
                        clockhair.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                        if (!isDoingPigSave && !hasRevivingSynergy)
                        {
                            __instance.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                        }
                        if (isDoingPigSave)
                        {
                            pigVFX.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                            pigVFX.transform.position += new Vector3(6f * GameManager.INVARIANT_DELTA_TIME, 0f, 0f);
                        }
                        yield return null;
                    }
                    elapsed = 0f;
                    duration = 1f;
                    while (elapsed < duration)
                    {
                        if (GameManager.INVARIANT_DELTA_TIME == 0f)
                        {
                            elapsed += 0.05f;
                        }
                        elapsed += GameManager.INVARIANT_DELTA_TIME;
                        if (clockhair.spriteAnimator.CurrentFrame == clockhair.spriteAnimator.CurrentClip.frames.Length - 1)
                        {
                            clockhair.renderer.enabled = false;
                        }
                        else
                        {
                            clockhair.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                        }
                        __instance.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                        if (isDoingPigSave)
                        {
                            pigVFX.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
                            pigVFX.transform.position += new Vector3(Mathf.Lerp(6f, 0f, elapsed / duration) * GameManager.INVARIANT_DELTA_TIME, 0f, 0f);
                        }
                        yield return null;
                    }
                    BraveInput.AllowPausedRumble = false;
                    if (isDoingPigSave)
                    {
                        yield return __instance.StartCoroutine(__instance.InvariantWait(1f));
                        Pixelator.Instance.saturation = 1f;
                        GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_HERO_PIG, true);
                        pigVFX.gameObject.SetLayerRecursively(LayerMask.NameToLayer("FG_Critical"));
                        Pixelator.Instance.FadeToColor(0.25f, Pixelator.Instance.FadeColor, true, 0f);
                        Pixelator.Instance.LerpToLetterbox(1f, 0.25f);
                        UnityEngine.Object.Destroy(spotlightSprite.gameObject);
                        Pixelator.Instance.DoFinalNonFadedLayer = false;
                        __instance.healthHaver.FullHeal();
                        if (__instance.ForceZeroHealthState)
                        {
                            __instance.healthHaver.Armor = 6f;
                        }
                        __instance.CurrentInputState = PlayerInputState.AllInput;
                        if (pigItem.HasGunTransformationSacrificeSynergy && __instance.HasActiveBonusSynergy(pigItem.GunTransformationSacrificeSynergy, false))
                        {
                            GunFormeSynergyProcessor.AssignTemporaryOverrideGun(__instance, pigItem.SacrificeGunID, pigItem.SacrificeGunDuration);
                        }
                        __instance.RemovePassiveItem(pigItem.PickupObjectId);
                        __instance.IsVisible = true;
                        __instance.ToggleGunRenderers(true, "death");
                        __instance.ToggleHandRenderers(true, "death");
                        __instance.ToggleAttachedRenderers(true);
                        __instance.gameObject.SetLayerRecursively(LayerMask.NameToLayer("FG_Reflection"));
                        GameManager.Instance.DungeonMusicController.ResetForNewFloor(GameManager.Instance.Dungeon);
                        if (__instance.CurrentRoom != null)
                        {
                            GameManager.Instance.DungeonMusicController.NotifyEnteredNewRoom(__instance.CurrentRoom);
                        }
                        GameManager.Instance.ForceUnpause();
                        GameManager.Instance.PreventPausing = false;
                        BraveTime.ClearMultiplier(GameManager.Instance.gameObject);
                        Exploder.DoRadialKnockback(__instance.CenterPosition, 50f, 5f);
                        if (wasPitFalling)
                        {
                            __instance.StartCoroutine(__instance.PitRespawn(Vector2.zero));
                        }
                        __instance.healthHaver.IsVulnerable = true;
                        __instance.healthHaver.TriggerInvulnerabilityPeriod(-1f);
                    }
                    else if (hasRevivingSynergy)
                    {
                        yield return __instance.StartCoroutine(__instance.InvariantWait(1f));
                        Pixelator.Instance.saturation = 1f;
                        Pixelator.Instance.FadeToColor(0.25f, Pixelator.Instance.FadeColor, true, 0f);
                        Pixelator.Instance.LerpToLetterbox(1f, 0.25f);
                        UnityEngine.Object.Destroy(spotlightSprite.gameObject);
                        Pixelator.Instance.DoFinalNonFadedLayer = false;
                        __instance.healthHaver.FullHeal();
                        if (__instance.ForceZeroHealthState)
                        {
                            __instance.healthHaver.Armor = 6f;
                        }
                        __instance.CurrentInputState = PlayerInputState.AllInput;
                        __instance.IsVisible = true;
                        __instance.ToggleGunRenderers(true, "death");
                        __instance.ToggleHandRenderers(true, "death");
                        __instance.ToggleAttachedRenderers(true);
                        __instance.gameObject.SetLayerRecursively(LayerMask.NameToLayer("FG_Reflection"));
                        GameManager.Instance.DungeonMusicController.ResetForNewFloor(GameManager.Instance.Dungeon);
                        if (__instance.CurrentRoom != null)
                        {
                            GameManager.Instance.DungeonMusicController.NotifyEnteredNewRoom(__instance.CurrentRoom);
                        }
                        GameManager.Instance.ForceUnpause();
                        GameManager.Instance.PreventPausing = false;
                        BraveTime.ClearMultiplier(GameManager.Instance.gameObject);
                        Exploder.DoRadialKnockback(__instance.CenterPosition, 50f, 5f);
                        if (wasPitFalling)
                        {
                            __instance.StartCoroutine(__instance.PitRespawn(Vector2.zero));
                        }
                        __instance.healthHaver.IsVulnerable = true;
                        __instance.healthHaver.TriggerInvulnerabilityPeriod(-1f);
                        __instance.RemovePassiveItem(110);
                    }
                    else
                    {
                        GameStatsManager.Instance.RegisterStatChange(TrackedStats.NUMBER_DEATHS, 1f);
                        if (GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.TIMES_REACHED_MINES) < 1f)
                        {
                            GameStatsManager.Instance.isChump = false;
                        }
                        AmmonomiconDeathPageController.LastKilledPlayerPrimary = __instance.IsPrimaryPlayer;
                        GameManager.Instance.DoGameOver(__instance.healthHaver.lastIncurredDamageSource);
                    }
                    yield break;
                }

                else
                    yield return orig;
            }
        }

        [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.Die))]
        public class PlayerController_Die
        {
            [HarmonyPrefix]
            public static void Prefix(PlayerController __instance)
            {
                for (int i = 0; i < __instance.inventory.AllGuns.Count; i++)
                {
                    if (__instance.inventory.AllGuns[i].PickupObjectId == Ankh_shield.ID)
                    {
                        __instance.ChangeToGunSlot(i);
                    }
                }
                for (int i = 0; i < __instance.inventory.AllGuns.Count; i++)
                {
                    if (__instance.inventory.AllGuns[i].PickupObjectId == Alt_ankh_shield.ID)
                    {
                        __instance.ChangeToGunSlot(i);
                    }
                }
            }
        }

        //[HarmonyPatch(typeof(Gun), nameof(Gun.Play))]
        //public class Gun_Play
        //{
        //    [HarmonyPrefix]
        //    public static void Prefix(Gun __instance, ref string animName)
        //    {
        //        Plugin.Log(animName);
        //    }
        //}

        //public static PlayerController currentPlayer = null;


        //[HarmonyPatch(typeof(tk2dSpriteAnimator), nameof(tk2dSpriteAnimator.PlayForDuration), new Type[] { typeof(string), typeof(float) })]
        //public class tk2dSpriteAnimator_PlayForDuration
        //{
        //    [HarmonyPrefix]
        //    public static void Prefix(tk2dSpriteAnimator __instance, ref string name, ref float duration)
        //    {
        //        if (currentPlayer != null)
        //        {
        //        if ((name == "timefall" || name == "timefall_skull" || name == "timefall_meat" || name == "timefall_nerve") && currentPlayer.characterIdentity.ToString() == "dainplay.etg.rpgworld.vladoge" && currentPlayer.UsingAlternateStartingGuns)
        //        {
        //            name = "pitfall_return";
        //            }
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(TimeTubeCreditsController), nameof(TimeTubeCreditsController.HandleTimefallCorpse))]
        //public class TimeTubeCreditsController_HandleTimefallCorpse
        //{
        //    [HarmonyPrefix]
        //    public static void Prefix(TimeTubeCreditsController __instance, ref PlayerController sourcePlayer)
        //    {
        //            //currentPlayer = sourcePlayer;
        //            if (sourcePlayer.characterIdentity.ToString() == "dainplay.etg.rpgworld.vladoge" && sourcePlayer.UsingAlternateStartingGuns)
        //            sourcePlayer.
        //    }
        //}
        //            Log("WOOOOOOOOOOOOOOOOOO");
        //            if (sourcePlayer.healthHaver.IsDead)
        //            {
        //                yield break;
        //            }
        //            sourcePlayer.IsVisible = false;
        //            sourcePlayer.IsOnFire = false;
        //            sourcePlayer.CurrentPoisonMeterValue = 0f;
        //            sourcePlayer.ToggleFollowerRenderers(false);
        //            GameObject timefallCorpseInstance = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Global Prefabs/TimefallCorpse", ".prefab"), sourcePlayer.sprite.transform.position, Quaternion.identity);
        //            timefallCorpseInstance.SetLayerRecursively(LayerMask.NameToLayer("Unoccluded"));
        //            tk2dSpriteAnimator targetTimefallAnimator = timefallCorpseInstance.GetComponent<tk2dSpriteAnimator>();
        //            SpriteOutlineManager.AddOutlineToSprite(targetTimefallAnimator.Sprite, Color.black);
        //            tk2dSpriteAnimation timefallGenericLibrary = targetTimefallAnimator.Library;
        //            tk2dSpriteAnimation timefallSpecificLibrary = (!(sourcePlayer is PlayerSpaceshipController)) ? sourcePlayer.sprite.spriteAnimator.Library : (sourcePlayer as PlayerSpaceshipController).TimefallCorpseLibrary;
        //            targetTimefallAnimator.Library = timefallSpecificLibrary;
        //            if (isShotPlayer)
        //            {
        //                if (sourcePlayer.ArmorlessAnimations && sourcePlayer.healthHaver.Armor == 0f)
        //                {
        //                    targetTimefallAnimator.Play("death_shot_armorless");
        //                }
        //                else
        //                {
        //                    targetTimefallAnimator.Play("death_shot");
        //                }
        //            }
        //            int iterator = 0;
        //            tk2dSpriteAnimationClip clip = null;
        //            float timePosition = 0f;
        //            Vector2[] noiseVectors = new Vector2[3];
        //            for (int i = 0; i < 3; i++)
        //            {
        //                float f = UnityEngine.Random.value * 3.1415927f * 2f;
        //                noiseVectors[i].Set(Mathf.Cos(f), Mathf.Sin(f));
        //            }
        //            Vector3 FallCenterPosOffset = Vector3.zero;
        //            if (!isShotPlayer)
        //            {
        //                FallCenterPosOffset = new Vector3(0.25f, -1.25f, 3f);
        //            }
        //            if (!sourcePlayer.IsPrimaryPlayer)
        //            {
        //                FallCenterPosOffset += new Vector3(0f, 0f, 1f);
        //            }
        //            Vector3 initialPosition = sourcePlayer.transform.position;
        //            float timefallElapsed = 0f;
        //            while (__instance.m_shouldTimefall)
        //            {
        //                timefallElapsed += GameManager.INVARIANT_DELTA_TIME;
        //                float positionOffsetStrength = 3f * __instance.m_timefallJitterMultiplier;
        //                float positionOffsetSpeed = 0.25f;
        //                timePosition += BraveTime.DeltaTime * positionOffsetSpeed;
        //                Vector3 p = new Vector3(JitterMotion.Fbm(noiseVectors[0] * timePosition, 2), JitterMotion.Fbm(noiseVectors[1] * timePosition, 2), 0f);
        //                p = p * positionOffsetStrength * 2f;
        //                Vector3 screenPoint = TunnelCamera.WorldToViewportPoint(TunnelTransform.position);
        //                Vector3 worldPoint = GameManager.Instance.MainCameraController.Camera.ViewportToWorldPoint(screenPoint);
        //                targetTimefallAnimator.transform.position = Vector3.Lerp(initialPosition, worldPoint + FallCenterPosOffset + p, Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(timefallElapsed)));
        //                if (!targetTimefallAnimator.IsPlaying(targetTimefallAnimator.CurrentClip))
        //                {
        //                    targetTimefallAnimator.ForceClearCurrentClip();
        //                    float num = 0.5f;
        //                    switch (iterator)
        //                    {
        //                        case 0:
        //                            targetTimefallAnimator.Library = timefallSpecificLibrary;
        //                            clip = targetTimefallAnimator.GetClipByName("timefall");
        //                            if (clip != null)
        //                            {
        //                                targetTimefallAnimator.PlayForDuration("timefall", clip.BaseClipLength);
        //                            }
        //                            break;
        //                        case 1:
        //                            targetTimefallAnimator.Library = timefallSpecificLibrary;
        //                            clip = targetTimefallAnimator.GetClipByName("timefall");
        //                            if (clip != null)
        //                            {
        //                                targetTimefallAnimator.PlayForDuration("timefall", clip.BaseClipLength);
        //                            }
        //                            break;
        //                        case 2:
        //                            targetTimefallAnimator.Library = timefallGenericLibrary;
        //                            clip = targetTimefallAnimator.GetClipByName("timefall_skull");
        //                            if (clip != null)
        //                            {
        //                                targetTimefallAnimator.PlayForDuration("timefall_skull", clip.BaseClipLength * 2f);
        //                            }
        //                            num = 1f;
        //                            break;
        //                        case 3:
        //                            targetTimefallAnimator.Library = timefallGenericLibrary;
        //                            clip = targetTimefallAnimator.GetClipByName("timefall_meat");
        //                            if (clip != null)
        //                            {
        //                                targetTimefallAnimator.PlayForDuration("timefall_meat", clip.BaseClipLength * 2f);
        //                            }
        //                            num = 1f;
        //                            break;
        //                        case 4:
        //                            targetTimefallAnimator.Library = timefallGenericLibrary;
        //                            clip = targetTimefallAnimator.GetClipByName("timefall_nerve");
        //                            if (clip != null)
        //                            {
        //                                targetTimefallAnimator.PlayForDuration("timefall_nerve", clip.BaseClipLength * 2f);
        //                            }
        //                            num = 1f;
        //                            break;
        //                        default:
        //                            targetTimefallAnimator.Library = timefallSpecificLibrary;
        //                            clip = targetTimefallAnimator.GetClipByName("timefall");
        //                            if (clip != null)
        //                            {
        //                                targetTimefallAnimator.PlayForDuration("timefall", clip.BaseClipLength);
        //                            }
        //                            break;
        //                    }
        //                    iterator = (iterator + ((UnityEngine.Random.value >= num) ? 0 : 1)) % 5;
        //                }
        //                yield return null;
        //            }
        //            float elapsed = 0f;
        //            float duration = 1f;
        //            while (elapsed < duration)
        //            {
        //                elapsed += BraveTime.DeltaTime;
        //                timefallCorpseInstance.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, elapsed / duration);
        //                yield return null;
        //            }
        //            UnityEngine.Object.Destroy(timefallCorpseInstance);
        //            yield break;
        //        }
        //    }
        //}

        [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.HandleStartDodgeRoll))]
        public class PlayerController_HandleStartDodgeRoll
        {
            [HarmonyPrefix]
            public static bool Prefix(PlayerController __instance, ref bool __result)
            {
                if ((PassiveItem.IsFlagSetForCharacter(__instance, typeof(Ankh_shield))) || (PassiveItem.IsFlagSetForCharacter(__instance, typeof(Alt_ankh_shield))) || PassiveItem.IsFlagSetForCharacter(__instance, typeof(Gravity_sign)))
                {
                    __result = false;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(FoyerAlternateGunShrineController), nameof(FoyerAlternateGunShrineController.HandleShrineConversation))]
        public class FoyerAlternateGunShrineController_HandleShrineConversation
        {
            [HarmonyPrefix]
            public static void Prefix(FoyerAlternateGunShrineController __instance, ref PlayerController interactor)
            {
                //Log(interactor.sprite.spriteAnimator.library.name);
                //Log(interactor.name);
                if (interactor.characterIdentity.ToString() == "dainplay.etg.rpgworld.kkorroll" || interactor.characterIdentity.ToString() == "dainplay.etg.rpgworld.vladoge") GameStatsManager.Instance.SetCharacterSpecificFlag(interactor.characterIdentity, CharacterSpecificGungeonFlags.KILLED_PAST_ALTERNATE_COSTUME, GameStatsManager.Instance.GetCharacterSpecificFlag(interactor.characterIdentity, CharacterSpecificGungeonFlags.CLEARED_BULLET_HELL));
            }
        }

        [HarmonyPatch(typeof(FoyerFloorController), nameof(FoyerFloorController.Start))]
        public class FoyerFloorController_Start
        {
            [HarmonyPrefix]
            public static void Prefix(FoyerFloorController __instance)
            {
                GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_ALTERNATE_GUNS_UNLOCKED, true);
            }
        }

        [HarmonyPatch(typeof(BaseShopController), nameof(BaseShopController.PlayerFired))]
        public class BaseShopController_PlayerFired
        {
            [HarmonyPrefix]
            public static bool Prefix(BaseShopController __instance)
            {
                for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
                {
                    PlayerController playerController = GameManager.Instance.AllPlayers[j];
                    if (playerController && playerController.healthHaver.IsAlive && playerController.CurrentRoom == __instance.m_room && playerController.IsFiring && (playerController.CurrentGun.PickupObjectId == Ankh_shield.ID || playerController.CurrentGun.PickupObjectId == Alt_ankh_shield.ID))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(GameUIAmmoController), nameof(GameUIAmmoController.UpdateUIGun))]
        private class GameUIAmmoControllerUpdateUIGunPatch
        {
            [HarmonyILManipulator]
            private static void GameUIAmmoControllerUpdateUIGunIL(ILContext il)
            {
                ILCursor cursor = new ILCursor(il);
                if (!cursor.TryGotoNext(MoveType.After, instr => instr.MatchLdfld<Gun>("IsHeroSword")))
                    return;

                cursor.Emit(OpCodes.Ldloc_0);
                cursor.Emit(OpCodes.Call, typeof(GameUIAmmoControllerUpdateUIGunPatch).GetMethod(
                  nameof(CheckHideAmmo), BindingFlags.Static | BindingFlags.NonPublic));

                return;
            }

            private static bool CheckHideAmmo(bool oldValue, Gun gun)
            {
                if (oldValue)
                    return true;
                if (gun.gameObject.GetComponent<Ankh_shield>() is not Ankh_shield && gun.gameObject.GetComponent<Alt_ankh_shield>() is not Alt_ankh_shield)
                {
                    return false;
                }
                if (gun.CurrentOwner is PlayerController player && player.PlayerHasActiveSynergy("RPGW Autumn Mirage")) return true;
                else return false;
            }
        }
        public void GMStart(GameManager g)
        {
            Alt_gruv_pistol.Add();
            Gruv_pistol.Add();
            Gruv_shotgun.Add();
            Gruv_assault_rifle.Add();
            Gruv_sniper_rifle.Add();
            Gruv_machine_gun.Add();
            Gruv_rocket_launcher.Add();
            Rapid_dagger_spell.Add();
            Alt_ankh_shield.Add();
            Ankh_shield.Add();
            Gruv_crown.Init();
            An_item.Init();
            Gravity_sign.Init();
            Dagger_bullets.Init();
            //Autumn_luxin_cannon.Add();
            InitialiseCharacters.Charactersetup();

            StartCoroutine(WaitForModLoad());

            LoadMessage();
        }
        public IEnumerator WaitForModLoad()
        {
            yield return null;
            InitialiseSynergies.DoInitialisation();
        }

        public static void ConsoleCommands()
        {
            ETGModConsole.Commands.AddGroup("rpgworld");
            ConsoleCommandGroup group = ETGModConsole.Commands.GetGroup("rpgworld");
            group.AddUnit("unlock_alt_guns", delegate (string[] x)
            {
                GameStatsManager.Instance.SetCharacterSpecificFlag(ETGModCompatibility.ExtendEnum<PlayableCharacters>(GUID, "vladoge"), CharacterSpecificGungeonFlags.CLEARED_BULLET_HELL, true);
                GameStatsManager.Instance.SetCharacterSpecificFlag(ETGModCompatibility.ExtendEnum<PlayableCharacters>(GUID, "kkorroll"), CharacterSpecificGungeonFlags.CLEARED_BULLET_HELL, true);
                LogRainbow("Alternative gun skins unlocked!");
            });
            Dictionary<string, string> commandDescriptions = ETGModConsole.CommandDescriptions;
            commandDescriptions["rpgworld unlock_alt_guns"] = "Unlocks alt guns for all of the RPGworld's characters";
        }


        public static void Log(string text, string color="#FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }
        public static void LogRainbow(string text)
        {
            var container = new SGroup() { Size = new Vector2(20000, 32), AutoLayoutPadding = 0 };
            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];

                if (c == ' ')
                {
                    container.Children.Add(new SRect(Color.clear) { Size = Vector2.one * 10 });
                }
                else
                {
                    var hue = Mathf.InverseLerp(0, text.Length, i);
                    var col = Color.HSVToRGB(hue, 1, 1);
                    var label = new SLabel(c.ToString()) { Foreground = col, With = { new SRainbowModifier() } };
                    container.Children.Add(label);
                }
            }
            container.AutoLayout = (SGroup g) => new Action<int, SElement>(g.AutoLayoutHorizontal);
            ETGModConsole.Instance.GUI[0].Children.Add(container);
        }
        public static void LoadMessage()
		{   
            var txt = $"Welcome to RPGworld v{VERSION}";

                var groupHeight = 48;
                var group = new SGroup() { Size = new Vector2(20000, groupHeight), AutoLayoutPadding = 0, Background = Color.clear, AutoLayout = x => x.AutoLayoutHorizontal };
                Color currentColor = Color.white;

            for (int i = 0; i < txt.Length; i++)
                {
                    var c = txt[i];

                    if (c == ' ')
                    {
                        group.Children.Add(new SRect(Color.clear) { Size = Vector2.one * 10 });
                    }
                    else
                    {
                    currentColor = Color.white;
                    switch (c) {
                        case 'R':
                            currentColor = new Color32(206, 28, 42, 255);
                            break;
                        case 'P':
                            currentColor = new Color32(25, 177, 63, 255);
                            break;
                        case 'G':
                            currentColor = new Color32(99, 69, 155, 255);
                            break;

                    }
                        group.Children.Add(new SLabel(c.ToString()) { With = { new MovementThingy(1f, 0.2f * i, 1.5f, currentColor) } });
                    }
                }

                ETGModConsole.Instance.GUI[0].Children.Add(group);
            }

        public class MovementThingy(float mult, float offs, float amplitude, Color color) : SModifier
        {
            public float offs = offs;
            public float amplitude = amplitude;
            public float mult = mult;

            public override void Update()
            {
                Elem.Position = Elem.Position.WithY(Mathf.Sin((Time.realtimeSinceStartup * mult + offs) * Mathf.PI * 2) * amplitude);
                Elem.Foreground = color;
            }
        }
    }
}
