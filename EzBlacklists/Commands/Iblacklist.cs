using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace EzBlacklists.Commands
{
    public class Iblacklist : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public List<string> Permissions
        {
            get
            {
                return new List<string>() {
                    "Iblacklist","EzBlacklists.Iblacklist"
                };
            }
        }
        public string Name = "iblacklist";

        public string Help => "Blacklist an item.";

        public string Syntax => "<id>";

        public List<string> Aliases => new List<string> { "additemblacklist" };

        string IRocketCommand.Name => "iblacklist";

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length > 1)
            {
                string perm = command[0];
                string veh = command[1];
                if(ushort.TryParse(veh, out ushort id))
                {
                    if (EzBlacklists.Instance.FastItemBL.ContainsKey(id))
                    {
                        EzBlacklists.Instance.FastItemBL.Remove(id);
                        Rocket.Unturned.Chat.UnturnedChat.Say(caller, "Removed (" + id.ToString() + ") from blacklist.");
                    }
                    else
                    {
                        EzBlacklists.Instance.FastItemBL.Add(id, perm);
                        Rocket.Unturned.Chat.UnturnedChat.Say(caller, "Added (" + id.ToString() + ") to blacklist.");
                    }
                }
                else
                {
                    Rocket.Unturned.Chat.UnturnedChat.Say(caller, "Invalid ID. Try find the number.");
                }
            }
            else
            {
                Rocket.Unturned.Chat.UnturnedChat.Say(caller, "/iblacklist <perm> <Item ID>");
            }
        }
    }
}
