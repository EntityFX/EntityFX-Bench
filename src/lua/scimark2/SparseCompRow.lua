SparseCompRow = class(function(f)
end)

function SparseCompRow.mult(n, cycles, vy, val, row, col, vx)
    for p=1,cycles do
        for r=1,n do
            local sum = 0
            for i=row[r],row[r+1]-1 do sum = sum + vx[col[i]] * val[i] end
            vy[r] = sum
        end
    end
end