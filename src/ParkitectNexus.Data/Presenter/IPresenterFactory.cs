// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.Presenter
{
    public interface IPresenterFactory
    {
        T InstantiatePresenter<T>();
        T InstantiatePresenter<T>(IPresenter parent);
    }
}
