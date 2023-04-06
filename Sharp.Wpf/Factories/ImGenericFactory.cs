﻿using System;

namespace Sharp.ImGui.Wpf.Factories
{
    internal class ImGenericFactory<TControl> : IImGuiControlFactory where TControl : IImGuiControl
    {
        public IImGuiControl CreateNew()
        {
            return Activator.CreateInstance<TControl>();
        }
    }
}
