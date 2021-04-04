using System;
using System.Device.I2c;
using System.Diagnostics;

namespace NanoFramework.Adafruit_LED_Backpack
{
    public class Adafruit_AlphaNum4 : Adafruit_LEDBackpack
    {
        /// <inheritdoc/>
        public Adafruit_AlphaNum4(I2cConnectionSettings ConnSetting) : base(ConnSetting)
        {
        }


        /// <summary>
        /// Write single character of alphanumeric display as raw bits (not a general print function).
        /// Writes to buffer, but does not print to screen
        /// </summary>
        /// <param name="Position">Character index (0-3)</param>
        /// <param name="Bitmask">Segment bitmask</param>
        public void WriteDigitRaw(byte Position, ushort Bitmask)
        {
            DisplayBuffer[Position] = Bitmask;
        }

        /// <summary>
        /// Write single ASCII character to alphanumeric display.
        /// </summary>
        /// <param name="Position">Character index (0-3)</param>
        /// <param name="AsciiChar">ASCII character</param>
        /// <param name="dot">If true, also light corresponding dot segment</param>
        public void WriteDigitAscii(byte Position, char AsciiChar, bool Dot = false)
            => WriteDigitAscii(Position, (ushort)AsciiChar, Dot);

        /// <summary>
        /// Write single ASCII character to alphanumeric display.
        /// </summary>
        /// <param name="Position">Character index (0-3)</param>
        /// <param name="AsciiChar">ASCII character</param>
        /// <param name="dot">If true, also light corresponding dot segment</param>
        public void WriteDigitAscii(byte Position, ushort AsciiChar, bool Dot = false)
        {
            ushort font = _alphaFontTable[AsciiChar];
            Debug.WriteLine(ToBinary(font).PadLeft(16));
            DisplayBuffer[Position] = font;

            if (Dot)
                DisplayBuffer[Position] |= (1 << 14);
        }

        public static string ToBinary(int n)
        {
            if (n < 2) return n.ToString();

            var divisor = n / 2;
            var remainder = n % 2;

            return ToBinary(divisor) + remainder;
        }

    }
}
