using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace EzBlacklists.Commands
{
    public class Vblacklist : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public List<string> Permissions
        {
            get
            {
                return new List<string>() {
                    "Vblacklist","EzBlacklists.Vblacklist"
                };
            }
        }
        public string Name = "vblacklist";

        public string Help => "Blacklist a vehicle.";

        public string Syntax => "<perm> <id>";

        public List<string> Aliases => new List<string> { "addvehicleblacklist" };

        string IRocketCommand.Name => "vblacklist";

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer p = (UnturnedPlayer)caller;
            if (command.Length >= 1)
            {
                System.Guid gid = System.Guid.Empty;
                string perm = command[0];
                if (command.Length > 1)
                {
                    string veh = command[1];
                    if(!System.Guid.TryParse(veh, out gid))
                    {
                        if(ushort.TryParse(veh, out ushort id))
                        {
                            SDG.Unturned.VehicleAsset va = (SDG.Unturned.VehicleAsset) SDG.Unturned.Assets.find(SDG.Unturned.EAssetType.VEHICLE, id);
                            if(va != null) gid = va.GUID;
                        }
                    }
                }else if (p.IsInVehicle)
                {
                    gid = p.CurrentVehicle.asset.GUID;
                }
                if(gid == System.Guid.Empty)
                {
                    Rocket.Unturned.Chat.UnturnedChat.Say(caller, "Invalid ID. Try finding the ID or the UUID.");
                    return;
                }

                if(EzBlacklists.Instance.FastVehicleBL.ContainsKey(gid))
                {
                    EzBlacklists.Instance.FastVehicleBL.Remove(gid);
                    Rocket.Unturned.Chat.UnturnedChat.Say(caller, "Removed (" + gid.ToString() + ") from blacklist.");
                }
                else
                {
                    EzBlacklists.Instance.FastVehicleBL.Add(gid, perm);
                    Rocket.Unturned.Chat.UnturnedChat.Say(caller, "Added (" + gid.ToString() + ") to blacklist.");
                }
            }
            else
            {
                Rocket.Unturned.Chat.UnturnedChat.Say(caller, "/vblacklist <perm> <Vehicle ID>");
            }
        }
    }
}
