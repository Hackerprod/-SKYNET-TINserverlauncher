using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;


namespace SKYNET
{
    public abstract class modCommon
    {


        public static Bitmap ChangeOpacity(Image img, float opacityvalue)
        {
            Bitmap bitmap4 = default(Bitmap);
            try
            {
                Bitmap bitmap = new Bitmap(img.Width, img.Height);
                Graphics graphics = Graphics.FromImage(bitmap);
                ColorMatrix newColorMatrix = new ColorMatrix
                {
                    Matrix33 = opacityvalue
                };
                ImageAttributes imageAttributes = new ImageAttributes();
                imageAttributes.SetColorMatrix(newColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                graphics.DrawImage(img, new Rectangle(0, 0, bitmap.Width, bitmap.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imageAttributes);
                graphics.Dispose();
                bitmap4 = bitmap;
                return bitmap4;
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                Exception ex2 = ex;
                ProjectData.ClearProjectError();
                return bitmap4;
            }
        }
    }



}