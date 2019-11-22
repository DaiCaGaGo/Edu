using System;
using System.Collections;

namespace AsyncSocketClient.smpp
{
    public class Smsc
    {
        private string _description;
        private string _host;
        private int _port;
        private string _systemId;
        private string _password;
        private string _systemType;
        private int _sequenceNumber;
        private byte _addrTon;
        private byte _addrNpi;
        private string _addressRange = "";

        public Smsc()
        {
        }

        public Smsc(string description, string host, int port, string systemId, string password, string systemType, int sequenceNumber)
        {
            _description = description;
            _host = host;
            _port = port;

            if (systemId.Length > 15)
                _systemId = systemId.Substring(0, 15);
            else
                _systemId = systemId;

            if (password.Length > 8)
                _password = password.Substring(0, 8);
            else
                _password = password;

            if (systemType.Length > 12)
                _systemType = systemType.Substring(0, 8);
            else
                _systemType = systemType;

            _sequenceNumber = sequenceNumber;
        }

        public Smsc(string description, string host, int port, string systemId, string password, string systemType, byte addrTon, byte addrNpi, string addressRange, int sequenceNumber)
            : this(description, host, port, systemId, password, systemType, sequenceNumber)
        {
            _addrTon = addrTon;
            _addrNpi = addrNpi;
            _addressRange = addressRange;
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }//Description

        public string Host
        {
            get
            {
                return _host;
            }
            set
            {
                _host = value;
            }
        }//Host

        public int Port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
            }
        }//Port

        public string SystemId
        {
            get
            {
                return _systemId;
            }
            set
            {
                _systemId = value;
            }
        }//SystemId

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }//Password

        public string SystemType
        {
            get
            {
                return _systemType;
            }
            set
            {
                _systemType = value;
            }
        }//SystemType

        public byte AddrTon
        {
            get
            {
                return _addrTon;
            }
            set
            {
                _addrTon = value;
            }
        }//AddrTon

        public byte AddrNpi
        {
            get
            {
                return _addrNpi;
            }
            set
            {
                _addrNpi = value;
            }
        }//AddrNpi

        public string AddressRange
        {
            get
            {
                return _addressRange;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    _addressRange = "";
                else
                {
                    if (value.Length > 40)
                        _addressRange = value.Substring(40);
                    else
                        _addressRange = value;
                }
            }
        }//AddressRange

        public int SequenceNumber
        {
            get
            {
                lock (this)
                {
                    if (_sequenceNumber == Int32.MaxValue)
                        _sequenceNumber = 0;
                    else
                        _sequenceNumber++;
                    return _sequenceNumber;
                }
            }
        }//SequenceNumber

        public int LastSequenceNumber
        {
            get
            {
                lock (this)
                {
                    return _sequenceNumber;
                }
            }
        }//LastSequenceNumber
    }

    public class SmscArray
    {
        private readonly ArrayList _smscAr = new ArrayList();
        private int _curSmsc;

        public void AddSmsc(Smsc pSmsc)
        {
            lock (this)
            {
                _smscAr.Add(pSmsc);
            }
        }//AddSMSC

        public void Clear()
        {
            lock (this)
            {
                _smscAr.Clear();
                _curSmsc = 0;
            }
        }//Clear

        public void NextSmsc()
        {
            lock (this)
            {
                _curSmsc++;
                if ((_curSmsc + 1) > _smscAr.Count)
                    _curSmsc = 0;
            }
        }//AddSMSC

        public Smsc CurrentSmsc
        {
            get
            {
                Smsc mSmsc = null;
                try
                {
                    lock (this)
                    {
                        if (_smscAr.Count == 0)
                            return null;
                        if (_curSmsc > (_smscAr.Count - 1))
                        {
                            _curSmsc = 0;
                        }
                        mSmsc = (Smsc)_smscAr[_curSmsc];
                    }
                }
                catch
                {
                    //
                }
                return mSmsc;
            }
        }//currentSMSC

        public bool HasItems
        {
            get
            {
                lock (this)
                {
                    if (_smscAr.Count > 0)
                        return true;
                    else
                        return false;
                }
            }
        }//HasItems
    }//SMSCArray
}