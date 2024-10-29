using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.CharacterAPI;
using Alexandria.ItemAPI;
using SGUI;
using UnityEngine;

namespace RPGworldMod
{
    class InitialiseCharacters
    {

        public static void Charactersetup()
        {
            SetupKKoRRoLL();
            SetupVlaDoge();
        }
        public static void SetupKKoRRoLL()
        {
            GlowMatDoer glowMatDoer = new GlowMatDoer(new Color32(0, 255, 0, 255), 3f, 3f, 0.2f);
            var KKoRRoLL = Loader.BuildCharacter("RPGworldMod/Characters/KKoRRoLL",
                   Plugin.GUID,
                    new Vector3(31f, 27f, 27.1f),
                     false,
                     new Vector3(21.8f, 29.5f, 25.3f),
                     true,
                     false,
                     false,
                     true, //Sprites used by paradox
                     true, //Glows
                     glowMatDoer, //Glow Mat
                     glowMatDoer, //Alt Skin Glow Mat
                     0, //Hegemony Cost
                     false, //HasPast
                     ""); //Past ID String
            var identity = ETGModCompatibility.ExtendEnum<PlayableCharacters>(Plugin.GUID, "kkorroll");
            ETGMod.Databases.Strings.Core.SetComplex("#PLAYER_NICK_" + identity.ToString().ToUpperInvariant(), "Green eyed", "King", "Smokey", "Shadow");
            ETGMod.Databases.Strings.Core.SetComplex(StringTableManager.GungeonSupportedLanguages.RUSSIAN, "#PLAYER_NICK_" + identity.ToString().ToUpperInvariant(), "Зеленоглазка", "Король", "Дымка", "Тень");

            var doer = KKoRRoLL.idleDoer;
            doer.coreIdleAnimation = "select_idle";
            doer.phases = new CharacterSelectIdlePhase[]
            {
                new CharacterSelectIdlePhase() { inAnimation = "crossarms", holdAnimation = "crossarms_hold", holdMin = 3, holdMax = 5, outAnimation = "crossarms_out" },
                new CharacterSelectIdlePhase() { outAnimation = "hair"},
                new CharacterSelectIdlePhase() { outAnimation = "trash"},

            };



        }
        public static void SetupVlaDoge()
        {
            var VlaDoge = Loader.BuildCharacter("RPGworldMod/Characters/VlaDoge",
                Plugin.GUID,
                new Vector3(16f, 27f, 27.1f),
                false,
                new Vector3(21.8f, 29.5f, 25.3f),
                true,
                false,
                false,
                     true, //Sprites used by paradox
                     false, //Glows
                     null, //Glow Mat
                     null, //Alt Skin Glow Mat
                     0, //Hegemony Cost
                     false, //HasPast
                     ""); //Past ID String
            var identity = ETGModCompatibility.ExtendEnum<PlayableCharacters>(Plugin.GUID, "vladoge");
            ETGMod.Databases.Strings.Core.SetComplex("#PLAYER_NICK_" + identity.ToString().ToUpperInvariant(), "Gaming youtuber" /*(C) Big Nickel*/, "Shield bearer", "Reader", "Outsider");
            ETGMod.Databases.Strings.Core.SetComplex(StringTableManager.GungeonSupportedLanguages.RUSSIAN, "#PLAYER_NICK_" + identity.ToString().ToUpperInvariant(), "Гейминг ютубер", "Щитоносец", "Чтец", "Аутсайдер");

            var doer = VlaDoge.idleDoer;
            doer.coreIdleAnimation = "select_idle";
            doer.phases = new CharacterSelectIdlePhase[]
            {
                new CharacterSelectIdlePhase() { inAnimation = "crossarms", holdAnimation = "crossarms_hold", holdMin = 2, holdMax = 4, outAnimation = "crossarms_out" },
                new CharacterSelectIdlePhase() { outAnimation = "hair"},
                new CharacterSelectIdlePhase() { outAnimation = "trash"},

            };
        }

    }
}
