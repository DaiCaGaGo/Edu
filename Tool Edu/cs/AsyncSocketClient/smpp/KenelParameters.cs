namespace AsyncSocketClient.smpp
{
    public class KernelParameters
    {
        public const int MaxBufferSize = 1048576; // 1MB

        public const int MaxPduSize = 131072;

        public const int ReconnectTimeout = 30000; // miliseconds

        public const int WaitPacketResponse = 30; //seconds

        public const int CanBeDisconnected = 180; //seconds - BETTER TO BE MORE THAN TryToReconnectTimeOut

        public const int NationalNumberLength = 8;

        public const int MaxUndeliverableMessages = 100;

        public const int AskDeliveryReceipt = 1; //NoReceipt = 0;

        public const bool SplitLongText = false;

        public const bool UseEnquireLink = true; //this is to keep connection alive when you are not submitting

        public const int EnquireLinkTimeout = 30000; //miliseconds
    }
}