using System.Windows.Forms;

namespace AshmawyX
{
    public static class KeyRoutingRules
    {
        // Permanent blocked keys (never mirrored)
        public static bool IsBlocked(int vkCode)
        {
            switch (vkCode)
            {
                // Movement keys
                case (int)Keys.W:
                case (int)Keys.A:
                case (int)Keys.S:
                case (int)Keys.D:

                // Extra movement / crouch / misc
                case (int)Keys.X:
                case (int)Keys.Z:
                case (int)Keys.C:

                // Space (jump)
                case (int)Keys.Space:

                // Slash '/'
                case (int)Keys.OemQuestion:

                // Chat / system keys
                case (int)Keys.Enter:
                case (int)Keys.Tab:

                    return true;

                default:
                    return false;
            }
        }
    }
}
