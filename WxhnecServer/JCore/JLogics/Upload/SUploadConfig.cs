﻿using System.Drawing.Imaging;

namespace JCore
{
    //
    // Summary:
    //      UploadConfig struct
    //
    public struct SUploadConfig
    {
        public int ContentLength;

        public string[] ContentTypeList;

        public ImageFormat[] ImageFormatList;

        public string Dir;

        public string OssUrl;
    }
}