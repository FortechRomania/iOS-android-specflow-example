XCODE_PATH="/Applications/Xcode_11.3.1.app"
MONO_VERSION="6.6.0"
XAMARIN_IOS_VERSION="13.4.0.2"
XAMARIN_ANDROID_VERSION="9.0.0-20"

USER=$(whoami)

function set_xcode {
    echo "Using Xcode path: $XCODE_PATH"
    sudo xcode-select -s $XCODE_PATH/Contents/Developer
    sudo xcodebuild -showsdks

    echo "Appending AppleSdkRoot to Settings.plist"
    mkdir -p /Users/$USER/Library/Preferences/Xamarin
    cd /Users/$USER/Library/Preferences/Xamarin

    echo "<?xml version=\"1.0\" encoding=\"UTF-8\"?><!DOCTYPE plist PUBLIC \"-//Apple//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\"><plist version=\"1.0\"><dict><key>AppleSdkRoot</key><string>$XCODE_PATH</string></dict></plist>" >> Settings.plist

    echo "-----------------------------------------"
}

function set_mono {
    echo "Available Mono versions:"
    echo "$(ls /Library/Frameworks/Mono.framework/Versions)"
    echo "Setting default Mono version $MONO_VERSION"
    echo "-----------------------------------------"

    sudo rm -f "/Library/Frameworks/Mono.framework/Versions/Current"
    sudo ln -s "/Library/Frameworks/Mono.framework/Versions/$MONO_VERSION" "/Library/Frameworks/Mono.framework/Versions/Current"
}

function set_xamarin_ios {
    echo "Available Xamarin.iOS versions:"
    echo "$(ls /Library/Frameworks/Xamarin.iOS.framework/Versions)"
    echo "Setting default Xamarin.iOS version $XAMARIN_IOS_VERSION"
    echo "-----------------------------------------"

    sudo rm -f "/Library/Frameworks/Xamarin.iOS.framework/Versions/Current"
    sudo ln -s "/Library/Frameworks/Xamarin.iOS.framework/Versions/$XAMARIN_IOS_VERSION" "/Library/Frameworks/Xamarin.iOS.framework/Versions/Current"
}

function set_xamarin_android {
    echo "Available Xamarin.Android versions:"
    echo "$(ls /Library/Frameworks/Xamarin.Android.framework/Versions)"
    echo "Setting default Xamarin.Android version $XAMARIN_ANDROID_VERSION"
    echo "-----------------------------------------"

    sudo rm -f "/Library/Frameworks/Xamarin.Android.framework/Versions/Current"
    sudo ln -s "/Library/Frameworks/Xamarin.Android.framework/Versions/$XAMARIN_ANDROID_VERSION" "/Library/Frameworks/Xamarin.Android.framework/Versions/Current"
}

set_xcode
set_mono
set_xamarin_ios
set_xamarin_android