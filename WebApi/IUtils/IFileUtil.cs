using System.Collections.Generic;
using System.Data;

namespace WebApi.IUtils
{
    public interface IFileUtil
    {
        /// <summary>
        /// Excel
        /// NPOI生成文件,不删除文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string ExportURL<T>(List<T> dataList, string fileName);
        /// <summary>
        /// Excel
        /// NPOI生成文件，读取二进制流后删除文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <param name="type">.xlsx/.xls</param>
        /// <returns></returns>
        public byte[] ExportByte<T>(List<T> dataList, string type);
        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="yzm2">大写验证码</param>
        /// <param name="yzm3">图片显示的验证码</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="codeLength">验证码个数</param>
        /// <returns>base64</returns>
        public string CreateCaptcha(out string yzm2, out string yzm3, int width = 160, int height = 40, int codeLength = 4);
        /// <summary>
        /// Excel
        /// NPOI生成文件，读取二进制流后删除文件
        /// </summary>
        /// <param name="type">.xlsx/.xls</param>
        public byte[] ExportByte(DataTable dt, string type);

        /// <summary>
        /// Excel
        /// NPOI生成文件，读取二进制流后删除文件
        /// </summary>
        /// <param name="type">.xlsx/.xls</param>
        public byte[] ExportByte(DataSet ds, string type);

        /// <summary>
        /// Excel
        /// NPOI生成文件,不删除文件
        /// </summary>
        /// <param name="fileName">包含文件及扩展名的路径</param>
        /// <returns>文件本地路径</returns>
        public string ExportURL(DataTable dt, string fileName);

        /// <summary>
        /// Excel
        /// NPOI生成文件,不删除文件
        /// </summary>
        /// <param name="fileName">包含文件及扩展名的路径</param>
        /// <returns>文件本地路径</returns>
        public string ExportURL(DataSet ds, string fileName);

    }
}
