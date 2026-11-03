#!/usr/bin/env python3
"""
Repository Dumper Utility (repo2md)
Dumps the repository into a single comprehensive Markdown file for LLM analysis.
"""

import argparse
import fnmatch
import os
import re
import sys
from pathlib import Path
from typing import Dict, List, Optional, Set, Tuple

# Default directories to skip
DEFAULT_EXCLUDED_DIRS: Set[str] = {
    ".git",
    ".slim",
    ".vs",
    ".idea",
    ".vscode",
    "bin",
    "obj",
    "node_modules",
    "dist",
    "cache",
    ".vitepress/dist",
    ".vitepress/cache",
    "publish-aot",
    "nupkg",
    "__pycache__",
    ".pytest_cache",
    ".mypy_cache",
    "coverage",
    "runner-logs",
    "test-logs",
}

# Standalone log and diagnostic files to skip from standard repository code listing
DEFAULT_EXCLUDED_FILES: Set[str] = {
    "runner-tests.log",
    "runner.log",
}

# Binary file extensions to skip
BINARY_EXTENSIONS: Set[str] = {
    # Compiled & executables
    ".dll", ".exe", ".so", ".dylib", ".a", ".lib", ".o", ".obj", ".bin", ".aot",
    # Packages & archives
    ".nupkg", ".snupkg", ".tar", ".gz", ".tgz", ".zst", ".zip", ".7z", ".rar", ".bz2", ".xz",
    # Images & graphics
    ".png", ".jpg", ".jpeg", ".gif", ".ico", ".webp", ".bmp", ".tiff", ".tif", ".svgz", ".psd",
    # Fonts
    ".woff", ".woff2", ".ttf", ".otf", ".eot",
    # Media
    ".mp3", ".mp4", ".wav", ".ogg", ".flac", ".avi", ".mov", ".mkv", ".webm",
    # Documents & databases
    ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".db", ".sqlite", ".sqlite3",
    # Python & JVM bytecode
    ".pyc", ".pyo", ".pyd", ".class", ".jar", ".war",
}

# Language mapping for syntax highlighting
LANGUAGE_MAP: Dict[str, str] = {
    ".cs": "csharp",
    ".fs": "fsharp",
    ".fsx": "fsharp",
    ".vb": "vb",
    ".csproj": "xml",
    ".sln": "text",
    ".slnx": "xml",
    ".props": "xml",
    ".targets": "xml",
    ".xml": "xml",
    ".config": "xml",
    ".xaml": "xml",
    ".resx": "xml",
    ".json": "json",
    ".jsonc": "json",
    ".yaml": "yaml",
    ".yml": "yaml",
    ".toml": "toml",
    ".md": "markdown",
    ".markdown": "markdown",
    ".sh": "bash",
    ".bash": "bash",
    ".zsh": "bash",
    ".ps1": "powershell",
    ".psm1": "powershell",
    ".psd1": "powershell",
    ".py": "python",
    ".pyi": "python",
    ".js": "javascript",
    ".mjs": "javascript",
    ".cjs": "javascript",
    ".ts": "typescript",
    ".mts": "typescript",
    ".cts": "typescript",
    ".jsx": "jsx",
    ".tsx": "tsx",
    ".html": "html",
    ".htm": "html",
    ".css": "css",
    ".scss": "scss",
    ".sass": "sass",
    ".less": "less",
    ".sql": "sql",
    ".rs": "rust",
    ".go": "go",
    ".java": "java",
    ".kt": "kotlin",
    ".kts": "kotlin",
    ".c": "c",
    ".h": "c",
    ".cpp": "cpp",
    ".hpp": "cpp",
    ".cc": "cpp",
    ".cxx": "cpp",
    ".proto": "protobuf",
    ".graphql": "graphql",
    ".gql": "graphql",
    ".ini": "ini",
    ".cfg": "ini",
    ".conf": "ini",
    ".editorconfig": "ini",
    ".gitignore": "gitignore",
    ".ignore": "gitignore",
    ".dockerignore": "dockerignore",
    ".env": "bash",
    ".txt": "text",
    ".log": "text",
    ".csv": "csv",
    ".tsv": "tsv",
    ".diff": "diff",
    ".patch": "diff",
}


def is_binary_file(filepath: Path) -> bool:
    """Check if a file is binary based on extension or content."""
    suffix = filepath.suffix.lower()
    if suffix in BINARY_EXTENSIONS:
        return True

    # Compound extensions like .tar.gz
    name_lower = filepath.name.lower()
    if any(name_lower.endswith(ext) for ext in [".tar.gz", ".tar.zst", ".tar.bz2", ".tar.xz"]):
        return True

    try:
        with open(filepath, "rb") as f:
            chunk = f.read(8192)
            if b"\x00" in chunk:
                return True
    except (OSError, PermissionError):
        return True

    return False


def get_syntax_lang(filepath: Path) -> str:
    """Determine syntax highlighting language identifier."""
    name = filepath.name
    name_lower = name.lower()

    if name_lower == "dockerfile" or name_lower.startswith("dockerfile."):
        return "dockerfile"

    if name in {".gitignore", ".ignore", ".dockerignore"}:
        return "gitignore"

    if name == ".editorconfig":
        return "ini"

    if name.startswith(".env"):
        return "bash"

    suffix = filepath.suffix.lower()
    if suffix in LANGUAGE_MAP:
        return LANGUAGE_MAP[suffix]

    if suffix.startswith(".") and suffix[1:].isalnum():
        return suffix[1:]

    return "text"


def should_exclude_path(
    rel_path_str: str,
    is_dir: bool,
    custom_patterns: List[str],
) -> bool:
    """Check whether a path matches default exclusions or custom patterns."""
    # Normalize path separators
    normalized = rel_path_str.replace("\\", "/").strip("/")
    parts = normalized.split("/")

    # Check against directory exclusions
    for excluded in DEFAULT_EXCLUDED_DIRS:
        ex_norm = excluded.replace("\\", "/").strip("/")
        if is_dir:
            if normalized == ex_norm or normalized.startswith(ex_norm + "/"):
                return True
            if any(part == ex_norm for part in parts):
                return True
        else:
            if normalized.startswith(ex_norm + "/"):
                return True
            if any(part == ex_norm for part in parts[:-1]):
                return True

    if not is_dir and (parts[-1] in DEFAULT_EXCLUDED_FILES or parts[-1].endswith(".trx")):
        return True

    # Check custom exclude patterns
    for pat in custom_patterns:
        pat_clean = pat.strip()
        if not pat_clean:
            continue
        # Pattern matching against relative path or filename
        if fnmatch.fnmatch(normalized, pat_clean):
            return True
        if fnmatch.fnmatch(parts[-1], pat_clean):
            return True
        if is_dir and fnmatch.fnmatch(normalized + "/", pat_clean):
            return True

    return False


def collect_files(
    root_path: Path,
    output_resolved: Path,
    custom_patterns: List[str],
) -> Tuple[List[Path], Dict[str, List[str]]]:
    """
    Recursively collect all valid files and build a directory structure dictionary for tree rendering.
    Returns (sorted_files, dir_structure).
    """
    collected: List[Path] = []
    tree_nodes: Dict[str, List[str]] = {}

    for dirpath, dirnames, filenames in os.walk(root_path, topdown=True):
        current_dir = Path(dirpath)
        try:
            rel_dir = current_dir.relative_to(root_path)
            rel_dir_str = "" if rel_dir == Path(".") else str(rel_dir).replace("\\", "/")
        except ValueError:
            continue

        # Filter out directories to prune os.walk
        kept_dirs = []
        for d in dirnames:
            sub_rel = f"{rel_dir_str}/{d}" if rel_dir_str else d
            if not should_exclude_path(sub_rel, is_dir=True, custom_patterns=custom_patterns):
                kept_dirs.append(d)
        dirnames[:] = sorted(kept_dirs, key=str.lower)

        # Filter files
        kept_files = []
        for f in sorted(filenames, key=str.lower):
            file_path = current_dir / f
            try:
                if file_path.resolve() == output_resolved:
                    continue
            except (OSError, RuntimeError):
                pass

            file_rel = f"{rel_dir_str}/{f}" if rel_dir_str else f
            if should_exclude_path(file_rel, is_dir=False, custom_patterns=custom_patterns):
                continue

            if is_binary_file(file_path):
                continue

            collected.append(file_path)
            kept_files.append(f)

        tree_nodes[rel_dir_str] = [d + "/" for d in dirnames] + kept_files

    return collected, tree_nodes


def generate_ascii_tree(root_name: str, tree_nodes: Dict[str, List[str]]) -> str:
    """Generate visual ASCII tree string from directory structure."""
    lines: List[str] = [f"{root_name}/"]

    def _render_level(current_rel: str, prefix: str):
        entries = tree_nodes.get(current_rel, [])
        total = len(entries)
        for i, entry in enumerate(entries):
            is_last = (i == total - 1)
            connector = "└── " if is_last else "├── "
            child_prefix = prefix + ("    " if is_last else "│   ")

            lines.append(f"{prefix}{connector}{entry}")

            if entry.endswith("/"):
                sub_dir_name = entry[:-1]
                sub_rel = f"{current_rel}/{sub_dir_name}" if current_rel else sub_dir_name
                _render_level(sub_rel, child_prefix)

    _render_level("", "")
    return "\n".join(lines)


def format_size(size_bytes: int) -> str:
    """Format bytes into human-readable string."""
    if size_bytes < 1024:
        return f"{size_bytes} B"
    elif size_bytes < 1024 * 1024:
        return f"{size_bytes / 1024:.1f} KB"
    else:
        return f"{size_bytes / (1024 * 1024):.2f} MB"


def build_safe_fence(content: str) -> str:
    """Construct a code fence that does not conflict with content."""
    count = 3
    while "`" * count in content:
        count += 1
    return "`" * count


UNNEEDED_RUNNER_PATTERNS: List[str] = [
    r"^Build started\b.*",
    r"^VSTest version\b.*",
    r"^Starting test execution, please wait\.\.\.",
    r"^A total of \d+ test files matched\b.*",
    r"^\[xUnit\.net [^\]]+\]\s+(?:Discovering|Discovered|Starting|Finished):.*",
    r"^\[xUnit\.net [^\]]+\]\s+xUnit\.net VSTest Adapter.*",
    r"^Determining projects to restore\.\.\.",
    r"^All projects are up-to-date for restore\.",
    r"^Build (?:succeeded|failed)\.",
    r"^\s*\d+ Warning\(s\)",
    r"^\s*\d+ Error\(s\)",
    r"^Time Elapsed\b.*",
    r"^Microsoft \(R\) Build Engine\b.*",
    r"^Copyright \(C\) Microsoft Corporation\..*",
    r"^Restore complete \(\d+ms\)\.",
]


def clean_and_filter_runner_logs(raw_content: str) -> Tuple[str, Dict[str, any]]:
    """
    Cleans raw test runner logs by removing ANSI escape sequences, MSBuild banners,
    VSTest adapter discovery chatter, and restore notices.
    Preserves test suites, passed/failed statuses, detailed failure stack traces,
    and summary metrics.
    """
    cleaned = re.sub(r"\x1b\[[0-9;]*[a-zA-Z]", "", raw_content)
    lines = cleaned.splitlines()

    filtered_lines: List[str] = []
    failed_tests: List[str] = []
    in_failure_block = False
    current_failure_lines: List[str] = []

    passed_individual = 0
    failed_individual = 0
    skipped_individual = 0

    passed_summary = 0
    failed_summary = 0
    skipped_summary = 0
    total_summary = 0
    has_test_run_failed = False

    for line in lines:
        stripped = line.strip()

        # Filter out unneeded runner noise
        if any(re.match(pat, stripped) for pat in UNNEEDED_RUNNER_PATTERNS):
            continue

        # Detect failed test entry
        if re.match(r"^\s*Failed\s+", line):
            failed_individual += 1
            if current_failure_lines:
                failed_tests.append("\n".join(current_failure_lines))
                current_failure_lines = []
            in_failure_block = True
            current_failure_lines.append(line.strip())
        elif in_failure_block:
            if (
                re.match(r"^\s*Passed\s+", line)
                or re.match(r"^Test Run (?:Successful|Failed)\.", stripped)
                or re.match(r"^(?:Failed|Passed)!\s+-", stripped)
                or stripped.startswith("===")
            ):
                in_failure_block = False
                if current_failure_lines:
                    failed_tests.append("\n".join(current_failure_lines))
                    current_failure_lines = []
            elif stripped:
                current_failure_lines.append(line.rstrip())

        if re.match(r"^\s*Passed\s+", line):
            passed_individual += 1
        elif re.match(r"^\s*Skipped\s+", line):
            skipped_individual += 1

        if "Test Run Failed" in stripped:
            has_test_run_failed = True

        m_inline = re.search(
            r"Failed:\s*(\d+),\s*Passed:\s*(\d+),\s*Skipped:\s*(\d+),\s*Total:\s*(\d+)",
            stripped,
        )
        if m_inline:
            f, p, s, t = map(int, m_inline.groups())
            failed_summary += f
            passed_summary += p
            skipped_summary += s
            total_summary += t

        m_tot = re.match(r"^Total tests:\s*(\d+)", stripped)
        if m_tot:
            total_summary += int(m_tot.group(1))

        m_pass = re.match(r"^Passed:\s*(\d+)", stripped)
        if m_pass:
            passed_summary += int(m_pass.group(1))

        m_fail = re.match(r"^Failed:\s*(\d+)", stripped)
        if m_fail:
            failed_summary += int(m_fail.group(1))

        m_skip = re.match(r"^Skipped:\s*(\d+)", stripped)
        if m_skip:
            skipped_summary += int(m_skip.group(1))

        filtered_lines.append(line)

    if current_failure_lines:
        failed_tests.append("\n".join(current_failure_lines))

    compact_lines: List[str] = []
    prev_blank = False
    for line in filtered_lines:
        is_blank = not line.strip()
        if is_blank and prev_blank:
            continue
        compact_lines.append(line)
        prev_blank = is_blank

    clean_text = "\n".join(compact_lines).strip()
    total_tests = total_summary or (passed_individual + failed_individual + skipped_individual)
    passed_tests = passed_summary or passed_individual
    failed_tests_count = failed_summary or failed_individual or len(failed_tests)
    skipped_tests = skipped_summary or skipped_individual

    status = "FAILED" if (has_test_run_failed or failed_tests_count > 0 or failed_tests) else "PASSED"

    stats = {
        "status": status,
        "passed": passed_tests,
        "failed": failed_tests_count,
        "skipped": skipped_tests,
        "total": total_tests,
        "failed_details": failed_tests,
    }
    return clean_text, stats


def load_runner_logs(
    log_targets: List[str],
    root_path: Path,
) -> Optional[Tuple[str, Dict[str, any]]]:
    """
    Loads runner logs from provided paths (or discovers default runner logs in repo root).
    Returns (cleaned_text, stats) or None if no runner logs exist.
    """
    discovered_files: List[Path] = []

    for target in log_targets:
        p = Path(target)
        if not p.is_absolute():
            p = (root_path / p).resolve()
        if p.is_dir():
            for f in sorted(p.glob("*.log")):
                discovered_files.append(f)
        elif p.is_file():
            discovered_files.append(p)

    if not discovered_files and not log_targets:
        # Check defaults in root directory
        for default_name in ["runner-tests.log", "runner.log", "test-logs/runner-tests.log"]:
            candidate = root_path / default_name
            if candidate.is_file():
                discovered_files.append(candidate)
                break

    if not discovered_files:
        return None

    raw_chunks: List[str] = []
    for log_file in discovered_files:
        try:
            content = log_file.read_text(encoding="utf-8", errors="replace")
            try:
                rel = log_file.relative_to(root_path).as_posix()
            except ValueError:
                rel = log_file.name
            if len(discovered_files) > 1:
                raw_chunks.append(f"=== Log: {rel} ===\n{content}")
            else:
                raw_chunks.append(content)
        except (OSError, PermissionError) as ex:
            sys.stderr.write(f"Warning: could not read runner log {log_file}: {ex}\n")

    if not raw_chunks:
        return None

    combined_raw = "\n\n".join(raw_chunks)
    return clean_and_filter_runner_logs(combined_raw)


def main() -> int:
    parser = argparse.ArgumentParser(
        description="Dump repository contents into a single comprehensive Markdown file for LLM analysis."
    )
    parser.add_argument(
        "-o",
        "--output",
        default="REPO_DUMP.md",
        help="Output Markdown file path (default: REPO_DUMP.md)",
    )
    parser.add_argument(
        "-r",
        "--root",
        default=".",
        help="Repository root directory to dump (default: current directory)",
    )
    parser.add_argument(
        "-e",
        "--exclude",
        action="append",
        default=[],
        help="Additional glob pattern(s) or names to exclude (can be specified multiple times or comma-separated)",
    )
    parser.add_argument(
        "--runner-logs",
        action="append",
        default=[],
        help="Path(s) to runner log file or directory (can be specified multiple times, auto-detects runner-tests.log if present)",
    )
    parser.add_argument(
        "--include-tree",
        action=argparse.BooleanOptionalAction,
        default=True,
        help="Include visual ASCII directory tree in output (default: True)",
    )
    parser.add_argument(
        "--telegram",
        action="store_true",
        help="Send generated dump directly to Telegram (uses TELEGRAM_BOT_TOKEN and TELEGRAM_CHAT_ID)",
    )
    parser.add_argument(
        "--telegram-token",
        default=os.environ.get("TELEGRAM_BOT_TOKEN"),
        help="Telegram Bot API Token",
    )
    parser.add_argument(
        "--telegram-chat-id",
        default=os.environ.get("TELEGRAM_CHAT_ID"),
        help="Telegram Chat ID",
    )

    args = parser.parse_args()

    root_path = Path(args.root).resolve()
    if not root_path.exists() or not root_path.is_dir():
        sys.stderr.write(f"Error: root directory not found: {args.root}\n")
        return 1

    output_path = Path(args.output).resolve()

    # Parse custom exclude patterns
    custom_patterns: List[str] = []
    for item in args.exclude:
        for part in item.split(","):
            part = part.strip()
            if part:
                custom_patterns.append(part)

    print(f"Scanning repository at: {root_path}")
    files, tree_nodes = collect_files(root_path, output_path, custom_patterns)

    total_files = len(files)
    total_lines = 0
    total_size = 0

    file_entries: List[Tuple[str, str, int, int, str]] = []

    for file_path in files:
        rel_path = file_path.relative_to(root_path).as_posix()
        try:
            raw = file_path.read_bytes()
            size = len(raw)
            try:
                content = raw.decode("utf-8")
            except UnicodeDecodeError:
                content = raw.decode("latin-1")

            lines_count = len(content.splitlines())
            lang = get_syntax_lang(file_path)

            total_lines += lines_count
            total_size += size
            file_entries.append((rel_path, content, lines_count, size, lang))
        except (OSError, PermissionError) as ex:
            sys.stderr.write(f"Warning: could not read {rel_path}: {ex}\n")

    print(f"Found {total_files} text/source files ({total_lines} lines, {format_size(total_size)})")

    # Load and filter test runner logs
    runner_logs_result = load_runner_logs(args.runner_logs, root_path)
    if runner_logs_result:
        _, runner_stats = runner_logs_result
        print(f"Loaded and filtered runner logs: Status={runner_stats['status']} (Passed={runner_stats['passed']}, Failed={runner_stats['failed']})")

    # Generate Markdown output
    output_path.parent.mkdir(parents=True, exist_ok=True)
    with open(output_path, "w", encoding="utf-8") as out:
        out.write(f"# Repository Dump: {root_path.name}\n\n")
        out.write("## Overview\n\n")
        out.write(f"- **Total Files**: {total_files}\n")
        out.write(f"- **Total Lines of Code**: {total_lines}\n")
        out.write(f"- **Total Size**: {format_size(total_size)}\n")
        out.write(f"- **Root Directory**: `{root_path}`\n\n")

        if runner_logs_result:
            cleaned_runner_text, runner_stats = runner_logs_result
            out.write("## Test Execution & Diagnostic Logs\n\n")
            if runner_stats["status"] == "FAILED":
                out.write("> [!CAUTION]\n")
                out.write("> **Test Execution Status: FAILED**\n")
                if runner_stats["total"] > 0:
                    out.write(
                        f"> - **Total Tests**: {runner_stats['total']} | **Passed**: {runner_stats['passed']} | **Failed**: {runner_stats['failed']} | **Skipped**: {runner_stats['skipped']}\n"
                    )
                out.write("\n")
                if runner_stats["failed_details"]:
                    out.write("### ❌ Failed Test Details\n\n")
                    out.write("```text\n")
                    for fd in runner_stats["failed_details"]:
                        out.write(fd.strip() + "\n\n")
                    out.write("```\n\n")
            else:
                out.write("> [!NOTE]\n")
                out.write("> **Test Execution Status: PASSED**\n")
                if runner_stats["total"] > 0:
                    out.write(
                        f"> - **Total Tests**: {runner_stats['total']} | **Passed**: {runner_stats['passed']} | **Failed**: {runner_stats['failed']} | **Skipped**: {runner_stats['skipped']}\n"
                    )
                out.write("\n")

            out.write("### Filtered Test Runner Logs\n\n")
            fence = build_safe_fence(cleaned_runner_text)
            out.write(f"{fence}text\n")
            out.write(cleaned_runner_text)
            if not cleaned_runner_text.endswith("\n"):
                out.write("\n")
            out.write(f"{fence}\n\n")
            out.write("---\n\n")

        if args.include_tree:
            out.write("## Repository Structure\n\n")
            out.write("```text\n")
            tree_str = generate_ascii_tree(root_path.name, tree_nodes)
            out.write(tree_str)
            out.write("\n```\n\n")

        out.write("---\n\n")

        for rel_path, content, lines_count, size, lang in file_entries:
            out.write(f"## File: {rel_path}\n\n")
            out.write(f"**Lines**: {lines_count} | **Size**: {format_size(size)} | **Language**: {lang}\n\n")
            fence = build_safe_fence(content)
            out.write(f"{fence}{lang}\n")
            out.write(content)
            if not content.endswith("\n"):
                out.write("\n")
            out.write(f"{fence}\n\n")

    print(f"Repository dump generated successfully at: {output_path}")

    if args.telegram:
        token = args.telegram_token or os.environ.get("TELEGRAM_BOT_TOKEN")
        chat_id = args.telegram_chat_id or os.environ.get("TELEGRAM_CHAT_ID")
        if token and chat_id:
            print(f"Sending repository dump to Telegram chat {chat_id}...")
            if send_telegram(output_path, token, chat_id, f"🌟 STARRY Repository Dump ({output_path.name})"):
                print("Successfully sent dump to Telegram!")
            else:
                print("Failed to send dump to Telegram.")
        else:
            sys.stderr.write("Telegram token or chat ID not set. Set TELEGRAM_BOT_TOKEN and TELEGRAM_CHAT_ID.\n")

    return 0


def send_telegram(file_path: Path, token: str, chat_id: str, caption: str = "📦 STARRY Repository Dump") -> bool:
    import urllib.request
    import uuid

    boundary = f"----WebKitFormBoundary{uuid.uuid4().hex}"
    body = []

    # chat_id field
    body.append(f"--{boundary}".encode("utf-8"))
    body.append(b'Content-Disposition: form-data; name="chat_id"')
    body.append(b"")
    body.append(str(chat_id).encode("utf-8"))

    # caption field
    body.append(f"--{boundary}".encode("utf-8"))
    body.append(b'Content-Disposition: form-data; name="caption"')
    body.append(b"")
    body.append(caption.encode("utf-8"))

    # document field
    filename = file_path.name
    content_type = "text/markdown"
    body.append(f"--{boundary}".encode("utf-8"))
    body.append(f'Content-Disposition: form-data; name="document"; filename="{filename}"'.encode("utf-8"))
    body.append(f"Content-Type: {content_type}".encode("utf-8"))
    body.append(b"")
    body.append(file_path.read_bytes())

    body.append(f"--{boundary}--".encode("utf-8"))
    body.append(b"")

    payload = b"\r\n".join(body)

    url = f"https://api.telegram.org/bot{token}/sendDocument"
    req = urllib.request.Request(
        url,
        data=payload,
        headers={"Content-Type": f"multipart/form-data; boundary={boundary}"},
    )

    try:
        with urllib.request.urlopen(req, timeout=30) as resp:
            resp.read()
            return True
    except Exception as ex:
        sys.stderr.write(f"Failed to send dump to Telegram: {ex}\n")
        return False


if __name__ == "__main__":
    sys.exit(main())
