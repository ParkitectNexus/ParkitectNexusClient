APP=ParkitectNexus
MONO_URL=https://kojipkgs.fedoraproject.org//packages/mono/4.2.2/3.fc24/x86_64/mono-core-4.2.2-3.fc24.x86_64.rpm
GTK_SHARP=http://download.mono-project.com/repo/centos/g/gtk-sharp2/gtk-sharp2-2.12.26-0.x86_64.rpm
GLIB_SHARP=http://download.mono-project.com/repo/centos/g/gtk-sharp2/glib-sharp2-2.12.26-0.x86_64.rpm

mkdir -p ./$APP/$APP.AppDir/usr/bin
mkdir -p ./$APP/$APP.AppDir/usr/opt

xbuild /p:Configuration=Release ./../src/ParkitectNexus.Client.Linux/ParkitectNexus.Client.Linux.csproj
cp -R ./../src/ParkitectNexus.Client.Linux/bin/Release/* ./$APP/$APP.AppDir/usr/bin
cp parkitectnexus.desktop $APP/$APP.AppDir/.
cp parkitectnexus $APP/$APP.AppDir/usr/bin/.
cp parkitectnexus.wrapper $APP/$APP.AppDir/usr/bin/
chmod a+x $APP/$APP.AppDir/usr/bin/parkitectnexus

cd ./$APP

wget -c --trust-server-names "$MONO_URL"
wget -c --trust-server-names "$GTK_SHARP"
wget -c --trust-server-names "$GLIB_SHARP"

cd $APP.AppDir/
rpm2cpio ../mono-core*.rpm . | cpio -idmv
rpm2cpio ../gtk-sharp2*.rpm . | cpio -idmv
rpm2cpio ../glib-sharp2*.rpm . | cpio -idmv


wget -c "https://github.com/probonopd/AppImageKit/releases/download/5/AppRun" # (64-bit)
chmod a+x ./AppRun

mv etc/ usr/

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
./AppImageAssistant ./$APP.AppDir/ ../out/$APP"-x86_64.AppImage"