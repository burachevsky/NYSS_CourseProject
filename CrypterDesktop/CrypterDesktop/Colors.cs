using System.Windows.Controls;
using System.Windows.Media;

namespace CrypterDesktop
{
    public static class Colors
    {
        public static Brush ValidationSuccessBrush => Brushes.Green;
        public static Brush ValidationSuccessFocusBrush => Brushes.MediumSeaGreen;
        public static Brush ValidationFaultBrush => Brushes.Red;
        public static Brush ValidationFaultFocusBrush => Brushes.IndianRed;

        public static void InitTextBoxBorderColor(TextBox textBox, bool isValid)
        {
            textBox.BorderBrush = textBox.IsFocused
                ? isValid
                    ? ValidationSuccessFocusBrush
                    : ValidationFaultFocusBrush
                : isValid
                    ? ValidationSuccessBrush
                    : ValidationFaultBrush;
        }
    }
}
