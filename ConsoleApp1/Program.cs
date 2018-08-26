﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using USBHIDDRIVER;
using System.Diagnostics;
using HidLibrary;

namespace ConsoleApp1

{
    class Program
    {
        private const int VendorId = 0x0483;
        private static readonly int ProductIds = 0x572b;
        private static int _currentProductId;
        private static HidDevice _device;
        private static bool _attached;
        static byte[] bytes ={  0xf, 0xF, 0x0, 0x2, 0x0,
                                0x06, 0x07, 0x08, 0xff, 0x10};


        enum Scancode
        {

            sc_escape = 0x01,
            sc_1 = 0x02,
            sc_2 = 0x03,
            sc_3 = 0x04,
            sc_4 = 0x05,
            sc_5 = 0x06,
            sc_6 = 0x07,
            sc_7 = 0x08,
            sc_8 = 0x09,
            sc_9 = 0x0A,
            sc_0 = 0x0B,
            sc_minus = 0x0C,
            sc_equals = 0x0D,
            sc_backspace = 0x0E,
            sc_tab = 0x0F,
            sc_q = 0x10,
            sc_w = 0x11,
            sc_e = 0x12,
            sc_r = 0x13,
            sc_t = 0x14,
            sc_y = 0x15,
            sc_u = 0x16,
            sc_i = 0x17,
            sc_o = 0x18,
            sc_p = 0x19,
            sc_bracketLeft = 0x1A,
            sc_bracketRight = 0x1B,
            sc_enter = 0x1C,
            sc_controlLeft = 0x1D,
            sc_a = 0x1E,
            sc_s = 0x1F,
            sc_d = 0x20,
            sc_f = 0x21,
            sc_g = 0x22,
            sc_h = 0x23,
            sc_j = 0x24,
            sc_k = 0x25,
            sc_l = 0x26,
            sc_semicolon = 0x27,
            sc_apostrophe = 0x28,
            sc_grave = 0x29,
            sc_shiftLeft = 0x2A,
            sc_backslash = 0x2B,
            sc_z = 0x2C,
            sc_x = 0x2D,
            sc_c = 0x2E,
            sc_v = 0x2F,
            sc_b = 0x30,
            sc_n = 0x31,
            sc_m = 0x32,
            sc_comma = 0x33,
            sc_preiod = 0x34,
            sc_slash = 0x35,
            sc_shiftRight = 0x36,
            sc_numpad_multiply = 0x37,
            sc_altLeft = 0x38,
            sc_space = 0x39,
            sc_capsLock = 0x3A,
            sc_f1 = 0x3B,
            sc_f2 = 0x3C,
            sc_f3 = 0x3D,
            sc_f4 = 0x3E,
            sc_f5 = 0x3F,
            sc_f6 = 0x40,
            sc_f7 = 0x41,
            sc_f8 = 0x42,
            sc_f9 = 0x43,
            sc_f10 = 0x44,
            sc_numLock = 0x45,
            sc_scrollLock = 0x46,
            sc_numpad_7 = 0x47,
            sc_numpad_8 = 0x48,
            sc_numpad_9 = 0x49,
            sc_numpad_minus = 0x4A,
            sc_numpad_4 = 0x4B,
            sc_numpad_5 = 0x4C,
            sc_numpad_6 = 0x4D,
            sc_numpad_plus = 0x4E,
            sc_numpad_1 = 0x4F,
            sc_numpad_2 = 0x50,
            sc_numpad_3 = 0x51,
            sc_numpad_0 = 0x52,
            sc_numpad_period = 0x53,
            sc_alt_printScreen = 0x54, /* Alt + print screen. MapVirtualKeyEx( VK_SNAPSHOT, MAPVK_VK_TO_VSC_EX, 0 ) returns scancode 0x54. */
            sc_bracketAngle = 0x56, /* Key between the left shift and Z. */
            sc_f11 = 0x57,
            sc_f12 = 0x58,
            sc_oem_1 = 0x5a, /* VK_OEM_WSCTRL */
            sc_oem_2 = 0x5b, /* VK_OEM_FINISH */
            sc_oem_3 = 0x5c, /* VK_OEM_JUMP */
            sc_eraseEOF = 0x5d,
            sc_oem_4 = 0x5e, /* VK_OEM_BACKTAB */
            sc_oem_5 = 0x5f, /* VK_OEM_AUTO */
            sc_zoom = 0x62,
            sc_help = 0x63,
            sc_f13 = 0x64,
            sc_f14 = 0x65,
            sc_f15 = 0x66,
            sc_f16 = 0x67,
            sc_f17 = 0x68,
            sc_f18 = 0x69,
            sc_f19 = 0x6a,
            sc_f20 = 0x6b,
            sc_f21 = 0x6c,
            sc_f22 = 0x6d,
            sc_f23 = 0x6e,
            sc_oem_6 = 0x6f, /* VK_OEM_PA3 */
            sc_katakana = 0x70,
            sc_oem_7 = 0x71, /* VK_OEM_RESET */
            sc_f24 = 0x76,
            sc_sbcschar = 0x77,
            sc_convert = 0x79,
            sc_nonconvert = 0x7B, /* VK_OEM_PA1 */

           
           
        };

        static void Main(string[] args)
        {
            /*USBHIDDRIVER.USBInterface usbI = new USBInterface("vid_0483","pid_572b");
            String[] list = usbI.getDeviceList();
            Console.WriteLine(bytes.Count());
            Console.WriteLine(usbI.Connect());
            Console.ReadKey();

            Console.WriteLine(usbI.write(bytes));
            Console.ReadKey();
            usbI.Disconnect();*/
            _device = HidDevices.Enumerate(VendorId, ProductIds).FirstOrDefault();
            if(_device != null) 
                { 

                Console.ReadKey();
                _device.OpenDevice();
                HidReport hidReport = new HidReport(10);
                hidReport.ReportId = 4;
                hidReport.Data = bytes;
                Console.WriteLine("Write Data:");

                hidReport.Data[0] = Byte.Parse( Console.ReadLine().ToString());
                Console.WriteLine("Report Builded");
                Console.ReadKey();

                bool sended = _device.WriteReport(hidReport);
                Console.WriteLine(sended + "");
                _device.ReadReport(OnReport);
                Console.ReadKey();
                _device.CloseDevice();
            }
            
            Console.WriteLine("Disconected");
            Console.ReadKey();



        }

        private static void OnReport(HidReport report)
        {
            if (!_device.IsConnected) { return; }

            for(int i =0; i< report.Data.Length; i++)
            {
                Console.Write(report.Data[i].ToString());
            }
            

            _device.ReadReport(OnReport);
        }

       
    }
}
