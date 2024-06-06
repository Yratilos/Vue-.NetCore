using System;

namespace WebApi.IUtils
{
    public interface IScreenRecorder
    {
        /// <summary>
        /// 开启前关闭原有进程
        /// </summary>
        public void Kill();

        /// <summary>
        /// 手动开启录制
        /// </summary>
        /// <param name="outputPath">文件名</param>
        /// <param name="framerate">帧率</param>
        public void StartRecording(string outputPath, int framerate, string fileName = "ffmpeg\\ffmpeg");

        /// <summary>
        /// 手动关闭
        /// </summary>
        /// <returns>地址</returns>
        public string StopRecording(string outputPath);
        [Obsolete]
        public void RecordScreen(string outputPath, int duration);
    }
}
