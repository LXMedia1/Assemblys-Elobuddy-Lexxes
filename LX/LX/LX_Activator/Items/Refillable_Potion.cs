using System.Reflection;
using EloBuddy;

namespace LX.LX_Activator.Items
{
	class Refillable_Potion : Item
	{
		private readonly string className = MethodBase.GetCurrentMethod().DeclaringType.Name;
		public override void CreateMenu()
		{
			Activator.Menu_Potions.AddPotionMenu(className, 20,10);
		}

		public override void Use()
		{
			if (ObjectManager.Player.IsPotionReady())
			{
				if (Activator.Menu_Potions.PotionConditionMatch(className) &&
					EloBuddy.SDK.Item.HasItem(ItemId.Refillable_Potion) &&
					EloBuddy.SDK.Item.CanUseItem(ItemId.Refillable_Potion))
					EloBuddy.SDK.Item.UseItem(ItemId.Refillable_Potion);
			}
		}
	}
}
