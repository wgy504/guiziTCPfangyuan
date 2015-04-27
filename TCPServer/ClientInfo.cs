﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace guiziTCPfangyuan
{
    class ClientInfo
    {
        private ClientInfo()
        { }

        public ClientInfo(Socket sck):this(sck,"无")
        {

        }

        public ClientInfo(Socket sck,string gzID)
        {
            this.sck = sck;
            this.gzID = gzID;
            this._sendBuf = new List<byte>();
        }

        private Socket sck;

        public Socket Sck
        {
            get { return sck; }
        }
        private string gzID;

        public string GzID
        {
            get { return gzID; }
            set { gzID = value; }
        }
        private List<byte> _sendBuf;
        public List<byte> SendBuffer
        {
            get { return _sendBuf; }
            set { _sendBuf = value; }
        }
    }
}
