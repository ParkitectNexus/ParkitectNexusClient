using System;
using MonoMac.AppKit;
using System.Drawing;
using ParkitectNexus.Clientmac;
using System.Collections.Generic;
using MonoMac.Foundation;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace ParkitectNexus.Client.View
{public class Person:NSObject
    {
        public string Name {
            get;
            set;
        }

        public int Age {
            get;
            set;
        }

        public List<Person> Children {
            get;
            set;
        }

        public Person (string name, int age)
        {
            Name = name;
            Age = age;
            Children = new List<Person>();
        }
    }
    public class MyDataSource:NSOutlineViewDataSource
    {
        /// The list of persons (top level)
        public List<Person> Persons {
            get;
            set;
        }
        // Constructor
        public MyDataSource()
        {
            // Create the Persons list
            Persons = new List<Person>();
        }

        public override int GetChildrenCount (NSOutlineView outlineView, NSObject item)
        {
            // If the item is not null, return the child count of our item
            if(item != null)
                return (item as Person).Children.Count;
            // Its null, that means its asking for our root element count.
            return Persons.Count();
        }

        public override NSObject GetObjectValue (NSOutlineView outlineView, NSTableColumn forTableColumn, NSObject byItem)
        {
            // Is it null? (It really shouldnt be...)
            if (byItem != null) {
                // Jackpot, typecast to our Person object
                var p = ((Person)byItem);
                // Get the table column identifier
                var ident = forTableColumn.Identifier.ToString();
                // We return the appropriate information for each column
                if (ident == "colName") {
                    return (NSString)p.Name;
                }
                if (ident == "colAge") {
                    return (NSString)p.Age.ToString();
                }
            }
            // Oh well.. errors dont have to be THAT depressing..
            return (NSString)"Not enough jQuery";
        }

        public override NSObject GetChild (NSOutlineView outlineView, int childIndex, NSObject ofItem)
        {
            // If the item is null, it's asking for a root element. I had serious trouble figuring this out...
            if(ofItem == null)
                return Persons[childIndex];
            // Return the child its asking for.
            return (NSObject)((ofItem as Person).Children[childIndex]);
        }

        public override bool ItemExpandable (NSOutlineView outlineView, NSObject item)
        {
            // Straight forward - it wants to know if its expandable.
            if(item == null)
                return false;
            return (item as Person).Children.Count > 0;
        }
    }

    public class ManageModsView : BaseView
    {
        private NSTextField _label1;
        //private NSScrollView _modsScrollView;
        private NSButton _backButton;

        public ManageModsView()
        {
            _label1 = new NSTextField(new Rectangle(18, 265, 300, 20)) {
                BackgroundColor = NSColor.Clear,
                TextColor = NSColor.Black,
                Editable = false,
                Bezeled = false,
                AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.MinYMargin,
                StringValue = "Select a mod to disable, update or uninstall it.",
                Font = NSFont.SystemFontOfSize(10)
            };

            // doesn't work:
//            _modsScrollView = new NSScrollView(new Rectangle(0, 50, 204, 210) {
//                
//            });
//
//            _modsScrollView.AutohidesScrollers = true;
//            _modsScrollView.HorizontalLineScroll = 19;
//            _modsScrollView.HorizontalPageScroll = 10;
//            _modsScrollView.VerticalLineScroll = 19;
//            _modsScrollView.VerticalPageScroll = 10;
//            _modsScrollView.UsesPredominantAxisScrolling = false;
//            _modsScrollView.AutoresizingMask = NSViewResizingMask.HeightSizable;
//
//            var ol = new NSOutlineView() {
//            };
//            ol.SetFrameSize(new Size(202, 0));
//            ol.RowSizeStyle = NSTableViewRowSizeStyle.Default;
//            ol.AutosaveTableColumns = false;
//            ol.AllowsMultipleSelection = false;
//            ol.SelectionHighlightStyle = NSTableViewSelectionHighlightStyle.SourceList;
//            ol.ColumnAutoresizingStyle = NSTableViewColumnAutoresizingStyle.LastColumnOnly;
//            ol.AutoresizingMask = new NSViewResizingMask();
//            ol.IntercellSpacing = new SizeF(3, 2);
//            ol.BackgroundColor = NSColor.Black;// NSColor.FromCatalogName("System", "_sourceListBackgroundColor");//sourceListBackgroundColor;
//            ol.GridColor = NSColor.Black;// NSColor.FromCatalogName("System", "gridColor");
//
//            var column = new NSTableColumn() {
//            };
//            column.Width = 199;
//            column.MinWidth = 16;
//            column.MaxWidth = 1000;
//
//            column.HeaderCell = new NSTableHeaderCell() {//"headerCell"
//                LineBreakMode = NSLineBreakMode.TruncatingTail,
//                Bordered = true,
//                Font = NSFont.SystemFontOfSize( NSFont.SmallSystemFontSize), //smallSystem
//                TextColor = NSColor.White,// NSColor.FromCatalogName("System", "headerTextColor"),
//                BackgroundColor = NSColor.Black// NSColor.FromCatalogName("System", "headerColor")
//            };
//
//            column.DataCell = new NSTextFieldCell("Text Cell") {
//                LineBreakMode = NSLineBreakMode.TruncatingTail,
//                Selectable = true,
//                Editable = true,
//                Title = "Text Cell",
//                Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize),
//                TextColor = NSColor.White,//NSColor.FromCatalogName("System", "controlTextColor"),
//                BackgroundColor =NSColor.Black// NSColor.FromCatalogName("System", "controlBackgroundColor")
//            };
//            column.ResizingMask = NSTableColumnResizing.UserResizingMask;
//
//            ol.AddColumn(column);
//
//            ds.Persons.Add(new Person("Joe Doe",10));
//            ds.Persons.Add(new Person("Joe Doe",11));
//            ds.Persons.Add(new Person("Joe Doe",12));
//            ds.Persons.Add(new Person("Joe Doe",13));
//            ds.Persons.Add(new Person("Joe Doe",14));
//            ds.Persons.Add(new Person("Joe Doe",15));
//            ds.Persons.Add(new Person("Joe Doe",16));
//            ds.Persons.Add(new Person("Joe Doe",17));
//            ds.Persons.Add(new Person("Joe Doe",18));
//            ds.Persons.Add(new Person("Joe Doe",19));
//            ds.Persons.Add(new Person("Joe Doe",20));
//            ds.Persons.Add(new Person("Joe Doe",21));
//            ds.Persons.Add(new Person("Joe Doe",22));
//            ds.Persons.Add(new Person("Joe Doe",23));
//            ds.Persons.Add(new Person("Joe Doe",24));
//            ds.Persons.Add(new Person("Joe Doe",25));
//            ds.Persons.Add(new Person("Joe Doe",26));
//            ds.Persons.Add(new Person("Joe Doe",27));
//            ds.Persons.Add(new Person("Joe Doe",28));
//            ol.ReloadData();

            _backButton = new NSButton(new Rectangle(399, 10, 81, 32)) {
                AutoresizingMask = NSViewResizingMask.MinYMargin,
                BezelStyle = NSBezelStyle.Rounded,
                Title = "< Back",
                Bordered = true,
            };

            AddSubview(_label1);
            AddSubview(_modsScrollView);
            AddSubview(_backButton);

            AwakeFromNib();
        }

        public static object vw;
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            _backButton.Activated += (sender, e) =>
            {
                Window.ContentView.ReplaceSubviewWith(this, new MainMenuView());
            };
        }
    }
}

