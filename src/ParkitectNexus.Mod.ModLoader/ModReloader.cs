// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using UnityEngine;

namespace ParkitectNexus.Mod.ModLoader
{
    public class ModReloader : MonoBehaviour
    {
        public ModLoader ModLoader { get; set; }

        private void Update()
        {
            if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) &&
                Input.GetKeyDown(KeyCode.R))
            {
                if (ModLoader == null)
                    return;

                ModLoader.LoadMods();
            }
        }
    }
}