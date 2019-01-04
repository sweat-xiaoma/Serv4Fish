using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Serv4Fish3.ServerSide;
using FishCommon3;

namespace Serv4Fish3.Controller
{
    public class ControllerManager
    {
        Dictionary<RequestCode, BaseController> controllerDict = new Dictionary<RequestCode, BaseController>();
        Server server;

        public ControllerManager(Server server)
        {
            this.server = server;
            Init();
        }

        void Init()
        {
            //DefaultController defaultController = new DefaultController();
            //controllerDict.Add(defaultController.RequestCode, defaultController);
            controllerDict.Add(RequestCode.None, new DefaultController());
            controllerDict.Add(RequestCode.User, new UserController());
            controllerDict.Add(RequestCode.Room, new RoomController());
            controllerDict.Add(RequestCode.Game, new GameController());
        }

        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            BaseController controller;
            bool isGet = controllerDict.TryGetValue(requestCode, out controller);
            if (!isGet)
            {
                Console.WriteLine("无法得到[{0}] 所对应的 Controller，无法处理请求", requestCode); // 写日志
            }

            string methodName = Enum.GetName(typeof(ActionCode), actionCode);
            MethodInfo methodInfo = controller.GetType().GetMethod(methodName);
            if (methodInfo == null)
            {
                Console.WriteLine("[警告] 在 Controller[{0}] 中没有对应的处理方法[{1}]", controller.GetType(), methodName);
            }

            object[] parameters = new object[] { data, client, this.server };
            Console.WriteLine("[ControllerManager - 处理] " +
                "\n\tController: {0} " +
                "\n\tmethod: {1} " +
                "\n\tdata: {2}",
                controller,
                methodName,
                data);
            object re = methodInfo.Invoke(controller, parameters);
            if (re == null || string.IsNullOrEmpty(re as string))
            {
                return;
            }
            server.SendResponse(client, actionCode, re as string);

        }

    }
}
