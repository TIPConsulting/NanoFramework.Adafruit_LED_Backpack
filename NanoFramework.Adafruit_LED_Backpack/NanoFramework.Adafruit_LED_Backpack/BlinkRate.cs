using System;
namespace NanoFramework.Adafruit_LED_Backpack
{
    /// <summary>
    /// Blink setting values. Register 0x80
    /// </summary>
    public enum BlinkRate : byte
    {
        /// <summary>
        /// I2C value for steady on
        /// </summary>
        SteadyOn = 0x01,
        /// <summary>
        /// I2C value for steady off
        /// </summary>
        SteadyOff = 0,
        /// <summary>
        /// Blink disabled
        /// </summary>
        BlinkOff = 0,
        /// <summary>
        /// I2C value for 2 Hz blink
        /// </summary>
        Blink2hz = 1,
        /// <summary>
        /// I2C value for 1 Hz blink
        /// </summary>
        Blink1hz = 2,
        /// <summary>
        /// I2C value for 0.5 Hz blink
        /// </summary>
        BlinkHalfhz = 3
    }
}
