#!/bin/bash
set -euf -o pipefail

cd "$(dirname "$0"..)" || exit

echo "🧹  Stopping containers and cleaning up."
echo ""

docker-compose down -v --remove-orphans

echo "💣  Deleting created docker images."

docker rmi producer_dotnet:1.0.0
docker rmi consumer_dotnet:1.0.0

echo ""
echo "✨  All done."
