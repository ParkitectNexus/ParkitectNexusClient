// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;

namespace ParkitectNexus.ModLoader.Debug
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //ModInjector
            var r = ModInjector.Inject("C:/Users/Tim/Desktop/ParkitectModTools.dll", "ParkitectModTools", "Main", "Load");

            Console.WriteLine(r);

            Console.ReadLine();
        }
    }
}