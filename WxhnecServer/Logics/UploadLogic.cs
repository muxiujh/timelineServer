using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using WxhnecServer.Logics.Enums;
using WxhnecServer.Logics.Structs;
using WxhnecServer.Tools;

namespace WxhnecServer.Logics
{

    public class UploadLogic
    {
        SUploadConfig m_config;
        public string Error { get; set; }

        public UploadLogic() {
            m_config = new SUploadConfig();
            m_config.ContentLength = 5 * 1024 * 1024;
            m_config.ContentTypeList = new string[] {
                "image/jpeg",
                "image/png"
            };
            m_config.ImageFormatList = new ImageFormat[] {
                ImageFormat.Png,
                ImageFormat.Jpeg
            };
            m_config.Dir = "e:/testpic/upload/";
            m_config.OssUrl = "http://wxhneclocal.oss-cn-hangzhou.aliyuncs.com/";
        }

        public string Upload(HttpPostedFileBase httpFile) {
            string tempFile = http2Temp(httpFile);
            string shortPath = temp2Local(tempFile);
            return shortPath;
        }

        public string Show(string shortPath, string imageSize = null) {
            string result = null;
            string sizePath = null;
            while (true) {
                if (string.IsNullOrWhiteSpace(shortPath)) {
                    Error = SUploadError.ShortPath;
                    break;
                }

                EImageSize imageSizeEnum = THelper.ConvertToEnum(imageSize, EImageSize.middle);
                int size = Convert.ToInt32(imageSizeEnum);
                string sizeShortPath = FileHelper.GetSizePath(shortPath, size);


                string sizePathTrue = m_config.Dir + sizeShortPath + ".true";
                if (File.Exists(sizePathTrue)) {
                    result = m_config.OssUrl + sizeShortPath;
                    break;
                }

                string path = m_config.Dir + shortPath;
                sizePath = m_config.Dir + sizeShortPath;

                if (!PsHelper.Thumb(path, sizePath, size, size)) {
                    Error = SUploadError.Thumb;
                    break;
                }

                AliOssLogic aliOss = new AliOssLogic();
                if (!aliOss.Add(sizeShortPath, sizePath)) {
                    Error = SUploadError.OssAdd;
                    break;
                }

                if (!FileHelper.CreateFile(sizePathTrue)) {
                    Error = SUploadError.CreateTrue;
                    break;
                }

                result = m_config.OssUrl + sizeShortPath;
                break;
            }

            FileHelper.DeleteFile(sizePath);
            return result;
        }

        string http2Temp(HttpPostedFileBase httpFile) {
            string result = null;
            while (true) {
                if(httpFile == null) {
                    Error = SUploadError.NoHttpFile;
                    break;
                }

                // check ContentLength
                if (httpFile.ContentLength > m_config.ContentLength) {
                    Error = SUploadError.ContentLength;
                    break;
                }

                // check ContentType
                if (!m_config.ContentTypeList.Contains(httpFile.ContentType)) {
                    Error = SUploadError.ContentType;
                    break;
                }

                // name
                string name = FileHelper.GetRandomName();

                // dir, path
                string dir = m_config.Dir + FileHelper.GetDateDir();
                if (!FileHelper.CheckDir(dir)) {
                    Error = SUploadError.Dir;
                    break;
                }
                string path = dir + name + httpFile.FileName;

                // save
                try {
                    httpFile.SaveAs(path);
                }
                catch {
                    Error = SUploadError.HttpSave;
                    break;
                }

                result = path;
                break;
            }
            return result;
        }

        string temp2Local(string tempFile, bool deleteTempFile = true) {
            string result = null;
            while (true) {
                if (!FileHelper.CheckFile(tempFile)) {
                    Error = SUploadError.NoLocalFile;
                    break;
                }

                // check ImageType, get ext
                ImageFormat format = PsHelper.GetImageFormat(tempFile);
                if (format == null || !m_config.ImageFormatList.Contains(format)) {
                    Error = SUploadError.ImageFormat;
                    break;
                }

                // name
                string md5 = FileHelper.GetMD5(tempFile);
                if(md5 == null) {
                    Error = SUploadError.MD5;
                    break;
                }
                string name = FileHelper.SubMD5(md5);

                // dir, path
                string shortDir = FileHelper.GetDateDir();
                string shortPath = FileHelper.GetPath(shortDir, name, format.ToString());

                // thumb
                if(!PsHelper.Thumb(tempFile, m_config.Dir + shortPath)) {
                    Error = SUploadError.Thumb;
                    break;
                }

                result = shortPath;
                break;
            }

            if (deleteTempFile) {
                FileHelper.DeleteFile(tempFile);
            }
            return result;
        }

    }
}