#!/bin/bash
set -euf -o pipefail

cd "$(dirname "$0"..)" || exit


# Don't need kafka-tools to start up
docker-compose up --detach

echo ""
echo "🐳  Kicked off the containers. Should be up in one minute (literally)."
echo ""
echo "😬  If you can't contain your curiousity, run ./auto/logs.sh"
echo ""
echo "💥 kafka"
