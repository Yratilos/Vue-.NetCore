using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using WebApi.IUtils;

namespace WebApi.Utils
{
    public class ScreenRecorder : IScreenRecorder
    {
        private Process process;
        [Obsolete]
        public static void Run()
        {
            string outputPath = "output.mp4";
            Console.WriteLine("输入start开始录制");
            ScreenRecorder s = new ScreenRecorder();
            s.Kill();
            var st = Console.ReadLine();
            if (st == "start")
            {
                bool isRecording = true;
                s.StartRecording(outputPath, 30);
                Thread timeThread = new Thread(() =>
                {
                    int seconds = 0;

                    while (isRecording)
                    {
                        Console.Clear();

                        Console.WriteLine($"录制中:{seconds}");
                        Console.WriteLine("任意键结束录制");

                        Thread.Sleep(1000);
                        seconds++;
                    }
                });
                timeThread.IsBackground = true;
                timeThread.Start();
                Console.ReadKey();
                isRecording = false;
                var path = s.StopRecording(outputPath);
                Console.WriteLine("");
                Console.WriteLine("结束录制");
                Console.WriteLine($"视频地址:{path}");
            }
            Console.ReadKey();
        }
        public void Kill()
        {
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                if (process.ProcessName.ToLower().Contains("ffmpeg"))
                {
                    process.Kill();
                }
            }
        }
        /// <summary>
        /// 手动开启录制
        /// </summary>
        /// <param name="outputPath">文件名</param>
        /// <param name="framerate">帧率</param>
        public void StartRecording(string outputPath, int framerate, string fileName = "ffmpeg\\ffmpeg")
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = $"-y -f gdigrab -framerate {framerate} -i desktop -c:v libx264 -pix_fmt yuv420p \"{outputPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardInput = true, // 添加对标准输入流的重定向设置
                UseShellExecute = false,
                CreateNoWindow = true
            };

            process = new Process { StartInfo = startInfo };
            process.Start();
        }
        /// <summary>
        /// 手动关闭
        /// </summary>
        public string StopRecording(string outputPath)
        {
            if (process != null && !process.HasExited)
            {
                process.StandardInput.WriteLine("q");
                process.StandardInput.Close(); // 关闭标准输入流

                process.WaitForExit();
            }
            return Path.GetFullPath(outputPath);
        }
        /// <summary>
        /// 设置录制时间自动录制
        /// </summary>
        /// <param name="outputPath"></param>
        /// <param name="duration"></param>
        [Obsolete]
        public void RecordScreen(string outputPath, int duration)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "Utils\\ffmpeg\\ffmpeg",
                Arguments = $"-y -t {duration} -f gdigrab -framerate 30 -i desktop -c:v libx264 -pix_fmt yuv420p \"{outputPath}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();
                process.WaitForExit();
            }
        }
    }
}