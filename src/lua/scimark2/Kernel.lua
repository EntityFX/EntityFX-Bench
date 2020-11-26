Kernel = class(function(f)
end)

function Kernel.FFT(n)
    local l2n = math.log(n)/math.log(2)
    if l2n % 1 ~= 0 then
        io.stderr:write("Error: FFT data length is not a power of 2\n")
        os.exit(1)
    end
    local v = Kernel.random_vector(n*2)
    return function(cycles)
        local norm = 1.0 / n
        for p=1,cycles do
            FFT.transform(v, n, -1)
            FFT.transform(v, n, 1)
            for i=1,n*2 do v[i] = v[i] * norm end
        end
        return ((5*n-2)*l2n + 2*(n+1)) * cycles
    end
end

function Kernel.SOR(n)
    local mat = Kernel.random_matrix(n, n)
    return function(cycles)
        SOR.execute(mat, n, n, cycles, 1.25)
        return (n-1)*(n-1)*cycles*6
    end
end

function Kernel.MC()
    return function(cycles)
        local res = MonteCarlo.integrate(cycles)
        assert(math.sqrt(cycles)*math.abs(res-math.pi) < 5.0, "bad MC result")
        return cycles * 4 -- Way off, but same as SciMark in C/Java.
    end
end

function Kernel.LU(n)
    local mat = Kernel.random_matrix(n, n)
    local tmp = LU.matrix_alloc(n, n)
    local pivot = {}
    return function(cycles)
        for i=1,cycles do
            LU.matrix_copy(tmp, mat, n, n)
            LU.factor(tmp, pivot, n, n)
        end
        return 2.0/3.0*n*n*n*cycles
    end
end

function Kernel.SPARSE(n, nz)
    local nr = math.floor(nz/n)
    local anz = nr*n
    local vx = Kernel.random_vector(n)
    local val = Kernel.random_vector(anz)
    local vy, col, row = {}, {}, {}
    row[1] = 1
    for r=1,n do
        local step = math.floor(r/nr)
        if step < 1 then step = 1 end
        local rr = row[r]
        row[r+1] = rr+nr
        for i=0,nr-1 do col[rr+i] = 1+i*step end
    end
    return function(cycles)
        SparseCompRow.mult(n, cycles, vy, val, row, col, vx)
        return anz*cycles*2
    end
end

function Kernel.random_vector(n)
    local v = {}
    for x=1,n do v[x] = math.random() end
    return v
end
  
function Kernel.random_matrix(m, n)
    local a = {}
    for y=1,m do
        local v = {}
        a[y] = v
        for x=1,n do v[x] = math.random() end
    end
    return a
end