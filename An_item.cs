using System;
using System.Collections;
using System.Collections.Generic;
using Alexandria.ItemAPI;
using Dungeonator;
using UnityEngine;

namespace RPGworldMod
{
    class An_item : SpawnObjectPlayerItem
    {

        public GameObject teleportObject;
        private int duration = 10;
        public static void Init()
        {
            string ItemName = "An Item";

            string SpriteDirectory = "RPGworldMod/Resources/item_sprite";

            GameObject obj = new GameObject(ItemName);

            An_item item = obj.AddComponent<An_item>();

            ItemBuilder.AddSpriteToObject(ItemName, SpriteDirectory, obj, null);

            string shortDesc = "Drops a shadow";

            string longDesc = "Can be thrown. While flying or laying, it can drop a shadow very similar to you! Exact match even.\n\n" +
            "Just an item with a shadow. In its place there really could be anything else besides glass.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rpgworld");

            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 1, StatModifier.ModifyMethod.ADDITIVE);

            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            item.quality = PickupObject.ItemQuality.C;
            
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 5f);
            item.consumable = false;
            item.objectToSpawn = An_item.BuildPrefab();
            item.tossForce = 24f;
            item.canBounce = false;
            item.IsCigarettes = false;
            item.RequireEnemiesInRoom = false;
            item.SpawnRadialCopies = false;
            item.RadialCopiesToSpawn = 0;
            item.AudioEvent = null;
            item.IsKageBunshinItem = false;
            item.usableDuringDodgeRoll = true;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        public override bool CanBeUsed(PlayerController user)
        {
            return true;
        }
        public override void DoEffect(PlayerController user)
        {
                base.StartCoroutine(ItemBuilder.HandleDuration(this, (float)this.duration, user, new Action<PlayerController>(this.EndEffect)));
                this.DoSpavn(user, 0f);
        }

        public void DoSpavn(PlayerController user, float angleFromAim)
        {
            if (!string.IsNullOrEmpty(this.enemyGuidToSpawn))
            {
                this.objectToSpawn = EnemyDatabase.GetOrLoadByGuid(this.enemyGuidToSpawn).gameObject;
            }
            GameObject synergyObjectToSpawn = this.objectToSpawn;
            this.teleportObject = synergyObjectToSpawn;
            if (this.HasOverrideSynergyItem && user.HasActiveBonusSynergy(this.RequiredSynergy, false))
            {
                synergyObjectToSpawn = this.SynergyObjectToSpawn;
            }
            Projectile component = synergyObjectToSpawn.GetComponent<Projectile>();
            this.m_elapsedCooldownWhileExtantTimer = 0f;
            if (component != null)
            {
                Vector2 v = user.unadjustedAimPoint - user.LockedApproximateSpriteCenter;
                this.spawnedPlayerObject = UnityEngine.Object.Instantiate<GameObject>(synergyObjectToSpawn, user.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, BraveMathCollege.Atan2Degrees(v)));
                this.teleportObject = this.spawnedPlayerObject;
            }
            else if (this.tossForce == 0f)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(synergyObjectToSpawn, user.specRigidbody.UnitCenter, Quaternion.identity);
                this.spawnedPlayerObject = gameObject;
                this.teleportObject = this.spawnedPlayerObject;
                tk2dBaseSprite component2 = gameObject.GetComponent<tk2dBaseSprite>();
                if (component2 != null)
                {
                    component2.PlaceAtPositionByAnchor(user.specRigidbody.UnitCenter.ToVector3ZUp(component2.transform.position.z), tk2dBaseSprite.Anchor.MiddleCenter);
                    if (component2.specRigidbody != null)
                    {
                        component2.specRigidbody.RegisterGhostCollisionException(user.specRigidbody);
                    }
                }
                KageBunshinController component3 = gameObject.GetComponent<KageBunshinController>();
                if (component3)
                {
                    component3.InitializeOwner(user);
                }
                if (this.IsKageBunshinItem && user.HasActiveBonusSynergy(CustomSynergyType.KINJUTSU, false))
                {
                    component3.UsesRotationInsteadOfInversion = true;
                    component3.RotationAngle = angleFromAim;
                }
                gameObject.transform.position = gameObject.transform.position.Quantize(0.0625f);
            }
            else
            {
                Vector3 vector = user.unadjustedAimPoint - user.LockedApproximateSpriteCenter;
                Vector3 vector2 = user.specRigidbody.UnitCenter;
                if (vector.y > 0f)
                {
                    vector2 += Vector3.up * 0.25f;
                }
                GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(synergyObjectToSpawn, vector2, Quaternion.identity);
                this.teleportObject = gameObject2;
                tk2dBaseSprite component4 = gameObject2.GetComponent<tk2dBaseSprite>();
                if (component4)
                {
                    component4.PlaceAtPositionByAnchor(vector2, tk2dBaseSprite.Anchor.MiddleCenter);
                }
                this.spawnedPlayerObject = gameObject2;
                Vector2 vector3 = user.unadjustedAimPoint - user.LockedApproximateSpriteCenter;
                vector3 = Quaternion.Euler(0f, 0f, angleFromAim) * vector3;
                DebrisObject debrisObject = LootEngine.DropItemWithoutInstantiating(gameObject2, gameObject2.transform.position, vector3, this.tossForce, false, false, true, false);
                if (gameObject2.GetComponent<BlackHoleDoer>())
                {
                    debrisObject.PreventFallingInPits = true;
                    debrisObject.PreventAbsorption = true;
                }
                if (vector.y > 0f && debrisObject)
                {
                    debrisObject.additionalHeightBoost = -1f;
                    if (debrisObject.sprite)
                    {
                        debrisObject.sprite.UpdateZDepth();
                    }
                }
                debrisObject.IsAccurateDebris = true;
                debrisObject.Priority = EphemeralObject.EphemeralPriority.Critical;
                debrisObject.bounceCount = ((!this.canBounce) ? 0 : 1);
            }
            if (this.spawnedPlayerObject)
            {
                PortableTurretController component5 = this.spawnedPlayerObject.GetComponent<PortableTurretController>();
                if (component5)
                {
                    component5.sourcePlayer = this.LastOwner;
                }
                Projectile componentInChildren = this.spawnedPlayerObject.GetComponentInChildren<Projectile>();
                if (componentInChildren)
                {
                    componentInChildren.Owner = this.LastOwner;
                    componentInChildren.TreatedAsNonProjectileForChallenge = true;
                }
                SpawnObjectItem componentInChildren2 = this.spawnedPlayerObject.GetComponentInChildren<SpawnObjectItem>();
                if (componentInChildren2)
                {
                    componentInChildren2.SpawningPlayer = this.LastOwner;
                }
            }
        }

        public override void DoActiveEffect(PlayerController user)
        {
            base.DoActiveEffect(user);
            if (this.teleportObject != null)
            {
                Vector2 worldCenter = this.teleportObject.GetComponent<tk2dBaseSprite>().WorldCenter;
                TeleportPlayerToCursorPosition.StartTeleport(user, worldCenter);
                this.TeleportEffect(this.teleportObject, user);
                this.EndEffect(user);
                base.IsCurrentlyActive = false;
            }
        }
        private void EndEffect(PlayerController user)
        {
            if (gameObject != null)
            {
                AkSoundEngine.PostEvent("Play_CHR_ninja_dash_01", this.teleportObject);
                GameObject gameObject = TeleportPlayerToCursorPosition.BloodiedScarfPoofVFX;
                GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                gameObject2.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(this.teleportObject.GetComponent<tk2dBaseSprite>().WorldCenter + new Vector2(0f, -0.5f), tk2dBaseSprite.Anchor.LowerCenter);
                gameObject2.transform.position = gameObject2.transform.position.Quantize(0.0625f);
                gameObject2.GetComponent<tk2dBaseSprite>().UpdateZDepth();
            }
            UnityEngine.Object.Destroy(this.teleportObject);
            this.m_isCurrentlyActive = false;
        }

        public override void OnPreDrop(PlayerController user)
        {
            base.OnPreDrop(user);
            if (this.teleportObject != null)
            {
                AkSoundEngine.PostEvent("Play_CHR_ninja_dash_01", this.teleportObject);
                GameObject gameObject = TeleportPlayerToCursorPosition.BloodiedScarfPoofVFX;
                if (gameObject != null)
                {
                    GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                    gameObject2.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(this.teleportObject.GetComponent<tk2dBaseSprite>().WorldCenter + new Vector2(0f, -0.5f), tk2dBaseSprite.Anchor.LowerCenter);
                    gameObject2.transform.position = gameObject2.transform.position.Quantize(0.0625f);
                    gameObject2.GetComponent<tk2dBaseSprite>().UpdateZDepth();
                }
                UnityEngine.Object.Destroy(this.teleportObject);
            }
        }
        public override void OnDestroy()
        {
            AkSoundEngine.PostEvent("Play_CHR_ninja_dash_01", this.teleportObject);
            GameObject gameObject = TeleportPlayerToCursorPosition.BloodiedScarfPoofVFX;
            if (gameObject != null)
            {
                GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                gameObject2.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(this.teleportObject.GetComponent<tk2dBaseSprite>().WorldCenter + new Vector2(0f, -0.5f), tk2dBaseSprite.Anchor.LowerCenter);
                gameObject2.transform.position = gameObject2.transform.position.Quantize(0.0625f);
                gameObject2.GetComponent<tk2dBaseSprite>().UpdateZDepth();
                UnityEngine.Object.Destroy(this.teleportObject);
            }
            base.OnDestroy();
        }
        private void TeleportEffect(GameObject g, PlayerController p)
        {
            Vector2 worldCenter = p.sprite.WorldCenter;
            if (g != null && g.GetComponent<tk2dBaseSprite>() != null)
            {
                worldCenter = g.GetComponent<tk2dBaseSprite>().WorldCenter;
            }
            AkSoundEngine.PostEvent("Play_CHR_ninja_dash_01", base.gameObject);
            for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
            {
                if (!GameManager.Instance.AllPlayers[i].IsGhost)
                {
                    GameManager.Instance.AllPlayers[i].healthHaver.TriggerInvulnerabilityPeriod(1f);
                    GameManager.Instance.AllPlayers[i].knockbackDoer.TriggerTemporaryKnockbackInvulnerability(1f);
                }
            }
            GameObject gameObject = TeleportPlayerToCursorPosition.BloodiedScarfPoofVFX;
            if (gameObject != null && g != null)
            {
                GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                gameObject2.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(worldCenter + new Vector2(0f, -0.5f), tk2dBaseSprite.Anchor.LowerCenter);
                gameObject2.transform.position = gameObject2.transform.position.Quantize(0.0625f);
                gameObject2.GetComponent<tk2dBaseSprite>().UpdateZDepth();
            }
            p.CurrentRoom.ApplyActionToNearbyEnemies(worldCenter, 2f, new Action<AIActor, float>(this.ProcessEnemy));
        }


        private void ProcessEnemy(AIActor a, float distance)
        {
            if (a.IsNormalEnemy && a.healthHaver && !a.healthHaver.IsBoss)
            {
                a.healthHaver.ApplyDamage(100000f, Vector2.zero, "Telefrag", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
            }
        }
        // Token: 0x06000525 RID: 1317 RVA: 0x00042E8C File Offset: 0x0004108C
        public static GameObject BuildPrefab()
        {
            GameObject gameObject = SpriteBuilder.SpriteFromResource("RPGworldMod/Resources/ThrowableActives/Item/item_toss_001.png", new GameObject("ItemToss"), null);
            FakePrefabExtensions.MakeFakePrefab(gameObject);
            tk2dSpriteAnimator tk2dSpriteAnimator = gameObject.AddComponent<tk2dSpriteAnimator>();
            tk2dSpriteCollectionData spriteCollection = (PickupObjectDatabase.GetById(108) as SpawnObjectPlayerItem).objectToSpawn.GetComponent<tk2dSpriteAnimator>().Library.clips[0].frames[0].spriteCollection;
            tk2dSpriteAnimationClip tk2dSpriteAnimationClip = SpriteBuilder.AddAnimation(tk2dSpriteAnimator, spriteCollection, new List<int>
            {
                SpriteBuilder.AddSpriteToCollection("RPGworldMod/Resources/ThrowableActives/Item/item_toss_001.png", spriteCollection, null)
            }, "deploy", tk2dSpriteAnimationClip.WrapMode.Once, 15f);
            tk2dSpriteAnimationClip tk2dSpriteAnimationClip3 = SpriteBuilder.AddAnimation(tk2dSpriteAnimator, spriteCollection, new List<int>
            {
                SpriteBuilder.AddSpriteToCollection("RPGworldMod/Resources/ThrowableActives/Item/item_toss_001.png", spriteCollection, null),
                SpriteBuilder.AddSpriteToCollection("RPGworldMod/Resources/ThrowableActives/Item/item_toss_002.png", spriteCollection, null),
                SpriteBuilder.AddSpriteToCollection("RPGworldMod/Resources/ThrowableActives/Item/item_toss_003.png", spriteCollection, null),
                SpriteBuilder.AddSpriteToCollection("RPGworldMod/Resources/ThrowableActives/Item/item_toss_004.png", spriteCollection, null),
                SpriteBuilder.AddSpriteToCollection("RPGworldMod/Resources/ThrowableActives/Item/item_toss_005.png", spriteCollection, null),
                SpriteBuilder.AddSpriteToCollection("RPGworldMod/Resources/ThrowableActives/Item/item_toss_006.png", spriteCollection, null)
            }, "toss", tk2dSpriteAnimationClip.WrapMode.LoopSection, 15f);
            tk2dSpriteAnimationClip tk2dSpriteAnimationClip4 = SpriteBuilder.AddAnimation(tk2dSpriteAnimator, spriteCollection, new List<int>
            {
                SpriteBuilder.AddSpriteToCollection("RPGworldMod/Resources/ThrowableActives/Item/item_vanish_001.png", spriteCollection, null),
                SpriteBuilder.AddSpriteToCollection("RPGworldMod/Resources/ThrowableActives/Item/item_vanish_002.png", spriteCollection, null),
                SpriteBuilder.AddSpriteToCollection("RPGworldMod/Resources/ThrowableActives/Item/item_vanish_003.png", spriteCollection, null),
                SpriteBuilder.AddSpriteToCollection("RPGworldMod/Resources/ThrowableActives/Item/item_vanish_004.png", spriteCollection, null),
                SpriteBuilder.AddSpriteToCollection("RPGworldMod/Resources/ThrowableActives/Item/item_vanish_005.png", spriteCollection, null),
                SpriteBuilder.AddSpriteToCollection("RPGworldMod/Resources/ThrowableActives/Item/item_vanish_006.png", spriteCollection, null)
            }, "vanish", tk2dSpriteAnimationClip.WrapMode.LoopSection, 15f);
            tk2dSpriteAnimationClip3.fps = 12f;
            tk2dSpriteAnimationClip3.loopStart = 0;
            CustomThrowableObject customThrowableObject = new CustomThrowableObject
            {
                doEffectOnHitGround = true,
                OnThrownAnimation = "deploy",
                OnHitGroundAnimation = "toss",
                DefaultAnim = "toss",
                destroyOnHitGround = false,
                thrownSoundEffect = "Play_OBJ_item_throw_01",
                effectSoundEffect = "Play_CHR_fall_land_01"
            };
            SpriteBuilder.AddComponent<CustomThrowableObject>(gameObject, customThrowableObject);
            return gameObject;
        }

    }
}
