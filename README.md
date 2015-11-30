CIXReader Source Code

This is the source code for the CIXReader CIX offline reader. It is being
released under the Apache license. See the LICENSE file for more details.

The code requires Microsoft Visual Studio 2012 or greater to build. The
solutions file provided is for Visual Studio 2012 which is the minimum
supported version.

Build
=====
To build, open CIXReader.sln in Visual Studio and do a full build. There
should be no warnings and warnings are explicitly treated as errors in all
projects.

On completion of a build, simply Run and it should pick up any existing
settings and the cache database. No need to copy the built binaries to your
current CIXReader installation.

Setup
=====
The setup package requires Inno Setup to build. An installation package
for Inno Setup is included in the Tools folder, or feel free to install the
latest directly from their website at http://www.jrsoftware.org/isdl.php.

Release
=======
To build a release package, open a command window and set the environment
variable _CRDEVROOT to the root of your CIXReader source folder (where the
CIXReader.sln solution file is located). Then run the build.cmd batch file
from the command window. This will do a clean beta build of the project and
create an installation package (assuming you have installed Inno Setup as
specified above) in the %_CRDEVROOT%\drops folder.

Contributions
=============
The easiest way to make contributions is to create your own branch and work
in that, then submit pull requests. The source code should be pretty well
documented by its structure and comments but feel free to ask in the
cix.developer forum on CIX if you have any questions.

