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
            
            Dictionary<string, List<ushort>> SortedItems = new Dictionary<string, List<ushort>>();
            foreach (ushort gg in EzBlacklists.Instance.FastItemBL.Keys)
            {
                string permNode = EzBlacklists.Instance.FastItemBL[gg];
                if (!SortedItems.TryGetValue(permNode, out List<ushort> val)) val = new List<ushort>();
                val.Add(gg);
            }
            foreach (string PermNode in SortedItems.Keys)
            {
                str += (str != "" ? EzBlacklists.Instance.Configuration.Instance.separator : "") + PermNode + EzBlacklists.Instance.Configuration.Instance.separator + string.Join(EzBlacklists.Instance.Configuration.Instance.separator, SortedItems[PermNode]);
            }

            System.IO.File.WriteAllText(dir + "/items", str);
            str = "";

            Dictionary<string, List<System.Guid>> SortedList = new Dictionary<string, List<System.Guid>>();
            foreach(System.Guid gg in EzBlacklists.Instance.FastVehicleBL.Keys)
            {
                string permNode = EzBlacklists.Instance.FastVehicleBL[gg];
                if (!SortedList.TryGetValue(permNode, out List<System.Guid> val)) val = new List<System.Guid>();
                val.Add(gg);
            }
            foreach (string PermNode in SortedList.Keys)
            {
                str += (str != "" ? EzBlacklists.Instance.Configuration.Instance.separator : "") + PermNode + EzBlacklists.Instance.Configuration.Instance.separator + string.Join(EzBlacklists.Instance.Configuration.Instance.separator, SortedList[PermNode]);
            }

            System.IO.File.WriteAllText(dir + "/vehicles", str);
            EzBlacklists.Instance.Configuration.Save();
            Rocket.Unturned.Chat.UnturnedChat.Say(caller, "Saved!");
        }
    }
}
