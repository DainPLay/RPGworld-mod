using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using Alexandria.ItemAPI;

namespace RPGworldMod
{
    class InitialiseSynergies
    {
        public static void DoInitialisation()
        {
            List<string> mandatorySynergyKingOfFirstChamber = new List<string>() { "rpgworld:gruv_crown", "master_round_1" };
            CustomSynergies.Add("RPGW King Of The First Chamber", mandatorySynergyKingOfFirstChamber);
            ETGMod.Databases.Strings.Synergy.Set("RPGW King Of The First Chamber", "King Of The First Chamber");
            ETGMod.Databases.Strings.Synergy.Set(StringTableManager.GungeonSupportedLanguages.RUSSIAN, "King Of The First Chamber", "Король первого зала");
            List<string> mandatorySynergyKingOfSecondChamber = new List<string>() { "rpgworld:gruv_crown", "master_round_2" };
            CustomSynergies.Add("RPGW King Of The Second Chamber", mandatorySynergyKingOfSecondChamber);
            ETGMod.Databases.Strings.Synergy.Set("RPGW King Of The Second Chamber", "King Of The Second Chamber");
            ETGMod.Databases.Strings.Synergy.Set(StringTableManager.GungeonSupportedLanguages.RUSSIAN, "King Of The Second Chamber", "Король второго зала");
            List<string> mandatorySynergyKingOfThirdChamber = new List<string>() { "rpgworld:gruv_crown", "master_round_3" };
            CustomSynergies.Add("RPGW King Of The Third Chamber", mandatorySynergyKingOfThirdChamber);
            ETGMod.Databases.Strings.Synergy.Set("RPGW King Of The Third Chamber", "King Of The Third Chamber");
            ETGMod.Databases.Strings.Synergy.Set(StringTableManager.GungeonSupportedLanguages.RUSSIAN, "King Of The Third Chamber", "Король третьего зала");
            List<string> mandatorySynergyKingOfFourthChamber = new List<string>() { "rpgworld:gruv_crown", "master_round_4" };
            CustomSynergies.Add("RPGW King Of The Fourth Chamber", mandatorySynergyKingOfFourthChamber);
            ETGMod.Databases.Strings.Synergy.Set("RPGW King Of The Fourth Chamber", "King Of The Fourth Chamber");
            ETGMod.Databases.Strings.Synergy.Set(StringTableManager.GungeonSupportedLanguages.RUSSIAN, "King Of The Fourth Chamber", "Король четвёртого зала");
            List<string> mandatorySynergyKingOfFifthChamber = new List<string>() { "rpgworld:gruv_crown", "master_round_5" };
            CustomSynergies.Add("RPGW King Of The Fifth Chamber", mandatorySynergyKingOfFifthChamber);
            ETGMod.Databases.Strings.Synergy.Set("RPGW King Of The Fifth Chamber", "King Of The Fifth Chamber");
            ETGMod.Databases.Strings.Synergy.Set(StringTableManager.GungeonSupportedLanguages.RUSSIAN, "King Of The Fifth Chamber", "Король пятого зала");
            List<string> mandatorySynergyBounceSign = new List<string>() { "rpgworld:gravity_sign", "springheel_boots" };
            CustomSynergies.Add("RPGW Bounce Sign", mandatorySynergyBounceSign);
            List<string> mandatorySynergyGreenScarf = new List<string>() { "rpgworld:an_item", "bloodied_scarf" };
            CustomSynergies.Add("RPGW Green Scarf", mandatorySynergyGreenScarf);
            ETGMod.Databases.Strings.Synergy.Set("RPGW Green Scarf", "Green Scarf");
            ETGMod.Databases.Strings.Synergy.Set(StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Green Scarf", "Зелёный шарф");
            List<string> mandatorySynergyDestructiveAnkhCandy = new List<string>() { "rpgworld:ankh_shield", "magic_sweet" };
            CustomSynergies.Add("RPGW Destructive Ankh Candy", mandatorySynergyDestructiveAnkhCandy);
            List<string> mandatorySynergyDestructiveAnkhCandy2 = new List<string>() { "rpgworld:alt_ankh_shield", "magic_sweet" };
            CustomSynergies.Add("RPGW Destructive Ankh Candy", mandatorySynergyDestructiveAnkhCandy2);
            ETGMod.Databases.Strings.Synergy.Set("RPGW Destructive Ankh Candy", "Destructive Ankh Candy");
            ETGMod.Databases.Strings.Synergy.Set(StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Destructive Ankh Candy", "Разрушительный леденец Анх");
            //List<string> mandatorySynergyAutumnMirage = new List<string>() { "rpgworld:ankh_shield", "luxin_cannon" };
            //CustomSynergies.Add("RPGW Autumn Mirage", mandatorySynergyAutumnMirage);
            //List<string> mandatorySynergyAutumnMirage2 = new List<string>() { "rpgworld:alt_ankh_shield", "luxin_cannon" };
            //CustomSynergies.Add("RPGW Autumn Mirage", mandatorySynergyAutumnMirage2);
            //ETGMod.Databases.Strings.Synergy.Set("RPGW Autumn Mirage", "Autumn Mirage");
            //ETGMod.Databases.Strings.Synergy.Set(StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Autumn Mirage", "Осенний мираж");
            try
            {
                List<string> mandatorySynergyBounceSign2 = new List<string>() { "rpgworld:gravity_sign", "ski:spring_roll" };
                CustomSynergies.Add("RPGW Bounce Sign", mandatorySynergyBounceSign2);
            }
            catch (Exception e)
            {

            }
            ETGMod.Databases.Strings.Synergy.Set("RPGW Bounce Sign", "Bounce Sign");
            ETGMod.Databases.Strings.Synergy.Set(StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Bounce Sign", "Знак отскока");
        }


        public static void AddSynergyForm(int baseGun, int newGun, List<string> mandatoryConsoleIDs, List<string> optionalConsoleIDs, string synergy, bool isSelectable)
        {
            GunTools.AddTransformSynergy(PickupObjectDatabase.GetById(baseGun) as Gun, newGun, true, synergy, !isSelectable);
            CustomSynergies.Add(synergy, mandatoryConsoleIDs, optionalConsoleIDs, true);
        }

    }
}
