using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RPGworldMod
{
    class Gruv_crown : PassiveItem
    {
        public static void Init()
        {
            string ItemName = "Gruv Crown";

            string SpriteDirectory = "RPGworldMod/Resources/gruv_crown_sprite";

            GameObject obj = new GameObject(ItemName);

            var item = obj.AddComponent<Gruv_crown>();

            ItemBuilder.AddSpriteToObject(ItemName, SpriteDirectory, obj);

            string shortDesc = "You Are In Power";

            string longDesc = "Grants weapons worthy of a king to those who have proven themselves worthy of them.\n\n" +
            "Symbol of power of the great water-powered planet Gruv. It has a built-in water generator connected to Gruv's core, but it is currently not working.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rpgworld");
            GunExt.SetName(item, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Корона Грув");
            GunExt.SetShortDescription(item, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Вы у власти");
            GunExt.SetLongDescription(item, StringTableManager.GungeonSupportedLanguages.RUSSIAN, "Дарует оружие, достойное короля, тем, кто доказал, что достоин его.\n\n" +
            "Символ власти короля великой планеты Грув, запитанной энергией воды. Корона имеет встроенный генератор воды, подключённый к ядру Грув, но в настоящее время он не работает.");
            item.ForcedPositionInAmmonomicon = PickupObjectDatabase.GetById(214).ForcedPositionInAmmonomicon;

            item.quality = PickupObject.ItemQuality.SPECIAL;
            item.ShouldBeExcludedFromShops = true;
        }
        public override void Update()
        {
            if (this.m_owner != null)
            {
                if (this.m_owner.PlayerHasActiveSynergy("RPGW King Of The First Chamber") && !this.m_owner.HasPickupID(Gruv_shotgun.ID))
                {
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(Gruv_shotgun.ID).gameObject, this.m_owner);
                }
                if (this.m_owner.PlayerHasActiveSynergy("RPGW King Of The Second Chamber") && !this.m_owner.HasPickupID(Gruv_assault_rifle.ID))
                {
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(Gruv_assault_rifle.ID).gameObject, this.m_owner);
                }
                if (this.m_owner.PlayerHasActiveSynergy("RPGW King Of The Third Chamber") && !this.m_owner.HasPickupID(Gruv_sniper_rifle.ID))
                {
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(Gruv_sniper_rifle.ID).gameObject, this.m_owner);
                }
                if (this.m_owner.PlayerHasActiveSynergy("RPGW King Of The Fourth Chamber") && !this.m_owner.HasPickupID(Gruv_machine_gun.ID))
                {
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(Gruv_machine_gun.ID).gameObject, this.m_owner);
                }
                if (this.m_owner.PlayerHasActiveSynergy("RPGW King Of The Fifth Chamber") && !this.m_owner.HasPickupID(Gruv_rocket_launcher.ID))
                {
                    LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(Gruv_rocket_launcher.ID).gameObject, this.m_owner);
                }
            }
            base.Update();
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            return base.Drop(player);
        }
    }
}
