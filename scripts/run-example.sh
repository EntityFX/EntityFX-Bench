#!/bin/bash
### NOTE: This script shouldn't be used as-is.
###       It requires you top have all necessary interpretes, compileres and VMs installed
###       It also require you to edit dotnet6 path to point to your dotnet SDK

# On Debian, to install everything except dotnet (you should install it from Microsoft website) and Python3 (should be already installed)
# you can use following command:
#  apt install nodejs php-cli mono-runtime pypy3 lua5.4 luajit openjdk-18-jre-headless golang-1.19

# Depending on how old is your distro, it might need some adjustments, e.x. openjdk version and golang version might be different

function check_already_run() {
	RES_DIR=${1}
	if [[ ! -f ./Output.log ]] && [[ ! -f ${RES_DIR}/Output.log ]]; then
		return 0
	fi
	return 1
}

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
check_already_run "${RES_DIR}" && { go build; ./entityfx; }
mv *.log "${RES_DIR}/"
popd

pushd "${BASE_DIR}/src/js"
RES_DIR="${BASE_DIR}/results/nodejs/${NAME}_node$(node --version)"
mkdir -p "${RES_DIR}"
check_already_run "${RES_DIR}" && { node ./node_main.js | sed -r "s/\x1B\[([0-9]{1,3}(;[0-9]{1,2})?)?[mGK]//g" | tee Output.log; }
mv *.log "${RES_DIR}/"
popd

pushd "${BASE_DIR}/src/lua"
LUA_VER=$(lua -v | awk '{print $2}')
RES_DIR="${BASE_DIR}/results/lua/${NAME}_lua${LUA_VER}"
mkdir -p "${RES_DIR}"
check_already_run "${RES_DIR}" &&  lua ./main.lua
mv *.log "${RES_DIR}/"

RES_DIR="${BASE_DIR}/results/lua/${NAME}_luajit"
mkdir -p "${RES_DIR}"
check_already_run "${RES_DIR}" &&  luajit ./main.lua
mv *.log "${RES_DIR}/"
mv *.log "${RES_DIR}/"
popd

pushd "${BASE_DIR}/src/php"
PHP_VER=$(php -v | head -n1 | awk '{print $2}' | cut -d'-' -f 1)
RES_DIR="${BASE_DIR}/results/php/${NAME}_php${PHP_VER}"
mkdir -p "${RES_DIR}"
check_already_run "${RES_DIR}" && php ./Main.php
mv *.log "${RES_DIR}/"
popd

pushd "${BASE_DIR}/src/python"
PYTHON_VER=$(python --version | awk '{print $2}')
RES_DIR="${BASE_DIR}/results/python/${NAME}_python${PYTHON_VER}"
mkdir -p "${RES_DIR}"
check_already_run "${RES_DIR}" &&  python ./main.py
mv *.log "${RES_DIR}/"

PYTHON_VER=$(python3 --version | awk '{print $2}')
RES_DIR="${BASE_DIR}/results/python/${NAME}_python${PYTHON_VER}"
mkdir -p "${RES_DIR}"
check_already_run "${RES_DIR}" && python3 ./main.py
mv *.log "${RES_DIR}/"



PYTHON_VER=$(pypy --version | tail -n 1 | awk '{print $2}')
RES_DIR="${BASE_DIR}/results/python/${NAME}_pypy3-${PYTHON_VER}"
mkdir -p "${RES_DIR}"
check_already_run "${RES_DIR}" && pypy ./main.py
mv *.log "${RES_DIR}/"
popd

pushd "${BASE_DIR}/release/dotnet/net6.0"
DOTNET="~/bin/dotnet-sdk-7.0.100-ea1-loongarch64/dotnet"
RES_DIR="${BASE_DIR}/results/dotnet/${NAME}_dotnet6"
mkdir -p "${RES_DIR}"
SDK_VERSION=$(${DOTNET} --list-runtimes | awk '{print $2}' | sort -u | head -n 1)
cp EntityFX.NetBenchmark.runtimeconfig.json{,.bak}
sed -i "s/6\.0\.0/${SDK_VERSION}/g" EntityFX.NetBenchmark.runtimeconfig.json
check_already_run "${RES_DIR}" && ${DOTNET} EntityFX.NetBenchmark.dll
mv EntityFX.NetBenchmark.runtimeconfig.json{.bak,}
mv *.log "${RES_DIR}/"
popd

pushd "${BASE_DIR}/release/dotnet/net45"
DOTNET="mono"
RES_DIR="${BASE_DIR}/results/dotnet/${NAME}_mono"
mkdir -p "${RES_DIR}"
check_already_run "${RES_DIR}" && ${DOTNET} EntityFX.NetBenchmark.exe
mv *.log "${RES_DIR}/"
popd

pushd "${BASE_DIR}/release/java"
RES_DIR="${BASE_DIR}/results/java/${NAME}_jre$(java --version | head -n 1 | awk '{print $2}' | cut -d. -f 1)"
mkdir -p "${RES_DIR}"
check_already_run "${RES_DIR}" && java -jar ./EntityFXBench.jar
mv *.log "${RES_DIR}/"
popd
