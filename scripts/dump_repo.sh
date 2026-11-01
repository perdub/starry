#!/usr/bin/env bash
set -euo pipefail

# Resolve directory of this script and repository root
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"

# Run repo2md utility forwarding all arguments
python3 "$SCRIPT_DIR/repo2md.py" --root "$REPO_ROOT" "$@"
