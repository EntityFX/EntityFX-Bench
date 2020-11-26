MIN_TIME = 2.0
SIZE_SELECT = "small"

SCIMARK_VERSION = "2010-12-10"
SCIMARK_COPYRIGHT = "Copyright (C) 2006-2010 Mike Pall"

constants = {
  "FFT", "SOR", "MC", "SPARSE", "LU",
  small = {
    FFT		= { 1024 },
    SOR		= { 100 },
    MC		= { },
    SPARSE	= { 1000, 5000 },
    LU		= { 100 },
  },
  large = {
    FFT		= { 1048576 },
    SOR		= { 1000 },
    MC		= { },
    SPARSE	= { 100000, 1000000 },
    LU		= { 1000 },
  },
}

local RESOLUTION_DEFAULT = 2.0 
local RANDOM_SEED = 101010 
local FFT_SIZE = 1024 
local SOR_SIZE = 100 
local SPARSE_SIZE_M = 1000 
local SPARSE_SIZE_NZ = 5000 
local LU_SIZE = 100 
local LG_FFT_SIZE = 1048576 
local LG_SOR_SIZE = 1000 
local LG_SPARSE_SIZE_M = 100000 
local LG_SPARSE_SIZE_NZ = 1000000 
local LG_LU_SIZE = 1000 
local TINY_FFT_SIZE = 16 
local TINY_SOR_SIZE = 10 
local TINY_SPARSE_SIZE_M = 10 
local TINY_SPARSE_SIZE_N = 10 
local TINY_SPARSE_SIZE_NZ = 50 
local TINY_LU_SIZE = 10