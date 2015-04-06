namespace Storm
{
    public enum ProxyEvent
    {
        DEFAULT = 1000,
        TOKEN_AVAILABLE = 2000,
        TX_BEGIN = 3001,
        TX_END,
        BATCH_BEGIN = 4001,
        BATCH_END,
        BATCH_CANCEL,
        ACK_TUPLE = 5001,
        FAIL_TUPLE,
        ACK_TX = 6001,
        FAIL_TX
    }
}