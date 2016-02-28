APP=ParkitectNexus
MONO_URL=https://kojipkgs.fedoraproject.org//packages/mono/4.2.2/3.fc24/x86_64/mono-core-4.2.2-3.fc24.x86_64.rpm


mkdir -p ./$APP/$APP.AppDir/usr/bin
mkdir -p ./$APP/$APP.AppDir/usr/opt
mkdir -p ./$APP/$APP.AppDir/usr/opt/nexus

cp -R ./../src/ParkitectNexus.Client.Linux/bin/Release/* ./$APP/$APP.AppDir/usr/opt/nexus
cp parkitectnexus.desktop $APP/$APP.AppDir/.
cp parkitectnexus $APP/$APP.AppDir/usr/bin/.
cp parkitectnexus.wrapper $APP/$APP.AppDir/usr/bin/
chmod a+x $APP/$APP.AppDir/usr/bin/parkitectnexus

cd ./$APP

wget -c --trust-server-names "$MONO_URL"

cd $APP.AppDir/
rpm2cpio ../*.rpm . | cpio -idmv


wget -c "https://github.com/probonopd/AppImageKit/releases/download/5/AppRun" # (64-bit)
chmod a+x ./AppRun

cp ./usr/opt/nexus/parkitectnexus_logo.png parkitectnexus.png


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