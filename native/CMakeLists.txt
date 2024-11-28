cmake_minimum_required(VERSION 3.12.4)

cmake_policy(SET CMP0054 NEW)

set (CMAKE_OSX_ARCHITECTURES "x86_64;arm64" CACHE STRING "macOS Build architectures" FORCE)

set (PROJECT_VERSION "1.0")

# Set the variable PROJ_NAME to whatever your library's name is, PROJECT_VERSION should be a version string like "0.1"
project(skeychain VERSION ${PROJECT_VERSION})

# To build shared libraries in Windows, we set CMAKE_WINDOWS_EXPORT_ALL_SYMBOLS to TRUE.
# See https://cmake.org/cmake/help/v3.4/variable/CMAKE_WINDOWS_EXPORT_ALL_SYMBOLS.html
# See https://blog.kitware.com/create-dlls-on-windows-without-declspec-using-new-cmake-export-all-feature/
#set(CMAKE_WINDOWS_EXPORT_ALL_SYMBOLS ON)

# Create our library target
add_library(skeychain SHARED)

target_sources(skeychain
  PUBLIC ${CMAKE_CURRENT_SOURCE_DIR}/src/main.cpp
)

# This will name your output .so files "libsomething.1.0" which is pretty useful
#set_target_properties(skeychain
#PROPERTIES
#    VERSION ${PROJECT_VERSION}
#    SOVERSION ${PROJECT_VERSION}
#)

# Let's set compiler-specific flags
if (CMAKE_CXX_COMPILER_ID STREQUAL "GNU")
    # G++
    target_compile_options(skeychain PRIVATE -Wall -Wextra)
    message("IS G++!!")
elseif(CMAKE_CXX_COMPILER_ID STREQUAL "MSVC")
    # MSVC
    target_compile_options(skeychain PRIVATE /EHsc /W2 /c)
    # Set the DLLEXPORT variable to export symbols
    target_compile_definitions(skeychain PRIVATE WIN_EXPORT)
    message("IS MSVC!!")
elseif(CMAKE_CXX_COMPILER_ID STREQUAL "Clang")
    # Activate all clang warnings except those that don't really affect much
    target_compile_options(roukavici PRIVATE -Weverything -Wno-c++98-compat -Wno-c++98-compat-pedantic -Wno-newline-eof -Wno-padded -Wno-exit-time-destructors -Wno-global-constructors -Wno-constant-conversion)
endif()

message(${CMAKE_CXX_COMPILER_ID})

# We can now add their CMakeList as a child to ours.
add_subdirectory(./lib/keychain/)

# And now, we tell CMake that libdependency should be linked to our library
target_link_libraries(skeychain PRIVATE keychain)
