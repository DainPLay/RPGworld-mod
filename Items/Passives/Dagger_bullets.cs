using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RPGworldMod
{
    public class Dagger_bullets : PassiveItem
    {
        public static void Init()
        {
            string ItemName = "Dagger Bullets";

            string SpriteDirectory = "RPGworldMod/Resources/dagger_bullets_sprite";

            GameObject obj = new GameObject(ItemName);

            var item = obj.AddComponent<Dagger_bullets>();

            ItemBuilder.AddSpriteToObject(ItemName, SpriteDirectory, obj);

            string shortDesc = "Right Behind You";

            string longDesc = "If shot in the back, oneshots normal enemies and deals double damage to bosses.\n\n" +
            "By shooting these bullets, you show everyone in the Gungeon that you think you can outsmart them. Maybe, maybe. But they've yet to meet one to outsmart bullet.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rpgworld");
            GunExt.SetName(item, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Пули-кинжалы");
            GunExt.SetShortDescription(item, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Он уже здесь");
            GunExt.SetLongDescription(item, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Попадание в спину моментально убивает обычных противников и наносит двойной урон боссам.\n\n" +
            "Используя эти пули, Вы демонстрируете, что считаете себя умнее всех в Оружелье. Посмотрим, посмотрим. Они ещё не встречали никого умнее пули.");

            item.quality = PickupObject.ItemQuality.B;
            item.SetTag("bullet_modifier");
            item.AddPassiveStatModifier(PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.ForcedPositionInAmmonomicon = PickupObjectDatabase.GetById(640).ForcedPositionInAmmonomicon;
        }

        public GameObject critVFX = VFXToolbox.CreateVFX("Dagger Critical Impact", new List<string>
            {
                "RPGworldMod/Resources/VFX/rapid_dagger_critical_impact_001",
                "RPGworldMod/Resources/VFX/rapid_dagger_critical_impact_002",
                "RPGworldMod/Resources/VFX/rapid_dagger_critical_impact_003",
                "RPGworldMod/Resources/VFX/rapid_dagger_critical_impact_004"
            }, 12, new IntVector2(24, 24), tk2dBaseSprite.Anchor.MiddleCenter, false, 0f, -1f, null, tk2dSpriteAnimationClip.WrapMode.Once, false, 0);
        // Token: 0x06000AD7 RID: 2775 RVA: 0x0008CF00 File Offset: 0x0008B100
        private void PostProcessProjectile(Projectile proj, float flot)
        {
            proj.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(proj.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HitEnemy));
        }
        private void HitEnemy(Projectile projectile, SpeculativeRigidbody enemy, bool killed)
        {
            Vector2 v = Vector2.Lerp(projectile.specRigidbody.UnitCenter, enemy.UnitCenter, 0.5f);
            BounceProjModifier orAddComponent = projectile.gameObject.GetOrAddComponent<BounceProjModifier>();
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

        // Token: 0x06000AD8 RID: 2776 RVA: 0x0008CF25 File Offset: 0x0008B125
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += this.PostProcessProjectile;
            base.Pickup(player);
        }

        // Token: 0x06000AD9 RID: 2777 RVA: 0x0008CF44 File Offset: 0x0008B144
        public override void DisableEffect(PlayerController player)
        {
            bool flag = player;
            if (flag)
            {
                player.PostProcessProjectile -= this.PostProcessProjectile;
            }
            base.DisableEffect(player);
        }

    }
}
