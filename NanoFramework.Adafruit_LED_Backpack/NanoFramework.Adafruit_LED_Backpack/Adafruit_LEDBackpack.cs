using System;
using System.Device.I2c;

namespace NanoFramework.Adafruit_LED_Backpack
{
    /// <summary>
    /// Base class for LED backpack devices
    /// </summary>
    public class Adafruit_LEDBackpack : IDisposable
    {
        private const byte REGISTER_BRIGHTNESS = 0xE0;
        private const byte REGISTER_BLINKRATE = 0x80;

        /// <summary>
        /// Font symbol table
        /// </summary>
        protected static readonly ushort[] AlphaFontTable =
        {
            0b0000000000000001, 0b0000000000000010, 0b0000000000000100,
            0b0000000000001000, 0b0000000000010000, 0b0000000000100000,
            0b0000000001000000, 0b0000000010000000, 0b0000000100000000,
            0b0000001000000000, 0b0000010000000000, 0b0000100000000000,
            0b0001000000000000, 0b0010000000000000, 0b0100000000000000,
            0b1000000000000000, 0b0000000000000000, 0b0000000000000000,
            0b0000000000000000, 0b0000000000000000, 0b0000000000000000,
            0b0000000000000000, 0b0000000000000000, 0b0000000000000000,
            0b0001001011001001, 0b0001010111000000, 0b0001001011111001,
            0b0000000011100011, 0b0000010100110000, 0b0001001011001000,
            0b0011101000000000, 0b0001011100000000,
            0b0000000000000000, //
            0b0000000000000110, // !    33
            0b0000001000100000, // "    34
            0b0001001011001110, // #
            0b0001001011101101, // $
            0b0000110000100100, // %
            0b0010001101011101, // &
            0b0000010000000000, // '
            0b0010010000000000, // (
            0b0000100100000000, // )
            0b0011111111000000, // *
            0b0001001011000000, // +
            0b0000100000000000, // ,
            0b0000000011000000, // -
            0b0000000000000000, // .
            0b0000110000000000, // /
            0b0000110000111111, // 0
            0b0000000000000110, // 1
            0b0000000011011011, // 2
            0b0000000010001111, // 3
            0b0000000011100110, // 4
            0b0010000001101001, // 5
            0b0000000011111101, // 6
            0b0000000000000111, // 7
            0b0000000011111111, // 8
            0b0000000011101111, // 9
            0b0001001000000000, // :
            0b0000101000000000, // ;
            0b0010010000000000, // <
            0b0000000011001000, // =
            0b0000100100000000, // >
            0b0001000010000011, // ?
            0b0000001010111011, // @
            0b0000000011110111, // A
            0b0001001010001111, // B
            0b0000000000111001, // C
            0b0001001000001111, // D
            0b0000000011111001, // E
            0b0000000001110001, // F
            0b0000000010111101, // G
            0b0000000011110110, // H
            0b0001001000000000, // I
            0b0000000000011110, // J
            0b0010010001110000, // K
            0b0000000000111000, // L
            0b0000010100110110, // M
            0b0010000100110110, // N
            0b0000000000111111, // O
            0b0000000011110011, // P
            0b0010000000111111, // Q
            0b0010000011110011, // R
            0b0000000011101101, // S
            0b0001001000000001, // T
            0b0000000000111110, // U
            0b0000110000110000, // V
            0b0010100000110110, // W
            0b0010110100000000, // X
            0b0001010100000000, // Y
            0b0000110000001001, // Z
            0b0000000000111001, // [
            0b0010000100000000, //
            0b0000000000001111, // ]
            0b0000110000000011, // ^
            0b0000000000001000, // _
            0b0000000100000000, // `
            0b0001000001011000, // a
            0b0010000001111000, // b
            0b0000000011011000, // c
            0b0000100010001110, // d
            0b0000100001011000, // e
            0b0000000001110001, // f
            0b0000010010001110, // g
            0b0001000001110000, // h
            0b0001000000000000, // i
            0b0000000000001110, // j
            0b0011011000000000, // k
            0b0000000000110000, // l
            0b0001000011010100, // m
            0b0001000001010000, // n
            0b0000000011011100, // o
            0b0000000101110000, // p
            0b0000010010000110, // q
            0b0000000001010000, // r
            0b0010000010001000, // s
            0b0000000001111000, // t
            0b0000000000011100, // u
            0b0010000000000100, // v
            0b0010100000010100, // w
            0b0010100011000000, // x
            0b0010000000001100, // y
            0b0000100001001000, // z
            0b0000100101001001, // {
            0b0001001000000000, // |
            0b0010010010001001, // }
            0b0000010100100000, // ~
            0b0011111111111111,
        };

        /// <summary>
        /// I2C connection
        /// </summary>
        public I2cDevice I2cDevice { get; }
        /// <summary>
        /// Raw symbol display buffer
        /// </summary>
        public ushort[] DisplayBuffer { get; }

        /// <summary>
        /// Creae new instance
        /// </summary>
        /// <param name="ConnSetting"></param>
        public Adafruit_LEDBackpack(I2cConnectionSettings ConnSetting)
        {
            if (ConnSetting is null)
            {
                throw new ArgumentNullException(nameof(ConnSetting));
            }

            DisplayBuffer = new ushort[8];
            I2cDevice = I2cDevice.Create(ConnSetting);
        }

        /// <summary>
        /// Start I2C and initialize display state (blink off, full brightness)
        /// </summary>
        public void Begin()
        {
            //turn on oscillator
            _ = I2cDevice.WriteByte(0x21);

            // internal RAM powers up with garbage/random values.
            // ensure internal RAM is cleared before turning on display
            // this ensures that no garbage pixels show up on the display
            // when it is turned on.
            Clear();
            Render();

            SetBlinkRate(BlinkRate.BlinkOff);

            SetBrightness(15); // max brightness
        }

        /// <summary>
        /// Set display brightness.
        /// </summary>
        /// <param name="Brightness">Brightness: 0 (min) to 15 (max).</param>
        public void SetBrightness(byte Brightness)
        {
            if (Brightness > 15)
                Brightness = 15;

            var val = (byte)(REGISTER_BRIGHTNESS | Brightness);
            _ = I2cDevice.WriteByte(val);
        }

        /// <summary>
        /// Set display blink rate.
        /// </summary>
        public void SetBlinkRate(BlinkRate Rate)
        {
            var val = (byte)(REGISTER_BLINKRATE | (byte)BlinkRate.SteadyOn | ((byte)Rate << 1));
            _ = I2cDevice.WriteByte(val);
        }

        /// <summary>
        /// Issue buffered data in RAM to display.
        /// </summary>
        public void Render()
        {
            var val = new SpanByte(new byte[] {
                (byte)0,
                (byte)(DisplayBuffer[0] & 0xFF),
                (byte)((DisplayBuffer[0] >> 8) & 0xFF),
                (byte)(DisplayBuffer[1] & 0xFF),
                (byte)((DisplayBuffer[1] >> 8) & 0xFF),
                (byte)(DisplayBuffer[2] & 0xFF),
                (byte)((DisplayBuffer[2] >> 8) & 0xFF),
                (byte)(DisplayBuffer[3] & 0xFF),
                (byte)((DisplayBuffer[3] >> 8) & 0xFF),
                (byte)(DisplayBuffer[4] & 0xFF),
                (byte)((DisplayBuffer[4] >> 8) & 0xFF),
                (byte)(DisplayBuffer[5] & 0xFF),
                (byte)((DisplayBuffer[5] >> 8) & 0xFF),
                (byte)(DisplayBuffer[6] & 0xFF),
                (byte)((DisplayBuffer[6] >> 8) & 0xFF),
                (byte)(DisplayBuffer[7] & 0xFF),
                (byte)((DisplayBuffer[7] >> 8) & 0xFF),
            });

            _ = I2cDevice.Write(val);
        }

        /// <summary>
        ///  Clear display.
        /// </summary>
        public void Clear()
        {
            DisplayBuffer[0] = 0;
            DisplayBuffer[1] = 0;
            DisplayBuffer[2] = 0;
            DisplayBuffer[3] = 0;
            DisplayBuffer[4] = 0;
            DisplayBuffer[5] = 0;
            DisplayBuffer[6] = 0;
            DisplayBuffer[7] = 0;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            I2cDevice.Dispose();
        }
    }
}
