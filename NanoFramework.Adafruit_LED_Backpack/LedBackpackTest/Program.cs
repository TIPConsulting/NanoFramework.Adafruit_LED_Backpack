using nanoFramework.Hardware.Esp32;
using NanoFramework.Adafruit_LED_Backpack;
using System;
using System.Device.I2c;
using System.Diagnostics;
using System.Threading;
using Windows.Devices.SerialCommunication;

namespace LedBackpackTest
{
    public class Program
    {
        public static void Main()
        {
            Debug.WriteLine(SerialDevice.GetDeviceSelector());

            Configuration.SetPinFunction(23, DeviceFunction.I2C1_DATA);
            Configuration.SetPinFunction(22, DeviceFunction.I2C1_CLOCK);

            Adafruit_AlphaNum4 led = new Adafruit_AlphaNum4(new I2cConnectionSettings(1, 0x70));
            led.Begin();  // pass in the address

            led.WriteDigitRaw(0, 0xFFFF);
            led.WriteDigitRaw(1, 0xFFFF);
            led.WriteDigitRaw(2, 0xFFFF);
            led.WriteDigitRaw(3, 0xFFFF);
            led.Render();
            Thread.Sleep(1000);

            led.Clear();
            led.Render();

            // display every character, 
            for (char i = '!'; i <= 'z'; ++i)
            {
                Debug.WriteLine(i.ToString());
                led.WriteDigitAscii(0, i);
                led.WriteDigitAscii(1, (char)(i + 1));
                led.WriteDigitAscii(2, (char)(i + 2));
                led.WriteDigitAscii(3, (char)(i + 3));
                led.Render();

                Thread.Sleep(300);
            }

        }


        //char displaybuffer[4] = { ' ', ' ', ' ', ' ' };

        //void loop()
        //{
        //    while (!Serial.available()) return;

        //    char c = Serial.read();
        //    if (!isprint(c)) return; // only printable!

        //    // scroll down display
        //    displaybuffer[0] = displaybuffer[1];
        //    displaybuffer[1] = displaybuffer[2];
        //    displaybuffer[2] = displaybuffer[3];
        //    displaybuffer[3] = c;

        //    // set every digit to the buffer
        //    alpha4.writeDigitAscii(0, displaybuffer[0]);
        //    alpha4.writeDigitAscii(1, displaybuffer[1]);
        //    alpha4.writeDigitAscii(2, displaybuffer[2]);
        //    alpha4.writeDigitAscii(3, displaybuffer[3]);

        //    // write it out!
        //    alpha4.writeDisplay();
        //    delay(200);
        //}
    }
}
