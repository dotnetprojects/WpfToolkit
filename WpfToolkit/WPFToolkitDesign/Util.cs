//---------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
//---------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Windows.Controls;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;

namespace Microsoft.Windows.Controls.Design 
{
    internal static class Util 
    {
        private const string ResourceLocationBase = @"/WPFToolkit.Design;component";

        internal static Image GetImage(string imageName, Size desiredSize) 
        {
            System.Uri resourceLocater = new System.Uri(ResourceLocationBase + "/Images/" + imageName, System.UriKind.RelativeOrAbsolute);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = resourceLocater;
            bi.EndInit();

            Image image = new Image();
            image.Source = bi;
            image.Width = desiredSize.Width;
            image.Height = desiredSize.Height;
            return image;
        }
    }
}
