using System;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Assets.Modding;

namespace ParkitectNexus.Client.Linux
{
        [Gtk.TreeNode (ListOnly=true)]
        public class TreeNodeModContainer : Gtk.TreeNode
        {
             private IModAsset _parkitectMod;
             public TreeNodeModContainer(IModAsset parkitectMod)
            {
                this._parkitectMod = parkitectMod;
                AvaliableVersion= "-";
            }

        public IModAsset ParkitectMod
            {
                get{ return _parkitectMod; }
            }


            [Gtk.TreeNodeValue(Column=3)]
            public string AvaliableVersion{ get; set;}

            [Gtk.TreeNodeValue (Column=2)]
            public string Version
            { 
                get
                { 
                return _parkitectMod.Tag ?? "-";
                }
            }

            [Gtk.TreeNodeValue (Column=1)]
            public string Name
            { 
                get
                { 
                    return _parkitectMod.Name;
                }
            }

            [Gtk.TreeNodeValue (Column=0)]
            public bool IsActive{ 
                get{ 
                return _parkitectMod.Information.IsEnabled;
                } 
                set
                { 
                _parkitectMod.Information.IsEnabled = value; 
                //_parkitectMod.Information..Save ();
                }
            }
        }
    }


