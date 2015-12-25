#! /bin/sh
# automatcially create CIXReader .deb and .rpm packages, see end for requirements
# e.g. ./autopackage.sh cixreader 1.1.8
project=${1}
autobuildversion=${2}
export autobuildversion

builddir="/tmp/${project}-${autobuildversion}"
if [ -d $builddir ]; then rm -r $builddir; fi 
mkdir $builddir # re-create the directory

# Collect the SVN username
echo -n "SVN Username:";
read username

# checkout the current code
svn checkout https://${username}@svn.cix.co.uk:443/CIXReader/branches/1.60 $builddir

# Update the version number in all the AssemblyVersion files in the source tree
find $builddir -name AssemblyInfo.cs -exec perl -pi -e 's/AssemblyVersion\(\"[0-9]+\.[0-9]+\.[0-9]+/AssemblyVersion\(\"$ENV{'autobuildversion'}/g' {} \;
find $builddir -name AssemblyInfo.cs -exec perl -pi -e 's/AssemblyFileVersion\(\"[0-9]+\.[0-9]+\.[0-9]+/AssemblyFileVersion\(\"$ENV{'autobuildversion'}/g' {} \;
# Update the version in the debian file
perl -pi -e 's/cixreader \([0-9]+\.[0-9]+.[0-9]+/cixreader \($ENV{'autobuildversion'}/g' ${builddir}/debian/changelog

echo "Tarring the original code up"
(cd $builddir; tar czf ../${project}-${autobuildversion}.orig.tar.gz . )
echo "Building .deb packages"
(cd $builddir; 
 debuild -us -uc -b
 )
(cd $builddir; 
 debuild -us -uc -b -ai386
 )
echo "Creating the .rpm packages from the .deb build"
(cd $builddir; 
 cd ..; 
 rm ${project}-${autobuildversion}*.rpm;
 for i in `ls ${project}_${autobuildversion}*.deb`;
 do
  fpm -s deb -t rpm --no-depends $i;
  done;
 mv ${project}-${autobuildversion}-*.i386.rpm ${project}_${autobuildversion}_i386.rpm
 mv ${project}-${autobuildversion}-*.x86_64.rpm ${project}_${autobuildversion}_amd64.rpm
 )

##
## For this script to work you would need to run the following commands on 
## Ubuntu 15.04 (and probably other Debian based O/S's
##
## sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
## echo "deb http://download.mono-project.com/repo/debian wheezy main" | sudo tee /etc/apt/sources.list.d/mono-xamarin.list
## sudo apt-get update
## sudo apt-get install mono-complete debhelper cli-common-dev libgtk2.0-cil-dev libglade2.0-cil-dev devscripts subversion ruby-dev 
## sudo gem install fpm
