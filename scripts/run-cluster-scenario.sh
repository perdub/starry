#!/usr/bin/env bash
set -euo pipefail

SCENARIO_ID="${1:-all}"

echo "=========================================================="
echo " STARRY CLUSTER SCENARIO RUNNER: Scenario ${SCENARIO_ID}"
echo "=========================================================="

cleanup() {
    echo "[TEARDOWN] Cleaning up cluster scenario ${SCENARIO_ID}..."
    rm -rf /tmp/starry-cluster-* /tmp/starry-scen-* 2>/dev/null || true
    if command -v docker >/dev/null 2>&1; then
        docker rm -f starry-live-box 2>/dev/null || true
        docker network rm starry-live-net 2>/dev/null || true
    fi
    echo "[TEARDOWN] Done."
}

trap cleanup EXIT INT TERM

# If Docker daemon is active on the runner, spin up lightweight live test container
if command -v docker >/dev/null 2>&1 && docker info >/dev/null 2>&1; then
    echo "[DOCKER] Docker daemon active. Setting up isolated live verification container..."
    docker network create starry-live-net 2>/dev/null || true
    docker run -d --name starry-live-box --network starry-live-net \
        -e TEST_PASS=secret-db-pass \
        alpine:3.20 sleep 3600 2>/dev/null || true
fi

if [ "$SCENARIO_ID" = "all" ]; then
    echo "Running all 24 cluster scenarios..."
    dotnet test tests/Starry.E2ETests/Starry.E2ETests.csproj --filter FullyQualifiedName~ClusterMatrixFuzzerTests -p:RunAnalyzers=false
else
    FORMATTED_ID=$(printf "%02d" "$SCENARIO_ID")
    echo "Running Scenario ${FORMATTED_ID}..."
    dotnet test tests/Starry.E2ETests/Starry.E2ETests.csproj --filter FullyQualifiedName~Scenario${FORMATTED_ID} -p:RunAnalyzers=false
fi

echo "Scenario ${SCENARIO_ID} completed successfully!"
