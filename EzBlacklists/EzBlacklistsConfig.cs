using Rocket.API;
using Rocket.Core.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace EzBlackLists
{
    public class EzBlacklistsConfig : IRocketPluginConfiguration
    {
        public List<int> vehicles { get; set; }
        public List<List<int>> items { get; set; }
        public List<int> itemz { get; set; }
        public List<List<int>> vehs { get; set; }
        public List<string> VehPerms { get; set; }
        public List<string> ItemPerms { get; set; }
        public byte maxSpawnItems { get; set; }
        public string separator{ get; set; }
        public string namesep { get; set; }
        public bool LoadMain { get; set; }
        public void LoadDefaults()
        {
            LoadMain = false;
            maxSpawnItems = 10;
            vehicles = new List<int>();
            itemz = new List<int>();
            items = new List<List<int>>();
            vehs = new List<List<int>>();
            ItemPerms = new List<string>();
            VehPerms = new List<string>();
            separator = ",";
            namesep = "_";
            char[] sep= { };
            for(int i = 0; i < separator.Length; i++)
            {
                sep[sep.Length] = (char)separator[i];
            }
        }
    }
}
