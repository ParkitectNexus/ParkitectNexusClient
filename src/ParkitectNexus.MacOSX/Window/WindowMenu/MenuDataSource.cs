using System;
using MonoMac.AppKit;
using System.Drawing;
using MonoMac.Foundation;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.MacOSX
{

    public class MenuDataSource:NSOutlineViewDataSource
    {
        public List<MenuItem> Items {
            get;
            set;
        }

        public MenuDataSource()
        {
            Items = new List<MenuItem>();
            Items.Add(new MenuItem(){ Name = "Main Menu" });
            Items.Add(new MenuItem(){ Name = "Mods" });
//            Items.Add(new MenuItem(){ Name = "Blueprints" });
//            Items.Add(new MenuItem(){ Name = "Savegames" });
            Items.Add(new MenuItem(){ Name = "Queue" });
        }

        public override int GetChildrenCount (NSOutlineView outlineView, NSObject item)
        {
            return item == null ? Items.Count : 0;
        }

        public override NSObject GetObjectValue (NSOutlineView outlineView, NSTableColumn forTableColumn, NSObject byItem)
        {
            return (NSString)((byItem as MenuItem)?.Name ?? string.Empty);
        }

        public override NSObject GetChild (NSOutlineView outlineView, int childIndex, NSObject ofItem)
        {
            return ofItem == null ? Items[childIndex] : null;
        }

        public override bool ItemExpandable (NSOutlineView outlineView, NSObject item)
        {
            return false;
        }
    }
}
