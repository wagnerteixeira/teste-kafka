#!/bin/bash
set -euf -o pipefail
cd "$(dirname "$0"..)" || exit
echo "🌲 Creating mongo sink connector"

docker-compose exec mongo mongo --host mongodb://mongo.local:27017 config-data.js
docker-compose exec broker-3 kafka-topics --command-config /etc/kafka/config/command.properties --bootstrap-server="broker-1.local:19092,broker-2.local:19093,broker-3.local:19094" --create --topic UserEmail --partitions 3 --replication-factor 3
curl -X POST -H "Content-Type: application/json" -d @sink-connector.json http://localhost:8083/connectors -w "\n"
echo "Sink connector created! 👌"
