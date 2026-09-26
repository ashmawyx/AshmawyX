using System.Windows.Forms;

namespace AshmawyX
{
    public static class ChatBlockFilter
    {
        public static bool ShouldBlock(int vkCode)
        {
            switch (vkCode)
            {
                case (int)Keys.Enter:
                case (int)Keys.OemPeriod:
                case (int)Keys.Oemcomma:
                case (int)Keys.OemQuestion:   // /
                case (int)Keys.OemSemicolon: // ;
                case (int)Keys.OemQuotes:    // '
                case (int)Keys.Oemtilde:     // `
                    return true;

                default:
                    return false;
            }
        }
    }
}
