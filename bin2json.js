const fs = require('fs');
const path = require('path');


/**
 * 将 ncnn .bin 文件转换为十六进制 JSON 数组（供 generate-headers.js 使用）
 * @param {string} binPath - 原始 .bin 文件路径
 * @param {string} jsonPath - 输出 .json 文件路径（默认同目录 + _bin.json 后缀）
 */
function convertBinToJson(binPath, jsonPath = binPath.replace(/\.bin$/, '_bin.json')) {
  try {
    // 1. 读取二进制文件
    const binBuffer = fs.readFileSync(binPath);
    if (binBuffer.length === 0) {
      throw new Error(`文件 ${binPath} 为空`);
    }

    // 2. 转换为十六进制数组（2 位小写，补零）
    const hexArray = Array.from(binBuffer).map(byte => 
      byte.toString Array.from(binBuffer).map(byte => 
      byte.toString(16).padStart(2, '0')
    );

    // 3. 写入 JSON（保留缩进，便于人工检查）
    fs.writeFileSync(jsonPath, JSON.stringify(hexArray, null, 2));
    console.log(`✅ 转换完成：${jsonPath}（${hexArray.length} 字节）`);
    return true;
  } catch (error) {
    console.error(`❌ 转换失败：${error.message}`);
    return false;
  }
}

// 示例用法（修改路径后直接运行）
// convertBinToJson('./models/retinaface_mnet25.bin');