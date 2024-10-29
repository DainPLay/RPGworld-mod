using Alexandria.ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RPGworldMod
{
    // Token: 0x02000293 RID: 659
    internal class VFXToolbox
    {
        // Token: 0x17000019 RID: 25
        // (get) Token: 0x06000C72 RID: 3186 RVA: 0x0009CFC8 File Offset: 0x0009B1C8
        public static tk2dSpriteCollectionData VFXCollection
        {
            get
            {
                return VFXToolbox.PrivateVFXCollection;
            }
        }
        public static GameObject CreateVFX(string name, List<string> spritePaths, int fps, IntVector2 Dimensions, tk2dBaseSprite.Anchor anchor, bool usesZHeight, float zHeightOffset, float emissivePower = -1f, Color? emissiveColour = null, tk2dSpriteAnimationClip.WrapMode wrap = tk2dSpriteAnimationClip.WrapMode.Once, bool persist = false, int loopStart = 0)
        {
            GameObject gameObject = new GameObject(name);
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
            tk2dSpriteAnimationClip.wrapMode = wrap;
            tk2dSpriteAnimationClip.loopStart = loopStart;
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
            vfxobject.persistsOnDeath = false;
            vfxobject.usesZHeight = usesZHeight;
            vfxobject.zHeight = zHeightOffset;
            vfxobject.alignment = VFXAlignment.NormalAligned;
            vfxobject.destructible = false;
            vfxobject.effect = gameObject;
            return gameObject;
        }
        // Token: 0x06000C83 RID: 3203 RVA: 0x0009F104 File Offset: 0x0009D304
        public static VFXPool CreateVFXPool(string name, List<string> spritePaths, int fps, IntVector2 Dimensions, tk2dBaseSprite.Anchor anchor, bool usesZHeight, float zHeightOffset, bool persist = false, VFXAlignment alignment = VFXAlignment.NormalAligned, float emissivePower = -1f, Color? emissiveColour = null, tk2dSpriteAnimationClip.WrapMode wrapmode = tk2dSpriteAnimationClip.WrapMode.Once, int loopStart = 0)
        {
            GameObject gameObject = new GameObject(name);
            VFXPool vfxpool = new VFXPool();
            vfxpool.type = VFXPoolType.All;
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
            tk2dSpriteAnimationClip.wrapMode = wrapmode;
            tk2dSpriteAnimationClip.loopStart = loopStart;
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
            vfxpool.effects = new VFXComplex[]
            {
                vfxcomplex
            };
            return vfxpool;
        }

        // Token: 0x06000C84 RID: 3204 RVA: 0x0009F50C File Offset: 0x0009D70C
        public static GameObject CreateCustomClip(string spriteName, int pixelWidth, int pixelHeight)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>((PickupObjectDatabase.GetById(95) as Gun).clipObject);
            gameObject.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(gameObject.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            gameObject.GetComponent<tk2dBaseSprite>().spriteId = VFXToolbox.VFXCollection.inst.GetSpriteIdByName(spriteName);
            VFXToolbox.SetupDefinitionForClipSprite(spriteName, gameObject.GetComponent<tk2dBaseSprite>().spriteId, pixelWidth, pixelHeight);
            return gameObject;
        }

        // Token: 0x06000C85 RID: 3205 RVA: 0x0009F588 File Offset: 0x0009D788
        public static void SetupDefinitionForClipSprite(string name, int id, int pixelWidth, int pixelHeight)
        {
            float num = 14f;
            float num2 = (float)pixelWidth / num;
            float num3 = (float)pixelHeight / num;
            tk2dSpriteDefinition tk2dSpriteDefinition = GunTools.CopyDefinitionFrom(VFXToolbox.VFXCollection.inst.spriteDefinitions[(PickupObjectDatabase.GetById(47) as Gun).clipObject.GetComponent<tk2dBaseSprite>().spriteId]);
            tk2dSpriteDefinition.boundsDataCenter = new Vector3(num2 / 2f, num3 / 2f, 0f);
            tk2dSpriteDefinition.boundsDataExtents = new Vector3(num2, num3, 0f);
            tk2dSpriteDefinition.untrimmedBoundsDataCenter = new Vector3(num2 / 2f, num3 / 2f, 0f);
            tk2dSpriteDefinition.untrimmedBoundsDataExtents = new Vector3(num2, num3, 0f);
            tk2dSpriteDefinition.position0 = new Vector3(0f, 0f, 0f);
            tk2dSpriteDefinition.position1 = new Vector3(0f + num2, 0f, 0f);
            tk2dSpriteDefinition.position2 = new Vector3(0f, 0f + num3, 0f);
            tk2dSpriteDefinition.position3 = new Vector3(0f + num2, 0f + num3, 0f);
            tk2dSpriteDefinition.colliderVertices[1].x = num2;
            tk2dSpriteDefinition.colliderVertices[1].y = num3;
            tk2dSpriteDefinition.name = name;
            VFXToolbox.VFXCollection.spriteDefinitions[id] = tk2dSpriteDefinition;
        }

        // Token: 0x06000C86 RID: 3206 RVA: 0x0009F6E0 File Offset: 0x0009D8E0
        public static GameObject CreateCustomShellCasing(string spriteName, int pixelWidth, int pixelHeight)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>((PickupObjectDatabase.GetById(202) as Gun).shellCasing);
            gameObject.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(gameObject.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            gameObject.GetComponent<tk2dBaseSprite>().spriteId = VFXToolbox.VFXCollection.inst.GetSpriteIdByName(spriteName);
            VFXToolbox.SetupDefinitionForShellSprite(spriteName, gameObject.GetComponent<tk2dBaseSprite>().spriteId, pixelWidth, pixelHeight, null);
            return gameObject;
        }

        // Token: 0x06000C87 RID: 3207 RVA: 0x0009F760 File Offset: 0x0009D960
        public static tk2dSpriteDefinition SetupDefinitionForShellSprite(string name, int id, int pixelWidth, int pixelHeight, tk2dSpriteDefinition overrideToCopyFrom = null)
        {
            float num = 14f;
            float num2 = (float)pixelWidth / num;
            float num3 = (float)pixelHeight / num;
            tk2dSpriteDefinition tk2dSpriteDefinition = overrideToCopyFrom ?? GunTools.CopyDefinitionFrom(VFXToolbox.VFXCollection.inst.spriteDefinitions[(PickupObjectDatabase.GetById(202) as Gun).shellCasing.GetComponent<tk2dBaseSprite>().spriteId]);
            tk2dSpriteDefinition.boundsDataCenter = new Vector3(num2 / 2f, num3 / 2f, 0f);
            tk2dSpriteDefinition.boundsDataExtents = new Vector3(num2, num3, 0f);
            tk2dSpriteDefinition.untrimmedBoundsDataCenter = new Vector3(num2 / 2f, num3 / 2f, 0f);
            tk2dSpriteDefinition.untrimmedBoundsDataExtents = new Vector3(num2, num3, 0f);
            tk2dSpriteDefinition.position0 = new Vector3(0f, 0f, 0f);
            tk2dSpriteDefinition.position1 = new Vector3(0f + num2, 0f, 0f);
            tk2dSpriteDefinition.position2 = new Vector3(0f, 0f + num3, 0f);
            tk2dSpriteDefinition.position3 = new Vector3(0f + num2, 0f + num3, 0f);
            tk2dSpriteDefinition.name = name;
            VFXToolbox.VFXCollection.spriteDefinitions[id] = tk2dSpriteDefinition;
            return tk2dSpriteDefinition;
        }

        // Token: 0x04000842 RID: 2114
        private static GameObject VFXScapeGoat;

        // Token: 0x04000843 RID: 2115
        private static tk2dSpriteCollectionData PrivateVFXCollection;

        // Token: 0x04000844 RID: 2116
        public static GameObject laserSightPrefab;
    }
}
