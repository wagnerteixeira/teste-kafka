#!/bin/bash
set -euf -o pipefail

cd "$(dirname "$0")/.." || exit

./auto/down.sh

echo "💣  Deleting volumes for a clean slate."

docker volume rm zk-data > /dev/null
docker volume rm zk-txn-logs > /dev/null
docker volume rm kafka-data > /dev/null

echo "💣  Deleting created secrets."

rm -rf ./secrets/*

echo "💣  Deleting created docker images."

docker rmi producer_dotnet:1.0.0
docker rmi producer_dotnet:1.0.0

echo "✅  All done."

