MonteCarlo = class(function(f)
end)

function MonteCarlo.integrate(cycles)
    local under_curve = 0
    local rand = rand
    for i=1,cycles do
        local x = math.random()
        local y = math.random()
        if x*x + y*y <= 1.0 then under_curve = under_curve + 1 end
    end
    return (under_curve/cycles) * 4
end