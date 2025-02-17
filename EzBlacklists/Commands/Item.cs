using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;


namespace EzBlacklists.Commands
{
    public class Item : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public List<string> Permissions
        {
            get
            {
                return new List<string>() {
                    "i","item","EzBlacklists.i"
                };
            }
        }
        public string Name = "item";
        public string Help => "Give you an item.";
        public string Syntax => "<id>";
        public List<string> Aliases => new List<string> { "i", "jit", "give" };
        string IRocketCommand.Name => "item";

        Random r = new System.Random();

        List<ItemAsset> sortedAssets = new List<ItemAsset>();
        Dictionary<string, ushort> SearchCache = new Dictionary<string, ushort>();
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (command.Length >= 1)
            {
                bool searched = false;
                byte amount = 1;
                if (!byte.TryParse(command[command.Length-1], out amount)) amount = 1;
                string it = command[0];
                string iname = "";
                if (!ushort.TryParse(it, out ushort id))
                {
                    string val = "";
                    for (int i = 0; i < command.Length; i++)
                    {
                        if (command[i].Substring(command[i].Length - 1) == "\"" || command[i].Substring(command[i].Length - 2) == "\"\"" || command[i].Substring(command[i].Length - 1) == "'")
                        {
                            command[i] = command[i].Substring(0, command[i].Length - 1);
                            break;
                        }
                        if (command[i].Substring(0, 1) == "\"" || command[i].Substring(0, 1) == "'")
                        {
                            command[i] = command[i].Substring(1);
                        }
                        val += (i > 0 ? " " : "") + command[i];
                    }
                    Assets.find(sortedAssets);
                    ItemAsset asset = sortedAssets.Where(i => i.itemName != null).OrderBy(i => i.itemName.Length).FirstOrDefault(i => i.itemName.ToLower().Contains(val.ToLower()));
                    if (asset != null) { id = asset.id; }
                    else
                    {
                        if (SearchCache.TryGetValue(val, out id)) { searched = true; }
                        else
                        {
                            Dictionary<ushort, int> score = new Dictionary<ushort, int>();

                            foreach (ItemAsset item in sortedAssets)
                            {
                                if (item == null || string.IsNullOrEmpty(item.itemName)) continue;

                                try
                                {
                                    string[] keywords = item.name.Split('_');
                                    if (item.itemName != null)
                                    {
                                        keywords = item.itemName.Split(' ');
                                    }
                                    if (!score.ContainsKey(item.id))
                                    {
                                        score[item.id] = 0;
                                    }
                                    if (keywords.Length == command.Length)
                                    {
                                        if (keywords == command)
                                        {
                                            score[item.id] += 20;
                                        }
                                        else
                                        {
                                            score[item.id]++;
                                        }
                                    }
                                    int rew = 0;
                                    for (int s = 0; s < command.Length; s++)
                                    {
                                        if (command[s] != null) if (command[s] != "")
                                                for (int i = 0; i < keywords.Length; i++)
                                                {
                                                    if (keywords[i] != null) if (keywords[i] != "")
                                                        {
                                                            if (keywords[i] == command[s])
                                                            {
                                                                if (s == i)
                                                                {
                                                                    rew += 2;
                                                                    score[item.id] += (keywords.Length == command.Length ? 5 : 4);
                                                                }
                                                                else
                                                                {
                                                                    rew++;
                                                                    score[item.id] += 3;
                                                                }
                                                            }
                                                            if (keywords[i].ToLower() == command[s].ToLower())
                                                            {
                                                                if (s == i)
                                                                {
                                                                    rew += 2;
                                                                    score[item.id] += keywords[i].Length + command[s].Length + (keywords.Length == command.Length ? 3 : 2) + (rew > 0 ? rew : 0);
                                                                }
                                                                else
                                                                {
                                                                    rew++;
                                                                    score[item.id] += keywords[i].Length + command[s].Length + (keywords.Length == command.Length ? 2 : 1) + (rew > 0 ? rew : 0);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                rew--;
                                                                int result = (int)Math.Round((double)(score[item.id] / 2));
                                                                if (result > 1) { score[item.id] = result; } else { score[item.id] -= keywords[i].Length; } //else { scores[vehs.IndexOf(vehicle.id)]--; }
                                                            }
                                                            if (keywords[i].Length > command[s].Length)
                                                            {
                                                                if (keywords[i].Contains(command[s]))
                                                                {
                                                                    if (s == i)
                                                                    {
                                                                        score[item.id] += 2;
                                                                    }
                                                                    else { score[item.id]++; }
                                                                }
                                                                if (keywords[i].ToLower().Contains(command[s].ToLower()))
                                                                {
                                                                    score[item.id]++;
                                                                }
                                                                else if (score[item.id] > 1)
                                                                {
                                                                    int result = (int)Math.Round((double)(score[item.id] / 2));
                                                                    if (result > 1) { score[item.id] = result; } else { score[item.id]--; }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (command[s].Contains(keywords[i]))
                                                                {
                                                                    if (s == i)
                                                                    {
                                                                        score[item.id] += 2;
                                                                    }
                                                                    else { score[item.id]++; }
                                                                }
                                                                if (command[s].ToLower().Contains(keywords[i].ToLower()))
                                                                {
                                                                    score[item.id]++;
                                                                }
                                                                else if (score[item.id] > 1)
                                                                {
                                                                    int result = (int)Math.Round((double)(score[item.id] / 2));
                                                                    if (result > 1) { score[item.id] = result; } else { score[item.id]--; }
                                                                }
                                                            }
                                                        }
                                                }
                                    }
                                }
                                catch (Exception) { }
                            }
                            ushort which = 0;
                            int record = 0;
                            foreach (ushort item in score.Keys)
                            {
                                if (score[item] > record)
                                {
                                    record = score[item];
                                    which = item;
                                }
                            }
                            if (which != 0)
                            {
                                id = which;
                                searched = true;
                                SearchCache[val] = id;
                            }
                            else
                            {
                                Rocket.Unturned.Chat.UnturnedChat.Say(player, EzBlacklists.Instance.Translate("item_no_search_results"));
                                return;
                            }

                        }
                    }
                }
                if (id != (ushort)0)
                {
                    ItemAsset gg = (ItemAsset) Assets.find(EAssetType.ITEM, id);
                    if (!searched && gg == null)
                    {
                        Rocket.Unturned.Chat.UnturnedChat.Say(player, EzBlacklists.Instance.Translate("item_not_found", id.ToString()));
                        return;
                    }
                    if(EzBlacklists.Instance.FastItemBL.TryGetValue(id, out string permNode))
                    {
                        if(!player.HasPermission(permNode))
                        {
                            Rocket.Unturned.Chat.UnturnedChat.Say(player, EzBlacklists.Instance.Translate("item_not_allowed", id.ToString()));
                            return;
                        }
                    }
                    if (amount > EzBlacklists.Instance.Configuration.Instance.maxSpawnItems && EzBlacklists.Instance.Configuration.Instance.maxSpawnItems>0) amount = EzBlacklists.Instance.Configuration.Instance.maxSpawnItems;
                    if(!ItemTool.tryForceGiveItem(player.Player, id, amount))
                    {
                        ItemManager.dropItem(new SDG.Unturned.Item(id, true), player.Position, true, true, false);
                    }
                    Rocket.Unturned.Chat.UnturnedChat.Say(player, EzBlacklists.Instance.Translate("spawned_item", amount, (iname != "" ? iname + ", " : ""), gg.name, id.ToString()));
                }
                else
                {
                    Rocket.Unturned.Chat.UnturnedChat.Say(player, EzBlacklists.Instance.Translate("no_item_found"));
                }
            }
            else
            {
                Rocket.Unturned.Chat.UnturnedChat.Say(player, "/i <Item ID>");
            }
        }
        private static bool IsValidItemId(ushort id) => Assets.find(EAssetType.ITEM, id) is ItemAsset;
    }
}
