#!/usr/bin/env bash
set -euo pipefail

SCENARIO_ID="${1:-all}"

echo "=========================================================="
echo " STARRY CLUSTER SCENARIO RUNNER: Scenario ${SCENARIO_ID}"
echo "=========================================================="

cleanup() {
    echo "[TEARDOWN] Cleaning up cluster scenario ${SCENARIO_ID}..."
    ip link del starry-test-br 2>/dev/null || true
    rm -rf /tmp/starry-cluster-* /tmp/starry-scen-* 2>/dev/null || true
    echo "[TEARDOWN] Done."
}

trap cleanup EXIT INT TERM

if [ "$SCENARIO_ID" = "all" ]; then
    echo "Running all 24 cluster scenarios..."
    dotnet test tests/Starry.E2ETests/Starry.E2ETests.csproj --filter FullyQualifiedName~ClusterMatrixFuzzerTests -p:RunAnalyzers=false
else
    # Pad single digit with zero if needed (e.g. 1 -> 01)
    FORMATTED_ID=$(printf "%02d" "$SCENARIO_ID")
    echo "Running Scenario ${FORMATTED_ID}..."
    dotnet test tests/Starry.E2ETests/Starry.E2ETests.csproj --filter FullyQualifiedName~Scenario${FORMATTED_ID} -p:RunAnalyzers=false
fi

echo "Scenario ${SCENARIO_ID} completed successfully!"
