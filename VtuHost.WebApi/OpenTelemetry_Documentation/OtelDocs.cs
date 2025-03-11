/*

The out-of-the-box prometheus.yml config file for Jaeger - official

global:
  scrape_interval:     15s # Set the scrape interval to every 15 seconds. Default is every 1 minute.
  evaluation_interval: 15s # Evaluate rules every 15 seconds. The default is every 1 minute.
  # scrape_timeout is set to the global default (10s).

scrape_configs:
  - job_name: aggregated-trace-metrics
    static_configs:
    - targets: ['spm_metrics_source:8889']







exporters:
  otlphttp/example:
    endpoint: <your-endpoint-url>

service:
  pipelines:
    traces:
      exporters: [spanmetrics, otlphttp/example]



Note
When merging YAML values with the Collector, objects are merged and 
arrays are replaced. The spanmetrics exporter must be included in the 
array of exporters for the traces pipeline if overridden. Not including 
this exporter will result in an error.





































































 
*/