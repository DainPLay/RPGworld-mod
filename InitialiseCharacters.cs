using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.CharacterAPI;
using UnityEngine;

namespace RPGworldMod
{
    class InitialiseCharacters
    {

        public static void Charactersetup()
        {
            SetupKKoRRoLL();
        }

        public static void SetupKKoRRoLL()
        {
            var KKoRRoLL = Loader.BuildCharacter("RPGworldMod/Characters/KKoRRoLL",
                   "rpgworld:KKoRRoLL",
                    new Vector3(31f, 27f, 27.1f),
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
            var identity = ETGModCompatibility.ExtendEnum<PlayableCharacters>("rpgworld:KKoRRoLL", "kkorroll");
            StringHandler.AddStringDefinition("#PLAYER_NICK_" + identity.ToString().ToUpperInvariant(), "Green eyed");
            StringHandler.AddStringDefinition("#PLAYER_NICK_" + identity.ToString().ToUpperInvariant(), "King");
            StringHandler.AddStringDefinition("#PLAYER_NICK_" + identity.ToString().ToUpperInvariant(), "Smokey");
            StringHandler.AddStringDefinition("#PLAYER_NICK_" + "GREEN_EYED", "Green eyed");
            StringHandler.AddStringDefinition("#PLAYER_NICK_" + "KING", "King");
            StringHandler.AddStringDefinition("#PLAYER_NICK_" + "SMOKEY", "Smokey");


            var doer = KKoRRoLL.idleDoer;
            doer.coreIdleAnimation = "select_idle";
            doer.phases = new CharacterSelectIdlePhase[]
            {
                new CharacterSelectIdlePhase() { inAnimation = "crossarms", holdAnimation = "crossarms_hold", holdMin = 3, holdMax = 5, outAnimation = "crossarms_out" },
                new CharacterSelectIdlePhase() { outAnimation = "hair"},
                new CharacterSelectIdlePhase() { outAnimation = "trash"},

            };



        }
    }
}
