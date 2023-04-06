using System.Windows;

namespace Sharp.ImGui.Wpf
{
    public interface IImGuiStyle
    {
        Thickness Margin { get; }
        Thickness Padding { get; }
    }
}
