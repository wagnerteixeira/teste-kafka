#!/bin/bash
set -euf -o pipefail

cd "$(dirname "$0"..)" || exit

echo "🌲  Here are some logs"

docker-compose logs --follow
