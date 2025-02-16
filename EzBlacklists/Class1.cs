using EzBlackLists;
using Rocket.Core.Plugins;
using System;
using System.Collections.Generic;
using System.IO;

namespace EzBlacklists
{
    public class EzBlacklists : RocketPlugin<EzBlacklistsConfig>
    {
        public Dictionary<ushort, string> FastItemBL = new Dictionary<ushort, string>();
        public Dictionary<System.Guid, string> FastVehicleBL = new Dictionary<System.Guid, string>();
        public static EzBlacklists Instance { get; private set; }
        protected override void Load()
        {
            Instance = this;
            Rocket.Core.Logging.Logger.Log("EzBlacklists is loading!");
            string directory = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string dir = directory.Substring(0, directory.Length - 4);
            try
            {
                if (File.Exists(dir + "/vehicles"))
                {
                    string content = File.ReadAllText(dir + "/vehicles");
                    try
                    {
                        string[] cont = content.Split(',');
                        Rocket.Core.Logging.Logger.Log(cont.Length.ToString() + " vehicles.");
                        string PermNode = "";
                        for (int i = 0; i < cont.Length; i++)
                        {
                            if (!System.Guid.TryParse(cont[i].Trim(), out System.Guid id))
                            {
                                PermNode = cont[i];
                            }else
                            {
                                FastVehicleBL[id] = PermNode;
                            }
                        }
                    }
                    catch (Exception err) {
                        Rocket.Core.Logging.Logger.LogException(err);
                    }
                }
            }
            catch (Exception)
            { }
            try
            {
                if (File.Exists(dir + "/items"))
                {
                    string content = File.ReadAllText(dir + "/items");
                    try
                    {
                        string[] cont = content.Split(',');
                        Rocket.Core.Logging.Logger.Log(cont.Length.ToString() + " items.");
                        string PermNode = "";
                        for (int i = 0; i < cont.Length; i++)
                        {
                            if (!ushort.TryParse(cont[i].Trim(), out ushort id))
                            {
                                PermNode = cont[i];
                            }
                            else
                            {
                                FastItemBL[id] = PermNode;
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        Rocket.Core.Logging.Logger.LogException(err);
                    }
                }
            }
            catch (Exception)
            { }
            Rocket.Core.Logging.Logger.Log("EzBlacklists special files loaded.");
            for(int i= 0; i < Configuration.Instance.VehPerms.Count; i++)
            {
                string PermNode = Instance.Configuration.Instance.VehPerms[i];
                List<ushort> gg = Instance.Configuration.Instance.vehicles[i];
                foreach(ushort g in gg)
                {
                    SDG.Unturned.VehicleAsset va = (SDG.Unturned.VehicleAsset)SDG.Unturned.Assets.find(SDG.Unturned.EAssetType.VEHICLE, g);
                    if (va != null) FastVehicleBL[va.GUID] = PermNode;
                }
            }
            Rocket.Core.Logging.Logger.Log("Loaded vehicle blacklist from config into memory.");
            for (int i = 0; i < Configuration.Instance.ItemPerms.Count; i++)
            {
                string PermNode = Instance.Configuration.Instance.ItemPerms[i];
                List<ushort> gg = Instance.Configuration.Instance.items[i];
                foreach (ushort g in gg)
                {
                    FastItemBL[g] = PermNode;
                }
            }
            Rocket.Core.Logging.Logger.Log("Loaded item blacklist from config into memory.");
            Rocket.Core.Logging.Logger.Log("EzBlacklists has been loaded!");
        }
        protected override void Unload()
        {
            FastItemBL.Clear();
            FastVehicleBL.Clear();
            Rocket.Core.Logging.Logger.Log("EzBlacklists has been unloaded!");
        }

        public override Rocket.API.Collections.TranslationList DefaultTranslations => new Rocket.API.Collections.TranslationList
        {
            {"no_permission_item", "You don't have permission to hold the item {0}!"},
            {"no_permission_vehicle", "You don't have permission to enter the {0} vehicle!"},
            {"spawned_vehicle", "Spawned {0} {1} ({2})"},
            {"vehicle_not_found", "{0} is not a valid Vehicle ID."},
            {"no_vehicle_found", "Failed to find ID to spawn."},
            {"vehicle_not_allowed", "You are not allowed to spawn that vehicle ({0})."},
            {"vehicle_no_search_results", "Vehicle name search failed. Try using the ID or try another search."},
            {"item_not_found", "{0} is not a valid Item ID."},
            {"item_not_allowed", "You are not allowed to spawn that item ({0})."},
            {"spawned_item", "Gave x{0} {1} {2} ({3})."},
            {"no_item_found", "Failed to find ID to spawn."},
            {"item_no_search_results", "Item name search failed. Try using the ID or try another search."}
        };
    }
}
