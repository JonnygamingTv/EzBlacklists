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
        public bool LoadMain { get; set; }
        public byte maxSpawnItems { get; set; }
        public string separator{ get; set; }
        public string namesep { get; set; }
        public List<List<ushort>> vehicles { get; set; }
        public List<List<ushort>> items { get; set; }
        public List<string> VehPerms { get; set; }
        public List<string> ItemPerms { get; set; }
        public void LoadDefaults()
        {
            LoadMain = false;
            maxSpawnItems = 10;
            separator = ",";
            namesep = "_";
            vehicles = new List<List<ushort>>();
            items = new List<List<ushort>>();
            ItemPerms = new List<string>();
            VehPerms = new List<string>();
        }
    }
}
