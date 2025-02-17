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
            vehicles = new List<List<ushort>> {
                new List<ushort>
                {
                    33, 108
                },
                new List<ushort> {
                    109
                }
            };
            items = new List<List<ushort>>
            {
                new List<ushort>
                {
                    99, 132
                },
                new List<ushort>
                {
                    519, 520
                }
            };
            ItemPerms = new List<string>
            {
                "bypass.one",
                "bypass.two"
            };
            VehPerms = new List<string>
            {
                "bypass2.one",
                "bypass2.two"
            };
        }
    }
}
