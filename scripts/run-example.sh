#!/bin/bash
### NOTE: This script shouldn't be used as-is.
###       It requires you top have all necessary interpretes, compileres and VMs installed
###       It also require you to edit dotnet6 path to point to your dotnet SDK

# On Debian, to install everything except dotnet (you should install it from Microsoft website) and Python3 (should be already installed)
# you can use following command:
#  apt install nodejs php-cli mono-runtime pypy3 lua5.4 luajit openjdk-18-jre-headless golang-1.19

# Depending on how old is your distro, it might need some adjustments, e.x. openjdk version and golang version might be different

BASE_DIR=$(pwd)
NAME="${1}"

if [[ -z ${NAME} ]]; then
	echo "Please specify name of the Results dir"
	exit 1
fi

# Go
pushd "${BASE_DIR}/src/go/entityfx"
RES_DIR="${BASE_DIR}/results/go/${NAME}-$(go version | awk '{print $3}')"
mkdir -p "${RES_DIR}"
go build
[[ ! -f ./Output.log ]] && ./entityfx
mv *.log "${RES_DIR}/"
popd

pushd "${BASE_DIR}/src/js"
RES_DIR="${BASE_DIR}/results/nodejs/${NAME}_node$(node --version)"
mkdir -p "${RES_DIR}"
[[ ! -f ./Output.log ]] && node ./node_main.js | sed -r "s/\x1B\[([0-9]{1,3}(;[0-9]{1,2})?)?[mGK]//g" | tee Output.log
mv *.log "${RES_DIR}/"
popd

pushd "${BASE_DIR}/src/lua"
LUA_VER=$(lua -v | awk '{print $2}')
RES_DIR="${BASE_DIR}/results/lua/${NAME}_lua${LUA_VER}"
mkdir -p "${RES_DIR}"
[[ ! -f ./Output.log ]] && lua ./main.lua
mv *.log "${RES_DIR}/"

RES_DIR="${BASE_DIR}/results/lua/${NAME}_luajit"
mkdir -p "${RES_DIR}"
[[ ! -f ./Output.log ]] && luajit ./main.lua
mv *.log "${RES_DIR}/"
popd

pushd "${BASE_DIR}/src/php"
PHP_VER=$(php -v | head -n1 | awk '{print $2}' | cut -d'-' -f 1)
RES_DIR="${BASE_DIR}/results/php/${NAME}_php${PHP_VER}"
mkdir -p "${RES_DIR}"
[[ ! -f ./Output.log ]] && php ./Main.php
mv *.log "${RES_DIR}/"
popd

pushd "${BASE_DIR}/src/python"
PYTHON_VER=$(python --version | awk '{print $2}')
RES_DIR="${BASE_DIR}/results/python/${NAME}_python${PYTHON_VER}"
mkdir -p "${RES_DIR}"
[[ ! -f ./Output.log ]] && python ./main.py
mv *.log "${RES_DIR}/"

PYTHON_VER=$(pypy3 --version | tail -n 1 | awk '{print $2}')
RES_DIR="${BASE_DIR}/results/python/${NAME}_pypy3-${PYTHON_VER}"
mkdir -p "${RES_DIR}"
[[ ! -f ./Output.log ]] && pypy3 ./main.py
mv *.log "${RES_DIR}/"
popd

pushd "${BASE_DIR}/release/dotnet/net6.0"
DOTNET="/opt/dotnet/dotnet"
RES_DIR="${BASE_DIR}/results/dotnet/${NAME}_dotnet6"
mkdir -p "${RES_DIR}"
[[ ! -f ./Output.log ]] && ${DOTNET} EntityFX.NetBenchmark.dll
mv *.log "${RES_DIR}/"
popd

pushd "${BASE_DIR}/release/dotnet/net45"
DOTNET="mono"
RES_DIR="${BASE_DIR}/results/dotnet/${NAME}_mono"
mkdir -p "${RES_DIR}"
[[ ! -f ./Output.log ]] && ${DOTNET} EntityFX.NetBenchmark.exe
mv *.log "${RES_DIR}/"
popd

pushd "${BASE_DIR}/release/java"
RES_DIR="${BASE_DIR}/results/java/${NAME}_jre$(java --version | head -n 1 | awk '{print $2}' | cut -d. -f 1)"
mkdir -p "${RES_DIR}"
[[ ! -f ./Output.log ]] && java -jar ./EntityFXBench.jar
mv *.log "${RES_DIR}/"
popd
