APP=ParkitectNexus

MONO_CORE=http://download.mono-project.com/repo/centos/m/mono-core/mono-core-4.2.2.30-0.xamarin.2.x86_64.rpm
MONO_LOCALE=https://kojipkgs.fedoraproject.org//packages/mono/4.0.5/3.fc23/x86_64/mono-locale-extras-4.0.5-3.fc23.x86_64.rpm
GTK_SHARP=http://download.mono-project.com/repo/centos/g/gtk-sharp2/gtk-sharp2-2.12.26-0.x86_64.rpm
GLIB_SHARP=http://download.mono-project.com/repo/centos/g/gtk-sharp2/glib-sharp2-2.12.26-0.x86_64.rpm
#GDK_PIX=https://kojipkgs.fedoraproject.org//packages/gdk-pixbuf2/2.33.2/2.fc24/x86_64/gdk-pixbuf2-2.33.2-2.fc24.x86_64.rpm
#GTK2=https://kojipkgs.fedoraproject.org//packages/gtk2/2.24.29/2.fc24/x86_64/gtk2-2.24.29-2.fc24.x86_64.rpm

#create APP directory
mkdir -p ./$APP/$APP.AppDir/usr/bin
mkdir -p ./$APP/$APP.AppDir/usr/opt

#build Nexus Client and copy release into bin
xbuild /p:Configuration=Release ./../src/ParkitectNexus.Client.Linux/ParkitectNexus.Client.Linux.csproj
cp -R ./../src/ParkitectNexus.Client.Linux/bin/Release/* ./$APP/$APP.AppDir/usr/bin

# Figure out $VERSION
VERSION=$(git describe origin/master  --tags $(git rev-list --tags --max-count=0))
echo $VERSION

#create directory structure
cp parkitectnexus.desktop $APP/$APP.AppDir/.
cp parkitectnexus $APP/$APP.AppDir/usr/bin/.
cp parkitectnexus.wrapper $APP/$APP.AppDir/usr/bin/
chmod a+x $APP/$APP.AppDir/usr/bin/parkitectnexus

#CD into application directory
cd ./$APP

#grab dependencies
wget -c --trust-server-names "$MONO_CORE"
wget -c --trust-server-names "$GTK_SHARP"
wget -c --trust-server-names "$GLIB_SHARP"
wget -c --trust-server-names "$MONO_LOCALE"
#wget -c --trust-server-names "$GDK_PIX"
#wget -c --trust-server-names "$GTK2"

cd $APP.AppDir/

#un-package repositories
rpm2cpio ../mono-core*.rpm . | cpio -idmv
rpm2cpio ../mono-basic*.rpm . | cpio -idmv
rpm2cpio ../gtk-sharp2*.rpm . | cpio -idmv
rpm2cpio ../glib-sharp2*.x86_64.rpm . | cpio -idmv
rpm2cpio ../mono-locale*.x86_64.rpm . | cpio -idmv
#rpm2cpio ../gdk-pixbuf*.x86_64.rpm . | cpio -idmv
#rpm2cpio ../gtk2*.rpm . | cpio -idmv

#grab AppRun
wget -c "https://github.com/probonopd/AppImageKit/releases/download/5/AppRun" # (64-bit)
chmod a+x ./AppRun

#move etc into the user folder
cp -R etc/ usr/
rm -rf etc/

#fixed mono .so dependency
sed -i -e "s|/usr/|././|g" usr/etc/mono/config
#sed -i -e "s|libgtk-x11-2.0.so.0|././lib64/libgtk-x11-2.0.so.0|g" usr/etc/mono/config
#find . -type f -exec sed -i -e 's|/usr|././|g' {} \;
#find . -type f -exec sed -i -e 's|target="libgtk-x11-2.0.so.0"|target="././lib64/libgtk-x11-2.0.so.0"|g' {} \;

#copy image into $APP.dir
cp ./usr/bin/parkitectnexus_logo.png parkitectnexus.png


# Add desktop integration
XAPP=parkitectnexus
#wget -O ./usr/bin/$XAPP.wrapper https://raw.githubusercontent.com/probonopd/AppImageKit/master/desktopintegration
chmod a+x ./usr/bin/$XAPP.wrapper
sed -i -e "s|Exec=$XAPP|Exec=$XAPP.wrapper|g" $XAPP.desktop

cd ..
wget -c "https://github.com/probonopd/AppImageKit/releases/download/5/AppImageAssistant" # (64-bit)
chmod a+x ./AppImageAssistant
mkdir -p ../out
rm ../out/$APP"-"$VERSION"-x86_64.AppImage" || true
./AppImageAssistant ./$APP.AppDir/ ../out/$APP"-"$VERSION"-x86_64.AppImage"
