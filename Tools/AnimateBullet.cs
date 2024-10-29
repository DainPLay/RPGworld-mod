using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RPGworldMod
{

    public static class AnimateBullet
    {
        // Token: 0x0600015B RID: 347 RVA: 0x0000DDD0 File Offset: 0x0000BFD0
        public static List<T> ConstructListOfSameValues<T>(T value, int length)
        {
            List<T> list = new List<T>();
            for (int i = 0; i < length; i++)
            {
                list.Add(value);
            }
            return list;
        }

        // Token: 0x0600015C RID: 348 RVA: 0x0000DE04 File Offset: 0x0000C004
        public static void AnimateProjectile(this Projectile proj, List<string> names, int fps, bool loops, List<IntVector2> pixelSizes, List<bool> lighteneds, List<tk2dBaseSprite.Anchor> anchors, List<bool> anchorsChangeColliders, List<bool> fixesScales, List<Vector3?> manualOffsets, List<IntVector2?> overrideColliderPixelSizes, List<IntVector2?> overrideColliderOffsets, List<Projectile> overrideProjectilesToCopyFrom)
        {
            tk2dSpriteAnimationClip tk2dSpriteAnimationClip = new tk2dSpriteAnimationClip();
            tk2dSpriteAnimationClip.name = "idle";
            tk2dSpriteAnimationClip.fps = (float)fps;
            List<tk2dSpriteAnimationFrame> list = new List<tk2dSpriteAnimationFrame>();
            for (int i = 0; i < names.Count; i++)
            {
                string name = names[i];
                IntVector2 intVector = pixelSizes[i];
                IntVector2? intVector2 = overrideColliderPixelSizes[i];
                IntVector2? intVector3 = overrideColliderOffsets[i];
                Vector3? vector = manualOffsets[i];
                bool changesCollider = anchorsChangeColliders[i];
                bool fixesScale = fixesScales[i];
                bool flag = vector == null;
                if (flag)
                {
                    vector = new Vector3?(Vector2.zero);
                }
                tk2dBaseSprite.Anchor anchor = anchors[i];
                bool lightened = lighteneds[i];
                Projectile overrideProjectileToCopyFrom = overrideProjectilesToCopyFrom[i];
                tk2dSpriteAnimationFrame tk2dSpriteAnimationFrame = new tk2dSpriteAnimationFrame();
                tk2dSpriteAnimationFrame.spriteId = ETGMod.Databases.Items.ProjectileCollection.inst.GetSpriteIdByName(name);
                tk2dSpriteAnimationFrame.spriteCollection = ETGMod.Databases.Items.ProjectileCollection;
                list.Add(tk2dSpriteAnimationFrame);
                int? overrideColliderPixelWidth = null;
                int? overrideColliderPixelHeight = null;
                bool flag2 = intVector2 != null;
                if (flag2)
                {
                    overrideColliderPixelWidth = new int?(intVector2.Value.x);
                    overrideColliderPixelHeight = new int?(intVector2.Value.y);
                }
                int? overrideColliderOffsetX = null;
                int? overrideColliderOffsetY = null;
                bool flag3 = intVector3 != null;
                if (flag3)
                {
                    overrideColliderOffsetX = new int?(intVector3.Value.x);
                    overrideColliderOffsetY = new int?(intVector3.Value.y);
                }
                tk2dSpriteDefinition tk2dSpriteDefinition = GunTools.SetupDefinitionForProjectileSprite(name, tk2dSpriteAnimationFrame.spriteId, intVector.x, intVector.y, lightened, overrideColliderPixelWidth, overrideColliderPixelHeight, overrideColliderOffsetX, overrideColliderOffsetY, overrideProjectileToCopyFrom);
                tk2dSpriteDefinition.ConstructOffsetsFromAnchor(anchor, new Vector2?(tk2dSpriteDefinition.position3), fixesScale, changesCollider);
                tk2dSpriteDefinition.position0 += vector.Value;
                tk2dSpriteDefinition.position1 += vector.Value;
                tk2dSpriteDefinition.position2 += vector.Value;
                tk2dSpriteDefinition.position3 += vector.Value;
                bool flag4 = i == 0;
                if (flag4)
                {
                    proj.GetAnySprite().SetSprite(tk2dSpriteAnimationFrame.spriteCollection, tk2dSpriteAnimationFrame.spriteId);
                }
            }
            tk2dSpriteAnimationClip.wrapMode = (loops ? tk2dSpriteAnimationClip.WrapMode.Loop : tk2dSpriteAnimationClip.WrapMode.Once);
            tk2dSpriteAnimationClip.frames = list.ToArray();
            bool flag5 = proj.sprite.spriteAnimator == null;
            if (flag5)
            {
                proj.sprite.spriteAnimator = proj.sprite.gameObject.AddComponent<tk2dSpriteAnimator>();
            }
            proj.sprite.spriteAnimator.playAutomatically = true;
            bool flag6 = proj.sprite.spriteAnimator.Library == null;
            bool flag7 = flag6;
            if (flag7)
            {
                proj.sprite.spriteAnimator.Library = proj.sprite.spriteAnimator.gameObject.AddComponent<tk2dSpriteAnimation>();
                proj.sprite.spriteAnimator.Library.clips = new tk2dSpriteAnimationClip[0];
                proj.sprite.spriteAnimator.Library.enabled = true;
            }
            proj.sprite.spriteAnimator.Library.clips = proj.sprite.spriteAnimator.Library.clips.Concat(new tk2dSpriteAnimationClip[]
            {
                tk2dSpriteAnimationClip
            }).ToArray<tk2dSpriteAnimationClip>();
            proj.sprite.spriteAnimator.DefaultClipId = proj.sprite.spriteAnimator.Library.GetClipIdByName("idle");
            proj.sprite.spriteAnimator.deferNextStartClip = false;
        }
    }
}
