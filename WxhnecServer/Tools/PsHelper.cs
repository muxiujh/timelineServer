using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace WxhnecServer.Tools
{
    public class PsHelper
    {
        static public ImageFormat GetImageFormat(string path) {
            if (!FileHelper.CheckFile(path)) {
                return null;
            }

            ImageFormat format = null;
            Image image = null;
            try {
                image = Image.FromFile(path);
                format = getImageFormatRaw(image);
            }
            catch { }
            finally {
                if (image != null) {
                    image.Dispose();
                }
            }
            return format;
        }

        static public bool Thumb(string path, string pathDest, int widthMax = 600, int heightMax = 600, long quality = 80) {
            bool result = false;
            while (true) {
                if (!FileHelper.CheckFile(path)) {
                    break;
                }

                if (!FileHelper.CheckDir(pathDest, true)) {
                    break;
                }

                Image image = null;
                Bitmap bitmap = null;
                Graphics graphics = null;
                try {
                    // source
                    image = Image.FromFile(path);
                    if(image == null) {
                        break;
                    }

                    // dest
                    Size size = calcSize(image.Size, new Size(widthMax, heightMax));
                    bitmap = new Bitmap(size.Width, size.Height, image.PixelFormat);
                    graphics = Graphics.FromImage(bitmap);
                    graphics.Clear(Color.Transparent);

                    // souce to dest
                    graphics.DrawImage(image, new Rectangle(0, 0, size.Width, size.Height));

                    // save

                    ImageCodecInfo imageCodecInfo = getImageCodecInfo(getImageFormatRaw(image));
                    if(imageCodecInfo != null) {
                        EncoderParameters encoderParameters = new EncoderParameters();
                        encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality); // 1 ~ 100 default 80
                        bitmap.Save(pathDest, imageCodecInfo, encoderParameters);
                    }
                    else {
                        bitmap.Save(pathDest);
                    }
                }
                catch {
                    break;
                }
                finally {
                    if (image != null) {
                        image.Dispose();
                    }
                    if (bitmap != null) {
                        bitmap.Dispose();
                    }
                    if (graphics != null) {
                        graphics.Dispose();
                    }
                }

                result = true;
                break;
            }
            return result;
        }

        static ImageFormat getImageFormatRaw(Image image) {
            if (image == null) {
                return null;
            }

            ImageFormat format = null;
            if (image.RawFormat.Equals(ImageFormat.Png)) {
                format = ImageFormat.Png;
            }
            else if (image.RawFormat.Equals(ImageFormat.Jpeg)) {
                format = ImageFormat.Jpeg;
            }
            else if (image.RawFormat.Equals(ImageFormat.Gif)) {
                format = ImageFormat.Gif;
            }
            return format;
        }

        static ImageCodecInfo getImageCodecInfo(ImageFormat format) {
            if (format == null) {
                return null;
            }

            ImageCodecInfo result = null;
            string formatStr = format.ToString().ToUpper();
            ImageCodecInfo[] imageCodecInfoList = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo info in imageCodecInfoList) {
                if (formatStr == info.FormatDescription) {
                    result = info;
                    break;
                }
            }
            return result;
        }

        static Size calcSize(Size size, Size sizeMax) {
            float ratio = (float)size.Width / size.Height;
            float ratioMax = (float)sizeMax.Width / sizeMax.Height;

            if (ratio > ratioMax) {
                if (size.Width > sizeMax.Width) {
                    size.Width = sizeMax.Width;
                    size.Height = (int)Math.Floor((float)size.Width / ratio + 0.5);
                }
            }
            else {
                if (size.Height > sizeMax.Height) {
                    size.Height = sizeMax.Height;
                    size.Width = (int)Math.Floor((float)size.Height * ratio + 0.5);
                }
            }
            return size;
        }

    }
}