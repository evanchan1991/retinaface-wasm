#!/bin/bash

# 激活 Emscripten 环境（需替换为你的 emsdk 路径）
cd ../emsdk
source emsdk_env.sh
cd -
mkdir build_wasm && cd build_wasm
# 关键：添加小程序必需的编译参数
emcmake cmake -DCMAKE_BUILD_TYPE=Release -DWASM_ASYNC_COMPILATION=0 ../ 
emmake make -j4
cd ../