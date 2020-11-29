local sha = require 'generic/sha2'

HashBase = class(BenchmarkBase, function(a, writer, printToConsole)
    BenchmarkBase.init(a, writer, printToConsole)
    a.iterrations = 2000000
    a.ratio = 10

    a.strs = {"the quick brown fox jumps over the lazy dog", "Some red wine",
        "Candels & Ropes"}
end)

function HashBase:doHash(i, preparedStrs)
    local sha1 = sha.sha1(preparedStrs[i % 3 + 1])
    local sha256 = sha.sha256(preparedStrs[(i + 1) % 3 + 1])
    local result = sha1 .. sha256
    return string.byte(hex_to_binary(result), 1, 256)
end