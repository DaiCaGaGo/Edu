namespace AsyncSocketServer.smpp
{
    public class LogLevels
    {
        public const int LogPdu = 1;
        public const int LogSteps = 2;
        public const int LogWarnings = 4;
        public const int LogErrors = 8;
        public const int LogExceptions = 16;
        public const int LogDebug = 32;
    }//LogLevels

    public class ConnectionStates
    {
        public const int SmppSocketConnectSent = 1;
        public const int SmppSocketConnected = 2;
        public const int SmppBindSent = 3;
        public const int SmppBinded = 4;
        public const int SmppUnbindSent = 5;
        public const int SmppUnbinded = 6;
        public const int SmppSocketDisconnected = 7;
    }//ConnectionStates

    public class StatusCodes
    {
        public const int EsmeRok = 0x00000000; // No Error
        public const int EsmeRinvmsglen = 0x00000001; // Message Length is invalid
        public const int EsmeRinvcmdlen = 0x00000002; // Command Length is invalid
        public const int EsmeRinvcmdid = 0x00000003; // Invalid Command ID
        public const int EsmeRinvbndsts = 0x00000004; // Incorrect BIND Status for given command
        public const int EsmeRalybnd = 0x00000005; // ESME Already in Bound State
        public const int EsmeRinvprtflg = 0x00000006; // Invalid Priority Flag
        public const int EsmeRinvregdlvflg = 0x00000007; // Invalid Registered Delivery Flag
        public const int EsmeRsyserr = 0x00000008; // System Error
        public const int EsmeRinvsrcadr = 0x0000000A; // Invalid Source Address
        public const int EsmeRinvdstadr = 0x0000000B; // Invalid Dest Addr
        public const int EsmeRinvmsgid = 0x0000000C; // Message ID is invalid
        public const int EsmeRbindfail = 0x0000000D; // bind Failed
        public const int EsmeRinvpaswd = 0x0000000E; // Invalid Password
        public const int EsmeRinvsysid = 0x0000000F; // Invalid System ID
        public const int EsmeRcancelfail = 0x00000011; // Cancel SM Failed
        public const int EsmeRreplacefail = 0x00000013; // Replace SM Failed
        public const int EsmeRmsgqful = 0x00000014; // Message Queue Full
        public const int EsmeRinvsertyp = 0x00000015; // Invalid Service Type
        public const int EsmeRinvnumdests = 0x00000033; // Invalid number of destinations
        public const int EsmeRinvdlname = 0x00000034; // Invalid Distribution List name
        public const int EsmeRinvdestflag = 0x00000040; // Destination flag is invalid(submit_multi)
        public const int EsmeRinvsubrep = 0x00000042; // Invalid `submit with replace? request(i.e. submit_sm with replace_if_present_flag set)
        public const int EsmeRinvesmclass = 0x00000043; // Invalid esm_class field data
        public const int EsmeRcntsubdl = 0x00000044; // Cannot Submit to Distribution List
        public const int EsmeRsubmitfail = 0x00000045; // submit_sm or submit_multi failed
        public const int EsmeRinvsrcton = 0x00000048; // Invalid Source address TON
        public const int EsmeRinvsrcnpi = 0x00000049; // Invalid Source address NPI
        public const int EsmeRinvdstton = 0x00000050; // Invalid Destination address TON
        public const int EsmeRinvdstnpi = 0x00000051; // Invalid Destination address NPI
        public const int EsmeRinvsystyp = 0x00000053; // Invalid system_type field
        public const int EsmeRinvrepflag = 0x00000054; // Invalid replace_if_present flag
        public const int EsmeRinvnummsgs = 0x00000055; // Invalid number of messages
        public const int EsmeRthrottled = 0x00000058; // Throttling error (ESME has exceeded allowed message limits)
        public const int EsmeRinvsched = 0x00000061; // Invalid Scheduled Delivery Time
        public const int EsmeRinvexpiry = 0x00000062; // Invalid message validity period (Expiry time)
        public const int EsmeRinvdftmsgid = 0x00000063; // Predefined Message Invalid or Not Found
        public const int EsmeRxTAppn = 0x00000064; // ESME Receiver Temporary App Error Code
        public const int EsmeRxPAppn = 0x00000065; // ESME Receiver Permanent App Error Code
        public const int EsmeRxRAppn = 0x00000066; // ESME Receiver Reject Message Error Code
        public const int EsmeRqueryfail = 0x00000067; // query_sm request failed
        public const int EsmeRcontentover = 0x00000068; // //content overflow
        public const int EsmeRsmsloop = 0x00000069; //Sms Loop
        public const int EsmeRinvoptparstream = 0x000000C0; // Error in the optional part of the PDU Body.
        public const int EsmeRoptparnotallwd = 0x000000C1; // Optional Parameter not allowed
        public const int EsmeRinvparlen = 0x000000C2; // Invalid Parameter Length.
        public const int EsmeRmissingoptparam = 0x000000C3; // Expected Optional Parameter missing
        public const int EsmeRinvoptparamval = 0x000000C4; // Invalid Optional Parameter Value
        public const int EsmeRdeliveryfailure = 0x000000FE; // Delivery Failure (used for data_sm_resp)
        public const int EsmeRunknownerr = 0x000000FF; // Unknown Error
    }//StatusCodes

    public class PriorityFlags
    {
        public const byte Bulk = 0;
        public const byte Normal = 1;
        public const byte Urgent = 2;
        public const byte VeryUrgent = 3;
    }//PriorityFlags

    public class DeliveryReceipts
    {
        public const byte NoReceipt = 0;
        public const byte OnSuccessOrFailure = 1;
        public const byte OnFailure = 2;
    }//DeliveryReceipt

    public class ReplaceIfPresentFlags
    {
        public const byte DoNotReplace = 0;
        public const byte Replace = 1;
    }//ReplaceIfPresentFlag

    public class AddressTons
    {
        public const byte Unknown = 0;
        public const byte International = 1;
        public const byte National = 2;
        public const byte NetworkSpecific = 3;
        public const byte SubscriberNumber = 4;
        public const byte Alphanumeric = 5;
        public const byte Abbreviated = 6;
    }//AddressTon

    public class AddressNpis
    {
        public const byte Unknown = 0;
        public const byte Isdn = 1;
    }//AddressTon
}