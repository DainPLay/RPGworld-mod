using BepInEx;
using System;
using UnityEngine;
using SGUI;

namespace RPGworldMod
{
    [BepInDependency(Alexandria.Alexandria.GUID)] // this mod depends on the Alexandria API: https://enter-the-gungeon.thunderstore.io/package/Alexandria/Alexandria/
    [BepInDependency(ETGModMainBehaviour.GUID)]
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "dainplay.etg.rpgworld";
        public const string NAME = "RPGworld mod";
        public const string VERSION = "1.0.3";
        public const string TEXT_COLOR = "#00FFAA";

        public void Start()
        {
            ETGModMainBehaviour.WaitForGameManagerStart(GMStart);
        }

        public void GMStart(GameManager g)
        {
            Gruv_pistol.Add();
            Gruv_shotgun.Add();
            Gruv_assault_rifle.Add();
            Gruv_sniper_rifle.Add();
            Gruv_machine_gun.Add();
            Gruv_rocket_launcher.Add();
            Gruv_crown.Init();
            An_item.Init();
            InitialiseCharacters.Charactersetup();
            InitialiseSynergies.DoInitialisation();

            LogRainbow($"{NAME} v{VERSION} started successfully.");
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
    }
}
