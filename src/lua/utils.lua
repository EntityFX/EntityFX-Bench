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

function split(inputstr, sep)
   if sep == nil then
      sep = "%s"
   end
   local t={}
   for str in string.gmatch(inputstr, "([^"..sep.."]+)") do
         table.insert(t, str)
   end
   return t
end

function randomIntArray(size, max)
   math.randomseed( os.time() )

   local ar = {}

   for i = 0, size do
      ar[i] = math.random(0, max)
   end

   return ar
end

-- Returns the sum of a sequence of values
function sum(x)
   local s = 0
   for _, v in ipairs(x) do s = s + v end
   return s
end
 
-- Calculates the arithmetic mean of a set of values
-- x       : an array of values
-- returns : the arithmetic mean
function arithmetic_mean(x)
   return (sum(x) / #x)
end

function compareTo(str1, str2)
   if str1 == str2 then
      return 0
   elseif str1 < str2 then
      return -1
   else
      return 1
   end
end

function hex_to_binary(hex)
   return hex:gsub('..', function(hexval)
         return string.char(tonumber(hexval, 16))
      end)
end

function getOS()
	local raw_os_name, raw_arch_name = '', ''

	-- LuaJIT shortcut
	if jit and jit.os and jit.arch then
		raw_os_name = jit.os
		raw_arch_name = jit.arch
	else
		-- is popen supported?
		local popen_status, popen_result = pcall(io.popen, "")
		if popen_status then
			popen_result:close()
			-- Unix-based OS
			raw_os_name = io.popen('uname -s','r'):read('*l')
			raw_arch_name = io.popen('uname -m','r'):read('*l')
		else
			-- Windows
			local env_OS = os.getenv('OS')
			local env_ARCH = os.getenv('PROCESSOR_ARCHITECTURE')
			if env_OS and env_ARCH then
				raw_os_name, raw_arch_name = env_OS, env_ARCH
			end
		end
	end

	raw_os_name = (raw_os_name):lower()
	raw_arch_name = (raw_arch_name):lower()

	local os_patterns = {
		['windows'] = 'Windows',
		['linux'] = 'Linux',
		['mac'] = 'Mac',
		['darwin'] = 'Mac',
		['^mingw'] = 'Windows',
		['^cygwin'] = 'Windows',
		['bsd$'] = 'BSD',
		['SunOS'] = 'Solaris',
	}
	
	local arch_patterns = {
		['^x86$'] = 'x86',
		['i[%d]86'] = 'x86',
		['amd64'] = 'x86_64',
		['x86_64'] = 'x86_64',
		['Power Macintosh'] = 'powerpc',
		['^arm'] = 'arm',
      ['^mips'] = 'mips',
      ['e2k'] = 'e2k',
	}

	local os_name, arch_name = 'unknown', 'unknown'

	for pattern, name in pairs(os_patterns) do
		if raw_os_name:match(pattern) then
			os_name = name
			break
		end
	end
	for pattern, name in pairs(arch_patterns) do
		if raw_arch_name:match(pattern) then
			arch_name = name
			break
		end
	end
	return os_name, arch_name
end