using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.CharacterAPI;
using Alexandria.cAPI;
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
            HatUtility.SetupHatOffsets("Playerkkorroll(Clone)", 0, -10, 0, -15);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_idle_001", 0, 0);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_idle_002", 0, -1);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_idle_003", 0, -1);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_idle_004", 0, 0);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_idle_backwards_001", 0, 0);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_idle_backwards_002", 0, -1);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_idle_backwards_003", 0, -1);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_idle_backwards_004", 0, 0);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_idle_back_002", 0, -1);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_idle_back_003", 0, -1);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_idle_front_001", 0, 0);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_idle_front_002", 0, -2);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_idle_front_003", 0, -2);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_idle_front_004", 0, -1);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_idle_front_005", 0, 0);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_idle_front_006", 0, 0);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_forward_001", 1, -1);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_forward_002", 0, 2);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_forward_003", 0, 0);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_forward_004", 1, -1);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_forward_005", 2, 3);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_forward_006", 2, 0);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_backward_001", 0, -1);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_backward_002", 0, 2);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_backward_003", 0, 0);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_backward_004", 0, -1);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_backward_005", 0, 3);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_backward_006", 0, 0);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_front_001", 1, 2);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_front_002", 1, 1);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_front_003", 0, -1);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_front_004", 1, 3);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_front_005", 1, 2);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_front_006", 0, -1);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_back_001", 0, 3);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_back_002", 0, 2);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_back_003", 0, -1);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_back_004", 0, 2);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_back_005", 0, 1);
            HatUtility.AddHatOffset("Playerkkorroll(Clone)", "convict_run_back_006", 0, 0);
            SetupVlaDoge();
            HatUtility.SetupHatOffsets("Playervladoge(Clone)", 0, -10, 0, -15);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_idle_002", 0, -1);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_idle_front_002", 0, -1);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_idle_back_002", 0, -1);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_idle_bw_002", 0, -1);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_idle_003", 0, -1);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_idle_front_003", 0, -1);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_idle_back_003", 0, -1);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_idle_bw_003", 0, -1);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_forward_001", 0, -1);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_forward_002", 0, 2);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_forward_003", 0, 0);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_forward_004", 0, -1);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_forward_005", 0, 3);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_forward_006", 0, -1);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_backwards_001", 0, -1);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_backwards_002", 0, 2);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_backwards_003", 0, 0);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_backwards_004", 0, -1);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_backwards_005", 0, 3);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_backwards_006", 0, -1);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_north_001", 0, 3);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_north_002", 0, 1);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_north_003", 0, -1);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_north_004", 0, 4);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_north_005", 0, 1);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_north_006", 0, -1);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_south_001", 0, 3);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_south_002", 0, 1);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_south_003", 0, -1);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_south_004", 0, 4);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_south_005", 0, 1);
            HatUtility.AddHatOffset("Playervladoge(Clone)", "convict_run_south_006", 0, -1);

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
            ETGMod.Databases.Strings.Core.Set("#PLAYER_NAME_" + identity.ToString().ToUpperInvariant(), "KKoRRoLL");
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
            ETGMod.Databases.Strings.Core.Set("#PLAYER_NAME_" + identity.ToString().ToUpperInvariant(), "VlaDoge");
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
            HatUtility.SetupHatOffsets("characterPrefabObject.vladoge", 10, 10, 0, 0);
        }

    }
}
