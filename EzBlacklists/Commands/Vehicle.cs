using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EzBlacklists.Commands
{
    public class Vehicle : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public List<string> Permissions
        {
            get
            {
                return new List<string>() {
                    "v","vehicle","EzBlacklists.v"
                };
            }
        }
        public string Name = "vehicle";

        public string Help => "Spawn a vehicle.";

        public string Syntax => "<id>";

        public List<string> Aliases => new List<string> { "v","jveh" };
        List<VehicleAsset> asset = new List<VehicleAsset>(); // move to onLoad?
        Random r = new System.Random();
        string IRocketCommand.Name => "vehicle";
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (command.Length >= 1)
            {
                bool searched = false;
                string veh = command[0];
                VehicleAsset which = null;
                if (!ushort.TryParse(veh, out ushort id))
                {
                    string val = "";
                    for (int i = 0; i < command.Length; i++) { if (command[i] != null) { val += (i > 0 ? " " : "") + command[i]; } }
                    Assets.find(asset);
                    foreach (VehicleAsset ia in asset)
                    {
                        if (ia?.vehicleName == null || !ia.vehicleName.ToLower().Contains(val.ToLower())) continue;
                        id = ia.id;
                        searched = true;
                    }
                    if (!searched)
                    {
                        Dictionary<VehicleAsset, int> score = new Dictionary<VehicleAsset, int>();
                        //String val = command[0];
                        //for (int i = 1; i < command.Length; i++) { if (command[i] != null) { val += " " + command[i]; } }
                        //VehicleAsset found = GetVehicleAsset(val);
                        //if (found == null)
                        //{
                        var assets = Assets.find(EAssetType.VEHICLE).Cast<VehicleAsset>().Where(k => k?.vehicleName != null && k.name != null).ToList();
                        foreach (var vehicle in assets)
                        {
                            if (vehicle != null)
                            {
                                try
                                {
                                    string[] keywords = vehicle.name.Split('_');
                                    if (vehicle.vehicleName != null)
                                    {
                                        keywords = vehicle.vehicleName.Split(' ');
                                    }
                                    //else { keywords = vehicle.name.Split('_'); }
                                    if (!score.ContainsKey(vehicle))
                                    {
                                        score[vehicle] = 0;
                                    }
                                    if (keywords.Length == command.Length)
                                    {
                                        if (keywords == command)
                                        {
                                            score[vehicle] += 20;
                                        }
                                        else
                                        {
                                            score[vehicle]++;
                                        }
                                    }
                                    int rew = 0;
                                    for (int s = 0; s < command.Length; s++)
                                    {
                                        if (command[s] != null && command[s] != "")
                                            for (int i = 0; i < keywords.Length; i++)
                                            {
                                                if (keywords[i] != null && keywords[i] != "")
                                                {
                                                    if (keywords[i] == command[s])
                                                    {
                                                        if (s == i)
                                                        {
                                                            rew += 2;
                                                            score[vehicle] += (keywords.Length == command.Length ? 5 : 4);
                                                        }
                                                        else
                                                        {
                                                            rew++;
                                                            score[vehicle] += 3;
                                                        }
                                                    }
                                                    if (keywords[i].ToLower() == command[s].ToLower())
                                                    {
                                                        if (s == i)
                                                        {
                                                            rew += 2;
                                                            score[vehicle] += keywords[i].Length + command[s].Length + (keywords.Length == command.Length ? 3 : 2) + (rew > 0 ? rew : 0);
                                                        }
                                                        else
                                                        {
                                                            rew++;
                                                            score[vehicle] += keywords[i].Length + command[s].Length + (keywords.Length == command.Length ? 2 : 1) + (rew > 0 ? rew : 0);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        rew--;
                                                        int result = (int)Math.Round((double)(score[vehicle] / r.Next(1, 3)));
                                                        if (result > 1)
                                                        {
                                                            score[vehicle] = result;
                                                        }
                                                        else
                                                        {
                                                            score[vehicle] -= keywords[i].Length;
                                                        } //else { scores[vehs.IndexOf(vehicle.id)]--; }
                                                    }
                                                    if (keywords[i].Length > command[s].Length)
                                                    {
                                                        if (keywords[i].Contains(command[s]))
                                                        {
                                                            if (s == i)
                                                            {
                                                                score[vehicle] += 2;
                                                            }
                                                            else
                                                            {
                                                                score[vehicle]++;
                                                            }
                                                        }
                                                        if (keywords[i].ToLower().Contains(command[s].ToLower()))
                                                        {
                                                            score[vehicle]++;
                                                        }
                                                        else if (score[vehicle] > 1)
                                                        {
                                                            int result = (int)Math.Round((double)(score[vehicle] / 2));
                                                            if (result > 1) { score[vehicle] = result; } else { score[vehicle]--; }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (command[s].Contains(keywords[i]))
                                                        {
                                                            if (s == i)
                                                            {
                                                                score[vehicle] += 2;
                                                            }
                                                            else { score[vehicle]++; }
                                                        }
                                                        if (command[s].ToLower().Contains(keywords[i].ToLower()))
                                                        {
                                                            score[vehicle]++;
                                                        }
                                                        else if (score[vehicle] > 1)
                                                        {
                                                            int result = (int)Math.Round((double)(score[vehicle] / 2));
                                                            if (result > 1) { score[vehicle] = result; } else { score[vehicle]--; }
                                                        }
                                                    }
                                                    //int rem = 0;
                                                    //if (keywords[i].Length > command[s].Length) { rem = keywords[i].Length - command[s].Length; } else { rem = command[s].Length - keywords[i].Length; }
                                                    //for (int b = 0; b < keywords[i].Length; b++) { if (command[s].IndexOf(keywords[i][b]) > -1) scores[vehs.IndexOf(vehicle.id)] += command[s].Length - b - rem; }
                                                    //for (int b = 0; b < command[s].Length; b++) { if (keywords[i].IndexOf(command[s][b]) > -1) scores[vehs.IndexOf(vehicle.id)] += keywords[i].Length - b - rem; }
                                                }
                                            }
                                    }
                                }
                                catch (Exception) { }
                            }
                        }
                        int record = 0;
                        
                        foreach (VehicleAsset vehicleAsset in score.Keys)
                        {
                            if (score[vehicleAsset] > record)
                            {
                                record = score[vehicleAsset];
                                which = vehicleAsset;
                            }
                        }
                        if (which != null)
                        {
                            searched = true;
                        }
                        else
                        {
                            Rocket.Unturned.Chat.UnturnedChat.Say(player, EzBlacklists.Instance.Translate("vehicle_no_search_results"));
                            return;
                        }
                    }
                }
                if (id != (ushort)0)
                {
                    which = (VehicleAsset) Assets.find(EAssetType.VEHICLE, id); // (VehicleAsset)Assets.FindBaseVehicleAssetByGuidOrLegacyId(System.Guid.Empty, id);
                    if (!searched && which != null)
                    {
                        UnturnedChat.Say(player, EzBlacklists.Instance.Translate("vehicle_not_found", id.ToString()));
                        return;
                    }

                }
                if (which != null)
                {
                    if (EzBlacklists.Instance.FastVehicleBL.TryGetValue(which.GUID, out string permNode))
                    {
                        if (!player.HasPermission(permNode))
                        {
                            UnturnedChat.Say(player, EzBlacklists.Instance.Translate("vehicle_not_allowed", id.ToString()));
                            return;
                        }
                    }

                    VehicleTool.giveVehicle(player.Player, id);

                    UnturnedChat.Say(player, EzBlacklists.Instance.Translate("spawned_vehicle", which.FriendlyName, which.name, id.ToString()));
                }
                else
                {
                    UnturnedChat.Say(player, EzBlacklists.Instance.Translate("no_vehicle_found"));
                }
            }
            else
            {
                UnturnedChat.Say(player, "/v <Vehicle ID>");
            }
        }
        public static VehicleAsset GetVehicleAsset(string itemNameOrId)
        {
            var assets = Assets.find(EAssetType.VEHICLE).Cast<VehicleAsset>().Where(k => k?.vehicleName != null && k.name != null).OrderBy(k => k.vehicleName.Length).ToList();
            VehicleAsset vehicleAsset;
            if (int.TryParse(itemNameOrId, out int id))
            {
                vehicleAsset = assets.FirstOrDefault(k => k.id == id);
            }
            else
            {
                vehicleAsset = assets.FirstOrDefault(k =>
                itemNameOrId.Equals(k.id.ToString(), StringComparison.OrdinalIgnoreCase) ||
                itemNameOrId.Split(' ').All(l => k.vehicleName.ToLower().Contains(l)) ||
                itemNameOrId.Split(' ').All(l => k.name.ToLower().Contains(l)));
            }


            return vehicleAsset;
        }
        private static bool IsValidVehicleId(ushort id) => Assets.find(EAssetType.VEHICLE, id) is VehicleAsset;
    }
}
