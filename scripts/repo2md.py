#!/usr/bin/env python3
"""
Repository Dumper Utility (repo2md)
Dumps the repository into a single comprehensive Markdown file for LLM analysis.
"""

import argparse
import fnmatch
import os
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
        "--include-tree",
        action=argparse.BooleanOptionalAction,
        default=True,
        help="Include visual ASCII directory tree in output (default: True)",
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

    # Generate Markdown output
    output_path.parent.mkdir(parents=True, exist_ok=True)
    with open(output_path, "w", encoding="utf-8") as out:
        out.write(f"# Repository Dump: {root_path.name}\n\n")
        out.write("## Overview\n\n")
        out.write(f"- **Total Files**: {total_files}\n")
        out.write(f"- **Total Lines of Code**: {total_lines}\n")
        out.write(f"- **Total Size**: {format_size(total_size)}\n")
        out.write(f"- **Root Directory**: `{root_path}`\n\n")

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
    return 0


if __name__ == "__main__":
    sys.exit(main())
