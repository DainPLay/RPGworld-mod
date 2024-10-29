using System;
using System.Collections;
using System.Reflection;
using Dungeonator;
using UnityEngine;

namespace RPGworldMod
{
    // Token: 0x0200006A RID: 106
    public static class Stuff
    {
        // Token: 0x0600038D RID: 909 RVA: 0x0001C536 File Offset: 0x0001A736
        public static void Init()
        {
        }

        // Token: 0x0600038E RID: 910 RVA: 0x0001C538 File Offset: 0x0001A738
        public static bool PlayerHasActiveSynergy(this PlayerController player, string synergyNameToCheck)
        {
            foreach (int num in player.ActiveExtraSynergies)
            {
                AdvancedSynergyEntry advancedSynergyEntry = GameManager.Instance.SynergyManager.synergies[num];
                bool flag = advancedSynergyEntry.NameKey == synergyNameToCheck;
                if (flag)
                {
                    return true;
                }
            }
            return false;
        }

        // Token: 0x0600038F RID: 911 RVA: 0x0001C5B0 File Offset: 0x0001A7B0
        public static T ReflectGetField<T>(Type classType, string fieldName, object o = null)
        {
            FieldInfo field = classType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | ((o != null) ? BindingFlags.Instance : BindingFlags.Static));
            return (T)((object)field.GetValue(o));
        }

        // Token: 0x06000390 RID: 912 RVA: 0x0001C5DC File Offset: 0x0001A7DC
        public static BeamController FreeFireBeamFromAnywhere(Projectile projectileToSpawn, PlayerController owner, GameObject otherShooter, Vector2 fixedPosition, bool usesFixedPosition, float targetAngle, float duration, bool skipChargeTime = false)
        {
            Vector2 vector;
            if (usesFixedPosition)
            {
                vector = fixedPosition;
            }
            else
            {
                vector = otherShooter.GetComponent<SpeculativeRigidbody>().UnitCenter;
            }
            GameObject gameObject = SpawnManager.SpawnProjectile(projectileToSpawn.gameObject, vector, Quaternion.identity, true);
            Projectile component = gameObject.GetComponent<Projectile>();
            component.Owner = owner;
            BeamController component2 = gameObject.GetComponent<BeamController>();
            if (skipChargeTime)
            {
                component2.chargeDelay = 0f;
                component2.usesChargeDelay = false;
            }
            component2.Owner = owner;
            component2.HitsPlayers = false;
            component2.HitsEnemies = true;
            Vector3 v = BraveMathCollege.DegreesToVector(targetAngle, 1f);
            component2.Direction = v;
            component2.Origin = vector;
            GameManager.Instance.Dungeon.StartCoroutine(Stuff.HandleFreeFiringBeam(component2, otherShooter, fixedPosition, usesFixedPosition, targetAngle, duration));
            return component2;
        }

        // Token: 0x06000391 RID: 913 RVA: 0x0001C69B File Offset: 0x0001A89B
        public static IEnumerator HandleFreeFiringBeam(BeamController beam, GameObject otherShooter, Vector2 fixedPosition, bool usesFixedPosition, float targetAngle, float duration)
        {
            float elapsed = 0f;
            yield return null;
            while (elapsed < duration && !(otherShooter == null) && !(otherShooter.GetComponent<SpeculativeRigidbody>() == null))
            {
                Vector2 vector;
                if (usesFixedPosition)
                {
                    vector = fixedPosition;
                }
                else
                {
                    vector = otherShooter.GetComponent<SpeculativeRigidbody>().UnitCenter;
                }
                elapsed += BraveTime.DeltaTime;
                beam.Origin = vector;
                beam.LateUpdatePosition(vector);
                yield return null;
            }
            beam.CeaseAttack();
            yield break;
        }

        // Token: 0x06000392 RID: 914 RVA: 0x0001C6C8 File Offset: 0x0001A8C8
        public static bool CanBlinkToPoint(PlayerController Owner, Vector2 point)
        {
            bool flag = Owner.IsValidPlayerPosition(point);
            if (flag && Owner.CurrentRoom != null)
            {
                CellData cellData = GameManager.Instance.Dungeon.data[point.ToIntVector2(VectorConversions.Floor)];
                if (cellData == null)
                {
                    return false;
                }
                RoomHandler nearestRoom = cellData.nearestRoom;
                if (cellData.type != CellType.FLOOR)
                {
                    flag = false;
                }
                if (Owner.CurrentRoom.IsSealed && nearestRoom != Owner.CurrentRoom)
                {
                    flag = false;
                }
                if (Owner.CurrentRoom.IsSealed && cellData.isExitCell)
                {
                    flag = false;
                }
                if (nearestRoom.visibility == RoomHandler.VisibilityStatus.OBSCURED || nearestRoom.visibility == RoomHandler.VisibilityStatus.REOBSCURED)
                {
                    flag = false;
                }
            }
            if (Owner.CurrentRoom == null)
            {
                flag = false;
            }
            return !(Owner.IsDodgeRolling | Owner.IsFalling | Owner.IsCurrentlyCoopReviving | Owner.IsInMinecart | Owner.IsInputOverridden) && flag;
        }

        // Token: 0x06000393 RID: 915 RVA: 0x0001C790 File Offset: 0x0001A990
        public static bool PositionIsInBounds(PlayerController Owner, Vector2 point)
        {
            bool flag = true;
            if (Owner.CurrentRoom != null)
            {
                CellData cellData = GameManager.Instance.Dungeon.data[point.ToIntVector2(VectorConversions.Floor)];
                if (cellData == null)
                {
                    return false;
                }
                RoomHandler nearestRoom = cellData.nearestRoom;
                if (cellData.type != CellType.FLOOR)
                {
                    flag = false;
                }
                if (Owner.CurrentRoom.IsSealed && nearestRoom != Owner.CurrentRoom)
                {
                    flag = false;
                }
                if (Owner.CurrentRoom.IsSealed && cellData.isExitCell)
                {
                    flag = false;
                }
                if (nearestRoom.visibility == RoomHandler.VisibilityStatus.OBSCURED || nearestRoom.visibility == RoomHandler.VisibilityStatus.REOBSCURED)
                {
                    flag = false;
                }
            }
            if (Owner.CurrentRoom == null)
            {
                flag = false;
            }
            return !(Owner.IsDodgeRolling | Owner.IsFalling | Owner.IsCurrentlyCoopReviving | Owner.IsInMinecart | Owner.IsInputOverridden) && flag;
        }

        // Token: 0x06000394 RID: 916 RVA: 0x0001C850 File Offset: 0x0001AA50
        public static Vector2 AdjustInputVector(Vector2 rawInput, float cardinalMagnetAngle, float ordinalMagnetAngle)
        {
            float num = BraveMathCollege.ClampAngle360(BraveMathCollege.Atan2Degrees(rawInput));
            float num2 = num % 90f;
            float num3 = (num + 45f) % 90f;
            float num4 = 0f;
            if (cardinalMagnetAngle > 0f)
            {
                if (num2 < cardinalMagnetAngle)
                {
                    num4 = -num2;
                }
                else if (num2 > 90f - cardinalMagnetAngle)
                {
                    num4 = 90f - num2;
                }
            }
            if (ordinalMagnetAngle > 0f)
            {
                if (num3 < ordinalMagnetAngle)
                {
                    num4 = -num3;
                }
                else if (num3 > 90f - ordinalMagnetAngle)
                {
                    num4 = 90f - num3;
                }
            }
            num += num4;
            return (Quaternion.Euler(0f, 0f, num) * Vector3.right).XY() * rawInput.magnitude;
        }

        // Token: 0x04000181 RID: 385
        public static GameObject WinchesterTargetHitVFX = ResourceManager.LoadAssetBundle("shared_auto_001").LoadAsset<GameObject>("VFX_Explosion_Firework");
    }
}
