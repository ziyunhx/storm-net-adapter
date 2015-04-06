namespace Storm
{
    public class Constants
    {
        public static readonly string SYSTEM_COMPONENT_ID = "__system";
        public static readonly string SYSTEM_TICK_STREAM_ID = "__tick";
        public static readonly string METRICS_COMPONENT_ID_PREFIX = "__metrics";
        public static readonly string METRICS_STREAM_ID = "__metrics";
        public static readonly string METRICS_TICK_STREAM_ID = "__metrics_tick";
        public static readonly string PERF_SCHEMA = "transaction.performance.collect";
        public static readonly string PERF_META = "SYS_PERF";
        public static readonly string APINITIALIZE_LOCK = "AP_Lock_CreateConfigIni";
        public static readonly string DEFAULT_STREAM_ID = "default";
        public static readonly string KAFKA_STREAM_ID = "kafka";
        public static readonly string KAFKA_META = "kafkameta";
        public static readonly string STORM_TX_ATTEMPT = "storm.tx.attempt";
        public static readonly string USER_CONFIG = "UserConfig";
        public static readonly string NONTRANSACTIONAL_ENABLE_ACK = "nontransactional.ack.enabled";
        public static readonly string STORM_ZOOKEEPER_SERVERS = "storm.zookeeper.servers";
        public static readonly string STORM_ZOOKEEPER_PORT = "storm.zookeeper.port";
        public static readonly string STORM_ZOOKEEPER_ROOT = "storm.zookeeper.root";
    }
}

