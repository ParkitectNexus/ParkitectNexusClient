using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ParkitectNexus.Data;

namespace ParkitectNexus.Mod.ModLoader
{
    public class Main
    {
        public static void Load()
        {
            Data.Parkitect pn = new Data.Parkitect();

            foreach (ParkitectMod mod in pn.InstalledMods)
            {
                // try catch just to be sure that one mod doesn't crash the rest
                try
                {
                    Assembly asm = Assembly.LoadFile(mod.AssemblyPath);

                    Type type = asm.GetType(mod.NameSpace + '.' + mod.ClassName);

                    IMod userMod = Activator.CreateInstance(type) as IMod;

                    if (userMod != null)
                        ModManager.Instance.addMod(userMod);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}
