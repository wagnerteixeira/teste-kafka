## 💡 Inspired in great repository https://github.com/Cybergern/kafka-docker-ssl.git

Thank you Cybergern!! ❤️

## 💥 Local Kafka Cluster in Docker

This project sets up a local Kafka cluster with Zookeeper, .Net Core Api Producer and Worker Consumer

### Initial set-up

0. Ensure Docker and Docker Compose are installed

### Starting up

1. Bring the cluster up.
  ```sh
    ./auto/up.sh
  ```

### Logs

```sh
  ./auto/logs.sh

```

### Shutting Down

```sh
  ./auto/down.sh

```
### Accessing the services

1. Broker is available at `localhost:19092`
2. To access .Net core Api, [http://localhost:8080/swagger](http://localhost:8080/swagger)

### Running Kafka Tools

```sh
  ./auto/kafka-tools.sh <tool-name> <arguments>
  # Note that the broker is available at "broker:19092"
  # Example, list all topics
  ./auto/kafka-tools.sh kafka-topics --bootstrap-server broker:19092 --list
  # Example, create a topic
  ./auto/kafka-tools.sh kafka-topics --bootstrap-server=broker:19092 --create --topic UserEmail --partitions 1 --replication-factor 1
  # Example, delete a Topic
  ./auto/kafka-tools.sh kafka-topics --bootstrap-server=broker:19092 --delete --topic UserEmail
-
```
