#!/bin/bash
set -euf -o pipefail

cd "$(dirname "$0"..)" || exit

echo "🧹  Stopping containers and cleaning up."
echo ""

docker-compose down -v --remove-orphans

echo ""
echo "✨  All done."
