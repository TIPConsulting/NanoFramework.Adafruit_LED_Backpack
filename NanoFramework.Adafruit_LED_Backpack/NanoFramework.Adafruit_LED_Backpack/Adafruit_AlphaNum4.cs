using System;
using System.Device.I2c;

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
        public void WriteDigitRaw(byte Position, short Bitmask)
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
        {
            short font = _alphaFontTable[AsciiChar];
            DisplayBuffer[Position] = font;

            if (Dot)
                DisplayBuffer[Position] |= (1 << 14);
        }

    }
}
