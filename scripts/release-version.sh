#!/bin/bash

set -e

# Run semantic-release, filtering out 'npm WARN' from stderr
SEMANTIC_OUTPUT=$(npx -y -p semantic-release@22.0.7 \
                      -p @semantic-release/commit-analyzer@11.0.0 \
                      semantic-release 2> >(grep -v "^npm WARN" >&2))

# Extract the version information from the output
NEXT_VERSION=$(echo "$SEMANTIC_OUTPUT" | \
               grep 'The next release version is' | \
               sed -E 's/.* ([0-9]+\.[0-9]+\.[0-9]+(-[a-zA-Z0-9]+(\.[0-9]+)?)?)$/\1/')

# If NEXT_VERSION is empty, try to extract the current version
if [ -z "$NEXT_VERSION" ]; then
    echo "No next version calculated."
    CURRENT_VERSION=$(echo "$SEMANTIC_OUTPUT" | \
                    grep 'Found git tag' | \
                    sed -E 's/.*associated with version ([0-9]+\.[0-9]+\.[0-9]+(-[a-zA-Z0-9]+(\.[0-9]+)?)?) .*/\1/')
fi

# Handle case where neither next nor current version is found
if [ -z "$NEXT_VERSION" ] && [ -z "$CURRENT_VERSION" ]; then
    echo "Error: No version information found"
    exit 1
fi

# Use the calculated next version or the current version if no next version is available
VERSION=${NEXT_VERSION:-$CURRENT_VERSION}
echo "Version: $VERSION"

# Parse major, minor, patch, and prerelease information
if [[ $VERSION =~ ([0-9]+)\.([0-9]+)\.([0-9]+)(-([a-zA-Z0-9]+)\.([0-9]+))? ]]; then
    MAJOR=${BASH_REMATCH[1]}
    MINOR=${BASH_REMATCH[2]}
    PATCH=${BASH_REMATCH[3]}
    PRERELEASE_TYPE=${BASH_REMATCH[5]}
    PRERELEASE_NUMBER=${BASH_REMATCH[6]}

    # Use prerelease number as revision if present, otherwise default to 0
    REVISION=${PRERELEASE_NUMBER:-0}

    ASSEMBLY_VERSION="$MAJOR.$MINOR.$PATCH.$REVISION"
else
    echo "Error: VERSION format is not recognized"
    exit 1
fi

echo "##vso[task.setvariable variable=NEXT_VERSION;isOutput=true]$VERSION"
echo "##vso[task.setvariable variable=ASSEMBLY_VERSION;isOutput=true]$ASSEMBLY_VERSION"
