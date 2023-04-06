﻿using System.Windows;

namespace Sharp.ImGui.Wpf.Styles
{
    public class DefaultStyle : IImGuiStyle
    {
        public Thickness Margin { get; } = new Thickness(2);
        public Thickness Padding { get; } = new Thickness(2);
    }
}
