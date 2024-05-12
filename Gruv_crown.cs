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
            string ItemName = "GRUV Crown";

            string SpriteDirectory = "RPGworldMod/Resources/gruv_crown_sprite";

            GameObject obj = new GameObject(ItemName);

            var item = obj.AddComponent<Gruv_crown>();

            ItemBuilder.AddSpriteToObject(ItemName, SpriteDirectory, obj);

            string shortDesc = "You are in power";

            string longDesc = "Grants weapons worthy of a king to those who have proven themselves worthy of them.\n\n" +
            "Symbol of power of the great water-powered planet Gruv. It has a built-in water generator connected to Gruv's core, but is currently not working.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "rpgworld");

            item.quality = PickupObject.ItemQuality.C;
            item.ShouldBeExcludedFromShops = true;
        }
        public override void Update()
        {
            if (this.m_owner.PlayerHasActiveSynergy("King of the first floor") && !this.m_owner.HasPickupID(Gruv_shotgun.ID))
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(Gruv_shotgun.ID).gameObject, this.m_owner);
            }
            if (this.m_owner.PlayerHasActiveSynergy("King of the second floor") && !this.m_owner.HasPickupID(Gruv_assault_rifle.ID))
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(Gruv_assault_rifle.ID).gameObject, this.m_owner);
            }
            if (this.m_owner.PlayerHasActiveSynergy("King of the third floor") && !this.m_owner.HasPickupID(Gruv_sniper_rifle.ID))
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(Gruv_sniper_rifle.ID).gameObject, this.m_owner);
            }
            if (this.m_owner.PlayerHasActiveSynergy("King of the fourth floor") && !this.m_owner.HasPickupID(Gruv_machine_gun.ID))
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(Gruv_machine_gun.ID).gameObject, this.m_owner);
            }
            if (this.m_owner.PlayerHasActiveSynergy("King of the fifth floor") && !this.m_owner.HasPickupID(Gruv_rocket_launcher.ID))
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(Gruv_rocket_launcher.ID).gameObject, this.m_owner);
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
