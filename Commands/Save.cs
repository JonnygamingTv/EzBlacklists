using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace EzBlacklists.Commands
{
    public class Save : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public List<string> Permissions
        {
            get
            {
                return new List<string>() {
                    "Iblacklistsave","EzBlacklists.save"
                };
            }
        }
        public string Name = "restrictsave";

        public string Help => "Save restrictions.";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "saverestrict", "restrictsave", "blacklistsave" };

        string IRocketCommand.Name => "restrictsave";

        public void Execute(IRocketPlayer caller, string[] command)
        {
            Rocket.Unturned.Chat.UnturnedChat.Say(caller, "Saving..");
            string directory = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string dir = directory.Substring(0, directory.Length - 4);
            string str = "";
            for (int i = 0; i < EzBlacklists.Instance.Configuration.Instance.ItemPerms.Count; i++)
            {
                str += (str != "" ? EzBlacklists.Instance.Configuration.Instance.separator : "") + EzBlacklists.Instance.Configuration.Instance.ItemPerms[i] + EzBlacklists.Instance.Configuration.Instance.separator + string.Join(EzBlacklists.Instance.Configuration.Instance.separator, EzBlacklists.Instance.Configuration.Instance.items[i]);
            }
            System.IO.File.WriteAllText(dir + "/items", str);
            str = "";
            for (int i = 0; i < EzBlacklists.Instance.Configuration.Instance.VehPerms.Count; i++)
            {
                str += (str != "" ? EzBlacklists.Instance.Configuration.Instance.separator : "") + EzBlacklists.Instance.Configuration.Instance.VehPerms[i] + EzBlacklists.Instance.Configuration.Instance.separator + string.Join(EzBlacklists.Instance.Configuration.Instance.separator, EzBlacklists.Instance.Configuration.Instance.vehicles[i]);
            }
            System.IO.File.WriteAllText(dir + "/vehicles", str);
            Rocket.Unturned.Chat.UnturnedChat.Say(caller, "Saved!");
        }
    }
}
