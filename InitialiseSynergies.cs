using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.ItemAPI;

namespace RPGworldMod
{
    class InitialiseSynergies
    {
        public static void DoInitialisation()
        {
            List<string> mandatorySynergyKingOfFirstFloor = new List<string>() { "rpgworld:gruv_crown", "master_round_1" };
            CustomSynergies.Add("King of the first floor", mandatorySynergyKingOfFirstFloor);
            List<string> mandatorySynergyKingOfSecondFloor = new List<string>() { "rpgworld:gruv_crown", "master_round_2" };
            CustomSynergies.Add("King of the second floor", mandatorySynergyKingOfSecondFloor);
            List<string> mandatorySynergyKingOfThirdFloor = new List<string>() { "rpgworld:gruv_crown", "master_round_3" };
            CustomSynergies.Add("King of the third floor", mandatorySynergyKingOfThirdFloor);
            List<string> mandatorySynergyKingOfFourthFloor = new List<string>() { "rpgworld:gruv_crown", "master_round_4" };
            CustomSynergies.Add("King of the fourth floor", mandatorySynergyKingOfFourthFloor);
            List<string> mandatorySynergyKingOfFifthFloor = new List<string>() { "rpgworld:gruv_crown", "master_round_5" };
            CustomSynergies.Add("King of the fifth floor", mandatorySynergyKingOfFifthFloor);
        }

    }
}
