using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Dungeonator;
using UnityEngine;

namespace RPGworldMod
{
    // Token: 0x0200006B RID: 107
    public class TeleportPlayerToCursorPosition : MonoBehaviour
    {
        // Token: 0x06000396 RID: 918 RVA: 0x0001C918 File Offset: 0x0001AB18
        public static void StartTeleport(PlayerController user, Vector2 newPosition)
        {
            user.healthHaver.TriggerInvulnerabilityPeriod(0.001f);
            user.DidUnstealthyAction();
            Vector2 targetPoint = BraveMathCollege.ClampToBounds(newPosition, GameManager.Instance.MainCameraController.MinVisiblePoint, GameManager.Instance.MainCameraController.MaxVisiblePoint);
            TeleportPlayerToCursorPosition.BlinkToPoint(user, targetPoint);
        }

        // Token: 0x06000397 RID: 919 RVA: 0x0001C968 File Offset: 0x0001AB68
        private static void BlinkToPoint(PlayerController Owner, Vector2 targetPoint)
        {
            TeleportPlayerToCursorPosition.lockedDodgeRollDirection = (targetPoint - Owner.specRigidbody.UnitCenter).normalized;
            Vector2 vector = Owner.transform.position;
            int num = (int)targetPoint.x;
            int num2 = (int)targetPoint.y;
            int num3 = (int)vector.x;
            int num4 = (int)vector.y;
            int num5 = Math.Abs(num3 - num);
            int num6 = (num < num3) ? 1 : -1;
            int num7 = -Math.Abs(num4 - num2);
            int num8 = (num2 < num4) ? 1 : -1;
            int num9 = num5 + num7;
            int num10 = 600;
            while (num10 > 0 && (num != num3 || num2 != num4))
            {
                if (TeleportPlayerToCursorPosition.CanBlinkToPoint(new Vector2((float)num, (float)num2), Owner))
                {
                    StaticCoroutine.Start(TeleportPlayerToCursorPosition.HandleBlinkTeleport(Owner, new Vector2((float)num, (float)num2), TeleportPlayerToCursorPosition.lockedDodgeRollDirection));
                    return;
                }
                int num11 = 2 * num9;
                if (num11 >= num7)
                {
                    num9 += num7;
                    num += num6;
                }
                if (num11 <= num5)
                {
                    num9 += num5;
                    num2 += num8;
                }
                num10--;
            }
        }

        // Token: 0x06000398 RID: 920 RVA: 0x0001CA6C File Offset: 0x0001AC6C
        private static bool CanBlinkToPoint(Vector2 point, PlayerController owner)
        {
            RoomHandler currentRoom = owner.CurrentRoom;
            bool flag = owner.IsValidPlayerPosition(point);
            if (flag && currentRoom != null)
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
                if (currentRoom.IsSealed && nearestRoom != currentRoom)
                {
                    flag = false;
                }
                if (currentRoom.IsSealed && cellData.isExitCell)
                {
                    flag = false;
                }
                if (nearestRoom.visibility == RoomHandler.VisibilityStatus.OBSCURED || nearestRoom.visibility == RoomHandler.VisibilityStatus.REOBSCURED)
                {
                    flag = false;
                }
            }
            if (currentRoom == null)
            {
                flag = false;
            }
            return flag;
        }

        // Token: 0x06000399 RID: 921 RVA: 0x0001CAFA File Offset: 0x0001ACFA
        private static IEnumerator HandleBlinkTeleport(PlayerController Owner, Vector2 targetPoint, Vector2 targetDirection)
        {
            AkSoundEngine.PostEvent("Play_ENM_wizardred_vanish_01", Owner.gameObject);
            List<AIActor> list = Stuff.ReflectGetField<List<AIActor>>(typeof(PlayerController), "m_rollDamagedEnemies", Owner);
            if (list != null)
            {
                list.Clear();
                FieldInfo field = typeof(PlayerController).GetField("m_rollDamagedEnemies", BindingFlags.Instance | BindingFlags.NonPublic);
                field.SetValue(Owner, list);
            }
            if (Owner.knockbackDoer)
            {
                Owner.knockbackDoer.ClearContinuousKnockbacks();
            }
            Owner.IsEthereal = true;
            Owner.IsVisible = false;
            float RecoverySpeed = GameManager.Instance.MainCameraController.OverrideRecoverySpeed;
            bool IsLerping = GameManager.Instance.MainCameraController.IsLerping;
            yield return new WaitForSeconds(0.1f);
            GameManager.Instance.MainCameraController.OverrideRecoverySpeed = 80f;
            GameManager.Instance.MainCameraController.IsLerping = true;
            if (Owner.IsPrimaryPlayer)
            {
                GameManager.Instance.MainCameraController.UseOverridePlayerOnePosition = true;
                GameManager.Instance.MainCameraController.OverridePlayerOnePosition = targetPoint;
                yield return new WaitForSeconds(0.12f);
                Owner.specRigidbody.Velocity = Vector2.zero;
                Owner.specRigidbody.Position = new Position(targetPoint);
                GameManager.Instance.MainCameraController.UseOverridePlayerOnePosition = false;
            }
            else
            {
                GameManager.Instance.MainCameraController.UseOverridePlayerTwoPosition = true;
                GameManager.Instance.MainCameraController.OverridePlayerTwoPosition = targetPoint;
                yield return new WaitForSeconds(0.12f);
                Owner.specRigidbody.Velocity = Vector2.zero;
                Owner.specRigidbody.Position = new Position(targetPoint);
                GameManager.Instance.MainCameraController.UseOverridePlayerTwoPosition = false;
            }
            GameManager.Instance.MainCameraController.OverrideRecoverySpeed = RecoverySpeed;
            GameManager.Instance.MainCameraController.IsLerping = IsLerping;
            Owner.IsEthereal = false;
            Owner.IsVisible = true;
            if (Owner.CurrentFireMeterValue <= 0f)
            {
                yield break;
            }
            Owner.CurrentFireMeterValue = Mathf.Max(0f, Owner.CurrentFireMeterValue -= 0.5f);
            if (Owner.CurrentFireMeterValue == 0f)
            {
                Owner.IsOnFire = false;
                yield break;
            }
            yield break;
        }

        // Token: 0x0600039A RID: 922 RVA: 0x0001CB10 File Offset: 0x0001AD10
        public static void CorrectForWalls(PlayerController portal)
        {
            bool flag = PhysicsEngine.Instance.OverlapCast(portal.specRigidbody, null, true, false, null, null, false, null, null, new SpeculativeRigidbody[0]);
            if (flag)
            {
                Vector2 a = portal.transform.position.XY();
                IntVector2[] cardinalsAndOrdinals = IntVector2.CardinalsAndOrdinals;
                int num = 0;
                int num2 = 1;
                for (; ; )
                {
                    for (int i = 0; i < cardinalsAndOrdinals.Length; i++)
                    {
                        portal.specRigidbody.Position = new Position(a + PhysicsEngine.PixelToUnit(cardinalsAndOrdinals[i] * num2));
                        portal.specRigidbody.Reinitialize();
                        if (!PhysicsEngine.Instance.OverlapCast(portal.specRigidbody, null, true, false, null, null, false, null, null, new SpeculativeRigidbody[0]))
                        {
                            return;
                        }
                    }
                    num2++;
                    num++;
                    if (num > 200)
                    {
                        goto Block_4;
                    }
                }
                return;
            Block_4:
                Debug.LogError("FREEZE AVERTED!  TELL RUBEL!  (you're welcome) 147");
                return;
            }
        }

        // Token: 0x04000182 RID: 386
        private static Vector2 lockedDodgeRollDirection;

        // Token: 0x04000183 RID: 387
        public static BlinkPassiveItem m_BlinkPassive = PickupObjectDatabase.GetById(436).GetComponent<BlinkPassiveItem>();

        // Token: 0x04000184 RID: 388
        public GameObject BlinkpoofVfx = TeleportPlayerToCursorPosition.m_BlinkPassive.BlinkpoofVfx;

        // Token: 0x04000185 RID: 389
        public static GameObject BloodiedScarfPoofVFX = PickupObjectDatabase.GetById(436).GetComponent<BlinkPassiveItem>().BlinkpoofVfx.gameObject;
    }
}
