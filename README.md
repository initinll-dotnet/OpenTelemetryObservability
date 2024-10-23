# OpenTelemetryObservability

This is an experimental project designed to explore observability in .NET. The project includes a simple Web API and a gRPC service that produce traces, metrics, and logs using the OpenTelemetry protocol, which is vendor-neutral.

## Tools Used
The following open-source tools are used to enable observability within the project:
- OpenTelemetry: For instrumentation, generation, collection, and export of telemetry data (metrics, logs, traces).
- Jaeger: For distributed tracing to monitor and troubleshoot workflows in complex distributed systems.
- Aspire: A distributed tracing platform by Microsoft for observability.
- Prometheus: A toolkit for collecting and storing metrics as time-series data.
- Grafana: For querying, visualizing, alerting on, and exploring metrics, logs, and traces.

## Running the Project
To run the project, use the following command:
```bash
Docker compose up
```

Once the services are up and running, you can access the API through Swagger at:
https://localhost:4040/swagger/index.html

![image](https://github.com/user-attachments/assets/964d6122-2917-4037-9e2f-b217e6c4bcca)


# Tool Overviews

## OpenTelemetry

OpenTelemetry (OTel) is a collection of APIs, SDKs, and tools used for observability. It provides a vendor-neutral framework for instrumenting, generating, collecting, and exporting telemetry data, such as metrics, logs, and traces. These insights can help you analyze your software's performance and behavior.

![image](https://github.com/user-attachments/assets/09d72d15-f514-4bc4-ba33-721a1fc77c1d)

## Jaeger

Jaeger is an open-source distributed tracing platform designed to monitor and troubleshoot workflows in microservices-based systems. By visualizing the flow of requests through a distributed system, Jaeger helps identify performance bottlenecks, track delays, and troubleshoot errors across multiple services.

Jaeger URL - http://localhost:16686

![image](https://github.com/user-attachments/assets/93f78ca3-d2cf-4ae9-aa65-425f3e6da69f)


## Prometheus

Prometheus is an open-source monitoring and alerting toolkit. It collects metrics and stores them as time-series data, where each metric is recorded with a timestamp and can include optional key-value labels.

Prometheus URL - http://localhost:9090

![image](https://github.com/user-attachments/assets/b0fcc1ad-223e-4693-8eaf-c0261f6c6531)


# Grafana

Grafana is an open-source platform for visualizing, querying, and alerting on your metrics, logs, and traces. It integrates with various data sources, including Prometheus, Elasticsearch, and SQL databases, allowing you to create live dashboards with insightful visualizations.

Grafana URL - http://localhost:3000

![image](https://github.com/user-attachments/assets/2576d811-83f5-46f7-9a78-c0b637573a0c)

## Aspire

Aspire is a distributed tracing and observability platform developed by Microsoft, offering comprehensive dashboards for app monitoring. It enables real-time tracking of logs, traces, and environment configurations to give developers a detailed overview of their application’s health and performance.

Aspire URL - http://localhost:18888

![image](https://github.com/user-attachments/assets/4c09a599-b3fa-4729-b850-3242bda02233)


This project provides a great hands-on opportunity to learn and apply modern observability principles in .NET applications using cutting-edge open-source tools.

