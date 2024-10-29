using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace RPGworldMod
{
    // Token: 0x0200006C RID: 108
    public class StaticCoroutine : MonoBehaviour
    {
        // Token: 0x0600039D RID: 925 RVA: 0x0001CC69 File Offset: 0x0001AE69
        private void OnDestroy()
        {
            StaticCoroutine.m_instance.StopAllCoroutines();
        }

        // Token: 0x0600039E RID: 926 RVA: 0x0001CC75 File Offset: 0x0001AE75
        private void OnApplicationQuit()
        {
            StaticCoroutine.m_instance.StopAllCoroutines();
        }

        // Token: 0x0600039F RID: 927 RVA: 0x0001CC84 File Offset: 0x0001AE84
        private static StaticCoroutine Build()
        {
            if (StaticCoroutine.m_instance != null)
            {
                return StaticCoroutine.m_instance;
            }
            StaticCoroutine.m_instance = (StaticCoroutine)UnityEngine.Object.FindObjectOfType(typeof(StaticCoroutine));
            if (StaticCoroutine.m_instance != null)
            {
                return StaticCoroutine.m_instance;
            }
            GameObject gameObject = new GameObject("StaticCoroutine");
            gameObject.AddComponent<StaticCoroutine>();
            StaticCoroutine.m_instance = gameObject.GetComponent<StaticCoroutine>();
            if (StaticCoroutine.m_instance != null)
            {
                return StaticCoroutine.m_instance;
            }
            ETGModConsole.Log("STATIC COROUTINE: Build did not generate a replacement instance. Method Failed!", false);
            return null;
        }

        // Token: 0x060003A0 RID: 928 RVA: 0x0001CD0D File Offset: 0x0001AF0D
        public static void Start(string methodName)
        {
            StaticCoroutine.Build().StartCoroutine(methodName);
        }

        // Token: 0x060003A1 RID: 929 RVA: 0x0001CD1B File Offset: 0x0001AF1B
        public static void Start(string methodName, object value)
        {
            StaticCoroutine.Build().StartCoroutine(methodName, value);
        }

        // Token: 0x060003A2 RID: 930 RVA: 0x0001CD2A File Offset: 0x0001AF2A
        public static Coroutine Start(IEnumerator routine)
        {
            return StaticCoroutine.Build().StartCoroutine(routine);
        }

        // Token: 0x04000186 RID: 390
        private static StaticCoroutine m_instance;

        // Token: 0x020000C0 RID: 192
        public class EasyTrailComponent : BraveBehaviour
        {
            // Token: 0x0600055F RID: 1375 RVA: 0x00026C30 File Offset: 0x00024E30
            public EasyTrailComponent()
            {
                this.TrailPos = new Vector3(0f, 0f, 0f);
                this.BaseColor = Color.red;
                this.StartColor = Color.red;
                this.EndColor = Color.white;
                this.LifeTime = 1f;
                this.StartWidth = 1f;
                this.EndWidth = 0f;
            }

            // Token: 0x06000560 RID: 1376 RVA: 0x00026CA4 File Offset: 0x00024EA4
            public void Start()
            {
                this.obj = base.gameObject;
                GameObject gameObject = ETGMod.AddChild(this.obj, "trail object", new Type[0]);
                gameObject.transform.position = this.obj.transform.position;
                gameObject.transform.localPosition = this.TrailPos;
                TrailRenderer trailRenderer = gameObject.AddComponent<TrailRenderer>();
                trailRenderer.shadowCastingMode = ShadowCastingMode.Off;
                trailRenderer.receiveShadows = false;
                Material material = new Material(Shader.Find("Sprites/Default"));
                material.mainTexture = this._gradTexture;
                trailRenderer.material = material;
                trailRenderer.minVertexDistance = 0.1f;
                material.SetColor("_Color", this.BaseColor);
                trailRenderer.startColor = this.StartColor;
                trailRenderer.endColor = this.EndColor;
                trailRenderer.time = this.LifeTime;
                trailRenderer.startWidth = this.StartWidth;
                trailRenderer.endWidth = this.EndWidth;
                trailRenderer.autodestruct = false;
            }

            // Token: 0x040002F3 RID: 755
            public Texture _gradTexture;

            // Token: 0x040002F4 RID: 756
            private GameObject obj;

            // Token: 0x040002F5 RID: 757
            public Vector2 TrailPos;

            // Token: 0x040002F6 RID: 758
            public Color BaseColor;

            // Token: 0x040002F7 RID: 759
            public Color StartColor;

            // Token: 0x040002F8 RID: 760
            public Color EndColor;

            // Token: 0x040002F9 RID: 761
            public float LifeTime;

            // Token: 0x040002FA RID: 762
            public float StartWidth;

            // Token: 0x040002FB RID: 763
            public float EndWidth;
        }
    }
}
