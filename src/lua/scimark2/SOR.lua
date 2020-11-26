SOR = class(function(f)
end)

function SOR.execute(mat, m, n, cycles, omega)
    local om4, om1 = omega*0.25, 1.0-omega
    m = m - 1
    n = n - 1
    for i=1,cycles do
        for y=2,m do
            local v, vp, vn = mat[y], mat[y-1], mat[y+1]
            for x=2,n do
                v[x] = om4*((vp[x]+vn[x])+(v[x-1]+v[x+1])) + om1*v[x]
            end
        end
    end
end