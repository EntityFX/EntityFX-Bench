LU = class(function(f)
end)

function LU.factor(a, pivot, m, n)
    local min_m_n = m < n and m or n
    for j=1,min_m_n do
        local jp, t = j, math.abs(a[j][j])
        for i=j+1,m do
            local ab = math.abs(a[i][j])
            if ab > t then
                jp = i
                t = ab
            end
        end
        pivot[j] = jp
        if a[jp][j] == 0 then error("zero pivot") end
        if jp ~= j then a[j], a[jp] = a[jp], a[j] end
        if j < m then
            local recp = 1.0 / a[j][j]
            for k=j+1,m do
                local v = a[k]
                v[j] = v[j] * recp
            end
        end
        if j < min_m_n then
            for i=j+1,m do
                local vi, vj = a[i], a[j]
                local eij = vi[j]
                for k=j+1,n do vi[k] = vi[k] - eij * vj[k] end
            end
        end
    end
end

function LU.matrix_alloc(m, n)
    local a = {}
    for y=1,m do a[y] = {} end
    return a
end
  
function LU.matrix_copy(dst, src, m, n)
    for y=1,m do
        local vd, vs = dst[y], src[y]
        for x=1,n do vd[x] = vs[x] end
    end
end