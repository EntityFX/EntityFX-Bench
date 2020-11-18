function class(base, init)
    local c = {}    -- a new class instance
    if not init and type(base) == 'function' then
       init = base
       base = nil
    elseif type(base) == 'table' then
     -- our new class is a shallow copy of the base class!
       for i,v in pairs(base) do
          c[i] = v
       end
       c._base = base
    end
    -- the class will be the metatable for all its objects,
    -- and they will look up their methods in it.
    c.__index = c
 
    -- expose a constructor which can be called by <classname>(<args>)
    local mt = {}
    mt.__call = function(class_tbl, ...)
    local obj = {}
    setmetatable(obj,c)
    if init then
       init(obj,...)
    else 
       -- make sure that any stuff from the base class is initialized!
       if base and base.init then
       base.init(obj, ...)
       end
    end
    return obj
    end
    c.init = init
    c.is_a = function(self, klass)
       local m = getmetatable(self)
       while m do 
          if m == klass then return true end
          m = m._base
       end
       return false
    end
    setmetatable(c, mt)
    return c
end

function isArray(t)
   if type(t)~="table" then return nil,"Argument is not a table! It is: "..type(t) end
   --check if all the table keys are numerical and count their number
   local count=0
   for k,v in pairs(t) do
       if type(k)~="number" then return false else count=count+1 end
   end
   --all keys are numerical. now let's see if they are sequential and start with 1
   for i=1,count do
       --Hint: the VALUE might be "nil", in that case "not t[i]" isn't enough, that's why we check the type
       if not t[i] and type(t[i])~="nil" then return false end
   end
   return true
end


function dumpTable(table, depth)
   if (depth > 200) then
     print("Error: Depth > 200 in dumpTable()")
     return
   end
   for k,v in pairs(table) do
     if (type(v) == "table") then
       print(string.rep("  ", depth)..k..":")
       dumpTable(v, depth+1)
     else
       print(string.rep("  ", depth)..k..": ",v)
     end
   end
 end