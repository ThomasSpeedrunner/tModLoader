using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Terraria
{
	public partial class Item : TagSerializable
	{
		public static readonly Func<TagCompound, Item> DESERIALIZER = ItemIO.Load;

		public ModItem modItem {
			get;
			internal set;
		}

		internal GlobalItem[] globalItems = new GlobalItem[0];

		/// <summary>
		/// The damage types that this Item is affected by.
		/// Vanilla classes use DamageClass.Melee/Ranged/Magic/Summon/Throwing. Use ModContent.GetInstance<T>() for custom damage types.
		/// These will only matter if you leave Item.allDamageTypes null.
		/// </summary>
		public Dictionary<DamageClass, bool> DamageTypes = new Dictionary<DamageClass, bool>();

		/// <summary>
		/// Should all damage types affect this Item? Should none of them? That's what you decide here.
		/// Set to true to make all damage types affect this Item.
		/// Set to false to make no damage types affect this Item.
		/// Leave this field null to let the individual DamageTypes entries decide.
		/// </summary>
		public bool? allDamageTypes;

		// Get

		/// <summary> Gets the instance of the specified GlobalItem type. This will throw exceptions on failure. </summary>
		/// <exception cref="KeyNotFoundException"/>
		/// <exception cref="IndexOutOfRangeException"/>
		public T GetGlobalItem<T>() where T : GlobalItem
			=> GetGlobalItem(ModContent.GetInstance<T>());

		/// <summary> Gets the local instance of the type of the specified GlobalItem instance. This will throw exceptions on failure. </summary>
		/// <exception cref="KeyNotFoundException"/>
		/// <exception cref="NullReferenceException"/>
		public T GetGlobalItem<T>(T baseInstance) where T : GlobalItem
			=> baseInstance.Instance(this) as T ?? throw new KeyNotFoundException($"Instance of '{typeof(T).Name}' does not exist on the current item.");
		
		/*
		// TryGet

		/// <summary> Gets the instance of the specified GlobalItem type. </summary>
		public bool TryGetGlobalItem<T>(out T result) where T : GlobalItem
			=> TryGetGlobalItem(ModContent.GetInstance<T>(), out result);

		/// <summary> Safely attempts to get the local instance of the type of the specified GlobalItem instance. </summary>
		/// <returns> Whether or not the requested instance has been found. </returns>
		public bool TryGetGlobalItem<T>(T baseInstance, out T result) where T : GlobalItem {
			if (baseInstance == null || baseInstance.index < 0 || baseInstance.index >= globalItems.Length) {
				result = default;

				return false;
			}

			result = baseInstance.Instance(this) as T;

			return result != null;
		}
		*/

		public TagCompound SerializeData() => ItemIO.Save(this);

		internal static void PopulateMaterialCache() {
			for (int i = 0; i < Recipe.numRecipes; i++) {
				foreach (Item item in Main.recipe[i].requiredItem) {
					ItemID.Sets.IsAMaterial[item.type] = true;
				}
			}

			foreach (RecipeGroup recipeGroup in RecipeGroup.recipeGroups.Values) {
				foreach (var item in recipeGroup.ValidItems) {
					ItemID.Sets.IsAMaterial[item] = true;
				}
			}

			ItemID.Sets.IsAMaterial[71] = false;
			ItemID.Sets.IsAMaterial[72] = false;
			ItemID.Sets.IsAMaterial[73] = false;
			ItemID.Sets.IsAMaterial[74] = false;
		}

		public static int NewItem(Rectangle rectangle, int Type, int Stack = 1, bool noBroadcast = false, int prefixGiven = 0, bool noGrabDelay = false, bool reverseLookup = false) => 
			Item.NewItem(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, Type, Stack, noBroadcast, prefixGiven, noGrabDelay, reverseLookup);
		public static int NewItem(Vector2 position, int Type, int Stack = 1, bool noBroadcast = false, int prefixGiven = 0, bool noGrabDelay = false, bool reverseLookup = false) => 
			NewItem((int)position.X, (int)position.Y, 0, 0, Type, Stack, noBroadcast, prefixGiven, noGrabDelay, reverseLookup);
	}
}