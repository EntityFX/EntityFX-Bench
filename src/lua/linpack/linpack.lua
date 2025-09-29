Linpack = class(function(w, printToConsole)
    w.output = Writer(nil)
    w.output.useConsole = printToConsole
    w.second_orig = -1
    w.loop_mops = {}
    w.loop_mflops = {}
    w.timeUsed = 0.0
    w.mwips = 0.0
    w.headings = {}
    w.check = 0.0
    w.results = {}
end)

function Linpack:abs(d)
    if d > 0 then return d else return -d end
end

function Linpack:second()
    if self.second_orig == -1 then
        self.second_orig = clock() / 1000.0
    end
    return clock() / 1000.0
end

function Linpack:run_benchmark(array_size)
    self.output:writeLine("Running Linpack " .. array_size .. "x" .. array_size .. " in Lua")

    local mflops_result = 0.0
    local residn_result = 0.0
    local time_result = 0.0
    local eps_result = 0.0

    local a, b, x, ipvt = {}, {}, {}, {}
    local total, norma, normx = 0.0, 0.0, 0.0
    local resid, time = 0.0, 0.0
    local info = 0
    local lda = array_size + 1
    local n = array_size
    local ops = (2*(n*n*n))/3 + 2*(n*n)


    for i = 1, lda do
      a[i] = {}
    end

    norma = self:matgen(a, lda, n, b)
    time = self:second()
    info = self:dgefa(a, lda, n, ipvt)
    self:dgesl(a, lda, n, ipvt, b, 0)
    total = self:second() - time

    for i = 1, n do
      x[i] = b[i]
    end
    norma = self:matgen(a, lda, n, b)
    for i = 1, n do
      b[i] = -b[i]
    end
    self:dmxpy(n, b, n, lda, x, a)
    resid = 0.0
    normx = 0.0
    for i = 1, n do
      if (resid > self:abs(b[i])) then resid = resid else resid = self:abs(b[i]) end
      if (normx > self:abs(x[i])) then normx = normx else normx = self:abs(x[i]) end
    end

    eps_result = self:epslon(1.0)
    residn_result = resid / (n * norma * normx * eps_result)
    residn_result = residn_result + 0.005 -- for rounding
    residn_result = (residn_result * 100)
    residn_result = residn_result / 100

    time_result = total
    time_result = time_result + 0.005 -- for rounding
    time_result = (time_result * 100)
    time_result = time_result / 100

    mflops_result = ops / (1.0e6 * total)
    mflops_result = mflops_result + 0.0005 -- for rounding
    mflops_result = (mflops_result * 1000)
    mflops_result = mflops_result / 1000

    self.output:writeLine("Norma is " .. norma)
    self.output:writeLine("Residual is " .. resid)
    self.output:writeLine("Normalised residual is " .. residn_result)
    self.output:writeLine("Machine result.Eepsilon is " .. eps_result)
    self.output:writeLine("x[0]-1 is " .. (x[1] - 1))
    self.output:writeLine("x[n-1]-1 is " .. (x[n] - 1))
    self.output:writeLine("Time is " .. time_result)
    self.output:writeLine("MFLOPS: " .. mflops_result)

    return {
      norma = norma,
      resid = resid,
      normalisedResidual = residn_result,
      epsilon = eps_result,
      time = time_result,
      mflops = mflops_result
    }
end

function Linpack:matgen(a, lda, n, b)
    local norma = 0.0
    local iseed = {}

    iseed[1] = 1
    iseed[2] = 2
    iseed[3] = 3
    iseed[4] = 1325

    norma = 0.0

    for i = 1, n do
        for j = 1, n do
            --a[i][j] = self:lran(iseed) - 0.5
            a[i][j] = math.random() - 0.5
            if (a[i][j] > norma) then norma = a[i][j] end
        end
    end
    for i = 1, n do
        b[i] = 0.0
    end
    for j = 1, n do
        for i = 1, n do
            b[i] = b[i] + a[j][i]
        end
    end

    return norma
end

function Linpack:lran(seed)
    local m1, m2, m3, m4, ipw2 = 0
    local it1, it2, it3, it4 = 0
    local r, result = 0.0

    m1 = 494
    m2 = 322
    m3 = 2508
    m4 = 2549
    ipw2 = 4096

    r = 1.0 / ipw2

    it4 = seed[4] * m4
    it3 = it4 / ipw2
    it4 = it4 - ipw2 * it3
    it3 = it3 + seed[3] * m4 + seed[4] * m3
    it2 = it3 / ipw2
    it3 = it3 - ipw2 * it2
    it2 = it2 + seed[2] * m4 + seed[3] * m3 + seed[4] * m2
    it1 = it2 / ipw2
    it2 = it2 - ipw2 * it1
    it1 = it1 + seed[1] * m4 + seed[2] * m3 + seed[3] * m2 + seed[4] * m1
    it1 = it1 % ipw2

    seed[1] = it1
    seed[2] = it2
    seed[3] = it3
    seed[4] = it4

    result = r * ( it1 + r * ( it2 + r * ( it3 + r *  it4)))

    return result
end

function Linpack:dgefa(a, lda, n, ipvt)
    local col_k, col_j = {}, {}
    local t = 0.0
    local j, k, kp1, l, nm1 = 0
    local info = 0
    -- gaussian elimination with partial pivoting
    info = 0
    nm1 = n - 1
    if (nm1 >= 0) then
      for k = 1, nm1 do
        col_k = a[k]
        kp1 = k + 1

        -- find l = pivot index

        l = self:idamax(n - k, col_k, k, 1) + k
        ipvt[k] = l

        -- zero pivot implies this column already triangularized

        if (col_k[l] ~= 0) then

          -- interchange if necessary

          if (l ~= k) then
            t = col_k[l]
            col_k[l] = col_k[k]
            col_k[k] = t
          end

          -- compute multipliers

          t = -1.0 / col_k[k]
          self:dscal(n - (kp1), t, col_k, kp1, 1)

          -- row elimination with column indexing

          for j = kp1, n do
            col_j = a[j]
            t = col_j[l]
            if (l ~= k) then
              col_j[l] = col_j[k]
              col_j[k] = t
            end
            self:daxpy(n - (kp1), t, col_k, kp1, 1, col_j, kp1, 1)
          end
        else
          info = k
        end
      end
    end
    ipvt[n - 1] = n - 1
    if (a[(n - 1)][(n - 1)] == 0) then
      info = n - 1
    end
    return info
end

function Linpack:dgesl(a, lda, n, ipvt, b, job)
    local t = 0.0
    local k, kb, l, nm1, kp1 = 0
    nm1 = n - 1
    if (job == 0) then

      -- job = 0 , solve a * x = b. first solve l*y = b

      if (nm1 >= 1) then
        for k = 1, nm1 do
          l = ipvt[k]
          t = b[l]
          if (l ~= k) then
            b[l] = b[k]
            b[k] = t
          end
          kp1 = k + 1
          self:daxpy(n - (kp1), t, a[k], kp1, 1, b, kp1, 1)
        end
      end
      

      -- now solve u*x = y

      for kb = 1, n do
        k = n - (kb - 1)
        b[k] = b[k] / a[k][k]
        t = -b[k]
        self:daxpy(k, t, a[k], 0, 1, b, 0, 1)
      end
    else

      -- job = nonzero, solve trans(a) * x = b. first solve trans(u)*y = b

      for k = 1, n do
        t = self:ddot(k, a[k], 0, 1, b, 0, 1)
        b[k] = (b[k] - t) / a[k][k]
      end

      -- now solve trans(l)*x = y

      if (nm1 >= 1) then
        for kb = 1, nm1 do
          k = n - kb
          kp1 = k + 1
          b[k] = b[k] + self:ddot(n - (kp1), a[k], kp1, 1, b, kp1, 1)
          l = ipvt[k]
          if (l ~= k) then
            t = b[l]
            b[l] = b[k]
            b[k] = t
          end
        end
      end
    end
end

function Linpack:daxpy(n, da, dx, dx_off, incx, dy, dy_off, incy)
  local i, ix, iy = 0

    if ((n > 0) and (da ~= 0)) then
      if (incx ~= 1 or incy ~= 1) then

        -- code for unequal increments or equal increments not equal to 1

        ix = 0
        iy = 0
        if (incx < 0) then
          ix = (-n + 1) * incx
        end
        if (incy < 0) then
          iy = (-n + 1) * incy
        end
        for i = 1, n do
          dy[iy + dy_off] = dy[iy + dy_off] + da * dx[ix + dx_off]
          ix = ix + incx
          iy = iy + incy
        end
        return
      else

        -- code for both increments equal to 1

        for i = 1, n do
          dy[i + dy_off] = dy[i + dy_off] + da * dx[i + dx_off]
        end
      end
    end
end

function Linpack:ddot(n, dx, dx_off, incx, dy, dy_off, incy)
    local dtemp = 0.0
    local i, ix, iy = 0

    dtemp = 0

    if (n > 0) then

      if (incx ~= 1 or incy ~= 1) then

        -- code for unequal increments or equal increments not equal to 1

        ix = 0
        iy = 0
        if (incx < 0) then
          ix = (-n + 1) * incx
        end
        if (incy < 0) then
          iy = (-n + 1) * incy
        end
        for i = 1, n do
          dtemp = dtemp + dx[ix + dx_off] * dy[iy + dy_off]
          ix = ix + incx
          iy = iy + incy
        end
      else

        -- code for both increments equal to 1

        for i = 1, n do
          dtemp = dtemp + dx[i + dx_off] * dy[i + dy_off]
        end
      end
    end
    return (dtemp)
end

function Linpack:dscal(n, da, dx, dx_off, incx)
  local i, nincx = 0

    if (n > 0) then
      if (incx ~= 1) then

        -- code for increment not equal to 1

        nincx = n * incx
        for i = 1, nincx, incx do
          dx[i + dx_off] = dx[i + dx_off] * da
        end
      else

        -- code for increment equal to 1

        for i = 1, n do
          dx[i + dx_off] = dx[i + dx_off] * da
        end
      end
    end
end

function Linpack:idamax(n, dx, dx_off, incx)
    local dmax, dtemp = 0.0
    local i, ix, itemp = 0

    if (n < 1) then
      itemp = -1
    elseif (n == 1) then
      itemp = 0
    elseif (incx ~= 1) then

      -- code for increment not equal to 1

      dmax = self:abs(dx[dx_off])
      ix = 1 + incx
      for i = 1, n do
        dtemp = self:abs(dx[ix + dx_off])
        if (dtemp > dmax) then
          itemp = i
          dmax = dtemp
        end
        ix = ix + incx
      end
    else

      -- code for increment equal to 1

      itemp = 0
      dmax = self:abs(dx[dx_off])
      for i = 1, n do
        dtemp = self:abs(dx[i + dx_off])
        if (dtemp > dmax) then
          itemp = i
          dmax = dtemp
        end
      end
    end
    return (itemp)
end

function Linpack:epslon(x)
    local a, b, c, eps = 0.0

    a = 4.0e0 / 3.0e0
    eps = 0
    while (eps == 0) do
      b = a - 1.0
      c = b + b + b
      eps = self:abs(c - 1.0)
    end
    return (eps * self:abs(x))
end

function Linpack:dmxpy(n1, y, n2, ldm, x, m)
	for j = 1, n2 do
		for i = 1, n1 do
			y[i] = y[i] + x[j] * m[j][i]
		end
	end
end