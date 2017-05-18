using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace JCore
{
    public class PsHelper
    {
        static public void Text2Image(Stream stream, string text, int size = 12) {
            if (string.IsNullOrEmpty(text) || stream == null) {
                return;
            }

            Bitmap image = null;
            Graphics graphics = null;
            try {
                // graphics
                var textWidth = text.Length * size;
                var width = (int)(textWidth * 1.2);
                var height = (int)(size * 2.5);
                image = new Bitmap(width, height);
                graphics = Graphics.FromImage(image);

                // background
                Color colorBg = Color.FromArgb(196, 185, 163);
                graphics.Clear(colorBg);

                // text
                Font font = new Font("Arial", size, FontStyle.Regular);
                Color color = Color.White;
                Brush brush = new SolidBrush(color);
                var left = (width - textWidth) / 2;
                var top = (float)((height - size * 1.6) / 2);
                graphics.DrawString(text, font, brush, left, top);

                // save
                image.Save(stream, ImageFormat.Jpeg);
            }
            finally {
                if (graphics != null) {
                    graphics.Dispose();
                }

                if (image != null) {
                    image.Dispose();
                }
            }
        }

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