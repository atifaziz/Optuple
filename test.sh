#!/usr/bin/env bash
[[ -e test.sh ]] || { echo >&2 "Please cd into the script location before running it."; exit 1; }
set -e
./build.sh
dotnet test --no-build tests -c Debug -p:CollectCoverage=true \
                                      -p:CoverletOutputFormat=opencover \
                                      -p:Exclude=[NUnit*]*
dotnet test --no-build tests -c Release

