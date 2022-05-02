#!/bin/bash
set -euf -o pipefail
cd "$(dirname "$0"..)" || exit
echo "🌲 Creating mongo sink connector"

docker-compose exec mongo mongo --host mongodb://localhost:27017 config-data.js
docker-compose exec broker kafka-topics --command-config /etc/kafka/config/command.properties --bootstrap-server=broker.local:19092 --create --topic UserEmail --partitions 1 --replication-factor 1
curl -X POST -H "Content-Type: application/json" -d @sink-connector.json http://localhost:8083/connectors -w "\n"
echo "Sink connector created! 👌"
