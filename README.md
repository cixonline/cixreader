# CIXReader Source Code

This is the source code for the CIXReader CIX offline reader. It is being released under the Apache license. See the LICENSE file for more details.

## Building CIXReader

These instructions assume starting from a clean Windows 10 or later machine with no additional software that may conflict with what is required.

1.	Install Git tools and Perl.

2.	Install Visual Studio 2017 Community Edition.
* In the Optional components section, you can uncheck everything except for the Web Development tools if you plan to do API or web development on the same machine.
* Make sure you install all updates and security updates.
* Launch Visual Studio once after installation to complete the first-time run initialisation.

2.	Create a folder for the CIXReader source.

3.	In that folder use Git to clone the latest CIXReader source from GitHub:

```
git clone https://github.com/cixonline/cixreader.git
```

4.	Install Inno Setup. Go into the Tools folder and run the issetup program. The actual filename will vary and will have a version number on the end.

5.	Get and copy codesigning.pfx from somewhere. This will be needed when you come to sign the packages for distribution. If you’re just debugging then you don’t need this.

6.	Open a Developer Command Prompt for VS 2017 window.  You’ll find this under the Visual Studio 2017 folder on the Windows Start menu.

7.	Set some variables:

```
set _CRDEVROOT=C:\where_you_cloned_CIXReader
set _PFXPATH=C:\path_to_codesigning_pfx
set _AM2SIGNPWD=password_for_codesigning_pfx
```

8.	Now go to the root of the CIXReader source folder and run:

```
build release
```

9.	On completion of a build, the installation packages will be placed in the drops folder. They are ready to be installed.

10.	To work in Visual Studio, open CIXReader.sln and do a full build. There should be no warnings and warnings are explicitly treated as errors in all projects. On completion of a build, simply Run and it should pick up any existing settings and the cache database. No need to copy the built binaries to your current CIXReader installation.

## Contributions

The easiest way to make contributions is to create your own branch and work
in that, then submit pull requests. The source code should be pretty well
documented by its structure and comments but feel free to ask in the
cix.developer forum on CIX if you have any questions.

