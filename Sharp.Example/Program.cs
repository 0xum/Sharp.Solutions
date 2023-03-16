using System.Drawing;

namespace Sharp.Example
{
    public class Program
    {
        private static void Main ( string [ ] args )
        {
            // Creates a window with ImGui rendering inside.
            ImGuiWindow.CreateImGuiWindow ( new Rectangle ( 160, 90, 1280, 720 ) );

            // Creates a ImGui overlay on a process window.
            // ImGuiOverlay.CrateImGuiOverlay ( "notepad" );
        }
    }
}