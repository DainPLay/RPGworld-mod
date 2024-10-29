using System;
using System.Collections.Generic;
using System.Linq;
using Alexandria.ItemAPI;
using UnityEngine;

namespace RPGworldMod
{
    // Token: 0x020000B7 RID: 183
    internal class MiscToolMethods
    {
        // Token: 0x060005F4 RID: 1524 RVA: 0x00036BFC File Offset: 0x00034DFC
        public static Projectile SpawnProjAtPosi(Projectile proj, Vector2 posi, PlayerController player, Gun gun, float var = 0f, float speedmult = 1f, bool postprocess = true)
        {
            GameObject gameObject = SpawnManager.SpawnProjectile(proj.gameObject, posi, Quaternion.Euler(0f, 0f, ((gun.CurrentOwner as PlayerController).CurrentGun == null) ? 0f : ((gun.CurrentOwner as PlayerController).CurrentGun.CurrentAngle + var)), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            bool flag = component != null;
            bool flag2 = flag;
            if (flag2)
            {
                component.Owner = player;
                component.Shooter = player.specRigidbody;
                component.baseData.speed *= speedmult;
                component.UpdateSpeed();
                if (postprocess)
                {
                    player.DoPostProcessProjectile(proj);
                }
            }
            return component;
        }

        // Token: 0x060005F5 RID: 1525 RVA: 0x00036CC4 File Offset: 0x00034EC4
        public static Projectile SpawnProjAtPosi(Projectile proj, Vector2 posi, PlayerController player, float Angle = 0f, float var = 0f, float speedmult = 1f, bool postprocess = true)
        {
            GameObject gameObject = SpawnManager.SpawnProjectile(proj.gameObject, posi, Quaternion.Euler(0f, 0f, Angle + var), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            bool flag = component != null;
            bool flag2 = flag;
            if (flag2)
            {
                component.Owner = player;
                component.Shooter = player.specRigidbody;
                component.baseData.speed *= speedmult;
                component.UpdateSpeed();
                if (postprocess)
                {
                    player.DoPostProcessProjectile(proj);
                }
            }
            return component;
        }

        // Token: 0x060005F7 RID: 1527 RVA: 0x00036D64 File Offset: 0x00034F64
        public static List<T> RandomNoRepeats<T>(List<T> candidates, int count)
        {
            List<T> list = new List<T>();
            int num = 0;
            do
            {
                num++;
                int index = UnityEngine.Random.Range(0, candidates.Count);
                bool flag = !list.Contains(candidates[index]);
                if (flag)
                {
                    list.Add(candidates[index]);
                }
            }
            while (num < count * 3 && list.Count < count);
            return list;
        }

        // Token: 0x060005F8 RID: 1528 RVA: 0x00036DD0 File Offset: 0x00034FD0
        public static float getPlayerDepth()
        {
            float result = 1f;
            bool flag = GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON;
            if (flag)
            {
                result = 1f;
            }
            bool flag2 = GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.SEWERGEON || GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.JUNGLEGEON;
            if (flag2)
            {
                result = 1.5f;
            }
            bool flag3 = GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.GUNGEON;
            if (flag3)
            {
                result = 2f;
            }
            bool flag4 = GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CATHEDRALGEON || GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.BELLYGEON;
            if (flag4)
            {
                result = 2.5f;
            }
            bool flag5 = GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.MINEGEON;
            if (flag5)
            {
                result = 3f;
            }
            bool flag6 = GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.RATGEON;
            if (flag6)
            {
                result = 3.5f;
            }
            bool flag7 = GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CATACOMBGEON;
            if (flag7)
            {
                result = 4f;
            }
            bool flag8 = GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.OFFICEGEON || GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.WESTGEON;
            if (flag8)
            {
                result = 4.5f;
            }
            bool flag9 = GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON;
            if (flag9)
            {
                result = 5f;
            }
            bool flag10 = GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FINALGEON || GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON;
            if (flag10)
            {
                result = 5.5f;
            }
            return result;
        }

        // Token: 0x060005F9 RID: 1529 RVA: 0x00036FDE File Offset: 0x000351DE
        // Token: 0x060005FA RID: 1530 RVA: 0x00036FF0 File Offset: 0x000351F0
        public static VFXComplex CreateVFXComplex(string name, List<string> spritePaths, int fps, IntVector2 Dimensions, tk2dBaseSprite.Anchor anchor, bool usesZHeight, float zHeightOffset, bool persist = false, VFXAlignment alignment = VFXAlignment.NormalAligned, float emissivePower = -1f, Color? emissiveColour = null, VFXPoolType type = VFXPoolType.All)
        {
            GameObject gameObject = new GameObject(name);
            VFXComplex vfxcomplex = new VFXComplex();
            VFXObject vfxobject = new VFXObject();
            gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(gameObject);
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            tk2dSpriteCollectionData tk2dSpriteCollectionData = SpriteBuilder.ConstructCollection(gameObject, name + "_Collection", false);
            int newSpriteId = SpriteBuilder.AddSpriteToCollection(spritePaths[0], tk2dSpriteCollectionData, null);
            tk2dSprite orAddComponent = gameObject.GetOrAddComponent<tk2dSprite>();
            orAddComponent.SetSprite(tk2dSpriteCollectionData, newSpriteId);
            tk2dSpriteDefinition currentSpriteDef = orAddComponent.GetCurrentSpriteDef();
            currentSpriteDef.colliderVertices = new Vector3[]
            {
                new Vector3(0f, 0f, 0f),
                new Vector3((float)(Dimensions.x / 16), (float)(Dimensions.y / 16), 0f)
            };
            tk2dSpriteAnimator orAddComponent2 = gameObject.GetOrAddComponent<tk2dSpriteAnimator>();
            tk2dSpriteAnimation orAddComponent3 = gameObject.GetOrAddComponent<tk2dSpriteAnimation>();
            orAddComponent3.clips = new tk2dSpriteAnimationClip[0];
            orAddComponent2.Library = orAddComponent3;
            tk2dSpriteAnimationClip tk2dSpriteAnimationClip = new tk2dSpriteAnimationClip
            {
                name = "start",
                frames = new tk2dSpriteAnimationFrame[0],
                fps = (float)fps
            };
            List<tk2dSpriteAnimationFrame> list = new List<tk2dSpriteAnimationFrame>();
            for (int i = 0; i < spritePaths.Count; i++)
            {
                tk2dSpriteCollectionData tk2dSpriteCollectionData2 = tk2dSpriteCollectionData;
                int num = SpriteBuilder.AddSpriteToCollection(spritePaths[i], tk2dSpriteCollectionData2, null);
                tk2dSpriteDefinition tk2dSpriteDefinition = tk2dSpriteCollectionData2.spriteDefinitions[num];
                GunTools.ConstructOffsetsFromAnchor(tk2dSpriteDefinition, anchor, null, false, true);
                tk2dSpriteDefinition.colliderVertices = currentSpriteDef.colliderVertices;
                bool flag = emissivePower > 0f;
                if (flag)
                {
                    tk2dSpriteDefinition.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
                }
                bool flag2 = emissivePower > 0f;
                if (flag2)
                {
                    tk2dSpriteDefinition.material.SetFloat("_EmissiveColorPower", emissivePower);
                }
                bool flag3 = emissiveColour != null;
                if (flag3)
                {
                    tk2dSpriteDefinition.material.SetColor("_EmissiveColor", emissiveColour.Value);
                }
                bool flag4 = emissivePower > 0f;
                if (flag4)
                {
                    tk2dSpriteDefinition.materialInst.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
                }
                bool flag5 = emissivePower > 0f;
                if (flag5)
                {
                    tk2dSpriteDefinition.materialInst.SetFloat("_EmissiveColorPower", emissivePower);
                }
                bool flag6 = emissiveColour != null;
                if (flag6)
                {
                    tk2dSpriteDefinition.materialInst.SetColor("_EmissiveColor", emissiveColour.Value);
                }
                list.Add(new tk2dSpriteAnimationFrame
                {
                    spriteId = num,
                    spriteCollection = tk2dSpriteCollectionData2
                });
            }
            bool flag7 = emissivePower > 0f;
            if (flag7)
            {
                orAddComponent.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
            }
            bool flag8 = emissivePower > 0f;
            if (flag8)
            {
                orAddComponent.renderer.material.SetFloat("_EmissiveColorPower", emissivePower);
            }
            bool flag9 = emissiveColour != null;
            if (flag9)
            {
                orAddComponent.renderer.material.SetColor("_EmissiveColor", emissiveColour.Value);
            }
            tk2dSpriteAnimationClip.frames = list.ToArray();
            tk2dSpriteAnimationClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            orAddComponent3.clips = orAddComponent3.clips.Concat(new tk2dSpriteAnimationClip[]
            {
                tk2dSpriteAnimationClip
            }).ToArray<tk2dSpriteAnimationClip>();
            bool flag10 = !persist;
            if (flag10)
            {
                SpriteAnimatorKiller spriteAnimatorKiller = orAddComponent2.gameObject.AddComponent<SpriteAnimatorKiller>();
                spriteAnimatorKiller.fadeTime = -1f;
                spriteAnimatorKiller.animator = orAddComponent2;
                spriteAnimatorKiller.delayDestructionTime = -1f;
            }
            orAddComponent2.playAutomatically = true;
            orAddComponent2.DefaultClipId = orAddComponent2.GetClipIdByName("start");
            vfxobject.attached = true;
            vfxobject.persistsOnDeath = persist;
            vfxobject.usesZHeight = usesZHeight;
            vfxobject.zHeight = zHeightOffset;
            vfxobject.alignment = alignment;
            vfxobject.destructible = false;
            vfxobject.effect = gameObject;
            vfxcomplex.effects = new VFXObject[]
            {
                vfxobject
            };
            return vfxcomplex;
        }

        // Token: 0x04000385 RID: 901
        public static Projectile standardproj = ((Gun)PickupObjectDatabase.GetById(15)).DefaultModule.projectiles[0].projectile;
    }
}
