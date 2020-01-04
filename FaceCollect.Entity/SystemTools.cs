using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Media;
using System.Reflection;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FaceWatcher.Entitys
{
    public class SystemTools
    {
        private static object obj = new object();
        private static string LogFileName = "logInfo.txt";
        public static Action<string,bool> PrintInfoHook = delegate { };
        private static FileStream fileStream;
        private static SoundPlayer player = new SoundPlayer();

        private static void PrintInfoToFile(string str,bool addTimeInfo=true)
        {
            lock (obj)
            {
                if (fileStream == null)
                {
                    fileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, LogFileName), FileMode.OpenOrCreate);
                }
                var stream = fileStream;
                stream.Position = stream.Length;
                byte[] bytes = null;
                if (addTimeInfo)
                {
                    bytes = Encoding.UTF8.GetBytes(string.Format("{0} \r\n{1} \r\n\r\n", DateTime.Now, str));
                }
                else
                {
                    bytes = Encoding.UTF8.GetBytes(str);
                }
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
            }
         
        }

        private static void PrintInfoToFile(string str, bool addTimeInfo = false,params object[] objs)
        {
            var msg = string.Format(str, objs);
            lock (obj)
            {
                PrintInfoToFile(msg, addTimeInfo);
            }
        }

        public static SynchronizationContext SynchronizationContext { get; set; }

        public static void UIThreadExeAction(Action action, bool synchronization = false)
        {
            if (SynchronizationContext.Current == null && SynchronizationContext != null)
            {
                if (synchronization)
                    SynchronizationContext.Send((e) => action(), null);
                else
                    SynchronizationContext.Post((e) => action(), null);
                return;
            }
            else
            {
                action();
            }
        }

        public static string GetPropertyName<T>(Expression<Func<T, object>> expr)
        {
            string rtn = string.Empty;
            if (expr.Body is UnaryExpression)
            {
                rtn = ((MemberExpression)((UnaryExpression)expr.Body).Operand).Member.Name;
            }
            else if (expr.Body is MemberExpression)
            {
                rtn = ((MemberExpression)expr.Body).Member.Name;
            }
            else if (expr.Body is ParameterExpression)
            {
                rtn = ((ParameterExpression)expr.Body).Type.Name;
            }
            return rtn;
        }

    

        /// <summary>
        /// 打印程序信息，信息会自动被格式化，增加时间等，默认控制台输出。
        /// </summary>
        /// <param name="info">需要打印的信息</param>
        /// <param name="isErrorInfo">是否为错误信息</param>
        /// <param name="writeToFile">是否写入日志文件</param>
        public static void PrintInfo(string info,bool isErrorInfo=false,bool writeToFile= false)
        {
            var split = isErrorInfo ? "××" : "--";
            var format = $"[ {DateTime.Now}] [{Thread.CurrentThread.ManagedThreadId}] {split}{info}\r\n ";
            PrintInfoHook(format,isErrorInfo);
            if (writeToFile)
            {
                Action action=()=>  PrintInfoToFile(format);
                Task.Run(action);
            }
            Console.Write(format);
        }

        /// <summary>
        /// 打印错误信息，会自动写入文件。
        /// </summary>
        /// <param name="errorInfo"></param>
        public static void PrintErrorInfo(string errorInfo)
        {
            PrintInfo(errorInfo, true, true);
        }

        public static byte[] GetResurce(Assembly ass,string resurceName)
        {
            var res = ass.GetManifestResourceNames();
            var cfg =res .FirstOrDefault(e => e.EndsWith(resurceName));
            if (!string.IsNullOrEmpty(cfg))
            {
                var stream = ass.GetManifestResourceStream(cfg);
                var bytes = new byte[stream.Length];
                var flag = 0;
                while (flag < stream.Length)
                {
                    flag += stream.Read(bytes, flag, bytes.Length - flag);
                }
                stream.Close();
                return bytes;
            }
            return null;
        }

        public static void Speek(string str)
        {
            System.Speech.Synthesis.SpeechSynthesizer speech = new System.Speech.Synthesis.SpeechSynthesizer();
            var cs = CultureInfo.GetCultureInfo("zh-CN");
            InstalledVoice neededVoice = speech.GetInstalledVoices(CultureInfo.CurrentCulture).FirstOrDefault();
            var phrase = str;
            speech.Rate = 2;
            if (neededVoice == null)
            {
                phrase = "Unsupported Language";
            }
            else if (!neededVoice.Enabled)
            {
                phrase = "Voice Disabled";
            }
            else
            {
                speech.SelectVoice(neededVoice.VoiceInfo.Name);
            }
            speech.SpeakAsync(phrase);
            speech.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>((e, ee) => speech.Dispose());
        }

        public static void PlaySound(string filePath)
        {
            //  Musicplayer.PlayMusic(filePath);
            if(player.SoundLocation!=filePath)
               player.SoundLocation = filePath;
            player.PlaySync();

        }
    }




}
