
using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Serv4Fish3.ServerSide;
using FishCommon3;

namespace Serv4Fish3.Controller
{
    public abstract class BaseController
    {
        protected RequestCode requestCode = RequestCode.None;

        public RequestCode RequestCode
        {
            get
            {
                return this.requestCode;
            }
        }

        // 处理消息
        public virtual string DefaultHandle(string data, Client client, Server server) { return null; }

        //public BaseController()
        //{
        //}

    }
}
