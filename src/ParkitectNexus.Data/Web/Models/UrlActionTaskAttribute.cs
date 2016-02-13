// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;

namespace ParkitectNexus.Data.Web.Models
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UrlActionTaskAttribute : Attribute
    {
        public UrlActionTaskAttribute(Type taskType)
        {
            TaskType = taskType;
        }

        public Type TaskType { get; }
    }
}