const fs = require('fs');
const fs = require('fs');
const path = require('path');

/**
 * 验证 .json 是否能正确还原为原始 .bin
 * @param {string} jsonPath - 待验证的 .json 文件路径
 * @param {string} originalBinPath - 原始 .bin 文件路径
 * @param {boolean} saveRecovered - 是否保存还原的 .bin 到 recovered.bin
 */
function verifyJsonToBin(jsonPath, originalBinPath, saveRecovered = true) {
  try {
    // 1. 读取 JSON 并解析
    const jsonContent = fs.readFileSync(jsonPath, 'utf8');
    const hexArray = JSON.parse(jsonContent);
    if (!Array.isArray(hexArray) || hexArray.some(h => typeof h !== 'string' || h.length !== 2)) {
      throw new Error(`JSON 格式错误：非 2 位十六进制数组`);
    }

    // 2. 转换为 Buffer
    const recoveredBuffer = Buffer.from(hexArray.join(''), 'hex');

    // 3. 读取原始 .bin
    const originalBuffer = fs.readFileSync(originalBinPath);
    if (recoveredBuffer.length !== originalBuffer.length) {
      throw new Error(`大小不一致：JSON 还原 ${recoveredBuffer.length}B vs 原始 ${originalBuffer.length}B`);
    }

    // 4. 逐字节比较
    for (let i = 0; i < originalBuffer.length; i++) {
      if (recoveredBuffer[i] !== originalBuffer[i]) {
        throw new Error(`字节 ${i} 不匹配：0x${originalBuffer[i].toString(16)} vs 0x${recoveredBuffer[i].toString(16)}`);
      }
    }

    // 5. 保存还原文件（可选）
    if (saveRecovered) {
      const recoveredPath = path.join(path.dirname(jsonPath), 'recovered.bin');
      fs.writeFileSync(recoveredPath, recoveredBuffer);
      console.log(`✅ 还原文件已保存：${recoveredPath}`);
    }

    console.log(`✅ 验证通过：JSON 与原始 .bin 完全一致`);
    return true;
  } catch (error) {
    console.error(`❌ 验证失败：${error.message}`);
    return false;
  }
}

// 示例用法（修改路径后直接运行）
// verifyJsonToBin('./models/retinaface_mnet25_bin.json', './models/retinaface_mnet25.bin');