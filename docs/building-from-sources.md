# Building from sources

There are two supported ways to build application from sources:

  * fully automatic build;
  * manual build.

# Preparing to build

  1. Clone this repository or download [source tarball](https://github.com/xvitaly/srcrepair/releases).
  2. Install pre-requirements.

# Installing pre-requirements

First you will need to install C# complier, Microsoft .NET Framework SDK and other required tools (all steps are mandatory):

  1. download [Microsoft Visual Studio 2019 Community](https://visualstudio.microsoft.com/vs/community/) installer and run it;
  2. select **Microsoft Visual Studio 2019 Community**, enable **Classic .NET application development** component, then switch to **Additional components** tab and enable **NuGet package manager**;
  3. install Microsoft Visual Studio 2019 Community;
  4. download latest version of [NuGet CLI](https://www.nuget.org/downloads) and copy executable to any directory, located in `%PATH%` environment variable;
  5. install [Doxygen for Windows](http://www.doxygen.nl/download.html) to default directory;
  6. download and install [HTML Help Workshop](https://www.microsoft.com/en-us/download/details.aspx?id=21138);
  7. download and install [Gpg4Win](https://www.gpg4win.org/) to default directory;
  8. download and install [Python 3.7 for Windows](https://www.python.org/downloads/windows/);
  9. open terminal and install Sphinx-doc 1.8.2 using PIP: `pip3 install "sphinx==1.8.2"`;
  10. download and install [7-Zip for Windows](https://www.7-zip.org/download.html) to default directory;
  11. download and install [InnoSetup](http://www.jrsoftware.org/isdl.php) to default directory.

# Automatic build

If you want to use automatic build, follow this steps:

  1. install pre-requirements;
  2. double click on `packaging/build_win.cmd`.

You will find results in `packaging/results` directory.

# Manual build

If you don't want to use automatic method, you can build this project manually.

## Building main project

  1. Run Microsoft Visual Studio 2019 Community.
  2. **File** - **Open** - **Project or solution**, select `srcrepair.sln`, then press **Open** button.
  3. **Tools** - **NuGet Package Manager** - **Manage NuGet packages for Solution**, press **Restore** button.
  4. **Build** - **Configuration manager** - **Active solution configuration** - **Release**, then press **Close** button.
  5. **Build** - **Build solution**.

You will find results in `srcrepair/bin/Release` directory.

## Building installer

  1. Run InnoSetup Compiler.
  2. Open file `packaging/inno/srcrepair.iss`.
  3. **Build** - **Compile**.

If InnoSetup will complain about missing `*.sig` files, you will need to manually sign compiled binaries with detached GnuPG signatures using Gpg4Win, or remove this rows from InnoSetup script.

You will find results in `packaging/results` directory.
