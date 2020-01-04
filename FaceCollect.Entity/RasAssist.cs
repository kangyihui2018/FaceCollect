using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Internal;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.Threading.Tasks;

namespace FaceWatcher.Entitys
{
    /// <summary>
    /// WCF远程调用封装类
    /// </summary>
    public class RasAssist
    {
        private static MethodInfo mInfo = typeof(BindingCollectionElement).GetMethod("GetDefault",
                                                                                     BindingFlags.NonPublic |
                                                                                     BindingFlags.Instance);
        private static MethodInfo methodInfo = typeof(BehaviorExtensionElement).GetMethod("CreateBehavior", BindingFlags.Instance | BindingFlags.NonPublic);
        private const string BINDING_CONFIG_NODENAME = @"system.serviceModel/bindings";
        private const string BEHAVIOR_CONFIG_NODENAME = @"system.serviceModel/behaviors";
        private static BindingsSection bindingSection;
        private static string wcfConfigPath = AppDomain.CurrentDomain.BaseDirectory + "wcfService.config";

       public static IPAddress ServerIP
        {
            get { return IPAddress.Parse(ConfigurationManager.AppSettings["wcfServiceIp"]); }
        }

       public static int ServerPort
        {
            get { return int.Parse(ConfigurationManager.AppSettings["wcfServicePort"]); }
        }

      public  static IPEndPoint ServerEndPoint
        {
            get { return new IPEndPoint(ServerIP, ServerPort); }
        }
        /// <summary>
        /// 默认的绑定类型
        /// </summary>
        private const string BindingType_Default = "netTcpBinding";
        /// <summary>
        /// 默认的url前缀
        /// </summary>
        private const string UrlPrefix_Default = @"net.Tcp://";

        /// <summary>
        /// 创建RPC代理对象
        /// </summary>
        /// <typeparam name="T">代理类型</typeparam>
        /// <param name="remoteAddress">远程终结点</param>
        /// <param name="bindType">Binding类型</param>
        /// <param name="bindingName">binding配置元素名称，默认为 default</param>
        /// <returns>返回代理对象</returns>
        public static T CreatProxy<T>(EndpointAddress remoteAddress, string bindType, string bindingName = "ntb1")
        {
            var binding = CreatBinding(bindType, bindingName);
            return ChannelFactory<T>.CreateChannel(binding, remoteAddress);
        }

        public static T CreatProxy<T>(IPEndPoint remoteAddress, string bindType = BindingType_Default,
                                      string bindingName = "ntb1", string urlPrefix = UrlPrefix_Default)
        {
            EndpointAddress epa = CreatRemoteEndpointAddress<T>(remoteAddress, urlPrefix);
            return CreatProxy<T>(epa, bindType, bindingName);
        }

        public static T CreatDuplexProxy<T>(InstanceContext instanceContext, IPEndPoint remoteAddress, string bindType = BindingType_Default,
                                            string bindingName = "ntb1", string urlPrefix = UrlPrefix_Default)
        {
            EndpointAddress epa = CreatRemoteEndpointAddress<T>(remoteAddress, urlPrefix);
            var binding = CreatBinding(bindType, bindingName);
            return DuplexChannelFactory<T>.CreateChannel(instanceContext, binding, epa);
        }

        private static Binding CreatBinding(string bindType, string bindingName = "ntb1")
        {
            if (!File.Exists(wcfConfigPath))
            {
                InitConfigFile();
            }
            Configuration cm = GetConfiguration();
            if (bindingSection == null)
            {
                bindingSection = BindingsSection.GetSection(cm);
            }
            //找到所属的绑定类型
            BindingCollectionElement bce = bindingSection.BindingCollections.Find((e) => e.BindingName == bindType);
            IBindingConfigurationElement element = bce.ConfiguredBindings.FirstOrDefault((e) => e.Name == bindingName);
            Binding binding = mInfo.Invoke(bce, null) as Binding;
            element.ApplyConfiguration(binding);
            return binding;
        }


        internal static Configuration GetConfiguration()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap() { ExeConfigFilename= wcfConfigPath };
            var tmp= ConfigurationManager.OpenMappedExeConfiguration(fileMap,ConfigurationUserLevel.None);
            return tmp;
        }
        private static ServiceHost CreatServiceHost<CT, ST>(IPEndPoint localEndpoint)
        {
            ServiceHost host = new ServiceHost(typeof(ST));
            Binding binding = CreatBinding(BindingType_Default);
            host.AddServiceEndpoint(typeof(CT), binding, CreatRemoteEndpointAddress<CT>(localEndpoint).Uri);
            var behaviors = CreatBehaviorsFromConfig();
            for (int i = 0; i < behaviors.Count; i++)
            {
                IServiceBehavior behavior = behaviors[i];
                if (host.Description.Behaviors.Contains(behavior.GetType()))
                {
                    host.Description.Behaviors.Remove(behavior.GetType());
                }
                host.Description.Behaviors.Add(behavior);
            }
            return host;
        }

        public static List<IServiceBehavior> CreatBehaviorsFromConfig(string behavaiorName = "sb1")
        {
            if (!File.Exists(wcfConfigPath))
            {
                InitConfigFile();
            }
            Configuration cm = GetConfiguration();
            List<IServiceBehavior> behaviors = new List<IServiceBehavior>();
            object obj = cm.GetSection(BEHAVIOR_CONFIG_NODENAME);
            BehaviorsSection behaviorsSection = obj as BehaviorsSection;
            if (behaviorsSection != null)
            {
                object vv = behaviorsSection.ServiceBehaviors[behavaiorName];
                ServiceBehaviorElement ele = vv as ServiceBehaviorElement;
                for (int i = 0; i < ele.Count; i++)
                {
                    BehaviorExtensionElement temp = ele[i];
                    IServiceBehavior behavior = methodInfo.Invoke(temp, null) as IServiceBehavior;
                    behaviors.Add(behavior);
                }
            }
            return behaviors;
        }
        /// <summary>
        /// 创建远程终结点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ip"></param>
        /// <param name="urlPrefix"></param>
        /// <returns></returns>
        private static EndpointAddress CreatRemoteEndpointAddress<T>(IPEndPoint ip, string urlPrefix = UrlPrefix_Default)
        {
            string url = string.Format("{0}{1}{2}", urlPrefix, ip.ToString() + @"/", typeof(T).FullName);
            return new EndpointAddress(url);
        }

        /// <summary>
        /// 创建远程终结点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="urlPrefix"></param>
        /// <returns></returns>
        private static EndpointAddress CreatRemoteEndpointAddress<T>(string ip, int port, string urlPrefix = UrlPrefix_Default)
        {
            return CreatRemoteEndpointAddress<T>(new IPEndPoint(IPAddress.Parse(ip), port), urlPrefix);
        }

        /// <summary>
        /// 关闭代理对象
        /// </summary>
        /// <param name="obj"></param>
        public static void CloseProxy(object obj)
        {
            try
            {
                var comObj = obj as ICommunicationObject;
                if (comObj != null)
                    comObj.Close();
            }
            catch
            {

            }
        }

        public static bool OpenHost<CT, ST>(IPEndPoint localEndPoint)
        {
            try
            {
                var host = CreatServiceHost<CT, ST>(localEndPoint);
                host.Open();
                SystemTools.PrintInfo($"启动{typeof(CT).FullName}服务成功. 监听IP:{localEndPoint}",false,false);
                return true;
            }
            catch (Exception ex)
            {
                SystemTools.PrintInfo($"启动{typeof(CT).FullName} 服务失败：{ex.Message},监听IP:{localEndPoint}", true,true);
                return false;
            }
        }

        public static bool OpenHost<CT, ST>()
        {
            return OpenHost<CT, ST>(new IPEndPoint(ServerIP, ServerPort));
        }

        private static List<Type> servcieEventArgKnownTypes = new List<Type>();

        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            return servcieEventArgKnownTypes;
        }

        public static void AddKnownType<T>()
        {
            var type = typeof(T);
            if (!servcieEventArgKnownTypes.Contains(type))
            {
                servcieEventArgKnownTypes.Add(type);
            }
        }

        public static TResult CallRemoteService<TContract, TResult>(Func<TContract, TResult> func)
        {
            try
            {
                var proxy = RasAssist.CreatProxy<TContract>(ServerEndPoint);
                var temp = func(proxy);
                RasAssist.CloseProxy(proxy);
                return temp;
            }
            catch (Exception ex)
            {

                SystemTools.PrintInfo(ex.ToString(),true,true);
                return default(TResult);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TContract"></typeparam>
        /// <param name="action"></param>
        public static void CallRemoteService<TContract>(Action<TContract> action)
        {
            try
            {
                var proxy = CreatProxy<TContract>(ServerEndPoint);
                action(proxy);
                CloseProxy(proxy);
            }
            catch (Exception ex)
            {

                SystemTools.PrintInfo(ex.ToString(),true,true);
            }
        }



        public static string GetClientEndPoint()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties messageProperties = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpointProperty =
                messageProperties[RemoteEndpointMessageProperty.Name]
                as RemoteEndpointMessageProperty;
            return $"{endpointProperty.Address}: {endpointProperty.Port}";
        }

        private static object GetField(object obj, string fieled)
        {
            return obj.GetType().GetField(fieled, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj);
        }

        private static object GetProperty(object obj, string fieled)
        {
            return obj.GetType().GetProperty(fieled, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj, null);
        }

        /// <summary>
        /// 从资源中获取文件并写入本地
        /// </summary>
        private static void InitConfigFile()
        {
            var ass=  System.Reflection.Assembly.GetExecutingAssembly();
            var temp = ass.GetManifestResourceNames();
            var cfg= temp.FirstOrDefault(e => e.EndsWith("wcfService.config"));
            if (!string.IsNullOrEmpty(cfg))
            {
                var stream= ass.GetManifestResourceStream(cfg);
                var bytes = new byte[stream.Length];
                var flag = 0;
                while(flag<stream.Length)
                {
                   flag+= stream.Read(bytes, flag, bytes.Length-flag);
                }
                stream.Close();
                FileStream fs = new FileStream(wcfConfigPath, FileMode.OpenOrCreate);
                fs.SetLength (0);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }
        }
    }
    
}
