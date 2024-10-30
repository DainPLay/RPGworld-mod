using Alexandria.ItemAPI;
using Dungeonator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static tk2dSpriteCollectionDefinition;
using static UnityEngine.UI.GridLayoutGroup;

namespace RPGworldMod
{
    class Gravity_sign : PassiveItem
    {
        public static void Init()
        {
            string ItemName = "Gravity Sign";

            string SpriteDirectory = "RPGworldMod/Resources/gravity_sign_sprite";

            GameObject obj = new GameObject(ItemName);

            var item = obj.AddComponent<Gravity_sign>();

            ItemBuilder.AddSpriteToObject(ItemName, SpriteDirectory, obj);

            string shortDesc = "Down To The Core";

            string longDesc = "Grants an ability to cancel roll into instant drop. Impact sends a shockwave outwards, knocking back enemies and blanking bullets.\n\n" +
            "The green arrow on the T-shirt naturally directs the gaze of those around you downwards. To where you are about to end up.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rpgworld");
            GunExt.SetName(item, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Знак гравитации");
            GunExt.SetShortDescription(item, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "И камнем вниз");
            GunExt.SetLongDescription(item, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Позволяет отменять перекат в моментальное приземление. Небольшая ударная волная отталкивает противников и уничтожает вражеские пули.\n\n" +
            "Зелёная стрелка на футболке естественно направляет взгляд окружающих вниз. Туда, где Вы скоро окажетесь.");

            item.quality = PickupObject.ItemQuality.SPECIAL;
            item.ShouldBeExcludedFromShops = true;
            item.ForcedPositionInAmmonomicon = PickupObjectDatabase.GetById(414).ForcedPositionInAmmonomicon;
        }

        // Token: 0x06001029 RID: 4137 RVA: 0x000AA68C File Offset: 0x000A888C
        public override void Pickup(PlayerController player)
        {
            player.OnIsRolling += this.Player_OnIsRolling;
            player.OnRolledIntoEnemy += this.Player_OnRolledIntoEnemy;
            base.Pickup(player);
        }
        public bool messedup;
        // Token: 0x0600102A RID: 4138 RVA: 0x000AA704 File Offset: 0x000A8904
        private void Player_OnRolledIntoEnemy(PlayerController arg1, AIActor arg2)
        {
            if (!cooldown)
            {
                var outlineMat = SpriteOutlineManager.GetOutlineMaterial(arg1.sprite);
                messedup = false;
                if (!arg1.IsOverPitAtAll) this.DoMicroBlank(arg1.specRigidbody.UnitCenter);
                else if(!arg1.DodgeRollIsBlink)
                {
                    arg1.ForceFall();
                    arg1.spriteAnimator.Stop();
                    arg1.QueueSpecificAnimation(arg1.spriteAnimator.GetClipByName((!arg1.UseArmorlessAnim) ? "pitfall_down" : "pitfall_down_armorless").name);
                    arg1.spriteAnimator.SetFrame(0, false);
                    messedup = true;
                }
                if (arg1.PlayerHasActiveSynergy("RPGW Bounce Sign"))
                {
                    if (!arg1.CheckDodgeRollDepth())
                    {
                        if (arg1.DodgeRollIsBlink)
                        {
                            PassiveItem.IncrementFlag(arg1, typeof(Gravity_sign));
                            StartCoroutine(CooldownCR());
                        }
                        else
                        {
                            if (!messedup)
                            {
                                arg1.ForceStopDodgeRoll();
                                arg1.ToggleGunRenderers(true, "");
                                arg1.ToggleHandRenderers(true, "");
                            }
                            PassiveItem.IncrementFlag(arg1, typeof(Gravity_sign));
                            arg1.spriteAnimator.Stop();
                            arg1.QueueSpecificAnimation(arg1.spriteAnimator.GetClipByName((!arg1.UseArmorlessAnim) ? "pitfall_return" : "pitfall_return_armorless").name);
                            arg1.spriteAnimator.SetFrame(0, false);
                            arg1.ToggleGunRenderers(false, "");
                            arg1.ToggleHandRenderers(false, "");
                            isdropping = true;
                            if (!cooldown) StartCoroutine(CooldownCR());
                        }
                    }
                    used_gravity = true;
                }
                else
                {
                    if (arg1.DodgeRollIsBlink)
                    {
                        PassiveItem.IncrementFlag(arg1, typeof(Gravity_sign));
                        StartCoroutine(CooldownCR());
                    }
                    else
                    {
                        if (!messedup)
                        {
                            arg1.ForceStopDodgeRoll();
                            arg1.ToggleGunRenderers(true, "");
                            arg1.ToggleHandRenderers(true, "");
                        }
                        PassiveItem.IncrementFlag(arg1, typeof(Gravity_sign));
                        arg1.spriteAnimator.Stop();
                        arg1.QueueSpecificAnimation(arg1.spriteAnimator.GetClipByName((!arg1.UseArmorlessAnim) ? "pitfall_return" : "pitfall_return_armorless").name);
                        arg1.spriteAnimator.SetFrame(0, false);
                        arg1.ToggleGunRenderers(false, "");
                        arg1.ToggleHandRenderers(false, "");
                        isdropping = true;
                        StartCoroutine(CooldownCR());
                    }
                }
            }
        }
        public bool used_gravity = false;
        // Token: 0x0600102B RID: 4139 RVA: 0x000AA746 File Offset: 0x000A8946
        private void Player_OnIsRolling(PlayerController obj)
        {
            if (Gravity_sign.Key(GungeonActions.GungeonActionType.DodgeRoll, obj) && !cooldown)
            {
                messedup = false;
                if (!obj.IsOverPitAtAll) this.DoMicroBlank(obj.specRigidbody.UnitCenter);
                else if (!obj.DodgeRollIsBlink)
                {
                    obj.ForceFall();
                    messedup = true;
                }
                var outlineMat = SpriteOutlineManager.GetOutlineMaterial(obj.sprite);
                if (obj.PlayerHasActiveSynergy("RPGW Bounce Sign"))
                {
                    if (!obj.CheckDodgeRollDepth())
                    {
                        if (obj.DodgeRollIsBlink)
                        {
                            PassiveItem.IncrementFlag(obj, typeof(Gravity_sign));
                            StartCoroutine(CooldownCR());
                        }
                        else
                        {
                            if (!messedup)
                            {
                                obj.ForceStopDodgeRoll();
                                obj.ToggleGunRenderers(true, "");
                                obj.ToggleHandRenderers(true, "");
                            }
                            PassiveItem.IncrementFlag(obj, typeof(Gravity_sign));
                            obj.spriteAnimator.Stop();
                            obj.QueueSpecificAnimation(obj.spriteAnimator.GetClipByName((!obj.UseArmorlessAnim) ? "pitfall_return" : "pitfall_return_armorless").name);
                            obj.spriteAnimator.SetFrame(0, false);
                            obj.ToggleGunRenderers(false, "");
                            obj.ToggleHandRenderers(false, "");
                            isdropping = true;
                            if (!cooldown) StartCoroutine(CooldownCR());
                        }
                    }
                    used_gravity = true;
                }
                else
                {
                    if (obj.DodgeRollIsBlink)
                    {
                        PassiveItem.IncrementFlag(obj, typeof(Gravity_sign));
                        StartCoroutine(CooldownCR());
                    }
                    else
                    {
                        if (!messedup)
                        {
                            obj.ForceStopDodgeRoll();
                            obj.ToggleGunRenderers(true, "");
                            obj.ToggleHandRenderers(true, "");
                        }
                        PassiveItem.IncrementFlag(obj, typeof(Gravity_sign));
                        obj.spriteAnimator.Stop();
                        obj.QueueSpecificAnimation(obj.spriteAnimator.GetClipByName((!obj.UseArmorlessAnim) ? "pitfall_return" : "pitfall_return_armorless").name);
                        obj.spriteAnimator.SetFrame(0, false);
                        obj.ToggleGunRenderers(false, "");
                        obj.ToggleHandRenderers(false, "");
                        isdropping = true;
                        StartCoroutine(CooldownCR());
                    }

                }
            }
        }
        bool isdropping = false;
        bool cooldown = false;
        bool isImmuneToKnockback = false;
        public override void Update()
        {

            if (this.m_owner.IsDodgeRolling && !cooldown)
            {
                if (!isImmuneToKnockback)
                {
                    isImmuneToKnockback = true;
                    PassiveItem.IncrementFlag(m_owner, typeof(LiveAmmoItem));
                }
            }
            else if (isImmuneToKnockback)
            {
                isImmuneToKnockback = false;
                PassiveItem.DecrementFlag(m_owner, typeof(LiveAmmoItem));
            }
            var outlineMat = SpriteOutlineManager.GetOutlineMaterial(this.m_owner.sprite);
            if (!cooldown) outlineMat.SetColor("_OverrideColor", new Color(0, 1, 0));
            else outlineMat.SetColor("_OverrideColor", new Color(0, 0, 0));
            if (used_gravity && !this.m_owner.IsDodgeRolling)
            {
                used_gravity = false;
                StartCoroutine(CooldownCR());
            }
            if (isdropping && !messedup && (this.m_owner.spriteAnimator.IsPlaying("pitfall_return") || this.m_owner.spriteAnimator.IsPlaying("pitfall_return_armorless")) && this.m_owner.spriteAnimator.CurrentFrame > 2)
            { this.m_owner.spriteAnimator.Stop(); this.m_owner.spriteAnimator.OnAnimationCompleted(); isdropping = false;
                
                    this.m_owner.ToggleGunRenderers(true, "");
                    this.m_owner.ToggleHandRenderers(true, "");
            }
            base.Update();
        }

        // Token: 0x0600102C RID: 4140 RVA: 0x000AA760 File Offset: 0x000A8960
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnIsRolling -= this.Player_OnIsRolling;
            player.OnRolledIntoEnemy -= this.Player_OnRolledIntoEnemy;
            return base.Drop(player);
        }
        public static bool Key(GungeonActions.GungeonActionType action, PlayerController player)
        {
            bool flag = !GameManager.Instance.IsLoadingLevel;
            return flag && BraveInput.GetInstanceForPlayer(player.PlayerIDX).ActiveActions.GetActionFromType(action).WasPressed;
        }
        private void DoMicroBlank(Vector2 center)
        {
                    GameObject silencerVFX = (GameObject)ResourceCache.Acquire("Global VFX/BlankVFX_Ghost");
                    AkSoundEngine.PostEvent("Play_OBJ_silenceblank_small_01", base.gameObject);
                    GameObject gameObject = new GameObject("silencer");
                    SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
                    float additionalTimeAtMaxRadius = 0.25f;
                    silencerInstance.TriggerSilencer(center, 25f, 5f, silencerVFX, 0f, 3f, 3f, 3f, 250f, 5f, additionalTimeAtMaxRadius, this.m_owner, false, false);
        }

        public IEnumerator CooldownCR() //SpecialAPI was here
        {
            if (this == null || Owner == null)
                yield break; // no point in handling the cooldown if this item or the player is destroyed

            cooldown = true;


            var chargeRollingInterval = 0.33f; // this will be the time between the bow charge sound playing and the cooldown ending
            var cooldownTime = 9f; // this will be your cooldown time in seconds
            if (Owner.PlayerHasActiveSynergy("RPGW Bounce Sign")) cooldownTime = 6f;
            cooldownTime -= chargeRollingInterval;
            var chargeSoundInterval = 0.4f; // this will be the time between the bow charge sound playing and the cooldown ending

            var soundWaitTime = cooldownTime - chargeSoundInterval;

            if (chargeRollingInterval > 0f)
                yield return new WaitForSeconds(chargeRollingInterval);

            if (this == null || Owner == null)
                yield break; // if the player or this item is destroyed at this point, stop the cooldown handling

            PassiveItem.DecrementFlag(Owner, typeof(Gravity_sign)); // disable the item's effect

            if (soundWaitTime > 0f)
                yield return new WaitForSeconds(soundWaitTime);

            if (this == null || Owner == null)
                yield break; // if the player or this item is destroyed at this point, stop the cooldown handling

            AkSoundEngine.PostEvent("Play_WPN_woodbow_charge_02", Owner.gameObject); // play charge sound

            if (chargeSoundInterval > 0f)
                yield return new WaitForSeconds(chargeSoundInterval);

            if (this == null || Owner == null)
                yield break;

            cooldown = false;
        }
    }
}
